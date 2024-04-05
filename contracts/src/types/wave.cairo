// Core imports

use core::debug::PrintTrait;

// Internal imports

use zklash::models::character::Character;
use zklash::waves::wave_01::{WaveImpl as Wave01Impl};
use zklash::waves::wave_02::{WaveImpl as Wave02Impl};

// Constants

const NONE: felt252 = 0;
const WAVE_01: felt252 = 'WAVE_01';
const WAVE_02: felt252 = 'WAVE_02';

mod errors {
    const WAVE_NOT_VALID: felt252 = 'Wave: not valid';
}

#[derive(Copy, Drop, Serde, PartialEq, Introspection)]
enum Wave {
    None,
    Wave01,
    Wave02,
}

#[generate_trait]
impl WaveImpl of WaveTrait {
    #[inline(always)]
    fn characters(self: Wave) -> Array<Character> {
        match self {
            Wave::None => array![],
            Wave::Wave01 => Wave01Impl::characters(),
            Wave::Wave02 => Wave02Impl::characters(),
        }
    }
}

#[generate_trait]
impl WaveAssert of AssertTrait {
    #[inline(always)]
    fn assert_is_valid(self: Wave) {
        assert(self != Wave::None, errors::WAVE_NOT_VALID);
    }
}

impl WaveIntoFelt252 of core::Into<Wave, felt252> {
    #[inline(always)]
    fn into(self: Wave) -> felt252 {
        match self {
            Wave::None => NONE,
            Wave::Wave01 => WAVE_01,
            Wave::Wave02 => WAVE_02,
        }
    }
}

impl WaveIntoU8 of core::Into<Wave, u8> {
    #[inline(always)]
    fn into(self: Wave) -> u8 {
        match self {
            Wave::None => 0,
            Wave::Wave01 => 1,
            Wave::Wave02 => 2,
        }
    }
}

impl Felt252IntoWave of core::Into<felt252, Wave> {
    #[inline(always)]
    fn into(self: felt252) -> Wave {
        if self == WAVE_01 {
            Wave::Wave01
        } else if self == WAVE_02 {
            Wave::Wave02
        } else {
            Wave::None
        }
    }
}

impl U8IntoWave of core::Into<u8, Wave> {
    #[inline(always)]
    fn into(self: u8) -> Wave {
        if self == 1 {
            Wave::Wave01
        } else if self == 2 {
            Wave::Wave02
        } else {
            Wave::None
        }
    }
}

impl WavePrint of PrintTrait<Wave> {
    #[inline(always)]
    fn print(self: Wave) {
        let felt: felt252 = self.into();
        felt.print();
    }
}
