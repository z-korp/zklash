// Starknet imports

use starknet::ContractAddress;

// Dojo imports

use dojo::world::IWorldDispatcher;

#[starknet::interface]
trait IAccount<TContractState> {
    fn create(self: @TContractState, world: IWorldDispatcher, name: felt252,);
}

#[starknet::contract]
mod account {
    // Core imports

    use core::debug::PrintTrait;

    // Starknet imports

    use starknet::ContractAddress;
    use starknet::info::{get_caller_address, get_block_timestamp};

    // Dojo imports

    use dojo::world;
    use dojo::world::IWorldDispatcher;
    use dojo::world::IWorldDispatcherTrait;
    use dojo::world::IWorldProvider;
    use dojo::world::IDojoResourceProvider;

    // Internal imports

    use zklash::constants::WORLD;
    use zklash::store::{Store, StoreImpl};
    use zklash::models::player::{Player, PlayerImpl, PlayerAssert};

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


    // impl: implement functions specified in trait
    #[abi(embed_v0)]
    impl AccountImpl of IAccount<ContractState> {
        fn create(self: @ContractState, world: IWorldDispatcher, name: felt252,) {
            // [Setup] Datastore
            let store: Store = StoreImpl::new(world);

            // [Check] Player not already exists
            let caller = get_caller_address();
            let player = store.player(caller.into());
            player.assert_not_exists();

            // [Effect] Create a new player
            let player = PlayerImpl::new(caller, name);
            store.set_player(player);
        }
    }
}
