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
use zklash::types::phase::Phase;

// Constants

const ROLE_COUNT: u8 = 3;
const NONE: felt252 = 'NONE';
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
    fn health(self: Role, phase: Phase, level: u8) -> u8 {
        match self {
            Role::None => 0,
            Role::Knight => KnightImpl::health(phase, level),
            Role::Bowman => BowmanImpl::health(phase, level),
            Role::Pawn => PawnImpl::health(phase, level),
            Role::Torchoblin => TorchoblinImpl::health(phase, level),
            Role::Dynamoblin => DynamoblinImpl::health(phase, level),
            Role::Bomboblin => BomboblinImpl::health(phase, level),
        }
    }

    #[inline(always)]
    fn attack(self: Role, phase: Phase, level: u8) -> u8 {
        match self {
            Role::None => 0,
            Role::Knight => KnightImpl::attack(phase, level),
            Role::Bowman => BowmanImpl::attack(phase, level),
            Role::Pawn => PawnImpl::attack(phase, level),
            Role::Torchoblin => TorchoblinImpl::attack(phase, level),
            Role::Dynamoblin => DynamoblinImpl::attack(phase, level),
            Role::Bomboblin => BomboblinImpl::attack(phase, level),
        }
    }

    #[inline(always)]
    fn absorb(self: Role, phase: Phase, level: u8) -> u8 {
        match self {
            Role::None => 0,
            Role::Knight => KnightImpl::absorb(phase, level),
            Role::Bowman => BowmanImpl::absorb(phase, level),
            Role::Pawn => PawnImpl::absorb(phase, level),
            Role::Torchoblin => TorchoblinImpl::absorb(phase, level),
            Role::Dynamoblin => DynamoblinImpl::absorb(phase, level),
            Role::Bomboblin => BomboblinImpl::absorb(phase, level),
        }
    }

    #[inline(always)]
    fn damage(self: Role, phase: Phase, level: u8) -> u8 {
        match self {
            Role::None => 0,
            Role::Knight => KnightImpl::damage(phase, level),
            Role::Bowman => BowmanImpl::damage(phase, level),
            Role::Pawn => PawnImpl::damage(phase, level),
            Role::Torchoblin => TorchoblinImpl::damage(phase, level),
            Role::Dynamoblin => DynamoblinImpl::damage(phase, level),
            Role::Bomboblin => BomboblinImpl::damage(phase, level),
        }
    }

    #[inline(always)]
    fn stun(self: Role, phase: Phase, level: u8) -> u8 {
        match self {
            Role::None => 0,
            Role::Knight => KnightImpl::stun(phase, level),
            Role::Bowman => BowmanImpl::stun(phase, level),
            Role::Pawn => PawnImpl::stun(phase, level),
            Role::Torchoblin => TorchoblinImpl::stun(phase, level),
            Role::Dynamoblin => DynamoblinImpl::stun(phase, level),
            Role::Bomboblin => BomboblinImpl::stun(phase, level),
        }
    }

    #[inline(always)]
    fn next_health(self: Role, phase: Phase, level: u8) -> u8 {
        match self {
            Role::None => 0,
            Role::Knight => KnightImpl::next_health(phase, level),
            Role::Bowman => BowmanImpl::next_health(phase, level),
            Role::Pawn => PawnImpl::next_health(phase, level),
            Role::Torchoblin => TorchoblinImpl::next_health(phase, level),
            Role::Dynamoblin => DynamoblinImpl::next_health(phase, level),
            Role::Bomboblin => BomboblinImpl::next_health(phase, level),
        }
    }

    #[inline(always)]
    fn next_attack(self: Role, phase: Phase, level: u8) -> u8 {
        match self {
            Role::None => 0,
            Role::Knight => KnightImpl::next_attack(phase, level),
            Role::Bowman => BowmanImpl::next_attack(phase, level),
            Role::Pawn => PawnImpl::next_attack(phase, level),
            Role::Torchoblin => TorchoblinImpl::next_attack(phase, level),
            Role::Dynamoblin => DynamoblinImpl::next_attack(phase, level),
            Role::Bomboblin => BomboblinImpl::next_attack(phase, level),
        }
    }

    #[inline(always)]
    fn next_absorb(self: Role, phase: Phase, level: u8) -> u8 {
        match self {
            Role::None => 0,
            Role::Knight => KnightImpl::next_absorb(phase, level),
            Role::Bowman => BowmanImpl::next_absorb(phase, level),
            Role::Pawn => PawnImpl::next_absorb(phase, level),
            Role::Torchoblin => TorchoblinImpl::next_absorb(phase, level),
            Role::Dynamoblin => DynamoblinImpl::next_absorb(phase, level),
            Role::Bomboblin => BomboblinImpl::next_absorb(phase, level),
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
