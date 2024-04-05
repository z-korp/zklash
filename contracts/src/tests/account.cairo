// Core imports

use core::debug::PrintTrait;

// Starknet imports

use starknet::testing::set_contract_address;

// Dojo imports

use dojo::world::{IWorldDispatcher, IWorldDispatcherTrait};

// Internal imports

use zklash::store::{Store, StoreTrait};
use zklash::models::player::{Player, PlayerTrait, PlayerAssert};
use zklash::models::team::{Team, TeamTrait, TeamAssert};
use zklash::models::shop::{Shop, ShopTrait, ShopAssert};
use zklash::systems::account::IAccountDispatcherTrait;
use zklash::tests::setup::{setup, setup::{Systems, PLAYER}};

#[test]
fn test_account_spawn() {
    // [Setup]
    let (world, systems, context) = setup::spawn_game();
    let store = StoreTrait::new(world);

    // [Assert] Player
    let player = store.player(context.player_id);
    assert(player.id == context.player_id, 'Create: wrong player id');
    assert(player.name == context.player_name, 'Create: wrong player name');
    assert(player.team_count == 0, 'Create: wrong player team_count');

    // [Spawn]
    systems.account.spawn(world);

    // [Assert] Player
    let player = store.player(context.player_id);
    assert(player.team_count == 1, 'Spawn: wrong player team_count');

    // [Assert] Team
    let team = store.team(context.player_id, player.team_id());
    assert(team.id == player.team_id(), 'Spawn: wrong team id');
    team.assert_exists();

    // [Assert] Shop
    let shop = store.shop(context.player_id, team.id);
    shop.assert_exists();
}
