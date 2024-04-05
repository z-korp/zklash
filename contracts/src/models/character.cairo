// Core imports

use core::poseidon::hades_permutation;

// Starknet imports

use starknet::ContractAddress;

// Internal imports

use zklash::constants;
use zklash::types::item::Item;
use zklash::types::role::{Role, RoleTrait};

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
    xp: u32,
    level: u8,
    health: u8,
    attack: u8,
}

mod errors {
    const CHARACTER_NOT_EXIST: felt252 = 'Character: does not exist';
    const CHARACTER_ALREADY_EXIST: felt252 = 'Character: already exist';
    const CHARACTER_INVALID_ROLE: felt252 = 'Character: invalid role';
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
        }
    }

    #[inline(always)]
    fn equip(ref self: Character, item: Item) {
        self.item = item.into();
    }
}

impl ZeroableCharacterImpl of core::Zeroable<Character> {
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
}
