// Core imports

use core::debug::PrintTrait;

// Internal imports

use zklash::roles::interface::{RoleTrait, Phase};

impl RoleImpl of RoleTrait {
    #[inline(always)]
    fn health(phase: Phase, level: u8) -> u8 {
        match phase {
            Phase::OnHire => { 2 },
            _ => 0,
        }
    }

    #[inline(always)]
    fn attack(phase: Phase, level: u8) -> u8 {
        match phase {
            Phase::OnHire => { 3 },
            _ => 0,
        }
    }

    #[inline(always)]
    fn absorb(phase: Phase, level: u8) -> u8 {
        0
    }

    #[inline(always)]
    fn damage(phase: Phase, level: u8) -> u8 {
        match phase {
            Phase::OnDeath => { level },
            _ => 0,
        }
    }

    #[inline(always)]
    fn stun(phase: Phase, level: u8) -> u8 {
        0
    }

    #[inline(always)]
    fn next_health(phase: Phase, level: u8) -> u8 {
        0
    }

    #[inline(always)]
    fn next_attack(phase: Phase, level: u8) -> u8 {
        0
    }

    #[inline(always)]
    fn next_absorb(phase: Phase, level: u8) -> u8 {
        0
    }
}
