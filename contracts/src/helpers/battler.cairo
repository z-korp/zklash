// Core imports

use core::debug::PrintTrait;
use core::zeroable::Zeroable;
use core::array::ArrayTrait;

// Internal imports

use zklash::models::character::{Character, CharacterTrait, Buff};
use zklash::types::phase::Phase;

#[generate_trait]
impl Battler of BattlerTrait {
    fn start(ref team1: Array<Character>, ref team2: Array<Character>) -> bool {
        // [Compute] Start the battle
        let mut tick: u32 = 0;
        Battler::battle(
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
        ref team1: Array<Character>,
        ref team2: Array<Character>,
        mut char1: Character,
        mut char2: Character,
        next_buff1: Buff,
        next_buff2: Buff,
        ref tick: u32,
    ) -> bool {
        // [Compute] If fighter is dead then get the next fighter if available
        if char1.is_dead() {
            char1 = match team1.pop_front() {
                Option::Some(char) => char,
                Option::None => { return false; },
            };
            // [Effect] Apply effects on dispatch
            Battler::apply_effects(ref char1, Phase::OnDispatch, tick);
            // [Effect] Apply floating buff
            char1.buff(next_buff1);
        };

        // [Compute] If fighter is dead then get the next fighter if available
        if char2.is_dead() {
            char2 = match team2.pop_front() {
                Option::Some(char) => char,
                Option::None => { return true; },
            };
            // [Effect] Apply effects on dispatch
            Battler::apply_effects(ref char2, Phase::OnDispatch, tick);
            // [Effect] Apply floating buff
            char2.buff(next_buff2);
        };

        // [Compute] Fight until one of the fighter is dead
        tick += 1;
        let (buff1, buff2) = Battler::duel(ref char1, ref char2, ref tick,);

        // [Compute] Continue the battle
        Battler::battle(ref team1, ref team2, char1, char2, buff1, buff2, ref tick,)
    }

    fn duel(ref char1: Character, ref char2: Character, ref tick: u32,) -> (Buff, Buff) {
        // [Effect] Apply talent and item buff for char1
        let (damage1, stun1, _) = Battler::apply_effects(ref char1, Phase::OnFight, tick);
        let (damage2, stun2, _) = Battler::apply_effects(ref char2, Phase::OnFight, tick);

        // [Effect] Apply stun effects
        char1.stun(stun2);
        char2.stun(stun1);

        // [Compute] Receive damage from opponents
        let damage = char2.attack() + damage2;
        char1.take_damage(damage);
        let damage = char1.attack() + damage1;
        char2.take_damage(damage);

        // [Compute] Post mortem effects
        let (next_buff1, next_buff2) = if char1.is_dead() {
            tick += 1;
            let next_buff1: Buff = Battler::post_mortem(ref char1, ref char2, tick);
            let next_buff2: Buff = Battler::post_mortem(ref char2, ref char1, tick);
            (next_buff1, next_buff2)
        } else if char2.is_dead() {
            tick += 1;
            let next_buff2: Buff = Battler::post_mortem(ref char2, ref char1, tick);
            let next_buff1: Buff = Battler::post_mortem(ref char1, ref char2, tick);
            (next_buff1, next_buff2)
        } else {
            (Zeroable::zero(), Zeroable::zero())
        };

        // [Compute] Stop duel is one of the fighter is dead
        if char1.is_dead() || char2.is_dead() {
            return (next_buff1, next_buff2);
        }
        tick += 1;
        Battler::duel(ref char1, ref char2, ref tick,)
    }

    #[inline(always)]
    fn post_mortem(ref char: Character, ref foe: Character, tick: u32,) -> Buff {
        // [Compute] On Death effects for char
        if char.is_dead() {
            // [Effect] Apply talent and item buff on death
            let (damage, stun, next_buff) = Battler::apply_effects(ref char, Phase::OnDeath, tick);
            foe.stun(stun);
            foe.take_damage(damage);
            next_buff
        } else {
            Zeroable::zero()
        }
    }

    #[inline(always)]
    fn apply_effects(ref char: Character, phase: Phase, tick: u32) -> (u8, u8, Buff) {
        // [Effect] Apply talent and item buff for char
        let (talent_damage, stun, next_buff) = char.talent(phase, tick);
        let item_damage = char.usage(phase, tick);
        (talent_damage + item_damage, stun, next_buff)
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

    use super::{Battler, Character, CharacterTrait, Phase};

    // Constants

    #[test]
    fn test_fighter_pumpkin_small() {
        let mut characters: Array<Character> = array![
            CharacterTrait::from(1, Role::Pawn, 1, Item::PumpkinSmall),
        ];
        let mut foes: Array<Character> = array![
            CharacterTrait::from(201, Role::Bomboblin, 1, Item::None),
            CharacterTrait::from(202, Role::Bomboblin, 1, Item::None),
        ];
        let win = Battler::start(ref characters, ref foes);
        assert(!win, 'Battler: invalid win status');
    }

    #[test]
    fn test_fighter_pumpkin_medium() {
        let mut characters: Array<Character> = array![
            CharacterTrait::from(1, Role::Pawn, 1, Item::PumpkinMedium),
        ];
        let mut foes: Array<Character> = array![
            CharacterTrait::from(201, Role::Bomboblin, 1, Item::None),
            CharacterTrait::from(202, Role::Bomboblin, 1, Item::None),
        ];
        let win = Battler::start(ref characters, ref foes);
        assert(win, 'Battler: invalid win status');
    }

    #[test]
    fn test_fighter_mushroom_large() {
        let mut characters: Array<Character> = array![
            CharacterTrait::from(1, Role::Pawn, 1, Item::MushroomSmall),
        ];
        let mut foes: Array<Character> = array![
            CharacterTrait::from(201, Role::Bomboblin, 1, Item::None),
        ];
        let win = Battler::start(ref characters, ref foes);
        assert(!win, 'Battler: invalid win status');
    }

    #[test]
    fn test_fighter_pawn_talent() {
        let mut characters: Array<Character> = array![
            CharacterTrait::from(1, Role::Pawn, 2, Item::None),
            CharacterTrait::from(2, Role::Knight, 1, Item::None),
        ];
        let mut foes: Array<Character> = array![
            CharacterTrait::from(201, Role::Torchoblin, 1, Item::None),
        ];
        let win = Battler::start(ref characters, ref foes);
        assert(win, 'Battler: invalid win status');
    }
}
