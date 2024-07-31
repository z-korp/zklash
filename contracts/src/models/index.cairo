#[derive(Copy, Drop, Serde, IntrospectPacked)]
#[dojo::model]
struct Character {
    #[key]
    player_id: felt252,
    #[key]
    team_id: u32,
    #[key]
    id: u8,
    role: u8,
    item: u8,
    xp: u8,
    level: u8,
    health: u8,
    attack: u8,
    absorb: u8,
    stun: u8,
}

#[derive(Copy, Drop, Serde, IntrospectPacked)]
#[dojo::model]
struct Foe {
    #[key]
    registry_id: u32,
    #[key]
    squad_id: u32,
    #[key]
    id: u8,
    role: u8,
    item: u8,
    level: u8,
    health: u8,
    attack: u8,
    absorb: u8,
    stun: u8,
}

#[derive(Copy, Drop, Serde, IntrospectPacked)]
#[dojo::model]
struct League {
    #[key]
    registry_id: u32,
    #[key]
    id: u8,
    size: u32,
}

#[derive(Copy, Drop, Serde, IntrospectPacked)]
#[dojo::model]
struct Player {
    #[key]
    id: felt252,
    name: felt252,
    team_count: u32,
    win_count: u32,
}

#[derive(Model, Copy, Drop, Serde, IntrospectPacked)]
#[dojo::model]
struct Registry {
    #[key]
    id: u32,
    squad_count: u32,
    leagues: felt252,
    seed: felt252,
}


#[derive(Copy, Drop, Serde, IntrospectPacked)]
#[dojo::model]
struct Shop {
    #[key]
    player_id: felt252,
    #[key]
    team_id: u32,
    reroll_cost: u8,
    item_count: u8,
    items: u32,
    role_count: u8,
    roles: u32,
}

#[derive(Copy, Drop, Serde, IntrospectPacked)]
#[dojo::model]
struct Slot {
    #[key]
    registry_id: u32,
    #[key]
    league_id: u8,
    #[key]
    index: u32,
    squad_id: u32,
}

#[derive(Copy, Drop, Serde, IntrospectPacked)]
#[dojo::model]
struct Squad {
    #[key]
    registry_id: u32,
    #[key]
    id: u32,
    league_id: u8,
    index: u32,
    rating: u32,
    size: u8,
    name: felt252,
}

#[derive(Copy, Drop, Serde, IntrospectPacked)]
#[dojo::model]
struct Team {
    #[key]
    player_id: felt252,
    #[key]
    id: u32,
    registry_id: u32,
    gold: u32,
    health: u32,
    level: u8,
    character_uuid: u8,
    battle_id: u8,
    foe_squad_id: u32,
}
