// Core imports

use core::debug::PrintTrait;

// Starknet imports

use starknet::ContractAddress;

// Internal imports

use zklash::constants;
use zklash::store::{Store, StoreImpl};
use zklash::helpers::packer::Packer;
use zklash::helpers::battler::Battler;
use zklash::models::squad::{Squad, SquadTrait};
use zklash::models::shop::{Shop, ShopTrait};
use zklash::models::character::{Character, CharacterTrait};
use zklash::types::item::{Item, ItemTrait};
use zklash::types::role::{Role, RoleTrait};
use zklash::types::wave::{Wave, WaveTrait};


#[derive(Model, Copy, Drop, Serde)]
struct Team {
    #[key]
    player_id: ContractAddress,
    #[key]
    id: u32,
    registry_id: u32,
    gold: u32,
    health: u32,
    level: u8,
    character_uuid: u8,
    battle_id: u8,
    foe_squad_id: u32,
}

mod errors {
    const TEAM_NOT_EXIST: felt252 = 'Team: does not exist';
    const TEAM_ALREADY_EXIST: felt252 = 'Team: already exist';
    const TEAM_INVALID_SEED: felt252 = 'Team: invalid seed';
    const TEAM_IS_EMPTY: felt252 = 'Team: is empty';
    const TEAM_NOT_AFFORDABLE: felt252 = 'Team: not affordable';
    const TEAM_IS_DEFEATED: felt252 = 'Team: is defeated';
    const TEAM_XP_INVALID_ROLE: felt252 = 'Team: invalid role for xp';
    const TEAM_ALREADY_WON: felt252 = 'Team: has already won';
    const TEAM_INVALID_TEAM: felt252 = 'Team: invalid team';
    const TEAM_INVALID_ROLE: felt252 = 'Team: invalid role';
}

#[generate_trait]
impl TeamImpl of TeamTrait {
    #[inline(always)]
    fn new(player_id: ContractAddress, id: u32) -> Team {
        // [Return] Team
        Team {
            player_id,
            id,
            registry_id: constants::DEFAULT_REGISTRY_ID,
            gold: constants::DEFAULT_GOLD,
            health: constants::DEFAULT_HEALTH,
            level: 1,
            character_uuid: 0,
            battle_id: 0,
            foe_squad_id: 0,
        }
    }

    #[inline(always)]
    fn spawn_shop(self: Team, seed: felt252) -> Shop {
        // [Return] Shop
        let shop: Shop = ShopTrait::new(self.player_id, self.id, seed);
        shop
    }

    #[inline(always)]
    fn equip(ref self: Team, ref shop: Shop, ref character: Character, index: u8) {
        // [Check] Not defeated
        self.assert_not_defeated();
        // [Check] Affordable
        let item: Item = shop.purchase_item(index);
        let cost: u8 = item.cost();
        self.assert_is_affordable(cost);
        // [Effect] Update Gold
        self.gold -= cost.into();
        // [Effect] Update Characters
        character.equip(item);
    }

    #[inline(always)]
    fn hire(ref self: Team, ref shop: Shop, index: u8) -> Character {
        // [Check] Not defeated
        self.assert_not_defeated();
        // [Check] Affordable
        let role: Role = shop.purchase_role(index);
        let cost = role.cost(1);
        self.assert_is_affordable(cost);
        // [Effect] Update Gold
        self.gold -= cost.into();
        // [Effect] Hire Character
        self.character_uuid += 1;
        let character_id = self.character_uuid;
        let character: Character = CharacterTrait::new(self.player_id, self.id, character_id, role);
        character
    }

    #[inline(always)]
    fn xp(ref self: Team, ref shop: Shop, ref character: Character, index: u8) {
        // [Check] Not defeated
        self.assert_not_defeated();
        // [Check] Affordable
        let role: Role = character.role.into();
        let cost: u8 = role.cost(1);
        self.assert_is_affordable(cost);
        // [Effect] Update Gold
        self.gold -= cost.into();
        // [Check] Roles match
        let purchased_role: Role = shop.purchase_role(index);
        assert(role == purchased_role, errors::TEAM_XP_INVALID_ROLE);
        // [Effect] Update Character
        character.xp();
    }

    #[inline(always)]
    fn merge(ref self: Team, ref from: Character, ref to: Character) {
        // [Check] Not defeated
        self.assert_not_defeated();
        // [Check] Roles match
        let from_role: Role = from.role.into();
        let to_role: Role = to.role.into();
        assert(from_role == to_role, errors::TEAM_XP_INVALID_ROLE);
        // [Effect] Update Character
        from.merge(ref to);
    }

    #[inline(always)]
    fn sell(ref self: Team, ref character: Character) {
        // [Check] Not defeated
        self.assert_not_defeated();
        // [Effect] Update Gold
        let role: Role = character.role.into();
        let item: Item = character.item.into();
        self.gold += 75 * item.cost().into() + role.cost(character.level).into() / 100;
        // [Effect] Update Character
        character.nullify();
    }

    #[inline(always)]
    fn reroll(ref self: Team, ref shop: Shop, seed: felt252) {
        // [Check] Not defeated
        self.assert_not_defeated();
        // [Check] Affordable
        self.assert_is_affordable(shop.reroll_cost);
        // [Effect] Update Gold
        self.gold -= shop.reroll_cost.into();
        // [Effect] Reroll Shop
        shop.shuffle(seed);
    }

    #[inline(always)]
    fn fight(
        ref self: Team,
        ref shop: Shop,
        ref team_squad: Squad,
        ref chars: Array<Character>,
        ref foe_squad: Squad,
        ref foes: Array<Character>,
        seed: felt252,
    ) -> bool {
        // [Check] Not defeated
        self.assert_not_defeated();
        // [Check] Not empty
        assert(chars.len() != 0, errors::TEAM_IS_EMPTY);
        assert(foes.len() != 0, errors::TEAM_IS_EMPTY);
        // [Effect] Fight and manage the win status
        let win = team_squad.fight(ref chars, ref foe_squad, ref foes);
        if win {
            self.level += 1;
        } else {
            self.health -= 1;
        };
        // [Effect] Reset gold
        self.gold += constants::DEFAULT_GOLD;
        // [Effect] Reroll Shop
        shop.shuffle(seed);
        // [Effect] Update foe squad id for client purpose
        self.foe_squad_id = foe_squad.id;
        // [Effect] Increase battle id
        self.battle_id += 1;
        // [Return] Won status
        if self.level == constants::TEAM_MAX_LEVEL {
            // [Effect] Game over
            self.health = 0;
            return true;
        };
        false
    }
}

#[generate_trait]
impl TeamAssert of AssertTrait {
    #[inline(always)]
    fn assert_exists(self: Team) {
        assert(self.is_non_zero(), errors::TEAM_NOT_EXIST);
    }

    #[inline(always)]
    fn assert_not_exists(self: Team) {
        assert(self.is_zero(), errors::TEAM_ALREADY_EXIST);
    }

    #[inline(always)]
    fn assert_is_affordable(self: Team, cost: u8) {
        assert(self.gold >= cost.into(), errors::TEAM_NOT_AFFORDABLE);
    }

    #[inline(always)]
    fn assert_not_defeated(self: Team) {
        assert(self.health > 0, errors::TEAM_IS_DEFEATED);
    }
}

impl ZeroableTeam of core::Zeroable<Team> {
    #[inline(always)]
    fn zero() -> Team {
        Team {
            player_id: Zeroable::zero(),
            id: 0,
            registry_id: 0,
            gold: 0,
            health: 0,
            level: 0,
            character_uuid: 0,
            battle_id: 0,
            foe_squad_id: 0,
        }
    }

    #[inline(always)]
    fn is_zero(self: Team) -> bool {
        0 == self.battle_id && 0 == self.health
    }

    #[inline(always)]
    fn is_non_zero(self: Team) -> bool {
        !self.is_zero()
    }
}
