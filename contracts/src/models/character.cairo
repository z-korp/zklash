// Core imports

use core::poseidon::hades_permutation;

// Starknet imports

use starknet::ContractAddress;

// Internal imports

use zklash::constants;
use zklash::helpers::math::Math;
use zklash::types::item::{Item, ItemTrait};
use zklash::types::role::{Role, RoleTrait};
use zklash::types::phase::Phase;

#[derive(Model, Copy, Drop, Serde)]
struct Character {
    #[key]
    player_id: ContractAddress,
    #[key]
    team_id: u32,
    #[key]
    id: u8,
    role: u8,
    item: u8,
    xp: u8,
    level: u8,
    health: u8,
    attack: u8,
    absorb: u8,
    stun: bool,
}

// Constants

const MAX_LEVEL: u8 = 3;

// Errors

mod errors {
    const CHARACTER_NOT_EXIST: felt252 = 'Character: does not exist';
    const CHARACTER_ALREADY_EXIST: felt252 = 'Character: already exist';
    const CHARACTER_INVALID_ROLE: felt252 = 'Character: invalid role';
    const CHARACTER_NOT_LEVELABLE: felt252 = 'Character: not levelable';
}

#[generate_trait]
impl CharacterImpl of CharacterTrait {
    #[inline(always)]
    fn new(player_id: ContractAddress, team_id: u32, id: u8, role: Role) -> Character {
        Character {
            player_id,
            team_id,
            id,
            role: role.into(),
            item: Item::None.into(),
            xp: 0,
            level: 1,
            health: role.health(),
            attack: role.attack(),
            absorb: 0,
            stun: false,
        }
    }

    fn from(role: Role, level: u8) -> Character {
        Character {
            player_id: Zeroable::zero(),
            team_id: 0,
            id: 0,
            role: role.into(),
            item: Item::None.into(),
            xp: 0,
            level,
            health: role.health(),
            attack: role.attack(),
            absorb: 0,
            stun: false,
        }
    }

    #[inline(always)]
    fn equip(ref self: Character, item: Item) {
        // [Effect] Remove the previous item's effect
        self.debuff(Phase::OnEquip);
        // [Effect] Equip and apply the new item's effect
        self.item = item.into();
        self.buff(Phase::OnEquip);
    }

    #[inline(always)]
    fn xp(ref self: Character) {
        // [Check] Character is levelable
        self.assert_is_levelable();
        // [Effect] Level up the character
        self.xp += 1;
        if self.xp == self.level + 1 {
            self.level += 1;
            self.xp = 0;
        };
    }

    #[inline(always)]
    fn buff(ref self: Character, phase: Phase) -> u8 {
        // [Effect] Update the item's effect
        let item: Item = self.item.into();
        self.health += item.health(phase);
        self.attack += item.attack(phase);
        self.absorb += item.absorb(phase);
        self.item = item.usage(phase).into();
        item.damage(phase)
    }

    #[inline(always)]
    fn debuff(ref self: Character, phase: Phase) {
        // [Effect] Update the item's effect
        let item: Item = self.item.into();
        self.health -= item.health(phase);
        self.attack -= item.attack(phase);
        self.absorb -= item.absorb(phase);
        self.item = item.usage(phase).into();
    }

    #[inline(always)]
    fn take_damage(ref self: Character, mut damage: u8) {
        // [Effect] Apply the damage to the character
        if damage > 0 {
            damage -= Math::min(damage, self.absorb);
            self.absorb = 0;
        }
        self.health -= Math::min(damage, self.health);
    }

    #[inline(always)]
    fn is_dead(self: Character) -> bool {
        self.health == 0
    }

    #[inline(always)]
    fn nullify(ref self: Character) {
        self.role = Role::None.into();
    }
}

impl ZeroableCharacter of core::Zeroable<Character> {
    #[inline(always)]
    fn zero() -> Character {
        Character {
            player_id: Zeroable::zero(),
            team_id: 0,
            id: 0,
            role: 0,
            item: 0,
            xp: 0,
            level: 0,
            health: 0,
            attack: 0,
            absorb: 0,
            stun: false,
        }
    }

    #[inline(always)]
    fn is_zero(self: Character) -> bool {
        Role::None == self.role.into()
    }

    #[inline(always)]
    fn is_non_zero(self: Character) -> bool {
        !self.is_zero()
    }
}

impl PartialEqCharacter of PartialEq<Character> {
    #[inline(always)]
    fn eq(lhs: @Character, rhs: @Character) -> bool {
        lhs.id == rhs.id
    }

    #[inline(always)]
    fn ne(lhs: @Character, rhs: @Character) -> bool {
        lhs.id != rhs.id
    }
}

#[generate_trait]
impl CharacterAssert of AssertTrait {
    #[inline(always)]
    fn assert_exists(self: Character) {
        assert(self.is_non_zero(), errors::CHARACTER_NOT_EXIST);
    }

    #[inline(always)]
    fn assert_not_exists(self: Character) {
        assert(self.is_zero(), errors::CHARACTER_ALREADY_EXIST);
    }

    #[inline(always)]
    fn assert_is_levelable(self: Character) {
        assert(self.level < MAX_LEVEL, errors::CHARACTER_NOT_LEVELABLE);
    }
}
