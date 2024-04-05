// Core imports

use core::debug::PrintTrait;

// Constants

const NONE: felt252 = 0;
const SMALL: felt252 = 'SMALL';
const MEDIUM: felt252 = 'MEDIUM';
const LARGE: felt252 = 'LARGE';

mod errors {
    const SIZE_NOT_VALID: felt252 = 'Size: not valid';
}

#[derive(Copy, Drop, Serde, PartialEq, Introspection)]
enum Size {
    None,
    Small,
    Medium,
    Large,
}

#[generate_trait]
impl SizeAssert of AssertTrait {
    #[inline(always)]
    fn assert_is_valid(self: Size) {
        assert(self != Size::None, errors::SIZE_NOT_VALID);
    }
}

impl SizeIntoFelt252 of core::Into<Size, felt252> {
    #[inline(always)]
    fn into(self: Size) -> felt252 {
        match self {
            Size::None => NONE,
            Size::Small => SMALL,
            Size::Medium => MEDIUM,
            Size::Large => LARGE,
        }
    }
}

impl SizeIntoU8 of core::Into<Size, u8> {
    #[inline(always)]
    fn into(self: Size) -> u8 {
        match self {
            Size::None => 0,
            Size::Small => 1,
            Size::Medium => 2,
            Size::Large => 3,
        }
    }
}

impl Felt252IntoSize of core::Into<felt252, Size> {
    #[inline(always)]
    fn into(self: felt252) -> Size {
        if self == SMALL {
            Size::Small
        } else if self == MEDIUM {
            Size::Medium
        } else if self == LARGE {
            Size::Large
        } else {
            Size::None
        }
    }
}

impl U8IntoSize of core::Into<u8, Size> {
    #[inline(always)]
    fn into(self: u8) -> Size {
        if self == 1 {
            Size::Small
        } else if self == 2 {
            Size::Medium
        } else if self == 3 {
            Size::Large
        } else {
            Size::None
        }
    }
}

impl SizePrint of PrintTrait<Size> {
    #[inline(always)]
    fn print(self: Size) {
        let felt: felt252 = self.into();
        felt.print();
    }
}
