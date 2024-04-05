// Starknet imports

use starknet::ContractAddress;

// Internal imports

use zklash::constants;

#[derive(Model, Copy, Drop, Serde)]
struct Team {
    #[key]
    player_id: ContractAddress,
    #[key]
    id: u32,
    seed: felt252,
    gold: u32,
    health: u32,
    level: u8,
    characters: u64,
}

mod errors {
    const TEAM_NOT_EXIST: felt252 = 'Team: does not exist';
    const TEAM_ALREADY_EXIST: felt252 = 'Team: already exist';
    const TEAM_INVALID_SEED: felt252 = 'Team: invalid seed';
    const TEAM_IS_EMPTY: felt252 = 'Team: is empty';
}

#[generate_trait]
impl TeamImpl of TeamTrait {
    #[inline(always)]
    fn new(player_id: ContractAddress, id: u32, seed: felt252) -> Team {
        // [Check] Seed is valid
        assert(seed != 0, errors::TEAM_INVALID_SEED);
        // [Return] Team
        Team {
            player_id,
            id,
            seed,
            gold: constants::DEFAULT_GOLD,
            health: constants::DEFAULT_HEALTH,
            level: constants::DEFAULT_LEVEL,
            characters: 0
        }
    }
}

#[generate_trait]
impl PlayerAssert of AssertTrait {
    #[inline(always)]
    fn assert_exists(self: Team) {
        assert(self.is_non_zero(), errors::TEAM_NOT_EXIST);
    }

    #[inline(always)]
    fn assert_not_exists(self: Team) {
        assert(self.is_zero(), errors::TEAM_ALREADY_EXIST);
    }

    #[inline(always)]
    fn assert_not_empty(self: Team) {
        assert(self.characters != 0, errors::TEAM_IS_EMPTY);
    }
}

impl ZeroablePlayerImpl of core::Zeroable<Team> {
    #[inline(always)]
    fn zero() -> Team {
        Team {
            player_id: Zeroable::zero(), id: 0, seed: 0, gold: 0, health: 0, level: 0, characters: 0
        }
    }

    #[inline(always)]
    fn is_zero(self: Team) -> bool {
        0 == self.seed
    }

    #[inline(always)]
    fn is_non_zero(self: Team) -> bool {
        !self.is_zero()
    }
}
