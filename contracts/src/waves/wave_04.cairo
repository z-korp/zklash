// Core imports

use core::debug::PrintTrait;

// Internal imports

use zklash::models::foe::{Foe, FoeTrait};
use zklash::types::role::Role;
use zklash::types::item::Item;
use zklash::waves::interface::WaveTrait;

impl WaveImpl of WaveTrait {
    #[inline(always)]
    fn foes(registry_id: u32, squad_id: u32) -> Array<Foe> {
        array![
            FoeTrait::new(registry_id, squad_id, 1, Role::Bomboblin, 2, Item::None),
            FoeTrait::new(registry_id, squad_id, 2, Role::Torchoblin, 2, Item::None),
            FoeTrait::new(registry_id, squad_id, 3, Role::Dynamoblin, 2, Item::None),
        ]
    }

    #[inline(always)]
    fn level() -> u8 {
        4
    }

    #[inline(always)]
    fn size() -> u8 {
        3
    }
}
