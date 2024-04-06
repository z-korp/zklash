// Internal imports

use zklash::types::phase::Phase;

trait RoleTrait {
    fn health(phase: Phase, level: u8) -> u8;
    fn attack(phase: Phase, level: u8) -> u8;
    fn absorb(phase: Phase, level: u8) -> u8;
    fn damage(phase: Phase, level: u8) -> u8;
    fn stun(phase: Phase, level: u8) -> u8;
    fn next_health(phase: Phase, level: u8) -> u8;
    fn next_attack(phase: Phase, level: u8) -> u8;
    fn next_absorb(phase: Phase, level: u8) -> u8;
}
