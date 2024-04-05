use starknet::ContractAddress;

#[derive(Model, Copy, Drop, Serde)]
struct Character {
    #[key]
    player_id: ContractAddress,
    #[key]
    team_id: u32,
    #[key]
    id: u16,
    role: u8,
    xp: u32,
    level: u8,
    health: u8,
    attack: u8,
}
