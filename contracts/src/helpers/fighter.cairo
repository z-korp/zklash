// Core imports

use core::zeroable::Zeroable;
use core::array::ArrayTrait;

// Internal imports

use zklash::models::character::{Character, CharacterTrait, Buff};
use zklash::types::role::RoleTrait;
use zklash::types::item::{Item, ItemTrait};
use zklash::types::phase::Phase;

#[generate_trait]
impl Fighter of FighterTrait {
    fn fight(ref team1: Array<Character>, ref team2: Array<Character>) -> bool {
        // [Compute] Start the battle
        Fighter::battle(
            ref team1,
            ref team2,
            Zeroable::zero(),
            Zeroable::zero(),
            Zeroable::zero(),
            Zeroable::zero()
        )
    }

    fn battle(
        ref team1: Array<Character>,
        ref team2: Array<Character>,
        mut char1: Character,
        mut char2: Character,
        next_buff1: Buff,
        next_buff2: Buff,
    ) -> bool {
        // [Compute] If fighter is dead then get the next fighter if available
        if char1.is_dead() {
            char1 = match team1.pop_front() {
                Option::Some(char) => char,
                Option::None => { return false; },
            };
            // [Effect] Apply talent effects on dispatch
            char1.talent(Phase::OnDispatch);
            // [Effect] Apply item effects on dispatch
            char1.usage(Phase::OnDispatch);
            // [Effect] Apply floating buff
            char1.buff(next_buff1);
        };

        // [Compute] If fighter is dead then get the next fighter if available
        if char2.is_dead() {
            char2 = match team2.pop_front() {
                Option::Some(char) => char,
                Option::None => { return true; },
            };
            // [Effect] Apply talent effects on dispatch
            char2.talent(Phase::OnDispatch);
            // [Effect] Apply item effects on dispatch
            char2.usage(Phase::OnDispatch);
            // [Effect] Apply floating buff
            char2.buff(next_buff2);
        };

        // [Compute] Fight until one of the fighter is dead
        let (buff1, buff2) = Fighter::duel(ref char1, ref char2);

        // [Compute] Continue the battle
        Fighter::battle(ref team1, ref team2, char1, char2, buff1, buff2)
    }

    fn duel(ref char1: Character, ref char2: Character) -> (Buff, Buff) {
        // [Effect] Apply talent and item buff for char1
        let (talent_damage1, talent_stun1, _) = char1.talent(Phase::OnFight);
        let item_damage1 = char1.usage(Phase::OnFight);

        // [Effect] Apply talent and item buff for char2
        let (talent_damage2, talent_stun2, _) = char2.talent(Phase::OnFight);
        let item_damage2 = char2.usage(Phase::OnFight);

        // [Effect] Apply stun effects
        char1.stun(talent_stun2);
        char2.stun(talent_stun1);

        // [Compute] Receive damage from opponents
        char1.take_damage(char2.attack() + talent_damage2 + item_damage2);
        char2.take_damage(char1.attack() + talent_damage1 + item_damage1);

        // [Compute] On Death effects for char1
        let next_buff1: Buff = if char1.is_dead() {
            // [Effect] Apply talent and item buff for char1
            let (talent_damage1, talent_stun1, next_buff1) = char1.talent(Phase::OnDeath);
            let item_damage1 = char1.usage(Phase::OnDeath);
            char2.stun(talent_stun1);
            char2.take_damage(talent_damage1 + item_damage1);
            next_buff1
        } else {
            Zeroable::zero()
        };

        // [Compute] On Death effects for char2
        let next_buff2: Buff = if char2.is_dead() {
            // [Effect] Apply talent and item buff for char2
            let (talent_damage2, talent_stun2, next_buff2) = char2.talent(Phase::OnDeath);
            let item_damage2 = char2.usage(Phase::OnDeath);
            char1.stun(talent_stun2);
            char1.take_damage(talent_damage2 + item_damage2);
            next_buff2
        } else {
            Zeroable::zero()
        };

        // [Compute] Stop duel is one of the fighter is dead
        if char1.is_dead() || char2.is_dead() {
            return (next_buff1, next_buff2);
        }
        Fighter::duel(ref char1, ref char2)
    }

    fn hit(health: u8, damage: u8, absorb: u8) -> u8 {
        if health < damage {
            return health;
        }
        damage
    }
}

fn min(a: u8, b: u8) -> u8 {
    if a < b {
        return a;
    }
    b
}
