use zklash::models::character::CharacterTrait;
use core::zeroable::Zeroable;
// Core imports

use core::array::ArrayTrait;

// Internal imports

use zklash::models::character::Character;
use zklash::types::role::RoleTrait;
use zklash::types::item::{Item, ItemTrait};
use zklash::types::phase::Phase;

#[generate_trait]
impl Fighter of FighterTrait {
    fn fight(ref team1: Array<Character>, ref team2: Array<Character>) -> bool {
        // [Compute] Start the battle
        Fighter::battle(ref team1, ref team2, Zeroable::zero(), Zeroable::zero())
    }

    fn battle(
        ref team1: Array<Character>,
        ref team2: Array<Character>,
        mut char1: Character,
        mut char2: Character,
    ) -> bool {
        // [Compute] If fighter is dead then get the next fighter if available
        if char1.is_dead() {
            char1 = match team1.pop_front() {
                Option::Some(char) => char,
                Option::None => { return false; },
            };

            // [Compute] Apply item effects on dispatch
            char1.buff(Phase::OnDispatch);
        };

        // [Compute] If fighter is dead then get the next fighter if available
        if char2.is_dead() {
            char2 = match team2.pop_front() {
                Option::Some(char) => char,
                Option::None => { return true; },
            };

            // [Compute] Apply item effects on dispatch
            char2.buff(Phase::OnDispatch);
        };

        // [Compute] Fight until one of the fighter is dead
        Fighter::duel(ref char1, ref char2);

        // [Compute] Continue the battle
        Fighter::battle(ref team1, ref team2, char1, char2)
    }

    fn duel(ref char1: Character, ref char2: Character) {
        // [Compute] Apply item effects on duel
        let damage1 = char1.buff(Phase::OnFight);
        let damage2 = char2.buff(Phase::OnFight);

        // [Compute] Receive damage from opponents
        char1.take_damage(char2.attack + damage2);
        char2.take_damage(char1.attack + damage1);

        // [Compute] Apply item effects on death if dead
        if char1.is_dead() {
            char1.buff(Phase::OnDeath);
        };
        if char2.is_dead() {
            char2.buff(Phase::OnDeath);
        };

        // [Compute] Stop duel is one of the fighter is dead
        if char1.is_dead() || char2.is_dead() {
            return;
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
