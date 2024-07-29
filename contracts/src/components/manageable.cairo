//! Manageable component

#[starknet::component]
mod ManageableComponent {
    // Starknet imports

    use starknet::ContractAddress;
    use starknet::info::get_caller_address;

    // Dojo imports

    use dojo::world::IWorldDispatcher;

    // Internal imports

    use zklash::store::{Store, StoreImpl};
    use zklash::models::player::{Player, PlayerImpl, PlayerAssert};

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
        fn _create(self: @ComponentState<TContractState>, world: IWorldDispatcher, name: felt252) {
            // [Setup] Datastore
            let store: Store = StoreImpl::new(world);

            // [Check] Player not already exists
            let caller = get_caller_address();
            let player = store.player(caller.into());
            player.assert_not_exists();

            // [Effect] Create a new player
            let player = PlayerImpl::new(caller.into(), name);
            store.set_player(player);
        }

        fn _rename(self: @ComponentState<TContractState>, world: IWorldDispatcher, name: felt252,) {
            // [Setup] Datastore
            let store: Store = StoreImpl::new(world);

            // [Check] Player exists
            let caller = get_caller_address();
            let mut player = store.player(caller.into());
            player.assert_exists();

            // [Effect] Create a new player
            player.rename(name);
            store.set_player(player);
        }
    }
}
