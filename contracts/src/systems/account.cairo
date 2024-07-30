// Starknet imports

use starknet::ContractAddress;

// Dojo imports

use dojo::world::IWorldDispatcher;

#[dojo::interface]
trait IAccount<TContractState> {
    fn create(ref world: IWorldDispatcher, name: felt252);
    fn rename(ref world: IWorldDispatcher, name: felt252);
    fn spawn(ref world: IWorldDispatcher);
}

#[dojo::contract]
mod account {
    // Starknet imports

    use starknet::ContractAddress;
    use starknet::info::{
        get_block_timestamp, get_block_number, get_caller_address, get_contract_address
    };

    // Component imports

    use zklash::components::initializable::InitializableComponent;
    use zklash::components::emitter::EmitterComponent;
    use zklash::components::manageable::ManageableComponent;

    // Local imports

    use super::IAccount;
    use zklash::store::{Store, StoreImpl, StoreTrait};
    use zklash::models::player::{AssertTrait, PlayerTrait};
    use zklash::models::team::{TeamTrait};
    use zklash::models::registry::{RegistryTrait};

    // Components

    component!(path: EmitterComponent, storage: emitter, event: EmitterEvent);
    impl EmitterImpl = EmitterComponent::EmitterImpl<ContractState>;
    component!(path: ManageableComponent, storage: manageable, event: ManageableEvent);
    impl ManageableInternalImpl = ManageableComponent::InternalImpl<ContractState>;

    // Storage

    #[storage]
    struct Storage {
        #[substorage(v0)]
        emitter: EmitterComponent::Storage,
        #[substorage(v0)]
        manageable: ManageableComponent::Storage,
    }

    // Events

    #[event]
    #[derive(Drop, starknet::Event)]
    enum Event {
        #[flat]
        EmitterEvent: EmitterComponent::Event,
        #[flat]
        ManageableEvent: ManageableComponent::Event,
    }

    // Constructor

    fn dojo_init(ref world: IWorldDispatcher) {}

    // Implementations

    #[abi(embed_v0)]
    impl AccountImpl of IAccount<ContractState> {
        fn create(ref world: IWorldDispatcher, name: felt252) {
            // [Effect] Create a player
            self.manageable._create(world, name);
        }

        fn rename(ref world: IWorldDispatcher, name: felt252) {
            self.manageable._rename(world, name);
        }

        // [Spawn] Spawn a new team for the player
        fn spawn(ref world: IWorldDispatcher) {
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
