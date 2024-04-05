// Core imports

use core::array::ArrayTrait;

// Internal imports

use zklash::models::character::Character;
use zklash::types::role::RoleTrait;

#[generate_trait]
impl Fighter of FighterTrait {
    fn fight(ref team1: Array<Character>, ref team2: Array<Character>) {
        if team1.is_empty() {
            return;
        }

        if team2.is_empty() {
            return;
        }

        let mut char1 = team1.pop_front().unwrap();
        let mut char2 = team2.pop_front().unwrap();

        let (first1, first2) = Fighter::battle(ref team1, ref team2, char1, char2);
        Fighter::update(first1, ref team1);
        Fighter::update(first2, ref team2);
    }

    fn battle(
        ref team1: Array<Character>,
        ref team2: Array<Character>,
        mut char1: Character,
        mut char2: Character,
    ) -> (Character, Character) {
        if char1.health == 0 {
            if team1.is_empty() {
                return (char1, char2);
            }
            char1 = team1.pop_front().unwrap();
        }
        if char1.health == 0 {
            if team2.is_empty() {
                return (char1, char2);
            }
            char2 = team2.pop_front().unwrap();
        }
        Fighter::duel(ref char1, ref char2);
        Fighter::battle(ref team1, ref team2, char1, char2)
    }

    fn update(char: Character, ref team: Array<Character>) {
        let n = team.len();
        if char.health != 0 {
            team.append(char);
        }

        if team.is_empty() {
            return;
        }

        let mut index = 0;
        loop {
            if index == n {
                break;
            }
            team.append(team.pop_front().unwrap());
            index += 1;
        }
    }

    fn duel(ref char1: Character, ref char2: Character) {
        char1.health -= Fighter::hit(@char1, @char2);
        char2.health -= Fighter::hit(@char2, @char1);
        if char1.health == 0 || char2.health == 0 {
            return;
        }
        Fighter::duel(ref char1, ref char2)
    }

    fn hit(defender: @Character, opponent: @Character) -> u8 {
        min(*defender.health, *opponent.attack)
    }
}

fn min(a: u8, b: u8) -> u8 {
    if a < b {
        return a;
    }
    return b;
}
