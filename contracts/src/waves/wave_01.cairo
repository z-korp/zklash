// Core imports

use core::debug::PrintTrait;

// Internal imports

use zklash::models::character::{Character, CharacterTrait};
use zklash::types::role::Role;
use zklash::waves::interface::WaveTrait;

impl WaveImpl of WaveTrait {
    #[inline(always)]
    fn characters() -> Array<Character> {
        array![
            CharacterTrait::from(Role::Torchoblin, 1), CharacterTrait::from(Role::Dynamoblin, 1),
        ]
    }
}
