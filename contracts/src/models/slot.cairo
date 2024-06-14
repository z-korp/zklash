// Starknet imports

use starknet::ContractAddress;

// Internal imports

use zklash::models::squad::{Squad, SquadTrait};

#[derive(Model, Copy, Drop, Serde)]
struct Slot {
    #[key]
    registry_id: u32,
    #[key]
    league_id: u8,
    #[key]
    index: u32,
    squad_id: u32,
}

#[generate_trait]
impl SlotImpl of SlotTrait {
    #[inline(always)]
    fn new(squad: Squad) -> Slot {
        Slot {
            registry_id: squad.registry_id,
            league_id: squad.league_id,
            index: squad.index,
            squad_id: squad.id,
        }
    }

    #[inline(always)]
    fn nullify(ref self: Slot) {
        self.squad_id = 0;
    }
}

#[cfg(test)]
mod tests {
    // Core imports

    use core::debug::PrintTrait;

    // Local imports

    use super::{Slot, SlotTrait, Squad, SquadTrait, ContractAddress};

    // Constants

    const SQUAD_ID: u32 = 1;
    const REGISTRY_ID: u32 = 1;
    const LEAGUE_ID: u8 = 2;
    const DEFAULT_LEVEL: u8 = 1;
    const DEFAULT_SIZE: u8 = 1;
    const INDEX: u32 = 3;

    #[test]
    fn test_new() {
        let mut squad = SquadTrait::new(REGISTRY_ID, SQUAD_ID, DEFAULT_LEVEL, DEFAULT_SIZE);
        squad.league_id = LEAGUE_ID;
        squad.index = INDEX;
        let slot = SlotTrait::new(squad);
        assert_eq!(slot.registry_id, REGISTRY_ID);
        assert_eq!(slot.league_id, LEAGUE_ID);
        assert_eq!(slot.index, INDEX);
        assert_eq!(slot.squad_id, SQUAD_ID);
    }
}
