// Core imports

use core::debug::PrintTrait;

// Internal imports

use zklash::roles::interface::RoleTrait;

impl RoleImpl of RoleTrait {
    #[inline(always)]
    fn health() -> u8 {
        3
    }

    #[inline(always)]
    fn attack() -> u8 {
        1
    }
}
