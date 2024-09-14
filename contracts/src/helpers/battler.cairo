// Core imports

use core::debug::PrintTrait;
use core::zeroable::Zeroable;
use core::array::ArrayTrait;

// Internal imports

use zklash::models::char::{
    Char, CharTrait, Buff, EffectResult, EffectResultDisplay, ZeroableChar, CharDisplay,
    ZeroableBuff, BuffAdd
};
use zklash::types::role::{Role, RoleIntoByteArray};
use zklash::types::phase::Phase;

#[generate_trait]
impl Battler of BattlerTrait {
    fn start(ref team1: Array<Char>, ref team2: Array<Char>) -> (bool, u32, u32, Char) {
        // [Compute] Start the battle
        let mut tick: u32 = 0;
        Self::battle(
            ref team1,
            ref team2,
            Zeroable::zero(),
            Zeroable::zero(),
            Zeroable::zero(),
            Zeroable::zero(),
            ref tick,
        )
    }

    fn battle(
        ref team1: Array<Char>,
        ref team2: Array<Char>,
        mut char1: Char,
        mut char2: Char,
        next_buff1: Buff,
        next_buff2: Buff,
        ref tick: u32,
    ) -> (bool, u32, u32, Char) {
        let char1_dead = char1.is_dead();
        let char2_dead = char2.is_dead();
        // [Compute] If fighter is dead then get the next fighter if available
        if char1_dead {
            println!("[char1] is dead");
            char1 = match team1.pop_front() {
                Option::Some(char) => char,
                Option::None => {
                    let survivors1 = 0;
                    let survivors2 = team2.len();
                    // If char2 is also dead, get the next character from team2
                    let winning_char = if char2_dead {
                        team2.pop_front().unwrap_or(char2)
                    } else {
                        char2
                    };
                    // Count char2 if it's alive
                    let survivors2 = survivors2 + if !char2_dead {
                        1
                    } else {
                        0
                    };
                    return (false, survivors1, survivors2, winning_char);
                },
            };
            println!("[char1] new spawn: {}", char1);

            // [Effect] Apply effects on dispatch
            Self::get_effects_and_use_item(ref char1, Phase::OnDispatch, tick);

            // [Effect] Apply floating buff
            if (!next_buff1.is_zero()) {
                println!("[char1] Apply floating buff: {}", next_buff1);
            }
            char1.buff(next_buff1);
        };

        // [Compute] If fighter is dead then get the next fighter if available
        if char2_dead {
            println!("[char2] is dead");
            char2 = match team2.pop_front() {
                Option::Some(char) => char,
                Option::None => {
                    let survivors1 = team1.len();
                    let survivors2 = 0;
                    // Count char1 if it's alive
                    let survivors1 = survivors1 + if !char1_dead {
                        1
                    } else {
                        0
                    };
                    return (true, survivors1, survivors2, char1);
                },
            };
            println!("[char2] new spawn: {}", char2);

            // [Effect] Apply effects on dispatch
            Self::get_effects_and_use_item(ref char2, Phase::OnDispatch, tick);

            // [Effect] Apply floating buff
            if (!next_buff2.is_zero()) {
                println!("[char2] Apply floating buff: {}", next_buff2);
            }
            char2.buff(next_buff2);
        };

        // TBD: stun, dmg, buff from OnDispatch?

        // [Compute] Fight until one of the fighter is dead
        tick += 1;
        let (buff1, buff2) = Self::duel(ref char1, ref char2, ref tick,);

        // [Compute] Continue the battle
        Self::battle(ref team1, ref team2, char1, char2, buff1, buff2, ref tick,)
    }

    fn duel(ref char1: Char, ref char2: Char, ref tick: u32,) -> (Buff, Buff) {
        println!("");
        println!("DUEL[{}] start\t{}\tvs\t{}", tick, char1, char2);

        let mut accumulated_buff1: Buff = Zeroable::zero();
        let mut accumulated_buff2: Buff = Zeroable::zero();

        // [Effect] Calculate talent and item buff for both characters
        // Talent OnFight example:
        // Item OnFight example: rock damage
        let char1_effects = Self::get_effects_and_use_item(ref char1, Phase::OnFight, tick);
        let char2_effects = Self::get_effects_and_use_item(ref char2, Phase::OnFight, tick);

        // Apply stun effects simultaneously
        // For now, no stun in OnFight phase
        char1.stun(char2_effects.stun);
        char2.stun(char1_effects.stun);

        // Apply damage simultaneously

        // ---------------------------
        // ITEMS DAMAGE
        if (!char2_effects.is_zero()) {
            println!("DUEL[{}] [char1] take {} (item_dmg)", tick, char2_effects.item_dmg);
        }
        char1.take_damage(char2_effects.item_dmg);
        if (!char1_effects.is_zero()) {
            println!("DUEL[{}] [char2] take {} (item_dmg)", tick, char1_effects.item_dmg);
        }
        char2.take_damage(char1_effects.item_dmg);

        // Handle post-mortem effects and potential revivals
        let (next_buff1, next_buff2) = Self::handle_post_mortem_loop(
            ref char1, ref char2, ref tick
        );
        accumulated_buff1 = accumulated_buff1 + next_buff1;
        accumulated_buff2 = accumulated_buff2 + next_buff2;

        // Check end conditions
        if char1.is_dead() || char2.is_dead() {
            println!("DUEL[{}] [End after item dmg]\t{}\tvs\t{}", tick, char1, char2);
            return (accumulated_buff1, accumulated_buff2);
        }

        // ---------------------------
        // TALENT DAMAGE
        println!(
            "DUEL[{}] [char1] take {} dmg ({} talent_dmg + {} attack)",
            tick,
            char2_effects.talent_dmg + char2.attack(),
            char2_effects.talent_dmg,
            char2.attack()
        );
        char1.take_damage(char2.attack() + char2_effects.talent_dmg);
        println!(
            "DUEL[{}] [char2] take {} dmg ({} talent_dmg + {} attack)",
            tick,
            char1_effects.talent_dmg + char1.attack(),
            char1_effects.talent_dmg,
            char1.attack()
        );
        char2.take_damage(char1.attack() + char1_effects.talent_dmg);

        // Handle post-mortem effects and potential revivals
        let (next_buff1, next_buff2) = Self::handle_post_mortem_loop(
            ref char1, ref char2, ref tick
        );
        accumulated_buff1 = accumulated_buff1 + next_buff1;
        accumulated_buff2 = accumulated_buff2 + next_buff2;

        // Check end conditions
        if char1.is_dead() || char2.is_dead() {
            println!("DUEL[{}] [End after talent dmg]\t{}\tvs\t{}", tick, char1, char2);
            return (accumulated_buff1, accumulated_buff2);
        }

        println!("DUEL[{}] end\t{}\tvs\t{}", tick, char1, char2);

        tick += 1;

        // Both survived, continue to next round
        // Recursive call to duel
        let (nextbuff_1, nextbuff_2) = Self::duel(ref char1, ref char2, ref tick);

        // Accumulate buffs from the recursive call
        (accumulated_buff1 + nextbuff_1, accumulated_buff2 + nextbuff_2)
    }

    fn handle_post_mortem_loop(ref char1: Char, ref char2: Char, ref tick: u32) -> (Buff, Buff) {
        let mut next_buff1: Buff = Zeroable::zero();
        let mut next_buff2: Buff = Zeroable::zero();

        // if no one is dead, return
        if (!char1.is_dead() && !char2.is_dead()) {
            return (next_buff1, next_buff2);
        }

        if (char1.is_dead()) {
            println!("DUEL[{}] [PMLoop] [char1] died", tick);
        }
        if (char2.is_dead()) {
            println!("DUEL[{}] [PMLoop] [char2] died", tick);
        }

        loop {
            let char1_was_alive = !char1.is_dead();
            let char2_was_alive = !char2.is_dead();

            let (char1_effects, char2_effects) = Self::calculate_post_mortem_effects_and_use_item(
                ref char1, ref char2, tick
            );

            Self::apply_post_mortem_effects(
                ref char1, ref char2, char1_effects, char2_effects, tick
            );

            next_buff1 = next_buff1 + char1_effects.next_buff;
            next_buff2 = next_buff2 + char2_effects.next_buff;

            // Check if both characters are alive now
            if (!char1.is_dead() && !char2.is_dead()) {
                break;
            }

            // Check for new deaths
            if ((char1_was_alive && char1.is_dead()) || (char2_was_alive && char2.is_dead())) {
                if (char1.is_dead()) {
                    println!("DUEL[{}] [PMLoop] [char1] died from post mortem", tick);
                }
                if (char2.is_dead()) {
                    println!("DUEL[{}] [PMLoop] [char2] died from post mortem", tick);
                }
                continue;
            }
            // Check for new revivals
            if ((!char1_was_alive && !char1.is_dead()) || (!char2_was_alive && !char2.is_dead())) {
                if (!char1.is_dead()) {
                    println!("DUEL[{}] [PMLoop] [char1] revived from post mortem", tick);
                }
                if (!char2.is_dead()) {
                    println!("DUEL[{}] [PMLoop] [char2] revived from post mortem", tick);
                }
                continue;
            }

            // If we reach here, no new deaths or revivals occurred
            break;
        };

        if (!next_buff1.is_zero() || !next_buff2.is_zero()) {
            println!(
                "DUEL[{}] [PMLoop] next_buff1: {} \t next_buff2: {}", tick, next_buff1, next_buff2
            );
        }
        (next_buff1, next_buff2)
    }

    fn calculate_post_mortem_effects_and_use_item(
        ref char1: Char, ref char2: Char, tick: u32
    ) -> (EffectResult, EffectResult) {
        let mut char1_effects = Zeroable::zero();
        let mut char2_effects = Zeroable::zero();
        if char1.is_dead() {
            char1_effects = Self::get_effects_and_use_item(ref char1, Phase::OnDeath, tick);
            if (!char1.is_dead()) {
                println!("DUEL[{}] [PMLoop] [char1] revived", tick);
            }
            if (!char1_effects.is_zero()) {
                println!("DUEL[{}] [PMLoop] [char1] post-mortem effects: {}", tick, char1_effects);
            }
        }
        if char2.is_dead() {
            char2_effects = Self::get_effects_and_use_item(ref char2, Phase::OnDeath, tick);
            if (!char2_effects.is_zero()) {
                println!("DUEL[{}] [PMLoop] [char2] post-mortem effects: {}", tick, char2_effects);
            }
        }

        (char1_effects, char2_effects)
    }

    fn apply_post_mortem_effects(
        ref char1: Char,
        ref char2: Char,
        char1_effects: EffectResult,
        char2_effects: EffectResult,
        tick: u32
    ) {
        // Apply
        // - talent OnDeath stun,
        // - talent OnDeath damage,
        // - item OnDeath damage
        Self::apply_post_mortem_on_foe(
            ref char2,
            ref char1,
            char2_effects.stun,
            char2_effects.talent_dmg,
            char2_effects.item_dmg,
            tick
        );
        Self::apply_post_mortem_on_foe(
            ref char1,
            ref char2,
            char1_effects.stun,
            char1_effects.talent_dmg,
            char1_effects.item_dmg,
            tick
        );
    }

    fn apply_post_mortem_on_foe(
        ref char: Char, ref foe: Char, stun: u8, talent_dmg: u8, item_dmg: u8, tick: u32
    ) {
        let role_foe: Role = foe.role.into(); // foe.role is a u8
        let role_foe_str: ByteArray = role_foe.into();

        let role_char: Role = char.role.into(); // char.role is a u8
        let role_char_str: ByteArray = role_char.into();

        if (stun > 0 || talent_dmg > 0 || item_dmg > 0) {
            println!(
                "DUEL[{}] [PMLoop] {} apply_post_mortem on {}: stun: {} | talent_dmg: {} | item_dmg: {}",
                tick,
                role_char_str,
                role_foe_str,
                stun,
                talent_dmg,
                item_dmg
            );
        }
        foe.stun(stun);
        foe.take_damage(talent_dmg);
        foe.take_damage(item_dmg);
    }

    // was previously called apply_effects
    fn get_effects_and_use_item(ref char: Char, phase: Phase, tick: u32) -> EffectResult {
        // [Effect] Apply talent and item buff for char
        let (talent_dmg, stun, next_buff) = char.talent(phase, tick);
        let item_dmg = char.usage(phase, tick);

        // if (talent_dmg > 0 || item_dmg > 0 || stun > 0 || !next_buff.is_zero()) {
        //     println!(
        //         "\t\t[Char] get_effects_and_use_item: talent_dmg: {} | item_dmg: {} | stun: {} |
        //         next_buff: {}", talent_dmg,
        //         item_dmg,
        //         stun,
        //         next_buff
        //     );
        // }

        EffectResult { talent_dmg, item_dmg, stun, next_buff }
    }
}

#[cfg(test)]
mod tests {
    // Core imports

    use core::debug::PrintTrait;
    use core::zeroable::Zeroable;
    use core::traits::Default;

    // Internal imports

    use zklash::types::role::{Role, RoleTrait};
    use zklash::types::item::{Item, ItemTrait};

    // Local imports

    use super::{Battler, Char, CharTrait, ZeroableChar, Phase};

    // Constants

    #[test]
    fn test_fighter_basic() {
        let mut team1: Array<Char> = array![CharTrait::from(1, Role::Knight, 1, Item::None),];
        let mut team2: Array<Char> = array![CharTrait::from(201, Role::Knight, 1, Item::None),];
        let (win, survivors1, survivors2, _) = Battler::start(ref team1, ref team2);

        // Assert the battle outcome
        assert(!win, 'Team 1 should loose');
    }


    // Test the postmortem effect
    // Team 1: Knight (1A/3H) with Large Mushroom -> (1A/7H)
    // Team 2: Dynamoblin lvl 1 (3A/2H) (deal 1 damage on postmortem)
    // Duel 1: K(1A/7H) vs D(3A/2H) -> K(1A/4H) vs D(3A/1H)
    // Duel 2: K(1A/4H) vs D(3A/1H) -> K(1A/1H) vs D(3A/0H) -> D deal 1 damage to K -> K(1A/0H) vs
    // D(3A/0H)
    // Team 1 loose
    #[test]
    fn test_fighter_postmortem_effect() {
        let mut team1: Array<Char> = array![
            CharTrait::from(1, Role::Knight, 1, Item::MushroomLarge),
        ];
        let mut team2: Array<Char> = array![CharTrait::from(201, Role::Dynamoblin, 1, Item::None),];
        let (win, survivors1, survivors2, _) = Battler::start(ref team1, ref team2);

        // Assert the battle outcome
        assert(!win, 'Team 1 should loose');

        // Check the state of survivors
        assert(survivors1 == 0, 'Team 1 should have 0 survivor');
        assert(survivors2 == 0, 'Team 2 should have 0 survivor');
    }

    // Test pumpkin does trigger post mortem effect because it does revive the mob
    // First case: Pumpkin holder is killed by POST MORTEM
    // Team 1: [Dynamoblin(1) (3A/2H) (None)]
    // Team 2: [Pawn(1) (1A/2H) (None), Dynamoblin(1) (2A/3H) (PumpkinSmall)] (deal 1 damage on
    // postmortem)
    // Duel 1: D(3A/2H) vs P(1A/2H) -> D(3A/1H) vs P(1A/0H)
    // Duel 2: D(3A/1H) vs D(4A/3H) -> D(3A/0H) vs D(4A/0H) -> revive D(3A/0H) vs B(2A/1H)
    // Team 1 loose
    #[test]
    fn test_fighter_pumpkin_post_mortem_when_killed_by_post_mortem() {
        let mut team1: Array<Char> = array![CharTrait::from(1, Role::Dynamoblin, 1, Item::None),];
        let mut team2: Array<Char> = array![
            CharTrait::from(201, Role::Pawn, 1, Item::None),
            CharTrait::from(202, Role::Dynamoblin, 1, Item::PumpkinSmall),
        ];
        let (win, survivors1, survivors2, _) = Battler::start(ref team1, ref team2);

        // Assert the battle outcome
        assert(!win, 'Team 1 should loose');

        // Check the state of survivors
        assert(survivors1 == 0, 'Team 1 should have 0 survivor');
        assert(survivors2 == 0, 'Team 2 should have 0 survivor');
    }

    // Test pumpkin does trigger post mortem effect because it does revive the mob
    // Second case: Pumpkin holder is killed by ATTACK
    // Team 1: [Torchoblin(1) (1A/7H) (MushroomLarge), Knight(1) (1A/3H)]
    // Team 2: [Dynamoblin(1) (3A/2H) (PumpkinSmall)]  (deal 1 damage on postmortem)
    // Duel 1: T(1A/7H) vs D(3A/2H) -> T(1A/4H) vs D(3A/1H)
    // Duel 1: T(1A/4H) vs D(3A/1H) -> T(1A/1H) vs D(3A/0H) -> T(1A/0H) vs D(3A/0H) -> revive
    // T(1A/0H) vs D(3A/1H)
    // Duel 2: K(2A/3H) vs D(3A/1H) -> T(2A/0H) vs D(3A/0H)
    // Team 1 loose
    #[test]
    fn test_fighter_pumpkin_post_mortem_when_killed_by_attack() {
        let mut team1: Array<Char> = array![
            CharTrait::from(1, Role::Torchoblin, 1, Item::MushroomLarge),
            CharTrait::from(2, Role::Knight, 1, Item::None)
        ];
        let mut team2: Array<Char> = array![
            CharTrait::from(201, Role::Dynamoblin, 1, Item::PumpkinSmall),
        ];
        let (win, survivors1, survivors2, _) = Battler::start(ref team1, ref team2);

        // Assert the battle outcome
        assert(!win, 'Team 1 should loose');

        // Check the state of survivors
        assert(survivors1 == 0, 'Team 1 should have 0 survivor');
        assert(survivors2 == 0, 'Team 2 should have 0 survivor');
    }

    // Test pumpkin trigger post mortem effect because it does revive the mob
    // Team 1: [Torchoblin lvl 1 (1A/4H) (PumpkinSmall), Knight lvl 1 (1A/3H)]
    // Team 2: [Dynamoblin lvl 1 (3A/5H) (MushroomLarge)]
    // Duel 1: T(1A/4H) vs D(3A/5H) -> T(1A/1H) vs D(3A/4H)
    // Duel 2: T(1A/1H) vs D(3A/4H) -> T(1A/0H) vs D(3A/3H) -> T(1A/1H) vs D(3A/3H) -> T boost
    // Knight from 1A to 2A
    // Duel 3: T(1A/1H) vs D(3A/3H) -> T(1A/0H) vs D(3A/2H) -> T boost Knight
    // from 2A to 3A
    // Duel 4: K(3A/3H) vs D(3A/2H) -> K(3A/0H) vs D(3A/0H)
    // Team 1 loose
    #[test]
    fn test_fighter_pumpkin_trigger_postmortem_twice() {
        let mut team1: Array<Char> = array![
            CharTrait::from(1, Role::Torchoblin, 1, Item::PumpkinSmall),
            CharTrait::from(2, Role::Knight, 1, Item::None)
        ];
        let mut team2: Array<Char> = array![
            CharTrait::from(201, Role::Dynamoblin, 1, Item::MushroomLarge),
        ];
        let (win, survivors1, survivors2, _) = Battler::start(ref team1, ref team2);

        // Assert the battle outcome
        assert(!win, 'Team 1 should loose');

        // Check the state of survivors
        assert(survivors1 == 0, 'Team 1 should have no survivors');
        assert(survivors2 == 0, 'Team 2 should have no survivors');
    }

    // Test stone does trigger post mortem effect
    // Team 1: [Knight lvl 1 (1A/4H) (RockLarge)]
    // Team 2: [Dynamoblin lvl 1 (3A/2H) (None)]
    // Duel 1: K(1A/4H) vs D(3A/2H) -> K(1A/4H) vs D(3A/0H) (rock large hit 3dmgs)
    // Duel 1: -> K(1A/3H) vs D(3A/0H)
    // Team 1 win
    #[test]
    fn test_fighter_stone_trigger_postmortem() {
        let mut team1: Array<Char> = array![CharTrait::from(1, Role::Knight, 1, Item::RockLarge),];
        let mut team2: Array<Char> = array![CharTrait::from(201, Role::Dynamoblin, 1, Item::None),];
        let (win, survivors1, survivors2, char_alive) = Battler::start(ref team1, ref team2);

        // Assert the battle outcome
        assert(win, 'Team 1 should win');

        // Check the state of survivors
        assert(survivors1 == 1, 'Team 1 should have 1 survivor');
        assert(survivors2 == 0, 'Team 2 should have 0 survivor');

        // Check the state of the survivor
        assert(char_alive.health == 3, 'Survivor should have 3H');
    }

    // Test stone that doesn't kill
    // Team 1: [Knight(1) (1A/4H) (RockSmall)]
    // Team 2: [Dynamoblin(1) (3A/2H) (None)]
    // Duel 1: K(1A/4H) vs D(3A/2H) -> K(1A/4H) vs D(3A/1H) (rock small hit 1dmg)
    // Duel 1: K(1A/4H) vs D(3A/1H) -> K(1A/1H) vs D(3A/0H)
    // Duel 1: -> K(1A/0H) vs D(3A/0H) postmortem effect
    // Team 1 win
    #[test]
    fn test_fighter_stone_doesnt_kill() {
        let mut team1: Array<Char> = array![CharTrait::from(1, Role::Knight, 1, Item::RockSmall),];
        let mut team2: Array<Char> = array![CharTrait::from(201, Role::Dynamoblin, 1, Item::None),];
        let (win, survivors1, survivors2, _) = Battler::start(ref team1, ref team2);

        // Assert the battle outcome
        assert(!win, 'Team 1 should loose');

        // Check the state of survivors
        assert(survivors1 == 0, 'Team 1 should have 0 survivor');
        assert(survivors2 == 0, 'Team 2 should have 0 survivor');
    }

    #[test]
    fn test_fighter_pawn_talent() {
        let mut characters: Array<Char> = array![
            CharTrait::from(1, Role::Pawn, 2, Item::None),
            CharTrait::from(2, Role::Knight, 1, Item::None),
        ];
        let mut foes: Array<Char> = array![CharTrait::from(201, Role::Torchoblin, 1, Item::None),];
        let (win, _, _, _) = Battler::start(ref characters, ref foes);
        assert(win, 'Battler: invalid win status');
    }

    #[test]
    fn test_battle_dual_stun() {
        let mut characters: Array<Char> = array![
            CharTrait::from(1, Role::Bowman, 2, Item::PumpkinSmall),
            CharTrait::from(2, Role::Pawn, 1, Item::None),
        ];
        let mut foes: Array<Char> = array![
            CharTrait::from(1, Role::Bowman, 2, Item::PumpkinSmall),
        ];
        let mut tick: u32 = 0;
        let (win, _, _, _) = Battler::battle(
            ref characters,
            ref foes,
            Zeroable::zero(),
            Zeroable::zero(),
            Zeroable::zero(),
            Zeroable::zero(),
            ref tick,
        );
        assert(win, 'Battler: invalid win status');
        assert(tick == 6, 'Battler: invalid tick count');
    }

    #[test]
    fn test_battle_dual_stun_reverse() {
        let mut characters: Array<Char> = array![
            CharTrait::from(1, Role::Bowman, 2, Item::PumpkinSmall),
        ];
        let mut foes: Array<Char> = array![
            CharTrait::from(1, Role::Bowman, 2, Item::PumpkinSmall),
            CharTrait::from(2, Role::Pawn, 1, Item::None),
        ];
        let mut tick: u32 = 0;
        let (win, _, _, _) = Battler::battle(
            ref characters,
            ref foes,
            Zeroable::zero(),
            Zeroable::zero(),
            Zeroable::zero(),
            Zeroable::zero(),
            ref tick,
        );
        assert(!win, 'Battler: invalid win status');
        assert(tick == 6, 'Battler: invalid tick count');
    }

    #[test]
    fn test_battle_knights_with_stone() {
        let mut characters: Array<Char> = array![
            CharTrait::from(1, Role::Knight, 1, Item::RockLarge),
            CharTrait::from(2, Role::Knight, 1, Item::None),
            CharTrait::from(3, Role::Knight, 1, Item::None),
        ];
        let mut foes: Array<Char> = array![
            CharTrait::from(1, Role::Dynamoblin, 1, Item::None),
            CharTrait::from(2, Role::Bomboblin, 1, Item::None),
        ];

        let mut tick: u32 = 0;
        let (win, _, _, _) = Battler::battle(
            ref characters,
            ref foes,
            Zeroable::zero(),
            Zeroable::zero(),
            Zeroable::zero(),
            Zeroable::zero(),
            ref tick,
        );
        assert(win, 'Battler: invalid win status');
    }

    #[test]
    fn test_battle_bug_win_loose_1() {
        let mut characters: Array<Char> = array![
            CharTrait::from(1, Role::Pawn, 3, Item::MushroomLarge),
            CharTrait::from(2, Role::Knight, 3, Item::PumpkinMedium),
            CharTrait::from(3, Role::Pawn, 3, Item::MushroomLarge),
            CharTrait::from(4, Role::Knight, 3, Item::PumpkinMedium),
        ];

        let mut foes: Array<Char> = array![
            CharTrait::from(1, Role::Bomboblin, 3, Item::BushLarge),
            CharTrait::from(2, Role::Torchoblin, 3, Item::MushroomMedium),
            CharTrait::from(3, Role::Dynamoblin, 3, Item::RockMedium),
            CharTrait::from(4, Role::Dynamoblin, 3, Item::PumpkinMedium),
        ];

        let mut tick: u32 = 0;
        let (win, _, _, _) = Battler::battle(
            ref characters,
            ref foes,
            Zeroable::zero(),
            Zeroable::zero(),
            Zeroable::zero(),
            Zeroable::zero(),
            ref tick,
        );
        assert(!win, 'Battler: invalid win status');
    }

    #[test]
    fn test_battle_bug_win_loose_2() {
        let mut characters: Array<Char> = array![
            CharTrait::from(1, Role::Pawn, 3, Item::MushroomLarge),
            CharTrait::from(2, Role::Knight, 3, Item::PumpkinMedium),
            CharTrait::from(3, Role::Pawn, 3, Item::MushroomLarge),
            CharTrait::from(4, Role::Knight, 3, Item::PumpkinMedium),
        ];

        let mut foes: Array<Char> = array![
            CharTrait::from(1, Role::Bomboblin, 3, Item::BushLarge),
            CharTrait::from(2, Role::Torchoblin, 3, Item::MushroomMedium),
            CharTrait::from(3, Role::Dynamoblin, 3, Item::RockMedium),
            CharTrait::from(4, Role::Dynamoblin, 3, Item::PumpkinMedium),
        ];

        let mut tick: u32 = 0;
        let (win, _, _, _) = Battler::battle(
            ref characters,
            ref foes,
            Zeroable::zero(),
            Zeroable::zero(),
            Zeroable::zero(),
            Zeroable::zero(),
            ref tick,
        );
        assert(!win, 'Battler: invalid win status');
    }

    #[test]
    fn test_battle_bug_win_loose_3() {
        let mut characters: Array<Char> = array![
            CharTrait::from(1, Role::Dynamoblin, 3, Item::RockSmall),
            CharTrait::from(2, Role::Torchoblin, 3, Item::None),
            CharTrait::from(3, Role::Pawn, 3, Item::BushMedium),
            CharTrait::from(4, Role::Knight, 3, Item::MushroomLarge),
        ];

        let mut foes: Array<Char> = array![
            CharTrait::from(1, Role::Dynamoblin, 2, Item::RockSmall),
            CharTrait::from(2, Role::Bomboblin, 2, Item::None),
            CharTrait::from(3, Role::Dynamoblin, 2, Item::None),
            CharTrait::from(4, Role::Torchoblin, 2, Item::MushroomSmall),
        ];

        let mut tick: u32 = 0;
        let (win, _, _, _) = Battler::battle(
            ref characters,
            ref foes,
            Zeroable::zero(),
            Zeroable::zero(),
            Zeroable::zero(),
            Zeroable::zero(),
            ref tick,
        );
        assert(win, 'Battler: invalid win status');
    }
}
