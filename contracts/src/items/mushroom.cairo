// Core imports

use core::debug::PrintTrait;

// Internal imports

use zklash::items::interface::{ItemTrait, Item, Phase, Size};

impl ItemImpl of ItemTrait {
    #[inline(always)]
    fn health(phase: Phase, size: Size) -> u8 {
        match phase {
            Phase::OnEquip => {
                match size {
                    Size::Small => 1,
                    Size::Medium => 2,
                    Size::Large => 3,
                    _ => 0,
                }
            },
            _ => 0,
        }
    }

    #[inline(always)]
    fn attack(phase: Phase, size: Size) -> u8 {
        0
    }

    #[inline(always)]
    fn damage(phase: Phase, size: Size) -> u8 {
        0
    }

    #[inline(always)]
    fn absorb(phase: Phase, size: Size) -> u8 {
        0
    }

    #[inline(always)]
    fn usage(phase: Phase, size: Size) -> Item {
        match size {
            Size::Small => Item::MushroomSmall,
            Size::Medium => Item::MushroomMedium,
            Size::Large => Item::MushroomLarge,
            _ => Item::None,
        }
    }
}
