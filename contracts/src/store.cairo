//! Store struct and component management methods.

// Core imports

use core::debug::PrintTrait;

// Straknet imports

use starknet::ContractAddress;

// Dojo imports

use dojo::world::{IWorldDispatcher, IWorldDispatcherTrait};

// Models imports

use zklash::models::character::Character;
use zklash::models::player::Player;
use zklash::models::shop::Shop;
use zklash::models::team::Team;


/// Store struct.
#[derive(Copy, Drop)]
struct Store {
    world: IWorldDispatcher,
}

/// Implementation of the `StoreTrait` trait for the `Store` struct.
#[generate_trait]
impl StoreImpl of StoreTrait {
    #[inline(always)]
    fn new(world: IWorldDispatcher) -> Store {
        Store { world: world }
    }

    #[inline(always)]
    fn player(self: Store, player_id: ContractAddress) -> Player {
        get!(self.world, player_id, (Player))
    }

    #[inline(always)]
    fn team(self: Store, player_id: ContractAddress, team_id: u32) -> Team {
        get!(self.world, (player_id, team_id), (Team))
    }

    #[inline(always)]
    fn set_player(self: Store, player: Player) {
        set!(self.world, (player))
    }

    #[inline(always)]
    fn set_team(self: Store, team: Team) {
        set!(self.world, (team))
    }
}