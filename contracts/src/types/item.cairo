// Core imports

use core::debug::PrintTrait;

// Constants

const ITEM_COUNT: u8 = 4;
const NONE: felt252 = 0;
const MUSHROOM: felt252 = 'MUSHROOM';
const ROCK: felt252 = 'ROCK';
const BUSH: felt252 = 'BUSH';
const PUMPKIN: felt252 = 'PUMPKIN';

mod errors {
    const ITEM_NOT_VALID: felt252 = 'Item: not valid';
}

#[derive(Copy, Drop, Serde, PartialEq, Introspection)]
enum Item {
    None,
    Mushroom,
    Rock,
    Bush,
    Pumpkin,
}

#[generate_trait]
impl ItemImpl of ItemTrait {}

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
            Item::Mushroom => MUSHROOM,
            Item::Rock => ROCK,
            Item::Bush => BUSH,
            Item::Pumpkin => PUMPKIN,
        }
    }
}

impl ItemIntoU8 of core::Into<Item, u8> {
    #[inline(always)]
    fn into(self: Item) -> u8 {
        match self {
            Item::None => 0,
            Item::Mushroom => 1,
            Item::Rock => 2,
            Item::Bush => 3,
            Item::Pumpkin => 4,
        }
    }
}

impl Felt252IntoItem of core::Into<felt252, Item> {
    #[inline(always)]
    fn into(self: felt252) -> Item {
        if self == MUSHROOM {
            Item::Mushroom
        } else if self == ROCK {
            Item::Rock
        } else if self == BUSH {
            Item::Bush
        } else if self == PUMPKIN {
            Item::Pumpkin
        } else {
            Item::None
        }
    }
}

impl U8IntoItem of core::Into<u8, Item> {
    #[inline(always)]
    fn into(self: u8) -> Item {
        if self == 1 {
            Item::Mushroom
        } else if self == 2 {
            Item::Rock
        } else if self == 3 {
            Item::Bush
        } else if self == 4 {
            Item::Pumpkin
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
