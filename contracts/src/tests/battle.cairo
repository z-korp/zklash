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
use zklash::systems::battle::IBattleDispatcherTrait;
use zklash::tests::setup::{setup, setup::{Systems, PLAYER}};

#[test]
fn test_battle_start_lose() {
    // [Setup]
    let (world, systems, context) = setup::spawn_game();
    let store = StoreTrait::new(world);

    // [Spawn]
    systems.account.spawn(world);

    // [Hire]
    let player: Player = store.player(context.player_id);
    systems.market.hire(world, player.team_id(), 0);

    // [Hydrate]
    systems.battle.hydrate(world);

    // [Start]
    let intiial_team = store.team(context.player_id, player.team_id());
    systems.battle.start(world, player.team_id(), 0x01);

    // [Assert] Team
    let team = store.team(context.player_id, player.team_id());
    assert(intiial_team.gold < team.gold, 'Hire: wrong team gold');
    assert(intiial_team.health > team.health, 'Hire: wrong team health');
}

#[test]
fn test_battle_start_win() {
    // [Setup]
    let (world, systems, context) = setup::spawn_game();
    let store = StoreTrait::new(world);

    // [Spawn]
    systems.account.spawn(world);

    // [Hire]
    let player: Player = store.player(context.player_id);
    systems.market.reroll(world, player.team_id());
    systems.market.hire(world, player.team_id(), 2);
    systems.market.hire(world, player.team_id(), 1);
    systems.market.hire(world, player.team_id(), 0);

    // [Hydrate]
    systems.battle.hydrate(world);

    // [Start]
    let intiial_team = store.team(context.player_id, player.team_id());
    systems.battle.start(world, player.team_id(), 0x010203);

    // [Assert] Team
    let team = store.team(context.player_id, player.team_id());
    assert(intiial_team.gold < team.gold, 'Hire: wrong team gold');
    assert(intiial_team.health == team.health, 'Hire: wrong team health');
}

#[test]
fn test_battle_start_with_item() {
    // [Setup]
    let (world, systems, context) = setup::spawn_game();
    let store = StoreTrait::new(world);

    // [Spawn]
    systems.account.spawn(world);

    // [Hire]
    let player: Player = store.player(context.player_id);
    systems.market.reroll(world, player.team_id());
    systems.market.hire(world, player.team_id(), 2);
    systems.market.hire(world, player.team_id(), 1);
    systems.market.equip(world, player.team_id(), 0x1, 0);

    // [Hydrate]
    systems.battle.hydrate(world);

    // [Start]
    let intiial_team = store.team(context.player_id, player.team_id());
    systems.battle.start(world, player.team_id(), 0x0102);

    // [Assert] Team
    let team = store.team(context.player_id, player.team_id());
    assert(intiial_team.gold < team.gold, 'Item: wrong team gold');
    assert(intiial_team.health == team.health, 'Item: wrong team health');
}
