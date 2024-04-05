use starknet::ContractAddress;

#[derive(Model, Drop, Serde)]
struct Team {
    #[key]
    player_id: ContractAddress,
    #[key]
    id: u32,
    seed: felt252,
    gold: u32,
    health: u32,
    level: u8,
    characters: u64,
}
