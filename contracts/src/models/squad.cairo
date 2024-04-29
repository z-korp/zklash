// Core imports

use core::zeroable::Zeroable;

// Starknet imports

use starknet::ContractAddress;

// External imports

use origami::rating::elo::EloTrait;

// Internal imports

use zklash::constants::{ZERO, DEFAULT_K_FACTOR, LEAGUE_MIN_THRESHOLD, LEAGUE_SIZE};
use zklash::helpers::battler::Battler;
use zklash::models::league::LeagueTrait;
use zklash::models::character::Character;

// Errors

mod errors {
    const SQUAD_DOES_NOT_EXIST: felt252 = 'Squad: does not exist';
    const SQUAD_ALREADY_EXIST: felt252 = 'Squad: already exist';
    const SQUAD_NOT_SUBSCRIBABLE: felt252 = 'Squad: not subscribable';
    const SQUAD_NOT_SUBSCRIBED: felt252 = 'Squad: not subscribed';
}

#[derive(Model, Copy, Drop, Serde)]
struct Squad {
    #[key]
    registry_id: u32,
    #[key]
    id: u32,
    league_id: u8,
    index: u32,
    rating: u32,
    size: u8,
}

#[generate_trait]
impl SquadImpl of SquadTrait {
    #[inline(always)]
    fn new(registry_id: u32, id: u32, level: u8, size: u8) -> Squad {
        let rating: u32 = level.into() * LEAGUE_SIZE.into() + LEAGUE_MIN_THRESHOLD;
        Squad { registry_id, id, league_id: 0, index: 0, rating, size }
    }

    #[inline(always)]
    fn fight(
        ref self: Squad,
        ref chars: Array<Character>,
        ref foe_squad: Squad,
        ref foes: Array<Character>
    ) -> bool {
        // [Effect] Fight and manage the win status
        let win = Battler::start(ref chars, ref foes);
        let score: u16 = match win {
            true => 100, // Win
            false => 0, // Lose
        };
        let (change, negative) = EloTrait::rating_change(
            self.rating, foe_squad.rating, score, DEFAULT_K_FACTOR
        );
        if negative {
            self.rating -= change;
            foe_squad.rating += change;
        } else {
            self.rating += change;
            foe_squad.rating -= change;
        };
        win
    }
}

#[generate_trait]
impl SquadAssert of AssertTrait {
    #[inline(always)]
    fn assert_does_exist(squad: Squad) {
        assert(squad.is_non_zero(), errors::SQUAD_DOES_NOT_EXIST);
    }

    #[inline(always)]
    fn assert_not_exist(squad: Squad) {
        assert(squad.is_zero(), errors::SQUAD_ALREADY_EXIST);
    }

    #[inline(always)]
    fn assert_subscribable(squad: Squad) {
        assert(squad.league_id == 0, errors::SQUAD_NOT_SUBSCRIBABLE);
    }

    #[inline(always)]
    fn assert_subscribed(squad: Squad) {
        assert(squad.league_id != 0, errors::SQUAD_NOT_SUBSCRIBED);
    }
}

impl SquadZeroable of Zeroable<Squad> {
    #[inline(always)]
    fn zero() -> Squad {
        Squad { registry_id: 0, id: 0, league_id: 0, index: 0, rating: 0, size: 0 }
    }

    #[inline(always)]
    fn is_zero(self: Squad) -> bool {
        self.league_id == 0 && self.index == 0 && self.rating == 0
    }

    #[inline(always)]
    fn is_non_zero(self: Squad) -> bool {
        !self.is_zero()
    }
}

#[cfg(test)]
mod tests {
    // Core imports

    use core::debug::PrintTrait;

    // Local imports

    use super::{Squad, SquadTrait, ContractAddress, AssertTrait, LEAGUE_SIZE, LEAGUE_MIN_THRESHOLD};

    // Constants

    const REGISTRY_ID: u32 = 1;
    const SQUAD_ID: u32 = 1;
    const DEFAULT_LEVEL: u8 = 1;
    const DEFAULT_SIZE: u8 = 1;

    #[test]
    fn test_new() {
        let squad = SquadTrait::new(REGISTRY_ID, SQUAD_ID, DEFAULT_LEVEL, DEFAULT_SIZE);
        let expected_rating: u32 = (DEFAULT_LEVEL * LEAGUE_SIZE).into() + LEAGUE_MIN_THRESHOLD;
        assert_eq!(squad.registry_id, REGISTRY_ID);
        assert_eq!(squad.id, SQUAD_ID);
        assert_eq!(squad.league_id, 0);
        assert_eq!(squad.index, 0);
        assert_eq!(squad.rating, expected_rating);
    }

    #[test]
    fn test_subscribable() {
        let squad = SquadTrait::new(REGISTRY_ID, SQUAD_ID, DEFAULT_LEVEL, DEFAULT_SIZE);
        AssertTrait::assert_subscribable(squad);
    }

    #[test]
    #[should_panic(expected: ('Squad: not subscribable',))]
    fn test_subscribable_revert_not_subscribable() {
        let mut squad = SquadTrait::new(REGISTRY_ID, SQUAD_ID, DEFAULT_LEVEL, DEFAULT_SIZE);
        squad.league_id = 1;
        AssertTrait::assert_subscribable(squad);
    }

    #[test]
    fn test_subscribed() {
        let mut squad = SquadTrait::new(REGISTRY_ID, SQUAD_ID, DEFAULT_LEVEL, DEFAULT_SIZE);
        squad.league_id = 1;
        AssertTrait::assert_subscribed(squad);
    }

    #[test]
    #[should_panic(expected: ('Squad: not subscribed',))]
    fn test_subscribed_revert_not_subscribed() {
        let mut squad = SquadTrait::new(REGISTRY_ID, SQUAD_ID, DEFAULT_LEVEL, DEFAULT_SIZE);
        AssertTrait::assert_subscribed(squad);
    }
}
