mod setup {
    // Core imports

    use core::debug::PrintTrait;

    // Starknet imports

    use starknet::ContractAddress;
    use starknet::testing::{set_contract_address};

    // Dojo imports

    use dojo::world::{IWorldDispatcherTrait, IWorldDispatcher};
    use dojo::utils::test::{spawn_test_world, deploy_contract};

    // Internal imports

    use zklash::models::player::Player;
    use zklash::models::team::Team;
    use zklash::models::shop::Shop;
    use zklash::models::char::Char;
    use zklash::models::registry::Registry;
    use zklash::models::league::League;
    use zklash::models::slot::Slot;
    use zklash::models::squad::Squad;
    use zklash::models::foe::Foe;
    use zklash::systems::account::{account, IAccountDispatcher, IAccountDispatcherTrait};
    use zklash::systems::battle::{battle, IBattleDispatcher, IBattleDispatcherTrait};
    use zklash::systems::market::{market, IMarketDispatcher, IMarketDispatcherTrait};

    // Constants

    fn PLAYER() -> ContractAddress {
        starknet::contract_address_const::<'PLAYER'>()
    }

    const PLAYER_NAME: felt252 = 'PLAYER';

    #[derive(Drop)]
    struct Systems {
        account: IAccountDispatcher,
        battle: IBattleDispatcher,
        market: IMarketDispatcher,
    }

    #[derive(Drop)]
    struct Context {
        player_id: felt252,
        player_name: felt252,
    }

    #[inline(always)]
    fn spawn_game() -> (IWorldDispatcher, Systems, Context) {
        // [Setup] World
        let mut models = core::array::ArrayTrait::new();
        models.append(zklash::models::index::player::TEST_CLASS_HASH);
        models.append(zklash::models::index::team::TEST_CLASS_HASH);
        models.append(zklash::models::index::shop::TEST_CLASS_HASH);
        models.append(zklash::models::index::char::TEST_CLASS_HASH);
        models.append(zklash::models::index::registry::TEST_CLASS_HASH);
        models.append(zklash::models::index::league::TEST_CLASS_HASH);
        models.append(zklash::models::index::slot::TEST_CLASS_HASH);
        models.append(zklash::models::index::squad::TEST_CLASS_HASH);
        models.append(zklash::models::index::foe::TEST_CLASS_HASH);
        let world = spawn_test_world("zklash", models);

        // [Setup] Systems
        let account_address = world
            .deploy_contract('account', account::TEST_CLASS_HASH.try_into().unwrap());
        let battle_address = world
            .deploy_contract('battle', battle::TEST_CLASS_HASH.try_into().unwrap());
        let market_address = world
            .deploy_contract('market', market::TEST_CLASS_HASH.try_into().unwrap());
        world.grant_writer(dojo::utils::bytearray_hash(@"zklash"), account_address);
        world.grant_writer(dojo::utils::bytearray_hash(@"zklash"), battle_address);
        world.grant_writer(dojo::utils::bytearray_hash(@"zklash"), market_address);

        let systems = Systems {
            account: IAccountDispatcher { contract_address: account_address },
            battle: IBattleDispatcher { contract_address: battle_address },
            market: IMarketDispatcher { contract_address: market_address },
        };

        // [Setup] Context
        set_contract_address(PLAYER());
        systems.account.create(PLAYER_NAME);
        let context = Context { player_id: PLAYER().into(), player_name: PLAYER_NAME, };

        // [Return]
        (world, systems, context)
    }
}
