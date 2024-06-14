// Starknet imports

use starknet::ContractAddress;

// Dojo imports

use dojo::world::IWorldDispatcher;

#[starknet::interface]
trait IAccount<TContractState> {
    fn create(self: @TContractState, world: IWorldDispatcher, name: felt252,);
    fn spawn(self: @TContractState, world: IWorldDispatcher,);
}

#[starknet::contract]
mod account {
    // Core imports

    use core::debug::PrintTrait;

    // Starknet imports

    use starknet::ContractAddress;
    use starknet::info::{get_caller_address, get_block_timestamp, get_block_number};

    // Dojo imports

    use dojo::world;
    use dojo::world::IWorldDispatcher;
    use dojo::world::IWorldDispatcherTrait;
    use dojo::world::IWorldProvider;
    use dojo::world::IDojoResourceProvider;

    // Internal imports

    use zklash::constants::WORLD;
    use zklash::store::{Store, StoreTrait};
    use zklash::models::registry::{Registry, RegistryTrait};
    use zklash::models::player::{Player, PlayerTrait, PlayerAssert};
    use zklash::models::team::{Team, TeamTrait, TeamAssert};
    use zklash::models::shop::{Shop, ShopTrait, ShopAssert};

    // Local imports

    use super::IAccount;

    // Storage

    #[storage]
    struct Storage {}

    // Implementations

    #[abi(embed_v0)]
    impl DojoResourceProviderImpl of IDojoResourceProvider<ContractState> {
        fn dojo_resource(self: @ContractState) -> felt252 {
            'account'
        }
    }

    #[abi(embed_v0)]
    impl WorldProviderImpl of IWorldProvider<ContractState> {
        fn world(self: @ContractState) -> IWorldDispatcher {
            IWorldDispatcher { contract_address: WORLD() }
        }
    }

    #[abi(embed_v0)]
    impl AccountImpl of IAccount<ContractState> {
        fn create(self: @ContractState, world: IWorldDispatcher, name: felt252,) {
            // [Setup] Datastore
            let store: Store = StoreTrait::new(world);

            // [Check] Player not already exists
            let caller = get_caller_address();
            let player = store.player(caller.into());
            player.assert_not_exists();

            // [Effect] Create a new player
            let player = PlayerTrait::new(caller, name);
            store.set_player(player);
        }

        fn spawn(self: @ContractState, world: IWorldDispatcher) {
            // [Setup] Datastore
            let store: Store = StoreTrait::new(world);

            // [Check] Player exists
            let caller = get_caller_address();
            let mut player = store.player(caller.into());
            player.assert_exists();

            // [Effect] Spawn a new team
            let team = player.spawn_team();

            // [Effect] Spawn a new shop
            let mut registry = store.registry(team.registry_id);
            registry.reseed();
            let shop = team.spawn_shop(registry.seed);

            // [Effect] Store the shop
            store.set_shop(shop);

            // [Effect] Store the team
            store.set_team(team);

            // [Effect] Update the player
            store.set_player(player);

            // [Effect] Update the registry
            store.set_registry(registry);
        }
    }
}
