// Core imports

use core::debug::PrintTrait;

// Internal imports

use zklash::items::bush::{ItemImpl as BushImpl};
use zklash::items::mushroom::{ItemImpl as MushroomImpl};
use zklash::items::pumpkin::{ItemImpl as PumpkinImpl};
use zklash::items::rock::{ItemImpl as RockImpl};
use zklash::types::phase::Phase;
use zklash::types::size::Size;

// Constants

const ITEM_COUNT: u8 = 11;
const NONE: felt252 = 0;
const MUSHROOM_SMALL: felt252 = 'MUSHROOM_SMALL';
const MUSHROOM_MEDIUM: felt252 = 'MUSHROOM_MEDIUM';
const MUSHROOM_LARGE: felt252 = 'MUSHROOM_LARGE';
const ROCK_SMALL: felt252 = 'ROCK_SMALL';
const ROCK_MEDIUM: felt252 = 'ROCK_MEDIUM';
const ROCK_LARGE: felt252 = 'ROCK_LARGE';
const BUSH_SMALL: felt252 = 'BUSH_SMALL';
const BUSH_MEDIUM: felt252 = 'BUSH_MEDIUM';
const BUSH_LARGE: felt252 = 'BUSH_LARGE';
const PUMPKIN_SMALL: felt252 = 'PUMPKIN_SMALL';
const PUMPKIN_MEDIUM: felt252 = 'PUMPKIN_MEDIUM';

mod errors {
    const ITEM_NOT_VALID: felt252 = 'Item: not valid';
}

#[derive(Copy, Drop, Serde, PartialEq, Introspection)]
enum Item {
    None,
    MushroomSmall,
    MushroomMedium,
    MushroomLarge,
    RockSmall,
    RockMedium,
    RockLarge,
    BushSmall,
    BushMedium,
    BushLarge,
    PumpkinSmall,
    PumpkinMedium,
}

#[generate_trait]
impl ItemImpl of ItemTrait {
    #[inline(always)]
    fn health(self: Item, phase: Phase) -> u8 {
        match self {
            Item::None => 0,
            Item::MushroomSmall => MushroomImpl::health(phase, Size::Small),
            Item::MushroomMedium => MushroomImpl::health(phase, Size::Medium),
            Item::MushroomLarge => MushroomImpl::health(phase, Size::Large),
            Item::RockSmall => RockImpl::health(phase, Size::Small),
            Item::RockMedium => RockImpl::health(phase, Size::Medium),
            Item::RockLarge => RockImpl::health(phase, Size::Large),
            Item::BushSmall => BushImpl::health(phase, Size::Small),
            Item::BushMedium => BushImpl::health(phase, Size::Medium),
            Item::BushLarge => BushImpl::health(phase, Size::Large),
            Item::PumpkinSmall => PumpkinImpl::health(phase, Size::Small),
            Item::PumpkinMedium => PumpkinImpl::health(phase, Size::Medium),
        }
    }

    #[inline(always)]
    fn attack(self: Item, phase: Phase) -> u8 {
        match self {
            Item::None => 0,
            Item::MushroomSmall => MushroomImpl::attack(phase, Size::Small),
            Item::MushroomMedium => MushroomImpl::attack(phase, Size::Medium),
            Item::MushroomLarge => MushroomImpl::attack(phase, Size::Large),
            Item::RockSmall => RockImpl::attack(phase, Size::Small),
            Item::RockMedium => RockImpl::attack(phase, Size::Medium),
            Item::RockLarge => RockImpl::attack(phase, Size::Large),
            Item::BushSmall => BushImpl::attack(phase, Size::Small),
            Item::BushMedium => BushImpl::attack(phase, Size::Medium),
            Item::BushLarge => BushImpl::attack(phase, Size::Large),
            Item::PumpkinSmall => PumpkinImpl::attack(phase, Size::Small),
            Item::PumpkinMedium => PumpkinImpl::attack(phase, Size::Medium),
        }
    }

    #[inline(always)]
    fn damage(self: Item, phase: Phase) -> u8 {
        match self {
            Item::None => 0,
            Item::MushroomSmall => MushroomImpl::damage(phase, Size::Small),
            Item::MushroomMedium => MushroomImpl::damage(phase, Size::Medium),
            Item::MushroomLarge => MushroomImpl::damage(phase, Size::Large),
            Item::RockSmall => RockImpl::damage(phase, Size::Small),
            Item::RockMedium => RockImpl::damage(phase, Size::Medium),
            Item::RockLarge => RockImpl::damage(phase, Size::Large),
            Item::BushSmall => BushImpl::damage(phase, Size::Small),
            Item::BushMedium => BushImpl::damage(phase, Size::Medium),
            Item::BushLarge => BushImpl::damage(phase, Size::Large),
            Item::PumpkinSmall => PumpkinImpl::damage(phase, Size::Small),
            Item::PumpkinMedium => PumpkinImpl::damage(phase, Size::Medium),
        }
    }

    #[inline(always)]
    fn absorb(self: Item, phase: Phase) -> u8 {
        match self {
            Item::None => 0,
            Item::MushroomSmall => MushroomImpl::absorb(phase, Size::Small),
            Item::MushroomMedium => MushroomImpl::absorb(phase, Size::Medium),
            Item::MushroomLarge => MushroomImpl::absorb(phase, Size::Large),
            Item::RockSmall => RockImpl::absorb(phase, Size::Small),
            Item::RockMedium => RockImpl::absorb(phase, Size::Medium),
            Item::RockLarge => RockImpl::absorb(phase, Size::Large),
            Item::BushSmall => BushImpl::absorb(phase, Size::Small),
            Item::BushMedium => BushImpl::absorb(phase, Size::Medium),
            Item::BushLarge => BushImpl::absorb(phase, Size::Large),
            Item::PumpkinSmall => PumpkinImpl::absorb(phase, Size::Small),
            Item::PumpkinMedium => PumpkinImpl::absorb(phase, Size::Medium),
        }
    }

    #[inline(always)]
    fn usage(self: Item, phase: Phase) -> Item {
        match self {
            Item::None => Item::None,
            Item::MushroomSmall => MushroomImpl::usage(phase, Size::Small),
            Item::MushroomMedium => MushroomImpl::usage(phase, Size::Medium),
            Item::MushroomLarge => MushroomImpl::usage(phase, Size::Large),
            Item::RockSmall => RockImpl::usage(phase, Size::Small),
            Item::RockMedium => RockImpl::usage(phase, Size::Medium),
            Item::RockLarge => RockImpl::usage(phase, Size::Large),
            Item::BushSmall => BushImpl::usage(phase, Size::Small),
            Item::BushMedium => BushImpl::usage(phase, Size::Medium),
            Item::BushLarge => BushImpl::usage(phase, Size::Large),
            Item::PumpkinSmall => PumpkinImpl::usage(phase, Size::Small),
            Item::PumpkinMedium => PumpkinImpl::usage(phase, Size::Medium),
        }
    }
}

#[generate_trait]
impl ItemAssert of AssertTrait {
    #[inline(always)]
    fn assert_is_valid(self: Item) {
        assert(self != Item::None, errors::ITEM_NOT_VALID);
    }
}

impl ItemIntoFelt252 of core::Into<Item, felt252> {
    #[inline(always)]
    fn into(self: Item) -> felt252 {
        match self {
            Item::None => NONE,
            Item::MushroomSmall => MUSHROOM_SMALL,
            Item::MushroomMedium => MUSHROOM_MEDIUM,
            Item::MushroomLarge => MUSHROOM_LARGE,
            Item::RockSmall => ROCK_SMALL,
            Item::RockMedium => ROCK_MEDIUM,
            Item::RockLarge => ROCK_LARGE,
            Item::BushSmall => BUSH_SMALL,
            Item::BushMedium => BUSH_MEDIUM,
            Item::BushLarge => BUSH_LARGE,
            Item::PumpkinSmall => PUMPKIN_SMALL,
            Item::PumpkinMedium => PUMPKIN_MEDIUM,
        }
    }
}

impl ItemIntoU8 of core::Into<Item, u8> {
    #[inline(always)]
    fn into(self: Item) -> u8 {
        match self {
            Item::None => 0,
            Item::MushroomSmall => 1,
            Item::MushroomMedium => 2,
            Item::MushroomLarge => 3,
            Item::RockSmall => 4,
            Item::RockMedium => 5,
            Item::RockLarge => 6,
            Item::BushSmall => 7,
            Item::BushMedium => 8,
            Item::BushLarge => 9,
            Item::PumpkinSmall => 10,
            Item::PumpkinMedium => 11,
        }
    }
}

impl Felt252IntoItem of core::Into<felt252, Item> {
    #[inline(always)]
    fn into(self: felt252) -> Item {
        if self == MUSHROOM_SMALL {
            Item::MushroomSmall
        } else if self == MUSHROOM_MEDIUM {
            Item::MushroomMedium
        } else if self == MUSHROOM_LARGE {
            Item::MushroomLarge
        } else if self == ROCK_SMALL {
            Item::RockSmall
        } else if self == ROCK_MEDIUM {
            Item::RockMedium
        } else if self == ROCK_LARGE {
            Item::RockLarge
        } else if self == BUSH_SMALL {
            Item::BushSmall
        } else if self == BUSH_MEDIUM {
            Item::BushMedium
        } else if self == BUSH_LARGE {
            Item::BushLarge
        } else if self == PUMPKIN_SMALL {
            Item::PumpkinSmall
        } else if self == PUMPKIN_MEDIUM {
            Item::PumpkinMedium
        } else {
            Item::None
        }
    }
}

impl U8IntoItem of core::Into<u8, Item> {
    #[inline(always)]
    fn into(self: u8) -> Item {
        if self == 1 {
            Item::MushroomSmall
        } else if self == 2 {
            Item::MushroomMedium
        } else if self == 3 {
            Item::MushroomLarge
        } else if self == 4 {
            Item::RockSmall
        } else if self == 5 {
            Item::RockMedium
        } else if self == 6 {
            Item::RockLarge
        } else if self == 7 {
            Item::BushSmall
        } else if self == 8 {
            Item::BushMedium
        } else if self == 9 {
            Item::BushLarge
        } else if self == 10 {
            Item::PumpkinSmall
        } else if self == 11 {
            Item::PumpkinMedium
        } else {
            Item::None
        }
    }
}

impl ItemPrint of PrintTrait<Item> {
    #[inline(always)]
    fn print(self: Item) {
        let felt: felt252 = self.into();
        felt.print();
    }
}
