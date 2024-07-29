// Core imports

use core::debug::PrintTrait;

// Starknet imports

use starknet::ContractAddress;

// Internal imports

use zklash::constants::{LEAGUE_SIZE, LEAGUE_COUNT, LEAGUE_MIN_THRESHOLD};
use zklash::models::index::{Squad, Slot, League};
use zklash::models::squad::{SquadTrait, SquadAssert};
use zklash::models::slot::{SlotTrait};

// Errors

mod errors {
    const LEAGUE_NOT_SUBSCRIBED: felt252 = 'League: squad not subscribed';
}

#[generate_trait]
impl LeagueImpl of LeagueTrait {
    #[inline(always)]
    fn new(registry_id: u32, league_id: u8) -> League {
        League { registry_id, id: league_id, size: 0 }
    }

    #[inline(always)]
    fn compute_id(rating: u32) -> u8 {
        if rating <= LEAGUE_MIN_THRESHOLD {
            return 1;
        }
        let max_rating = LEAGUE_MIN_THRESHOLD + LEAGUE_SIZE.into() * LEAGUE_COUNT.into();
        if rating >= max_rating {
            return LEAGUE_COUNT;
        }
        let id = 1 + (rating - LEAGUE_MIN_THRESHOLD) / LEAGUE_SIZE.into();
        if id > 251 {
            251
        } else if id < 1 {
            1
        } else {
            id.try_into().unwrap()
        }
    }

    #[inline(always)]
    fn subscribe(ref self: League, ref squad: Squad) -> Slot {
        // [Check] Squad can subscribe
        SquadAssert::assert_subscribable(squad);
        // [Effect] Update
        let index = self.size;
        self.size += 1;
        squad.league_id = self.id;
        squad.index = index;
        // [Return] Corresponding slot
        SlotTrait::new(squad)
    }

    #[inline(always)]
    fn unsubscribe(ref self: League, ref squad: Squad) {
        // [Check] Squad belongs to the league
        LeagueAssert::assert_subscribed(self, squad);
        // [Effect] Update
        self.size -= 1;
        squad.league_id = 0;
        squad.index = 0;
    }

    #[inline(always)]
    fn search_squad(ref self: League, seed: felt252) -> u32 {
        let seed: u256 = seed.into();
        let index = seed % self.size.into();
        index.try_into().unwrap()
    }
}

#[generate_trait]
impl LeagueAssert of AssertTrait {
    #[inline(always)]
    fn assert_subscribed(self: League, squad: Squad) {
        assert(squad.league_id == self.id, errors::LEAGUE_NOT_SUBSCRIBED);
    }
}

#[cfg(test)]
mod tests {
    // Core imports

    use core::debug::PrintTrait;

    // Local imports

    use super::{
        League, LeagueTrait, Squad, SquadTrait, ContractAddress, LEAGUE_SIZE, LEAGUE_COUNT,
        LEAGUE_MIN_THRESHOLD
    };

    // Constants

    const REGISTRY_ID: u32 = 1;
    const LEAGUE_ID: u8 = 1;
    const SQUAD_ID: u32 = 1;
    const DEFAULT_LEVEL: u8 = 1;
    const DEFAULT_SIZE: u8 = 1;

    #[test]
    fn test_new() {
        let league = LeagueTrait::new(REGISTRY_ID, LEAGUE_ID);
        assert_eq!(league.registry_id, REGISTRY_ID);
        assert_eq!(league.id, LEAGUE_ID);
        assert_eq!(league.size, 0);
    }

    #[test]
    fn test_compute_id() {
        let rating = LEAGUE_MIN_THRESHOLD - 1;
        let league_id = LeagueTrait::compute_id(rating);
        assert_eq!(league_id, 1);
    }

    #[test]
    fn test_compute_id_overflow() {
        let max_rating = LEAGUE_MIN_THRESHOLD + LEAGUE_SIZE.into() * LEAGUE_COUNT.into() + 1;
        let league_id = LeagueTrait::compute_id(max_rating);
        assert_eq!(league_id, LEAGUE_COUNT);
    }

    #[test]
    fn test_compute_id_underflow() {
        let rating = 0;
        let league_id = LeagueTrait::compute_id(rating);
        assert_eq!(league_id, 1);
    }

    #[test]
    fn test_subscribe_once() {
        let mut squad = SquadTrait::new(REGISTRY_ID, SQUAD_ID, DEFAULT_LEVEL, DEFAULT_SIZE, 'test');
        let mut league = LeagueTrait::new(REGISTRY_ID, LEAGUE_ID);
        let slot = LeagueTrait::subscribe(ref league, ref squad);
        // [Assert] League
        assert_eq!(league.size, 1);
        // [Assert] Squad
        assert_eq!(squad.league_id, LEAGUE_ID);
        assert_eq!(squad.index, 0);
        // [Assert] Slot
        assert_eq!(slot.squad_id, squad.id);
    }

    #[test]
    #[should_panic(expected: ('Squad: not subscribable',))]
    fn test_subscribe_twice() {
        let mut squad = SquadTrait::new(REGISTRY_ID, SQUAD_ID, DEFAULT_LEVEL, DEFAULT_SIZE, 'test');
        let mut league = LeagueTrait::new(REGISTRY_ID, LEAGUE_ID);
        LeagueTrait::subscribe(ref league, ref squad);
        LeagueTrait::subscribe(ref league, ref squad);
    }

    #[test]
    fn test_unsubscribe_once() {
        let mut squad = SquadTrait::new(REGISTRY_ID, SQUAD_ID, DEFAULT_LEVEL, DEFAULT_SIZE, 'test');
        let mut league = LeagueTrait::new(REGISTRY_ID, LEAGUE_ID);
        LeagueTrait::subscribe(ref league, ref squad);
        LeagueTrait::unsubscribe(ref league, ref squad);
        // [Assert] League
        assert_eq!(league.size, 0);
        // [Assert] Squad
        assert_eq!(squad.league_id, 0);
        assert_eq!(squad.index, 0);
    }

    #[test]
    #[should_panic(expected: ('League: squad not subscribed',))]
    fn test_unsubscribe_twice() {
        let mut squad = SquadTrait::new(REGISTRY_ID, SQUAD_ID, DEFAULT_LEVEL, DEFAULT_SIZE, 'test');
        let mut league = LeagueTrait::new(REGISTRY_ID, LEAGUE_ID);
        LeagueTrait::subscribe(ref league, ref squad);
        LeagueTrait::unsubscribe(ref league, ref squad);
        LeagueTrait::unsubscribe(ref league, ref squad);
    }
}
