// Core imports

use core::debug::PrintTrait;

// Internal imports

use zklash::models::character::Character;
use zklash::waves::wave_01::WaveImpl as Wave01Impl;
use zklash::waves::wave_02::WaveImpl as Wave02Impl;
use zklash::waves::wave_03::WaveImpl as Wave03Impl;
use zklash::waves::wave_04::WaveImpl as Wave04Impl;
use zklash::waves::wave_05::WaveImpl as Wave05Impl;
use zklash::waves::wave_06::WaveImpl as Wave06Impl;
use zklash::waves::wave_07::WaveImpl as Wave07Impl;
use zklash::waves::wave_08::WaveImpl as Wave08Impl;
use zklash::waves::wave_09::WaveImpl as Wave09Impl;
use zklash::waves::wave_10::WaveImpl as Wave10Impl;

// Constants

const NONE: felt252 = 'NONE';
const WAVE_01: felt252 = 'WAVE_01';
const WAVE_02: felt252 = 'WAVE_02';
const WAVE_03: felt252 = 'WAVE_03';
const WAVE_04: felt252 = 'WAVE_04';
const WAVE_05: felt252 = 'WAVE_05';
const WAVE_06: felt252 = 'WAVE_06';
const WAVE_07: felt252 = 'WAVE_07';
const WAVE_08: felt252 = 'WAVE_08';
const WAVE_09: felt252 = 'WAVE_09';
const WAVE_10: felt252 = 'WAVE_10';

mod errors {
    const WAVE_NOT_VALID: felt252 = 'Wave: not valid';
}

#[derive(Copy, Drop, Serde, PartialEq, Introspection)]
enum Wave {
    None,
    Wave01,
    Wave02,
    Wave03,
    Wave04,
    Wave05,
    Wave06,
    Wave07,
    Wave08,
    Wave09,
    Wave10,
}

#[generate_trait]
impl WaveImpl of WaveTrait {
    #[inline(always)]
    fn characters(self: Wave) -> Array<Character> {
        match self {
            Wave::None => array![],
            Wave::Wave01 => Wave01Impl::characters(),
            Wave::Wave02 => Wave02Impl::characters(),
            Wave::Wave03 => Wave03Impl::characters(),
            Wave::Wave04 => Wave04Impl::characters(),
            Wave::Wave05 => Wave05Impl::characters(),
            Wave::Wave06 => Wave06Impl::characters(),
            Wave::Wave07 => Wave07Impl::characters(),
            Wave::Wave08 => Wave08Impl::characters(),
            Wave::Wave09 => Wave09Impl::characters(),
            Wave::Wave10 => Wave10Impl::characters(),
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
            Wave::Wave03 => WAVE_03,
            Wave::Wave04 => WAVE_04,
            Wave::Wave05 => WAVE_05,
            Wave::Wave06 => WAVE_06,
            Wave::Wave07 => WAVE_07,
            Wave::Wave08 => WAVE_08,
            Wave::Wave09 => WAVE_09,
            Wave::Wave10 => WAVE_10,
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
            Wave::Wave03 => 3,
            Wave::Wave04 => 4,
            Wave::Wave05 => 5,
            Wave::Wave06 => 6,
            Wave::Wave07 => 7,
            Wave::Wave08 => 8,
            Wave::Wave09 => 9,
            Wave::Wave10 => 10,
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
        } else if self == WAVE_03 {
            Wave::Wave03
        } else if self == WAVE_04 {
            Wave::Wave04
        } else if self == WAVE_05 {
            Wave::Wave05
        } else if self == WAVE_06 {
            Wave::Wave06
        } else if self == WAVE_07 {
            Wave::Wave07
        } else if self == WAVE_08 {
            Wave::Wave08
        } else if self == WAVE_09 {
            Wave::Wave09
        } else if self == WAVE_10 {
            Wave::Wave10
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
        } else if self == 3 {
            Wave::Wave03
        } else if self == 4 {
            Wave::Wave04
        } else if self == 5 {
            Wave::Wave05
        } else if self == 6 {
            Wave::Wave06
        } else if self == 7 {
            Wave::Wave07
        } else if self == 8 {
            Wave::Wave08
        } else if self == 9 {
            Wave::Wave09
        } else if self == 10 {
            Wave::Wave10
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
