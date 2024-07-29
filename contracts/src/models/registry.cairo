// Core imports

use core::poseidon::hades_permutation;

// Starknet imports

use starknet::ContractAddress;

// Internal imports

use zklash::store::{Store, StoreTrait};
use zklash::models::index::{Registry, Team, League, Squad, Slot};
use zklash::models::league::{LeagueTrait};
use zklash::models::squad::{SquadTrait, SquadAssert};
use zklash::models::slot::{SlotTrait};
use zklash::helpers::bitmap::Bitmap;

// Errors

mod errors {
    const REGISTRY_INVALID_INDEX: felt252 = 'Registry: invalid bitmap index';
    const REGISTRY_IS_EMPTY: felt252 = 'Registry: is empty';
    const REGISTRY_LEAGUE_NOT_FOUND: felt252 = 'Registry: league not found';
}

#[generate_trait]
impl RegistryImpl of RegistryTrait {
    #[inline(always)]
    fn new(id: u32) -> Registry {
        Registry { id, squad_count: 0, leagues: 0, seed: 0 }
    }

    #[inline(always)]
    fn reseed(ref self: Registry) {
        let (new_seed, _, _) = hades_permutation(self.seed, self.leagues, self.squad_count.into());
        self.seed = new_seed;
    }

    #[inline(always)]
    fn create_squad(ref self: Registry, level: u8, size: u8, name: felt252) -> Squad {
        // [Effect] Create new squad
        self.squad_count += 1;
        // [Effect] Update seed
        let (new_seed, _, _) = hades_permutation(self.seed, self.leagues, self.squad_count.into());
        self.seed = new_seed;
        // [Return] Squad
        SquadTrait::new(self.id, self.squad_count, level, size, name)
    }

    #[inline(always)]
    fn subscribe(ref self: Registry, ref league: League, ref squad: Squad) -> Slot {
        // [Effect] Subscribe to league
        let slot = league.subscribe(ref squad);
        Private::update(ref self, league.id, league.size);
        // [Effect] Update seed
        let (new_seed, _, _) = hades_permutation(self.seed, self.leagues, self.squad_count.into());
        self.seed = new_seed;
        // [Return] Slot
        slot
    }

    #[inline(always)]
    fn unsubscribe(ref self: Registry, ref league: League, ref squad: Squad) {
        // [Effect] Unsubscribe to league
        league.unsubscribe(ref squad);
        Private::update(ref self, league.id, league.size);
        // [Effect] Update seed
        let (new_seed, _, _) = hades_permutation(self.seed, self.leagues, self.squad_count.into());
        self.seed = new_seed;
    }

    #[inline(always)]
    fn search_league(self: Registry, league: League) -> u8 {
        // [Check] Registry is not empty
        RegistryAssert::assert_not_empty(self);
        // [Compute] Loop over the bitmap to find the nearest league with at least 1 squad
        match Bitmap::nearest_significant_bit(self.leagues.into(), league.id) {
            Option::Some(bit) => bit,
            Option::None => {
                panic(array![errors::REGISTRY_LEAGUE_NOT_FOUND]);
                0
            },
        }
    }
}

#[generate_trait]
impl Private of PrivateTrait {
    #[inline(always)]
    fn update(ref registry: Registry, index: u8, count: u32,) {
        let bit = Bitmap::get_bit_at(registry.leagues.into(), index.into());
        let new_bit = count != 0;
        if bit != new_bit {
            let leagues = Bitmap::set_bit_at(registry.leagues.into(), index.into(), new_bit);
            registry.leagues = leagues.try_into().expect(errors::REGISTRY_INVALID_INDEX);
        }
    }
}

#[generate_trait]
impl RegistryAssert of AssertTrait {
    #[inline(always)]
    fn assert_not_empty(registry: Registry) {
        // [Check] Registry is not empty
        assert(registry.leagues.into() > 0_u256, errors::REGISTRY_IS_EMPTY);
    }
}

#[cfg(test)]
mod tests {
    // Core imports

    use core::debug::PrintTrait;

    // Local imports

    use super::{
        Registry, RegistryTrait, PrivateTrait, League, LeagueTrait, Slot, SlotTrait, Squad,
        SquadTrait, ContractAddress
    };

    // Constants

    const FOE_NAME: felt252 = 'NAME';
    const REGISTRY_ID: u32 = 1;
    const LEAGUE_ID: u8 = 1;
    const SQUAD_ID: u32 = 1;
    const FOE_SQUAD_ID: u32 = 2;
    const DEFAULT_LEVEL: u8 = 1;
    const DEFAULT_SIZE: u8 = 1;
    const CLOSEST_LEAGUE_ID: u8 = 2;
    const TARGET_LEAGUE_ID: u8 = 100;
    const FAREST_LEAGUE_ID: u8 = 251;
    const INDEX: u8 = 3;

    #[test]
    fn test_new() {
        let registry = RegistryTrait::new(REGISTRY_ID);
        assert_eq!(registry.id, REGISTRY_ID);
        assert_eq!(registry.leagues, 0);
    }

    #[test]
    fn test_subscribe() {
        let mut registry = RegistryTrait::new(REGISTRY_ID);
        let mut league = LeagueTrait::new(REGISTRY_ID, LEAGUE_ID);
        let mut squad = SquadTrait::new(REGISTRY_ID, SQUAD_ID, DEFAULT_LEVEL, DEFAULT_SIZE, 'test');
        registry.subscribe(ref league, ref squad);
        // [Assert] Registry
        assert(registry.leagues.into() > 0_u256, 'Registry: wrong leagues value');
    }

    #[test]
    fn test_unsubscribe() {
        let mut registry = RegistryTrait::new(REGISTRY_ID);
        let mut league = LeagueTrait::new(REGISTRY_ID, LEAGUE_ID);
        let mut squad = SquadTrait::new(REGISTRY_ID, SQUAD_ID, DEFAULT_LEVEL, DEFAULT_SIZE, 'test');
        registry.subscribe(ref league, ref squad);
        registry.unsubscribe(ref league, ref squad);
        // [Assert] Registry
        assert_eq!(registry.leagues, 0);
    }

    #[test]
    fn test_search_league_same() {
        let mut registry = RegistryTrait::new(REGISTRY_ID);
        let mut league = LeagueTrait::new(REGISTRY_ID, LEAGUE_ID);
        let mut foe_squad = SquadTrait::new(
            REGISTRY_ID, FOE_SQUAD_ID, DEFAULT_LEVEL, DEFAULT_SIZE, 'test'
        );
        registry.subscribe(ref league, ref foe_squad);
        let league_id = registry.search_league(league);
        // [Assert] Registry
        assert(league_id == LEAGUE_ID, 'Registry: wrong search league');
    }

    #[test]
    fn test_search_league_close() {
        let mut registry = RegistryTrait::new(REGISTRY_ID);
        let mut league = LeagueTrait::new(REGISTRY_ID, LEAGUE_ID);
        let mut foe_league = LeagueTrait::new(REGISTRY_ID, CLOSEST_LEAGUE_ID);
        let mut foe_squad = SquadTrait::new(
            REGISTRY_ID, FOE_SQUAD_ID, DEFAULT_LEVEL, DEFAULT_SIZE, 'test'
        );
        registry.subscribe(ref foe_league, ref foe_squad);
        let league_id = registry.search_league(league);
        // [Assert] Registry
        assert(league_id == CLOSEST_LEAGUE_ID, 'Registry: wrong search league');
    }

    #[test]
    fn test_search_league_target() {
        let mut registry = RegistryTrait::new(REGISTRY_ID);
        let mut league = LeagueTrait::new(REGISTRY_ID, LEAGUE_ID);
        let mut foe_league = LeagueTrait::new(REGISTRY_ID, TARGET_LEAGUE_ID);
        let mut foe_squad = SquadTrait::new(
            REGISTRY_ID, FOE_SQUAD_ID, DEFAULT_LEVEL, DEFAULT_SIZE, 'test'
        );
        registry.subscribe(ref foe_league, ref foe_squad);
        let league_id = registry.search_league(league);
        // [Assert] Registry
        assert(league_id == TARGET_LEAGUE_ID, 'Registry: wrong search league');
    }

    #[test]
    fn test_search_league_far_down_top() {
        let mut registry = RegistryTrait::new(REGISTRY_ID);
        let mut league = LeagueTrait::new(REGISTRY_ID, LEAGUE_ID);
        let mut foe_league = LeagueTrait::new(REGISTRY_ID, FAREST_LEAGUE_ID);
        let mut foe_squad = SquadTrait::new(
            REGISTRY_ID, FOE_SQUAD_ID, DEFAULT_LEVEL, DEFAULT_SIZE, 'test'
        );
        registry.subscribe(ref foe_league, ref foe_squad);
        let league_id = registry.search_league(league);
        // [Assert] Registry
        assert(league_id == FAREST_LEAGUE_ID, 'Registry: wrong search league');
    }

    #[test]
    fn test_search_league_far_top_down() {
        let mut registry = RegistryTrait::new(REGISTRY_ID);
        let mut league = LeagueTrait::new(REGISTRY_ID, FAREST_LEAGUE_ID);
        let mut foe_league = LeagueTrait::new(REGISTRY_ID, LEAGUE_ID);
        let mut foe_squad = SquadTrait::new(
            REGISTRY_ID, FOE_SQUAD_ID, DEFAULT_LEVEL, DEFAULT_SIZE, 'test'
        );
        registry.subscribe(ref foe_league, ref foe_squad);
        let league_id = registry.search_league(league);
        // [Assert] Registry
        assert(league_id == LEAGUE_ID, 'Registry: wrong search league');
    }

    #[test]
    #[should_panic(expected: ('Registry: is empty',))]
    fn test_search_league_revert_empty() {
        let mut registry = RegistryTrait::new(REGISTRY_ID);
        let mut league = LeagueTrait::new(REGISTRY_ID, LEAGUE_ID);
        registry.search_league(league);
    }
}
