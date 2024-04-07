// Core imports

use core::debug::PrintTrait;
use core::poseidon::hades_permutation;

// Starknet imports

use starknet::ContractAddress;

// Internal imports

use zklash::constants;
use zklash::events::{Fighter, Hit, Stun, Absorb, Usage, Talent,};
use zklash::store::{Store, StoreImpl};
use zklash::helpers::packer::Packer;
use zklash::helpers::battler::Battler;
use zklash::models::shop::{Shop, ShopTrait};
use zklash::models::character::{Character, CharacterTrait};
use zklash::types::item::Item;
use zklash::types::role::Role;
use zklash::types::wave::{Wave, WaveTrait};


#[derive(Model, Copy, Drop, Serde)]
struct Team {
    #[key]
    player_id: ContractAddress,
    #[key]
    id: u32,
    seed: felt252,
    nonce: felt252,
    gold: u32,
    health: u32,
    level: u8,
    character_count: u8,
    battle_id: u8,
}

mod errors {
    const TEAM_NOT_EXIST: felt252 = 'Team: does not exist';
    const TEAM_ALREADY_EXIST: felt252 = 'Team: already exist';
    const TEAM_INVALID_SEED: felt252 = 'Team: invalid seed';
    const TEAM_IS_EMPTY: felt252 = 'Team: is empty';
    const TEAM_NOT_AFFORDABLE: felt252 = 'Team: not affordable';
    const TEAM_IS_DEFEATED: felt252 = 'Team: is defeated';
    const TEAM_XP_INVALID_ROLE: felt252 = 'Team: invalid role for xp';
    const TEAM_CANNOT_FIGHT: felt252 = 'Team: cannot fight';
}

#[generate_trait]
impl TeamImpl of TeamTrait {
    #[inline(always)]
    fn new(player_id: ContractAddress, id: u32, salt: felt252) -> Team {
        // [Check] Seed is valid
        let (seed, _, _) = hades_permutation(id.into(), salt, 0);
        // [Return] Team
        Team {
            player_id,
            id,
            seed,
            nonce: 0,
            gold: constants::DEFAULT_GOLD,
            health: constants::DEFAULT_HEALTH,
            level: 0,
            character_count: 0,
            battle_id: 0,
        }
    }

    #[inline(always)]
    fn spawn_shop(ref self: Team) -> Shop {
        // [Return] Shop
        let shop: Shop = ShopTrait::new(self.player_id, self.id, self.seed());
        self.nonce += 1;
        shop
    }

    #[inline(always)]
    fn seed(self: Team) -> felt252 {
        let (seed, _, _) = hades_permutation(self.seed, self.nonce, 0);
        seed
    }

    #[inline(always)]
    fn equip(ref self: Team, ref shop: Shop, ref character: Character, index: u8) {
        // [Check] Not defeated
        self.assert_not_defeated();
        // [Check] Affordable
        self.assert_is_affordable(shop.purchase_cost);
        // [Effect] Update Gold
        self.gold -= shop.purchase_cost.into();
        // [Effect] Purchase item from the shop
        let item: Item = shop.purchase_item(index);
        // [Effect] Update Characters
        character.equip(item);
    }

    #[inline(always)]
    fn hire(ref self: Team, ref shop: Shop, index: u8) -> Character {
        // [Check] Not defeated
        self.assert_not_defeated();
        // [Check] Affordable
        self.assert_is_affordable(shop.purchase_cost);
        // [Effect] Update Gold
        self.gold -= shop.purchase_cost.into();
        // [Effect] Hire Character
        let role: Role = shop.purchase_role(index);
        self.character_count += 1;
        let character_id = self.character_count;
        let character: Character = CharacterTrait::new(self.player_id, self.id, character_id, role);
        character
    }

    #[inline(always)]
    fn xp(ref self: Team, ref shop: Shop, ref character: Character, index: u8) {
        // [Check] Not defeated
        self.assert_not_defeated();
        // [Check] Affordable
        self.assert_is_affordable(shop.purchase_cost);
        // [Effect] Update Gold
        self.gold -= shop.purchase_cost.into();
        // [Check] Roles match
        let role: Role = character.role.into();
        let purchased_role: Role = shop.purchase_role(index);
        assert(role == purchased_role, errors::TEAM_XP_INVALID_ROLE);
        // [Effect] Update Character
        character.xp();
    }

    #[inline(always)]
    fn sell(ref self: Team, ref character: Character) {
        // [Check] Not defeated
        self.assert_not_defeated();
        // [Effect] Update Gold
        self.gold += character.level.into();
        // [Effect] Update Character
        character.nullify();
    }

    #[inline(always)]
    fn reroll(ref self: Team, ref shop: Shop) {
        // [Check] Not defeated
        self.assert_not_defeated();
        // [Check] Affordable
        self.assert_is_affordable(shop.reroll_cost);
        // [Effect] Update Gold
        self.gold -= shop.reroll_cost.into();
        // [Effect] Reroll Shop
        shop.shuffle(self.seed());
        self.nonce += 1;
    }

    #[inline(always)]
    fn fight(
        ref self: Team, ref shop: Shop, ref chars: Array<Character>
    ) -> (Array<Fighter>, Array<Hit>, Array<Stun>, Array<Absorb>, Array<Usage>, Array<Talent>,) {
        // [Check] Not defeated
        self.assert_not_defeated();
        // [Check] Not empty
        assert(chars.len() != 0, errors::TEAM_IS_EMPTY);
        // [Compute] Generate foes according to level
        let wave: Wave = (self.level + 1).into();
        let mut foes: Array<Character> = wave.characters();
        // [Effect] Fight and manage the win status
        let (win, fighters, hits, stuns, absorbs, usages, talents) = Battler::start(
            ref chars, ref foes, self.battle_id,
        );
        if win {
            self.level += 1;
        } else {
            self.health -= 1;
        }
        // [Effect] Reset gold
        self.gold = constants::DEFAULT_GOLD;
        // [Effect] Reroll Shop
        shop.shuffle(self.seed());
        self.nonce += 1;
        // [Effect] Increase battle id
        self.battle_id += 1;
        (fighters, hits, stuns, absorbs, usages, talents,)
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

    #[inline(always)]
    fn assert_can_fight(self: Team) {
        assert(self.level < 10, errors::TEAM_CANNOT_FIGHT);
    }
}

impl ZeroableTeam of core::Zeroable<Team> {
    #[inline(always)]
    fn zero() -> Team {
        Team {
            player_id: Zeroable::zero(),
            id: 0,
            seed: 0,
            nonce: 0,
            gold: 0,
            health: 0,
            level: 0,
            character_count: 0,
            battle_id: 0,
        }
    }

    #[inline(always)]
    fn is_zero(self: Team) -> bool {
        0 == self.seed
    }

    #[inline(always)]
    fn is_non_zero(self: Team) -> bool {
        !self.is_zero()
    }
}
