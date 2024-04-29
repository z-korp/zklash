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
use zklash::models::character::{Character, CharacterTrait, CharacterAssert};
use zklash::systems::account::IAccountDispatcherTrait;
use zklash::systems::market::IMarketDispatcherTrait;
use zklash::tests::setup::{setup, setup::{Systems, PLAYER}};

#[test]
fn test_market_hire_one() {
    // [Setup]
    let (world, systems, context) = setup::spawn_game();
    let store = StoreTrait::new(world);

    // [Spawn]
    systems.account.spawn(world);

    // [Hire]
    let player: Player = store.player(context.player_id);
    let intiial_team = store.team(context.player_id, player.team_id());
    systems.market.hire(world, player.team_id(), 0);

    // [Assert] Team
    let team = store.team(context.player_id, player.team_id());
    assert(intiial_team.gold >= team.gold, 'Hire: wrong team gold');
    assert(team.size > 0, 'Hire: wrong team char count');
    assert(team.character_uuid > 0, 'Hire: wrong team char count');

    // [Assert] Character
    let character_id = team.character_uuid;
    let character = store.character(context.player_id, team.id, character_id);
    character.assert_exists();
}

#[test]
fn test_market_hire_all_from_first_to_last() {
    // [Setup]
    let (world, systems, context) = setup::spawn_game();
    let store = StoreTrait::new(world);

    // [Spawn]
    systems.account.spawn(world);

    // [Hire]
    let player: Player = store.player(context.player_id);
    systems.market.hire(world, player.team_id(), 0);
    systems.market.hire(world, player.team_id(), 1);
    systems.market.hire(world, player.team_id(), 2);

    // [Assert] Shop
    let shop = store.shop(context.player_id, player.team_id());
    assert(shop.roles == 0, 'Hire: wrong shop roles');
}

#[test]
fn test_market_hire_all_from_last_to_first() {
    // [Setup]
    let (world, systems, context) = setup::spawn_game();
    let store = StoreTrait::new(world);

    // [Spawn]
    systems.account.spawn(world);

    // [Hire]
    let player: Player = store.player(context.player_id);
    systems.market.hire(world, player.team_id(), 2);
    systems.market.hire(world, player.team_id(), 1);
    systems.market.hire(world, player.team_id(), 0);

    // [Assert] Shop
    let shop = store.shop(context.player_id, player.team_id());
    assert(shop.roles == 0, 'Hire: wrong shop roles');
}
