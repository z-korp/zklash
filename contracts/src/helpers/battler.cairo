// Core imports

use core::debug::PrintTrait;
use core::zeroable::Zeroable;
use core::array::ArrayTrait;

// Internal imports

use zklash::events::{Fighter, Hit, HitTrait, Stun, StunTrait, Absorb, AbsorbTrait, Usage, Talent,};
use zklash::models::character::{Character, CharacterTrait, Buff};
use zklash::types::phase::Phase;

#[generate_trait]
impl Battler of BattlerTrait {
    fn start(
        ref team1: Array<Character>, ref team2: Array<Character>, battle_id: u8,
    ) -> (
        bool, Array<Fighter>, Array<Hit>, Array<Stun>, Array<Absorb>, Array<Usage>, Array<Talent>
    ) {
        // [Compute] Start the battle
        let mut fighters: Array<Fighter> = array![];
        Battler::setup(team1.span(), ref fighters, battle_id);
        Battler::setup(team2.span(), ref fighters, battle_id);
        let mut hits: Array<Hit> = array![];
        let mut stuns: Array<Stun> = array![];
        let mut absorbs: Array<Absorb> = array![];
        let mut usages: Array<Usage> = array![];
        let mut talents: Array<Talent> = array![];
        let mut tick: u32 = 0;
        let win = Battler::battle(
            ref team1,
            ref team2,
            Zeroable::zero(),
            Zeroable::zero(),
            Zeroable::zero(),
            Zeroable::zero(),
            battle_id,
            ref tick,
            ref hits,
            ref stuns,
            ref absorbs,
            ref usages,
            ref talents,
        );
        (win, fighters, hits, stuns, absorbs, usages, talents)
    }

    fn setup(mut team: Span<Character>, ref events: Array<Fighter>, battle_id: u8) {
        let mut idx: u8 = 0;
        loop {
            match team.pop_front() {
                Option::Some(char) => {
                    let fighter = (*char).to_fighter(battle_id, idx);
                    events.append(fighter);
                    idx += 1;
                },
                Option::None => { break; },
            }
        };
    }

    fn battle(
        ref team1: Array<Character>,
        ref team2: Array<Character>,
        mut char1: Character,
        mut char2: Character,
        next_buff1: Buff,
        next_buff2: Buff,
        battle_id: u8,
        ref tick: u32,
        ref hits: Array<Hit>,
        ref stuns: Array<Stun>,
        ref absorbs: Array<Absorb>,
        ref usages: Array<Usage>,
        ref talents: Array<Talent>,
    ) -> bool {
        // [Compute] If fighter is dead then get the next fighter if available
        if char1.is_dead() {
            char1 = match team1.pop_front() {
                Option::Some(char) => char,
                Option::None => { return false; },
            };
            // [Effect] Apply effects on dispatch
            let (_, _, _, usage, talent) = Battler::apply_effects(
                ref char1, Phase::OnDispatch, battle_id, tick
            );
            usages.append(usage);
            talents.append(talent);
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
            let (_, _, _, usage, talent) = Battler::apply_effects(
                ref char2, Phase::OnDispatch, battle_id, tick
            );
            usages.append(usage);
            talents.append(talent);
            // [Effect] Apply floating buff
            char2.buff(next_buff2);
        };

        // [Compute] Fight until one of the fighter is dead
        tick += 1;
        let (buff1, buff2) = Battler::duel(
            ref char1,
            ref char2,
            battle_id,
            ref tick,
            ref hits,
            ref stuns,
            ref absorbs,
            ref usages,
            ref talents
        );

        // [Compute] Continue the battle
        Battler::battle(
            ref team1,
            ref team2,
            char1,
            char2,
            buff1,
            buff2,
            battle_id,
            ref tick,
            ref hits,
            ref stuns,
            ref absorbs,
            ref usages,
            ref talents
        )
    }

    fn duel(
        ref char1: Character,
        ref char2: Character,
        battle_id: u8,
        ref tick: u32,
        ref hits: Array<Hit>,
        ref stuns: Array<Stun>,
        ref absorbs: Array<Absorb>,
        ref usages: Array<Usage>,
        ref talents: Array<Talent>,
    ) -> (Buff, Buff) {
        // [Effect] Apply talent and item buff for char1
        let (damage1, stun1, _, usage, talent) = Battler::apply_effects(
            ref char1, Phase::OnFight, battle_id, tick
        );
        usages.append(usage);
        talents.append(talent);
        let (damage2, stun2, _, usage, talent) = Battler::apply_effects(
            ref char2, Phase::OnFight, battle_id, tick
        );
        usages.append(usage);
        talents.append(talent);

        // [Effect] Apply stun effects
        let stun = char1.stun(stun2);
        stuns.append(StunTrait::new(battle_id, tick, char1, char2, stun));
        let stun = char2.stun(stun1);
        stuns.append(StunTrait::new(battle_id, tick, char2, char1, stun));

        // [Compute] Receive damage from opponents
        let damage = char1.take_damage(char2.attack() + damage2);
        hits.append(HitTrait::new(battle_id, tick, char2, char1, damage));
        let damage = char2.take_damage(char1.attack() + damage1);
        hits.append(HitTrait::new(battle_id, tick, char1, char2, damage));

        // [Compute] Post mortem effects
        let (next_buff1, next_buff2) = if char1.is_dead() {
            tick += 1;
            let next_buff1: Buff = Battler::post_mortem(
                ref char2, ref char1, battle_id, tick, ref hits, ref usages, ref talents
            );
            let next_buff2: Buff = Battler::post_mortem(
                ref char1, ref char2, battle_id, tick, ref hits, ref usages, ref talents
            );
            (next_buff1, next_buff2)
        } else {
            tick += 1;
            let next_buff2: Buff = Battler::post_mortem(
                ref char2, ref char1, battle_id, tick, ref hits, ref usages, ref talents
            );
            let next_buff1: Buff = Battler::post_mortem(
                ref char1, ref char2, battle_id, tick, ref hits, ref usages, ref talents
            );
            (next_buff1, next_buff2)
        };

        // [Compute] Stop duel is one of the fighter is dead
        if char1.is_dead() || char2.is_dead() {
            return (next_buff1, next_buff2);
        }
        tick += 1;
        Battler::duel(
            ref char1,
            ref char2,
            battle_id,
            ref tick,
            ref hits,
            ref stuns,
            ref absorbs,
            ref usages,
            ref talents
        )
    }

    #[inline(always)]
    fn post_mortem(
        ref char: Character,
        ref foe: Character,
        battle_id: u8,
        tick: u32,
        ref hits: Array<Hit>,
        ref usages: Array<Usage>,
        ref talents: Array<Talent>
    ) -> Buff {
        // [Compute] On Death effects for char
        if char.is_dead() {
            // [Effect] Apply talent and item buff on death
            let (damage, stun, next_buff, usage, talent) = Battler::apply_effects(
                ref char, Phase::OnDeath, battle_id, tick
            );
            usages.append(usage);
            talents.append(talent);
            foe.stun(stun);
            foe.take_damage(damage);
            hits.append(HitTrait::new(battle_id, tick, char, foe, damage));
            next_buff
        } else {
            Zeroable::zero()
        }
    }

    #[inline(always)]
    fn apply_effects(
        ref char: Character, phase: Phase, battle_id: u8, tick: u32
    ) -> (u8, u8, Buff, Usage, Talent) {
        // [Effect] Apply talent and item buff for char
        let (talent_damage, stun, next_buff, talent) = char.talent(phase, battle_id, tick);
        let (item_damage, usage) = char.usage(phase, battle_id, tick);
        (talent_damage + item_damage, stun, next_buff, usage, talent)
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

    #[test]
    fn test_fighter_pumpkin_small() {
        let mut characters: Array<Character> = array![
            CharacterTrait::from(1, Role::Pawn, 1, Item::PumpkinSmall),
        ];
        let mut foes: Array<Character> = array![
            CharacterTrait::from(201, Role::Bomboblin, 1, Item::None),
            CharacterTrait::from(202, Role::Bomboblin, 1, Item::None),
        ];
        let (win, _, _, _, _, _, _) = Battler::start(ref characters, ref foes, 0);
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
        let (win, _, _, _, _, _, _) = Battler::start(ref characters, ref foes, 0);
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
        let (win, _, _, _, _, _, _) = Battler::start(ref characters, ref foes, 0);
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
        let (win, _, _, _, _, _, _) = Battler::start(ref characters, ref foes, 0);
        assert(win, 'Battler: invalid win status');
    }
}
