// Core imports

use core::debug::PrintTrait;

// Internal imports

use zklash::roles::knight::{RoleImpl as KnightImpl};
use zklash::roles::bowman::{RoleImpl as BowmanImpl};
use zklash::roles::pawn::{RoleImpl as PawnImpl};
use zklash::roles::torchoblin::{RoleImpl as TorchoblinImpl};
use zklash::roles::dynamoblin::{RoleImpl as DynamoblinImpl};
use zklash::roles::bomboblin::{RoleImpl as BomboblinImpl};
use zklash::types::item::Item;

// Constants

const ROLE_COUNT: u8 = 6;
const NONE: felt252 = 0;
const KNIGHT: felt252 = 'KNIGHT';
const BOWMAN: felt252 = 'BOWMAN';
const PAWN: felt252 = 'PAWN';
const TORCHOBLIN: felt252 = 'TORCHOBLIN';
const DYNAMOBLIN: felt252 = 'DYNAMOBLIN';
const BOMBOBLIN: felt252 = 'BOMBOBLIN';

mod errors {
    const ROLE_NOT_VALID: felt252 = 'Role: not valid';
}

#[derive(Copy, Drop, Serde, PartialEq, Introspection)]
enum Role {
    None,
    Knight,
    Bowman,
    Pawn,
    Torchoblin,
    Dynamoblin,
    Bomboblin,
}

#[generate_trait]
impl RoleImpl of RoleTrait {
    #[inline(always)]
    fn health(self: Role) -> u8 {
        match self {
            Role::None => 0,
            Role::Knight => KnightImpl::health(),
            Role::Bowman => BowmanImpl::health(),
            Role::Pawn => PawnImpl::health(),
            Role::Torchoblin => TorchoblinImpl::health(),
            Role::Dynamoblin => DynamoblinImpl::health(),
            Role::Bomboblin => BomboblinImpl::health(),
        }
    }

    #[inline(always)]
    fn attack(self: Role) -> u8 {
        match self {
            Role::None => 0,
            Role::Knight => KnightImpl::attack(),
            Role::Bowman => BowmanImpl::attack(),
            Role::Pawn => PawnImpl::attack(),
            Role::Torchoblin => TorchoblinImpl::attack(),
            Role::Dynamoblin => DynamoblinImpl::attack(),
            Role::Bomboblin => BomboblinImpl::attack(),
        }
    }
}

#[generate_trait]
impl RoleAssert of AssertTrait {
    #[inline(always)]
    fn assert_is_valid(self: Role) {
        assert(self != Role::None, errors::ROLE_NOT_VALID);
    }
}

impl RoleIntoFelt252 of core::Into<Role, felt252> {
    #[inline(always)]
    fn into(self: Role) -> felt252 {
        match self {
            Role::None => NONE,
            Role::Knight => KNIGHT,
            Role::Bowman => BOWMAN,
            Role::Pawn => PAWN,
            Role::Torchoblin => TORCHOBLIN,
            Role::Dynamoblin => DYNAMOBLIN,
            Role::Bomboblin => BOMBOBLIN,
        }
    }
}

impl RoleIntoU8 of core::Into<Role, u8> {
    #[inline(always)]
    fn into(self: Role) -> u8 {
        match self {
            Role::None => 0,
            Role::Knight => 1,
            Role::Bowman => 2,
            Role::Pawn => 3,
            Role::Torchoblin => 4,
            Role::Dynamoblin => 5,
            Role::Bomboblin => 6,
        }
    }
}

impl Felt252IntoRole of core::Into<felt252, Role> {
    #[inline(always)]
    fn into(self: felt252) -> Role {
        if self == KNIGHT {
            Role::Knight
        } else if self == BOWMAN {
            Role::Bowman
        } else if self == PAWN {
            Role::Pawn
        } else if self == TORCHOBLIN {
            Role::Torchoblin
        } else if self == DYNAMOBLIN {
            Role::Dynamoblin
        } else if self == BOMBOBLIN {
            Role::Bomboblin
        } else {
            Role::None
        }
    }
}

impl U8IntoRole of core::Into<u8, Role> {
    #[inline(always)]
    fn into(self: u8) -> Role {
        if self == 1 {
            Role::Knight
        } else if self == 2 {
            Role::Bowman
        } else if self == 3 {
            Role::Pawn
        } else if self == 4 {
            Role::Torchoblin
        } else if self == 5 {
            Role::Dynamoblin
        } else if self == 6 {
            Role::Bomboblin
        } else {
            Role::None
        }
    }
}

impl RolePrint of PrintTrait<Role> {
    #[inline(always)]
    fn print(self: Role) {
        let felt: felt252 = self.into();
        felt.print();
    }
}
