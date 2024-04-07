// Core imports

use core::debug::PrintTrait;

// Internal imports

use zklash::models::character::{Character, CharacterTrait};
use zklash::types::role::Role;
use zklash::types::item::Item;
use zklash::waves::interface::WaveTrait;

impl WaveImpl of WaveTrait {
    #[inline(always)]
    fn characters() -> Array<Character> {
        array![
            CharacterTrait::from(201, Role::Bomboblin, 3, Item::BushLarge),
            CharacterTrait::from(202, Role::Torchoblin, 3, Item::MushroomMedium),
            CharacterTrait::from(203, Role::Dynamoblin, 3, Item::RockMedium),
            CharacterTrait::from(204, Role::Dynamoblin, 3, Item::PumpkinMedium),
        ]
    }
}
