// Core imports

use core::debug::PrintTrait;
use core::zeroable::Zeroable;
use core::array::ArrayTrait;

// Internal imports

use zklash::models::char::{Char, CharTrait, Buff, ZeroableChar};
use zklash::types::phase::Phase;

#[generate_trait]
impl Battler of BattlerTrait {
    fn start(ref team1: Array<Char>, ref team2: Array<Char>) -> bool {
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
    ) -> bool {
        // [Compute] If fighter is dead then get the next fighter if available
        if char1.is_dead() {
            char1 = match team1.pop_front() {
                Option::Some(char) => char,
                Option::None => { return false; },
            };
            // [Effect] Apply effects on dispatch
            Self::apply_effects(ref char1, Phase::OnDispatch, tick);
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
            Self::apply_effects(ref char2, Phase::OnDispatch, tick);
            // [Effect] Apply floating buff
            char2.buff(next_buff2);
        };

        // [Compute] Fight until one of the fighter is dead
        tick += 1;
        let (buff1, buff2) = Self::duel(ref char1, ref char2, ref tick,);

        // [Compute] Continue the battle
        Self::battle(ref team1, ref team2, char1, char2, buff1, buff2, ref tick,)
    }

    fn duel(ref char1: Char, ref char2: Char, ref tick: u32,) -> (Buff, Buff) {
        // [Effect] Apply talent and item buff for char1
        let (damage1, stun1, _) = Self::apply_effects(ref char1, Phase::OnFight, tick);
        let (damage2, stun2, _) = Self::apply_effects(ref char2, Phase::OnFight, tick);

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
            let next_buff1: Buff = Self::post_mortem(ref char1, ref char2, tick);
            let next_buff2: Buff = Self::post_mortem(ref char2, ref char1, tick);
            (next_buff1, next_buff2)
        } else if char2.is_dead() {
            tick += 1;
            let next_buff2: Buff = Self::post_mortem(ref char2, ref char1, tick);
            let next_buff1: Buff = Self::post_mortem(ref char1, ref char2, tick);
            (next_buff1, next_buff2)
        } else {
            (Zeroable::zero(), Zeroable::zero())
        };

        // [Compute] Stop duel is one of the fighter is dead
        if char1.is_dead() || char2.is_dead() {
            return (next_buff1, next_buff2);
        }
        tick += 1;
        Self::duel(ref char1, ref char2, ref tick,)
    }

    #[inline(always)]
    fn post_mortem(ref char: Char, ref foe: Char, tick: u32,) -> Buff {
        // [Compute] On Death effects for char
        if char.is_dead() {
            // [Effect] Apply talent and item buff on death
            let (damage, stun, next_buff) = Self::apply_effects(ref char, Phase::OnDeath, tick);
            foe.stun(stun);
            foe.take_damage(damage);
            next_buff
        } else {
            Zeroable::zero()
        }
    }

    #[inline(always)]
    fn apply_effects(ref char: Char, phase: Phase, tick: u32) -> (u8, u8, Buff) {
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

    use super::{Battler, Char, CharTrait, ZeroableChar, Phase};

    // Constants

    #[test]
    fn test_fighter_pumpkin_small() {
        let mut characters: Array<Char> = array![
            CharTrait::from(1, Role::Pawn, 1, Item::PumpkinSmall),
        ];
        let mut foes: Array<Char> = array![
            CharTrait::from(201, Role::Bomboblin, 1, Item::None),
            CharTrait::from(202, Role::Bomboblin, 1, Item::None),
        ];
        let win = Battler::start(ref characters, ref foes);
        assert(!win, 'Battler: invalid win status');
    }

    #[test]
    fn test_fighter_pumpkin_medium() {
        let mut characters: Array<Char> = array![
            CharTrait::from(1, Role::Pawn, 1, Item::PumpkinMedium),
        ];
        let mut foes: Array<Char> = array![
            CharTrait::from(201, Role::Bomboblin, 1, Item::None),
            CharTrait::from(202, Role::Bomboblin, 1, Item::None),
        ];
        let win = Battler::start(ref characters, ref foes);
        assert(win, 'Battler: invalid win status');
    }

    #[test]
    fn test_fighter_mushroom_large() {
        let mut characters: Array<Char> = array![
            CharTrait::from(1, Role::Pawn, 1, Item::MushroomSmall),
        ];
        let mut foes: Array<Char> = array![CharTrait::from(201, Role::Bomboblin, 1, Item::None),];
        let win = Battler::start(ref characters, ref foes);
        assert(!win, 'Battler: invalid win status');
    }

    #[test]
    fn test_fighter_pawn_talent() {
        let mut characters: Array<Char> = array![
            CharTrait::from(1, Role::Pawn, 2, Item::None),
            CharTrait::from(2, Role::Knight, 1, Item::None),
        ];
        let mut foes: Array<Char> = array![CharTrait::from(201, Role::Torchoblin, 1, Item::None),];
        let win = Battler::start(ref characters, ref foes);
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
        let win = Battler::battle(
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
        let win = Battler::battle(
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
        let win = Battler::battle(
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
        let win = Battler::battle(
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
        let win = Battler::battle(
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
            CharTrait::from(2, Role::Bomboblin, 2, Item::MushroomMedium),
            CharTrait::from(3, Role::Dynamoblin, 2, Item::None),
            CharTrait::from(4, Role::Torchoblin, 2, Item::MushroomSmall),
        ];

        let mut tick: u32 = 0;
        let win = Battler::battle(
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
