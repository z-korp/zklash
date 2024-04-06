//! Events definition.

#[derive(Drop, starknet::Event)]
struct Hit {
    #[key]
    player_id: felt252,
    #[key]
    team_id: u32,
    #[key]
    battle_id: u8,
    tick: u32,
    from_character_id: u8,
    to_character_id: u8,
    damage: u8,
}

#[generate_trait]
impl HitImpl of HitTrait {
    #[inline(always)]
    fn new(tick: u32, from: u8, to: u8, damage: u8) -> Hit {
        Hit {
            player_id: 0,
            team_id: 0,
            battle_id: 0,
            tick: tick,
            from_character_id: from,
            to_character_id: to,
            damage,
        }
    }
}
