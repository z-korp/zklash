// Core imports

use core::debug::PrintTrait;

// Internal imports

use zklash::models::foe::Foe;
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
    One,
    Two,
    Three,
    Four,
    Five,
    Six,
    Seven,
    Eight,
    Nine,
    Ten,
}

#[generate_trait]
impl WaveImpl of WaveTrait {
    #[inline(always)]
    fn foes(self: Wave, registry_id: u32, squad_id: u32) -> Array<Foe> {
        match self {
            Wave::None => array![],
            Wave::One => Wave01Impl::foes(registry_id, squad_id),
            Wave::Two => Wave02Impl::foes(registry_id, squad_id),
            Wave::Three => Wave03Impl::foes(registry_id, squad_id),
            Wave::Four => Wave04Impl::foes(registry_id, squad_id),
            Wave::Five => Wave05Impl::foes(registry_id, squad_id),
            Wave::Six => Wave06Impl::foes(registry_id, squad_id),
            Wave::Seven => Wave07Impl::foes(registry_id, squad_id),
            Wave::Eight => Wave08Impl::foes(registry_id, squad_id),
            Wave::Nine => Wave09Impl::foes(registry_id, squad_id),
            Wave::Ten => Wave10Impl::foes(registry_id, squad_id),
        }
    }

    #[inline(always)]
    fn attributes(self: Wave) -> (u8, u8) {
        match self {
            Wave::None => (0, 0),
            Wave::One => (Wave01Impl::level(), Wave01Impl::size()),
            Wave::Two => (Wave02Impl::level(), Wave02Impl::size()),
            Wave::Three => (Wave03Impl::level(), Wave03Impl::size()),
            Wave::Four => (Wave04Impl::level(), Wave04Impl::size()),
            Wave::Five => (Wave05Impl::level(), Wave05Impl::size()),
            Wave::Six => (Wave06Impl::level(), Wave06Impl::size()),
            Wave::Seven => (Wave07Impl::level(), Wave07Impl::size()),
            Wave::Eight => (Wave08Impl::level(), Wave08Impl::size()),
            Wave::Nine => (Wave09Impl::level(), Wave09Impl::size()),
            Wave::Ten => (Wave10Impl::level(), Wave10Impl::size()),
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
            Wave::One => WAVE_01,
            Wave::Two => WAVE_02,
            Wave::Three => WAVE_03,
            Wave::Four => WAVE_04,
            Wave::Five => WAVE_05,
            Wave::Six => WAVE_06,
            Wave::Seven => WAVE_07,
            Wave::Eight => WAVE_08,
            Wave::Nine => WAVE_09,
            Wave::Ten => WAVE_10,
        }
    }
}

impl WaveIntoU8 of core::Into<Wave, u8> {
    #[inline(always)]
    fn into(self: Wave) -> u8 {
        match self {
            Wave::None => 0,
            Wave::One => 1,
            Wave::Two => 2,
            Wave::Three => 3,
            Wave::Four => 4,
            Wave::Five => 5,
            Wave::Six => 6,
            Wave::Seven => 7,
            Wave::Eight => 8,
            Wave::Nine => 9,
            Wave::Ten => 10,
        }
    }
}

impl Felt252IntoWave of core::Into<felt252, Wave> {
    #[inline(always)]
    fn into(self: felt252) -> Wave {
        if self == WAVE_01 {
            Wave::One
        } else if self == WAVE_02 {
            Wave::Two
        } else if self == WAVE_03 {
            Wave::Three
        } else if self == WAVE_04 {
            Wave::Four
        } else if self == WAVE_05 {
            Wave::Five
        } else if self == WAVE_06 {
            Wave::Six
        } else if self == WAVE_07 {
            Wave::Seven
        } else if self == WAVE_08 {
            Wave::Eight
        } else if self == WAVE_09 {
            Wave::Nine
        } else if self == WAVE_10 {
            Wave::Ten
        } else {
            Wave::None
        }
    }
}

impl U8IntoWave of core::Into<u8, Wave> {
    #[inline(always)]
    fn into(self: u8) -> Wave {
        if self == 1 {
            Wave::One
        } else if self == 2 {
            Wave::Two
        } else if self == 3 {
            Wave::Three
        } else if self == 4 {
            Wave::Four
        } else if self == 5 {
            Wave::Five
        } else if self == 6 {
            Wave::Six
        } else if self == 7 {
            Wave::Seven
        } else if self == 8 {
            Wave::Eight
        } else if self == 9 {
            Wave::Nine
        } else if self == 10 {
            Wave::Ten
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
