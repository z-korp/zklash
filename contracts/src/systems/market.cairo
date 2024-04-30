// Starknet imports

use starknet::ContractAddress;

// Dojo imports

use dojo::world::IWorldDispatcher;

#[starknet::interface]
trait IMarket<TContractState> {
    fn equip(
        self: @TContractState, world: IWorldDispatcher, team_id: u32, character_id: u8, index: u8,
    );
    fn hire(self: @TContractState, world: IWorldDispatcher, team_id: u32, index: u8,);
    fn xp(
        self: @TContractState, world: IWorldDispatcher, team_id: u32, character_id: u8, index: u8,
    );
    fn merge(self: @TContractState, world: IWorldDispatcher, team_id: u32, from_id: u8, to_id: u8,);
    fn sell(self: @TContractState, world: IWorldDispatcher, team_id: u32, character_id: u8);
    fn reroll(self: @TContractState, world: IWorldDispatcher, team_id: u32,);
}

#[starknet::contract]
mod market {
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

    use zklash::constants::{WORLD, DEFAULT_SHOP_REROLL_COST};
    use zklash::store::{Store, StoreTrait};
    use zklash::models::registry::{Registry, RegistryTrait};
    use zklash::models::player::{Player, PlayerTrait, PlayerAssert};
    use zklash::models::team::{Team, TeamTrait, TeamAssert};
    use zklash::models::shop::{Shop, ShopTrait, ShopAssert};
    use zklash::models::character::{Character, CharacterTrait, CharacterAssert};

    // Local imports

    use super::IMarket;

    // Storage

    #[storage]
    struct Storage {}

    // Implementations

    #[abi(embed_v0)]
    impl DojoResourceProviderImpl of IDojoResourceProvider<ContractState> {
        fn dojo_resource(self: @ContractState) -> felt252 {
            'market'
        }
    }

    #[abi(embed_v0)]
    impl WorldProviderImpl of IWorldProvider<ContractState> {
        fn world(self: @ContractState) -> IWorldDispatcher {
            IWorldDispatcher { contract_address: WORLD() }
        }
    }

    #[abi(embed_v0)]
    impl MarketImpl of IMarket<ContractState> {
        fn equip(
            self: @ContractState,
            world: IWorldDispatcher,
            team_id: u32,
            character_id: u8,
            index: u8,
        ) {
            // [Setup] Datastore
            let store: Store = StoreTrait::new(world);

            // [Check] Player exists
            let caller = get_caller_address();
            let player = store.player(caller.into());
            player.assert_exists();

            // [Check] Team exists
            let mut team = store.team(player.id, team_id);
            team.assert_exists();

            // [Check] Shop exists
            let mut shop = store.shop(player.id, team_id);
            shop.assert_exists();

            // [Check] Character exists
            let mut character = store.character(player.id, team_id, character_id);
            character.assert_exists();

            // [Effect] Purchase item
            team.equip(ref shop, ref character, index);

            // [Effect] Update character
            store.set_character(character);

            // [Effect] Update shop
            store.set_shop(shop);

            // [Effect] Update team
            store.set_team(team);
        }

        fn hire(self: @ContractState, world: IWorldDispatcher, team_id: u32, index: u8,) {
            // [Setup] Datastore
            let store: Store = StoreTrait::new(world);

            // [Check] Player exists
            let caller = get_caller_address();
            let player = store.player(caller.into());
            player.assert_exists();

            // [Check] Team exists
            let mut team = store.team(player.id, team_id);
            team.assert_exists();

            // [Check] Shop exists
            let mut shop = store.shop(player.id, team_id);
            shop.assert_exists();

            // [Effect] Purchase item
            let character = team.hire(ref shop, index);

            // [Effect] Update character
            store.set_character(character);

            // [Effect] Update shop
            store.set_shop(shop);

            // [Effect] Update team
            store.set_team(team);
        }

        fn xp(
            self: @ContractState,
            world: IWorldDispatcher,
            team_id: u32,
            character_id: u8,
            index: u8,
        ) {
            // [Setup] Datastore
            let store: Store = StoreTrait::new(world);

            // [Check] Player exists
            let caller = get_caller_address();
            let player = store.player(caller.into());
            player.assert_exists();

            // [Check] Team exists
            let mut team = store.team(player.id, team_id);
            team.assert_exists();

            // [Check] Shop exists
            let mut shop = store.shop(player.id, team_id);
            shop.assert_exists();

            // [Check] Character exists
            let mut character = store.character(player.id, team_id, character_id);
            character.assert_exists();

            // [Effect] Purchase item
            team.xp(ref shop, ref character, index);

            // [Effect] Update character
            store.set_character(character);

            // [Effect] Update shop
            store.set_shop(shop);

            // [Effect] Update team
            store.set_team(team);
        }

        fn merge(
            self: @ContractState, world: IWorldDispatcher, team_id: u32, from_id: u8, to_id: u8,
        ) {
            // [Setup] Datastore
            let store: Store = StoreTrait::new(world);

            // [Check] Player exists
            let caller = get_caller_address();
            let player = store.player(caller.into());
            player.assert_exists();

            // [Check] Team exists
            let team = store.team(player.id, team_id);
            team.assert_exists();

            // [Check] From Character exists
            let mut from = store.character(player.id, team_id, from_id);
            from.assert_exists();

            // [Check] To Character exists
            let mut to = store.character(player.id, team_id, to_id);
            to.assert_exists();

            // [Effect] Purchase item
            from.merge(ref to);

            // [Effect] Update characters
            store.set_character(from);
            store.set_character(to);
        }

        fn sell(self: @ContractState, world: IWorldDispatcher, team_id: u32, character_id: u8,) {
            // [Setup] Datastore
            let store: Store = StoreTrait::new(world);

            // [Check] Player exists
            let caller = get_caller_address();
            let player = store.player(caller.into());
            player.assert_exists();

            // [Check] Team exists
            let mut team = store.team(player.id, team_id);
            team.assert_exists();

            // [Check] Character exists
            let mut character = store.character(player.id, team_id, character_id);
            character.assert_exists();

            // [Effect] Sell character
            team.sell(ref character);

            // [Effect] Update character
            store.set_character(character);

            // [Effect] Update team
            store.set_team(team);
        }

        fn reroll(self: @ContractState, world: IWorldDispatcher, team_id: u32,) {
            // [Setup] Datastore
            let store: Store = StoreTrait::new(world);

            // [Check] Player exists
            let caller = get_caller_address();
            let player = store.player(caller.into());
            player.assert_exists();

            // [Check] Team exists
            let mut team = store.team(player.id, team_id);
            team.assert_exists();

            // [Check] Shop exists
            let mut shop = store.shop(player.id, team_id);
            shop.assert_exists();

            // [Effect] Reroll shop
            let mut registry = store.registry(team.registry_id);
            registry.reseed();
            team.reroll(ref shop, registry.seed);

            // [Effect] Update shop
            store.set_shop(shop);

            // [Effect] Update team
            store.set_team(team);

            // [Effect] Update registry
            store.set_registry(registry);
        }
    }
}
