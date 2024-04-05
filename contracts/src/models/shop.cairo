use starknet::ContractAddress;

#[derive(Model, Copy, Drop, Serde)]
struct Shop {
    #[key]
    player_id: ContractAddress,
    #[key]
    team_id: u32,
    cost: u8,
    roles: u128,
    items: u128,
}
