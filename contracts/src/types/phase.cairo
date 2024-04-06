// Core imports

use core::debug::PrintTrait;

// Constants

const NONE: felt252 = 0;
const ON_HIRE: felt252 = 'ON_HIRE';
const ON_EQUIP: felt252 = 'ON_EQUIP';
const ON_DISPATCH: felt252 = 'ON_DISPATCH';
const ON_DAMAGE: felt252 = 'ON_DAMAGE';
const ON_DEATH: felt252 = 'ON_DEATH';

mod errors {
    const PHASE_NOT_VALID: felt252 = 'Phase: not valid';
}

#[derive(Copy, Drop, Serde, PartialEq, Introspection)]
enum Phase {
    None,
    OnHire,
    OnEquip,
    OnDispatch,
    OnFight,
    OnDeath,
}

#[generate_trait]
impl PhaseAssert of AssertTrait {
    #[inline(always)]
    fn assert_is_valid(self: Phase) {
        assert(self != Phase::None, errors::PHASE_NOT_VALID);
    }
}

impl PhaseIntoFelt252 of core::Into<Phase, felt252> {
    #[inline(always)]
    fn into(self: Phase) -> felt252 {
        match self {
            Phase::None => NONE,
            Phase::OnHire => ON_HIRE,
            Phase::OnEquip => ON_EQUIP,
            Phase::OnDispatch => ON_DISPATCH,
            Phase::OnFight => ON_DAMAGE,
            Phase::OnDeath => ON_DEATH,
        }
    }
}

impl PhaseIntoU8 of core::Into<Phase, u8> {
    #[inline(always)]
    fn into(self: Phase) -> u8 {
        match self {
            Phase::None => 0,
            Phase::OnHire => 1,
            Phase::OnEquip => 2,
            Phase::OnDispatch => 3,
            Phase::OnFight => 4,
            Phase::OnDeath => 5,
        }
    }
}

impl Felt252IntoPhase of core::Into<felt252, Phase> {
    #[inline(always)]
    fn into(self: felt252) -> Phase {
        if self == ON_HIRE {
            Phase::OnHire
        } else if self == ON_EQUIP {
            Phase::OnEquip
        } else if self == ON_DISPATCH {
            Phase::OnDispatch
        } else if self == ON_DAMAGE {
            Phase::OnFight
        } else if self == ON_DEATH {
            Phase::OnDeath
        } else {
            Phase::None
        }
    }
}

impl U8IntoPhase of core::Into<u8, Phase> {
    #[inline(always)]
    fn into(self: u8) -> Phase {
        if self == 1 {
            Phase::OnHire
        } else if self == 2 {
            Phase::OnEquip
        } else if self == 3 {
            Phase::OnDispatch
        } else if self == 4 {
            Phase::OnFight
        } else if self == 5 {
            Phase::OnDeath
        } else {
            Phase::None
        }
    }
}

impl PhasePrint of PrintTrait<Phase> {
    #[inline(always)]
    fn print(self: Phase) {
        let felt: felt252 = self.into();
        felt.print();
    }
}
