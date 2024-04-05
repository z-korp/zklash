// World

fn WORLD() -> starknet::ContractAddress {
    starknet::contract_address_const::<0x1>()
}

// Game

const DEFAULT_GOLD: u32 = 10;
const DEFAULT_HEALTH: u32 = 10;
const DEFAULT_LEVEL: u8 = 1;
