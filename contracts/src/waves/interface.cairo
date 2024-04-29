// Internal import

use zklash::models::foe::Foe;

trait WaveTrait {
    fn foes(registry_id: u32, squad_id: u32) -> Array<Foe>;
    fn level() -> u8;
    fn size() -> u8;
}
