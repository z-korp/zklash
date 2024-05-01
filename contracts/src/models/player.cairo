// Starknet imports

use starknet::ContractAddress;

// Internal imports

use zklash::models::team::{Team, TeamTrait};

mod errors {
    const PLAYER_NOT_EXIST: felt252 = 'Player: does not exist';
    const PLAYER_ALREADY_EXIST: felt252 = 'Player: already exist';
    const PLAYER_INVALID_NAME: felt252 = 'Player: invalid name';
}

#[derive(Model, Copy, Drop, Serde)]
struct Player {
    #[key]
    id: ContractAddress,
    name: felt252,
    team_count: u32,
    win_count: u32,
}

#[generate_trait]
impl PlayerImpl of PlayerTrait {
    #[inline(always)]
    fn new(id: ContractAddress, name: felt252) -> Player {
        // [Check] Name is valid
        assert(name != 0, errors::PLAYER_INVALID_NAME);

        // [Return] Player
        Player { id, name, team_count: 0, win_count: 0 }
    }

    #[inline(always)]
    fn team_id(self: Player) -> u32 {
        // [Return] Team id
        self.team_count
    }

    #[inline(always)]
    fn spawn_team(ref self: Player) -> Team {
        // [Return] Team
        self.team_count += 1;
        let team = TeamTrait::new(self.id, self.team_count);
        team
    }
}

#[generate_trait]
impl PlayerAssert of AssertTrait {
    #[inline(always)]
    fn assert_exists(self: Player) {
        assert(self.is_non_zero(), errors::PLAYER_NOT_EXIST);
    }

    #[inline(always)]
    fn assert_not_exists(self: Player) {
        assert(self.is_zero(), errors::PLAYER_ALREADY_EXIST);
    }
}

impl ZeroablePlayerImpl of core::Zeroable<Player> {
    #[inline(always)]
    fn zero() -> Player {
        Player { id: Zeroable::zero(), name: 0, team_count: 0, win_count: 0 }
    }

    #[inline(always)]
    fn is_zero(self: Player) -> bool {
        0 == self.name
    }

    #[inline(always)]
    fn is_non_zero(self: Player) -> bool {
        !self.is_zero()
    }
}
