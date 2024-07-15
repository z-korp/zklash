// Internal imports

use zklash::types::item::Item;
use zklash::types::phase::Phase;
use zklash::types::size::Size;

trait ItemTrait {
    fn health(phase: Phase, size: Size) -> u8;
    fn attack(phase: Phase, size: Size) -> u8;
    fn damage(phase: Phase, size: Size) -> u8;
    fn absorb(phase: Phase, size: Size) -> u8;
    fn usage(phase: Phase, size: Size) -> Item;
    fn cost(size: Size) -> u8;
}
