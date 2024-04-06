//! Events definition.

// Internal imports

use zklash::models::character::Character;

#[derive(Drop, starknet::Event)]
struct Fighter {
    #[key]
    player_id: felt252,
    #[key]
    team_id: u32,
    #[key]
    battle_id: u8,
    character_id: u8,
    index: u8,
    role: u8,
    item: u8,
    xp: u8,
    level: u8,
    health: u8,
    attack: u8,
    absorb: u8,
    stun: u8,
}

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
    fn new(battle_id: u8, tick: u32, from: Character, to: Character, damage: u8) -> Hit {
        Hit {
            player_id: from.player_id.into(),
            team_id: from.team_id.into(),
            battle_id,
            tick: tick,
            from_character_id: from.id,
            to_character_id: to.id,
            damage,
        }
    }
}

#[derive(Drop, starknet::Event)]
struct Stun {
    #[key]
    player_id: felt252,
    #[key]
    team_id: u32,
    #[key]
    battle_id: u8,
    tick: u32,
    from_character_id: u8,
    to_character_id: u8,
    value: u8,
}

#[generate_trait]
impl StunImpl of StunTrait {
    #[inline(always)]
    fn new(battle_id: u8, tick: u32, from: Character, to: Character, value: u8) -> Stun {
        Stun {
            player_id: from.player_id.into(),
            team_id: from.team_id,
            battle_id,
            tick: tick,
            from_character_id: from.id,
            to_character_id: from.id,
            value,
        }
    }
}

#[derive(Drop, starknet::Event)]
struct Absorb {
    #[key]
    player_id: felt252,
    #[key]
    team_id: u32,
    #[key]
    battle_id: u8,
    tick: u32,
    character_id: u8,
    value: u8,
}

#[generate_trait]
impl AbsorbImpl of AbsorbTrait {
    #[inline(always)]
    fn new(battle_id: u8, tick: u32, char: Character, value: u8) -> Absorb {
        Absorb {
            player_id: char.player_id.into(),
            team_id: char.team_id,
            battle_id: 0,
            tick: tick,
            character_id: char.id,
            value,
        }
    }
}

#[derive(Drop, starknet::Event)]
struct Usage {
    #[key]
    player_id: felt252,
    #[key]
    team_id: u32,
    #[key]
    battle_id: u8,
    tick: u32,
    character_id: u8,
    item: u8,
    new_item: u8,
    health: u8,
    attack: u8,
    absorb: u8,
    damage: u8,
}

#[generate_trait]
impl UsageImpl of UsageTrait {
    #[inline(always)]
    fn new(
        battle_id: u8,
        tick: u32,
        char: Character,
        item: u8,
        new_item: u8,
        health: u8,
        attack: u8,
        absorb: u8,
        damage: u8,
    ) -> Usage {
        Usage {
            player_id: char.player_id.into(),
            team_id: char.team_id,
            battle_id: battle_id,
            tick: tick,
            character_id: char.id,
            item,
            new_item,
            health,
            attack,
            absorb,
            damage,
        }
    }
}

#[derive(Drop, starknet::Event)]
struct Talent {
    #[key]
    player_id: felt252,
    #[key]
    team_id: u32,
    #[key]
    battle_id: u8,
    tick: u32,
    character_id: u8,
    role: u8,
    health: u8,
    attack: u8,
    absorb: u8,
    damage: u8,
    stun: u8,
    next_health: u8,
    next_attack: u8,
    next_absorb: u8,
}

#[generate_trait]
impl TalentImpl of TalentTrait {
    #[inline(always)]
    fn new(
        battle_id: u8,
        tick: u32,
        char: Character,
        health: u8,
        attack: u8,
        absorb: u8,
        damage: u8,
        stun: u8,
        next_health: u8,
        next_attack: u8,
        next_absorb: u8,
    ) -> Talent {
        Talent {
            player_id: char.player_id.into(),
            team_id: char.team_id,
            battle_id: battle_id,
            tick: tick,
            character_id: char.id,
            role: char.role,
            health,
            attack,
            absorb,
            damage,
            stun,
            next_health,
            next_attack,
            next_absorb,
        }
    }
}
