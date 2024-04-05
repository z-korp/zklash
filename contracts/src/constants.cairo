// World

fn WORLD() -> starknet::ContractAddress {
    starknet::contract_address_const::<0x1>()
}

// Game

const DEFAULT_GOLD: u32 = 10;
const DEFAULT_HEALTH: u32 = 10;
const DEFAULT_LEVEL: u8 = 1;
const DEFAULT_SHOP_PURCHASE_COST: u8 = 3;
const DEFAULT_SHOP_REROLL_COST: u8 = 1;
const DEFAULT_ITEM_COUNT: u8 = 1;
const DEFAULT_ROLE_COUNT: u8 = 3;
