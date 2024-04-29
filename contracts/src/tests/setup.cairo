mod setup {
    // Core imports

    use core::debug::PrintTrait;

    // Starknet imports

    use starknet::ContractAddress;
    use starknet::testing::{set_contract_address};

    // Dojo imports

    use dojo::world::{IWorldDispatcherTrait, IWorldDispatcher};
    use dojo::test_utils::{spawn_test_world, deploy_contract};

    // Internal imports

    use zklash::models::player::Player;
    use zklash::models::team::Team;
    use zklash::models::shop::Shop;
    use zklash::models::character::Character;
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
        player_id: ContractAddress,
        player_name: felt252,
    }

    #[inline(always)]
    fn spawn_game() -> (IWorldDispatcher, Systems, Context) {
        // [Setup] World
        let mut models = core::array::ArrayTrait::new();
        models.append(zklash::models::player::player::TEST_CLASS_HASH);
        models.append(zklash::models::team::team::TEST_CLASS_HASH);
        models.append(zklash::models::shop::shop::TEST_CLASS_HASH);
        models.append(zklash::models::character::character::TEST_CLASS_HASH);
        models.append(zklash::models::registry::registry::TEST_CLASS_HASH);
        models.append(zklash::models::league::league::TEST_CLASS_HASH);
        models.append(zklash::models::slot::slot::TEST_CLASS_HASH);
        models.append(zklash::models::squad::squad::TEST_CLASS_HASH);
        models.append(zklash::models::foe::foe::TEST_CLASS_HASH);
        let world = spawn_test_world(models);

        // [Setup] Systems
        let account_address = deploy_contract(account::TEST_CLASS_HASH, array![].span());
        let battle_address = deploy_contract(battle::TEST_CLASS_HASH, array![].span());
        let market_address = deploy_contract(market::TEST_CLASS_HASH, array![].span());
        let systems = Systems {
            account: IAccountDispatcher { contract_address: account_address },
            battle: IBattleDispatcher { contract_address: battle_address },
            market: IMarketDispatcher { contract_address: market_address },
        };

        // [Setup] Context
        set_contract_address(PLAYER());
        systems.account.create(world, PLAYER_NAME);
        let context = Context { player_id: PLAYER().into(), player_name: PLAYER_NAME, };

        // [Return]
        (world, systems, context)
    }
}
