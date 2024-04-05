// Starknet imports

use starknet::ContractAddress;

// Dojo imports

use dojo::world::IWorldDispatcher;

#[starknet::interface]
trait IBattle<TContractState> {
    fn start(self: @TContractState, world: IWorldDispatcher, team_id: u32, order: u128,);
}

#[starknet::contract]
mod battle {
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
    use zklash::helpers::packer::Packer;
    use zklash::models::player::{Player, PlayerImpl, PlayerAssert};
    use zklash::models::team::{Team, TeamImpl, TeamAssert};
    use zklash::models::shop::{Shop, ShopImpl, ShopAssert};
    use zklash::models::character::{Character, CharacterImpl, CharacterAssert};

    // Local imports

    use super::IBattle;

    // Storage

    #[storage]
    struct Storage {}

    // Implementations

    #[abi(embed_v0)]
    impl DojoResourceProviderImpl of IDojoResourceProvider<ContractState> {
        fn dojo_resource(self: @ContractState) -> felt252 {
            'battle'
        }
    }

    #[abi(embed_v0)]
    impl WorldProviderImpl of IWorldProvider<ContractState> {
        fn world(self: @ContractState) -> IWorldDispatcher {
            IWorldDispatcher { contract_address: WORLD() }
        }
    }

    #[abi(embed_v0)]
    impl BattleImpl of IBattle<ContractState> {
        fn start(self: @ContractState, world: IWorldDispatcher, team_id: u32, order: u128,) {
            // [Setup] Datastore
            let store: Store = StoreImpl::new(world);

            // [Check] Player exists
            let caller = get_caller_address();
            let player = store.player(caller.into());
            player.assert_exists();

            // [Check] Team exists
            let mut team = store.team(player.id, team_id);
            team.assert_exists();

            // [Check] Characters exist
            let mut character_ids = Packer::unpack(order);
            loop {
                match character_ids.pop_front() {
                    Option::Some(character_id) => {
                        let character = store.character(player.id, team.id, character_id);
                        character.assert_exists();
                    },
                    Option::None => { break; },
                }
            };

            // [Effect] Update team characters
            team.characters = order;
        }
    }
}
