// Starknet imports

use starknet::ContractAddress;

// Dojo imports

use dojo::world::IWorldDispatcher;

#[dojo::interface]
trait IBattle<TContractState> {
    fn hydrate(ref world: IWorldDispatcher);
    fn start(ref world: IWorldDispatcher, team_id: u32, order: u32,);
}

#[dojo::contract]
mod battle {
    // Core imports

    use zklash::models::registry::RegistryTrait;
    use zklash::store::StoreTrait;
    use core::array::ArrayTrait;
    use core::debug::PrintTrait;

    // Starknet imports

    use starknet::ContractAddress;
    use starknet::info::{get_caller_address, get_block_timestamp};

    // Internal imports

    use zklash::constants::{DEFAULT_REGISTRY_ID, IDS_SIZE};
    use zklash::models::index::{League, Player, Team, Shop, Foe, Slot, Squad, Character};
    use zklash::store::{Store, StoreImpl};
    use zklash::helpers::packer::Packer;
    use zklash::helpers::array::ArrayTraitExt;
    use zklash::models::registry::{Registry, RegistryImpl, RegistryAssert};
    use zklash::models::league::{LeagueTrait, LeagueAssert};
    use zklash::models::squad::{SquadImpl, SquadAssert};
    use zklash::models::foe::FoeImpl;
    use zklash::models::player::{PlayerImpl, PlayerAssert};
    use zklash::models::team::{TeamImpl, TeamAssert};
    use zklash::models::shop::{ShopImpl, ShopAssert};
    use zklash::models::character::{CharacterImpl, CharacterAssert, PartialEqCharacter};
    use zklash::types::wave::{Wave, WaveTrait};

    // Component imports

    use zklash::components::initializable::InitializableComponent;

    // Local imports

    use super::IBattle;

    // Errors

    mod errors {
        const CHARACTER_DUPLICATE: felt252 = 'Battle: character duplicate';
    }

    // Implementations
    #[abi(embed_v0)]
    impl BattleImpl of IBattle<ContractState> {
        fn hydrate(ref world: IWorldDispatcher) {
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
                        let mut squad: Squad = registry.create_squad(level, size, 'zKorp');
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

        fn start(ref world: IWorldDispatcher, team_id: u32, order: u32,) {
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
            let mut character_ids = Packer::unpack(order, IDS_SIZE);
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
            let team_size: u8 = characters.len().try_into().unwrap();
            let mut team_squad = registry.create_squad(team.level, team_size, player.name);

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
