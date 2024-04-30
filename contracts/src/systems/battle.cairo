// Starknet imports

use starknet::ContractAddress;

// Dojo imports

use dojo::world::IWorldDispatcher;

#[starknet::interface]
trait IBattle<TContractState> {
    fn hydrate(self: @TContractState, world: IWorldDispatcher);
    fn start(self: @TContractState, world: IWorldDispatcher, team_id: u32, order: u32,);
}

#[starknet::contract]
mod battle {
    // Core imports

    use zklash::models::registry::RegistryTrait;
    use zklash::store::StoreTrait;
    use core::array::ArrayTrait;
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

    use zklash::constants::{WORLD, DEFAULT_REGISTRY_ID};
    use zklash::store::{Store, StoreImpl};
    use zklash::helpers::packer::Packer;
    use zklash::helpers::array::ArrayTraitExt;
    use zklash::models::registry::{Registry, RegistryImpl, RegistryAssert};
    use zklash::models::league::{League, LeagueTrait, LeagueAssert};
    use zklash::models::squad::{Squad, SquadImpl, SquadAssert};
    use zklash::models::foe::{Foe, FoeImpl};
    use zklash::models::player::{Player, PlayerImpl, PlayerAssert};
    use zklash::models::team::{Team, TeamImpl, TeamAssert};
    use zklash::models::shop::{Shop, ShopImpl, ShopAssert};
    use zklash::models::character::{Character, CharacterImpl, CharacterAssert};
    use zklash::types::wave::{Wave, WaveTrait};

    // Local imports

    use super::IBattle;

    // Errors

    mod errors {
        const CHARACTER_DUPLICATE: felt252 = 'Battle: character duplicate';
    }

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
        fn hydrate(self: @ContractState, world: IWorldDispatcher) {
            // [Setup] Datastore
            let store: Store = StoreImpl::new(world);

            // [Effect] Hydrate registry
            let mut registry = store.registry(DEFAULT_REGISTRY_ID);

            // [Effect] Subscribe waves
            let mut waves = array![
                Wave::One,
                Wave::Two,
                Wave::Three,
                Wave::Four,
                Wave::Five,
                Wave::Six,
                Wave::Seven,
                Wave::Eight,
                Wave::Nine,
                Wave::Ten
            ];
            loop {
                match waves.pop_front() {
                    Option::Some(wave) => {
                        let (level, size) = WaveTrait::attributes(wave);
                        let mut squad: Squad = registry.create_squad(level, size);
                        let mut foes: Array<Foe> = WaveTrait::foes(wave, registry.id, squad.id);
                        let league_id: u8 = LeagueTrait::compute_id(squad.rating);
                        let mut league = store.league(registry.id, league_id);
                        let mut slot = registry.subscribe(ref league, ref squad);
                        store.set_foes(ref foes);
                        store.set_slot(slot);
                        store.set_squad(squad);
                        store.set_league(league);
                    },
                    Option::None => { break; },
                }
            };

            // [Effect] Update registry
            store.set_registry(registry);
        }

        fn start(self: @ContractState, world: IWorldDispatcher, team_id: u32, order: u32,) {
            // [Setup] Datastore
            let store: Store = StoreImpl::new(world);

            // [Check] Player exists
            let caller = get_caller_address();
            let mut player = store.player(caller.into());
            player.assert_exists();

            // [Check] Team exists
            let mut team = store.team(player.id, team_id);
            team.assert_exists();

            // [Check] Shop exists
            let mut shop = store.shop(player.id, team_id);
            shop.assert_exists();

            // [Check] Characters exist
            let mut characters: Array<Character> = array![];
            let mut character_ids = Packer::unpack(order);
            loop {
                match character_ids.pop_front() {
                    Option::Some(character_id) => {
                        let character = store.character(player.id, team.id, character_id);
                        character.assert_exists();
                        assert(!characters.contains(character), errors::CHARACTER_DUPLICATE);
                        characters.append(character);
                    },
                    Option::None => { break; },
                }
            };

            // [Effect] Create squad
            let mut registry = store.registry(team.registry_id);
            let mut team_squad = registry.create_squad(team.level, team.size);

            // [Effect] Create foes
            store.set_character_foes(registry.id, team_squad.id, characters.span());

            // [Effect] Search opponent league
            let team_league_id: u8 = LeagueTrait::compute_id(team_squad.rating);
            let team_league: League = store.league(registry.id, team_league_id);
            let foe_league_id = registry.search_league(team_league);

            // [Compute] Foe
            let mut foe_league = store.league(registry.id, foe_league_id);
            let foe_slot_id = foe_league.search_squad(team_squad.id.into());
            let foe_slot = store.slot(registry.id, foe_league_id, foe_slot_id);
            let mut foe_squad = store.squad(registry.id, foe_slot.squad_id);

            // [Effect] Remove foe slot, unsubscribe and update league
            store.remove_squad_slot(foe_squad);
            registry.unsubscribe(ref foe_league, ref foe_squad);
            store.set_league(foe_league);

            // [Effect] Fight and update players
            let mut foes = store.characters(foe_squad);
            let won = team
                .fight(
                    ref shop, ref team_squad, ref characters, ref foe_squad, ref foes, registry.seed
                );

            // [Effect] Update player, league and slot
            let league_id = LeagueTrait::compute_id(team_squad.rating);
            let mut league = store.league(registry.id, league_id);
            let slot = registry.subscribe(ref league, ref team_squad);
            store.set_league(league);
            store.set_slot(slot);
            store.set_squad(team_squad);

            // [Effect] Update Foe, league and slot
            let league_id = LeagueTrait::compute_id(foe_squad.rating);
            let mut league = store.league(registry.id, league_id);
            let slot = registry.subscribe(ref league, ref foe_squad);
            store.set_league(league);
            store.set_slot(slot);
            store.set_squad(foe_squad);

            // [Effect] Update shop
            store.set_shop(shop);

            // [Effect] Update team
            store.set_team(team);

            // [Effect] Update Registry
            store.set_registry(registry);

            // [Effect] Update Player win count if won
            if won {
                player.win_count += 1;
                store.set_player(player);
            }
        }
    }
}
