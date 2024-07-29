// Starknet imports

use starknet::ContractAddress;

// Component

#[starknet::component]
mod HostableComponent {
    // Starknet imports

    use starknet::ContractAddress;
    use starknet::info::{get_contract_address, get_caller_address, get_block_timestamp};

    // Dojo imports

    use dojo::world;
    use dojo::world::IWorldDispatcher;
    use dojo::world::IWorldDispatcherTrait;
    use dojo::world::IWorldProvider;
    use dojo::world::IDojoResourceProvider;

    // Internal imports

    use zklash::constants;
    use zklash::store::{Store, StoreImpl};
    use zklash::models::game::{Game, GameImpl, GameAssert};
    use zklash::models::player::{Player, PlayerImpl, PlayerAssert};
    use zklash::models::builder::{Builder, BuilderImpl, BuilderAssert};
    use zklash::models::tile::{Tile, TilePosition, TileImpl};
    use zklash::models::tournament::{Tournament, TournamentImpl, TournamentAssert};
    use zklash::types::orientation::Orientation;
    use zklash::types::direction::Direction;
    use zklash::types::mode::{Mode, ModeTrait};
    use zklash::types::role::Role;
    use zklash::types::spot::Spot;
    use zklash::types::plan::Plan;
    use zklash::types::deck::Deck;

    // Storage

    #[storage]
    struct Storage {}

    // Events

    #[event]
    #[derive(Drop, starknet::Event)]
    enum Event {}

    #[generate_trait]
    impl InternalImpl<
        TContractState, +HasComponent<TContractState>
    > of InternalTrait<TContractState> {
        fn _spawn(
            self: @ComponentState<TContractState>, world: IWorldDispatcher, mode: Mode
        ) -> (u32, u256) {
            // [Setup] Datastore
            let store: Store = StoreImpl::new(world);

            // [Check] Player exists
            let caller = get_caller_address();
            let player = store.player(caller.into());
            player.assert_exists();

            // [Effect] Create game
            let game_id = world.uuid() + 1;
            let time = get_block_timestamp();
            let mut game = GameImpl::new(game_id, time, mode);

            // [Effect] Start game
            let tile = game.start(time);

            // [Effect] Store tile
            store.set_tile(tile);

            // [Effect] Create a new builder
            let mut builder = BuilderImpl::new(game.id, player.id);
            let (tile_id, plan) = game.draw_plan();
            let tile = builder.reveal(tile_id, plan);

            // [Effect] Store builder
            store.set_builder(builder);

            // [Effect] Store tile
            store.set_tile(tile);

            // [Effect] Update tournament
            let tournament_id = TournamentImpl::compute_id(time, game.duration());
            let mut tournament = store.tournament(tournament_id);
            tournament.buyin(game.price());

            // [Effect] Store tournament
            store.set_tournament(tournament);

            // [Effect] Store game
            store.set_game(game);

            // [Return] Game ID and amount to pay
            let amount: u256 = game.price().into();
            (game_id, amount)
        }

        fn _claim(
            self: @ComponentState<TContractState>,
            world: IWorldDispatcher,
            tournament_id: u64,
            rank: u8,
            mode: Mode,
        ) -> u256 {
            // [Setup] Datastore
            let store: Store = StoreImpl::new(world);

            // [Check] Player exists
            let caller = get_caller_address();
            let mut player = store.player(caller.into());
            player.assert_exists();

            // [Check] Tournament exists
            let mut tournament = store.tournament(tournament_id);
            tournament.assert_exists();

            // [Effect] Update claim
            let time = get_block_timestamp();
            let reward = tournament.claim(player.id, rank, time, mode.duration());
            store.set_tournament(tournament);

            // [Return] Pay reward
            reward
        }

        fn _sponsor(
            self: @ComponentState<TContractState>,
            world: IWorldDispatcher,
            amount: felt252,
            mode: Mode
        ) -> u256 {
            // [Setup] Datastore
            let store: Store = StoreImpl::new(world);

            // [Check] Tournament exists
            let time = get_block_timestamp();
            let tournament_id = TournamentImpl::compute_id(time, mode.duration());
            let mut tournament = store.tournament(tournament_id);
            tournament.assert_exists();

            // [Effect] Add amount to the current tournament prize pool
            tournament.buyin(amount);
            store.set_tournament(tournament);

            // [Return] Amount to pay
            amount.into()
        }
    }
}
