// Core imports

use core::debug::PrintTrait;
use core::poseidon::hades_permutation;

// Starknet imports

use starknet::ContractAddress;

// Internal imports

use zklash::constants;
use zklash::helpers::math::Math;
use zklash::types::item::{Item, ItemTrait};
use zklash::types::role::{Role, RoleTrait};
use zklash::types::phase::Phase;

#[derive(Model, Copy, Drop, Serde)]
struct Character {
    #[key]
    player_id: ContractAddress,
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

#[derive(Copy, Drop)]
struct Buff {
    health: u8,
    attack: u8,
    absorb: u8,
}

// Constants

const MAX_LEVEL: u8 = 3;

// Errors

mod errors {
    const CHARACTER_NOT_EXIST: felt252 = 'Character: does not exist';
    const CHARACTER_ALREADY_EXIST: felt252 = 'Character: already exist';
    const CHARACTER_INVALID_ROLE: felt252 = 'Character: invalid role';
    const CHARACTER_NOT_LEVELABLE: felt252 = 'Character: not levelable';
}

#[generate_trait]
impl CharacterImpl of CharacterTrait {
    #[inline(always)]
    fn new(player_id: ContractAddress, team_id: u32, id: u8, role: Role) -> Character {
        let level: u8 = 1;
        Character {
            player_id,
            team_id,
            id,
            role: role.into(),
            item: Item::None.into(),
            xp: 0,
            level,
            health: role.health(Phase::OnHire, level),
            attack: role.attack(Phase::OnHire, level),
            absorb: role.absorb(Phase::OnHire, level),
            stun: 0,
        }
    }

    #[inline(always)]
    fn from(id: u8, role: Role, level: u8, item: Item) -> Character {
        Character {
            player_id: Zeroable::zero(),
            team_id: 0,
            id,
            role: role.into(),
            item: item.into(),
            xp: 0,
            level,
            health: role.health(Phase::OnHire, level),
            attack: role.attack(Phase::OnHire, level),
            absorb: role.absorb(Phase::OnHire, level),
            stun: 0,
        }
    }

    #[inline(always)]
    fn health(ref self: Character) -> u8 {
        self.health
    }

    #[inline(always)]
    fn attack(ref self: Character) -> u8 {
        if self.stun > 0 {
            self.stun -= 1;
            return 0;
        }
        self.attack
    }

    #[inline(always)]
    fn absorb(ref self: Character) -> u8 {
        self.absorb
    }

    #[inline(always)]
    fn equip(ref self: Character, item: Item) {
        // [Effect] Remove the previous item's effect
        self.unequip();
        // [Effect] Equip and apply the new item's effect
        let buff = Buff {
            health: item.health(Phase::OnEquip),
            attack: item.attack(Phase::OnEquip),
            absorb: item.absorb(Phase::OnEquip),
        };
        self.buff(buff);
        self.item = item.into();
    }

    #[inline(always)]
    fn unequip(ref self: Character) {
        // [Effect] Update the item's effect
        let item: Item = self.item.into();
        let buff = Buff {
            health: item.health(Phase::OnEquip),
            attack: item.attack(Phase::OnEquip),
            absorb: item.absorb(Phase::OnEquip),
        };
        self.debuff(buff);
        self.item = Item::None.into();
    }

    #[inline(always)]
    fn xp(ref self: Character) {
        // [Check] Character is levelable
        self.assert_is_levelable();
        // [Effect] Level up the character
        self.xp += 1;
        if self.xp == self.level + 1 {
            self.level += 1;
            self.xp = 0;
        };
    }

    #[inline(always)]
    fn talent(ref self: Character, phase: Phase, battle_id: u8, tick: u32) -> (u8, u8, Buff) {
        // [Effect] Update the item's effect
        let role: Role = self.role.into();
        let buff = Buff {
            health: role.health(phase, self.level),
            attack: role.attack(phase, self.level),
            absorb: role.absorb(phase, self.level),
        };
        self.buff(buff);
        let damage = role.damage(phase, self.level);
        let stun = role.stun(phase, self.level);
        let next_buff = Buff {
            health: role.next_health(phase, self.level),
            attack: role.next_attack(phase, self.level),
            absorb: role.next_absorb(phase, self.level),
        };
        (damage, stun, next_buff)
    }

    #[inline(always)]
    fn usage(ref self: Character, phase: Phase, battle_id: u8, tick: u32) -> u8 {
        // [Effect] Update the item's effect
        let item: Item = self.item.into();
        let buff = Buff {
            health: item.health(phase), attack: item.attack(phase), absorb: item.absorb(phase),
        };
        self.buff(buff);
        self.item = item.usage(phase).into();
        // [Effect] Return the item damage
        let damage = item.damage(phase);
        damage
    }

    #[inline(always)]
    fn buff(ref self: Character, buff: Buff) {
        // [Effect] Apply buff
        self.health += buff.health;
        self.attack += buff.attack;
        self.absorb += buff.absorb;
    }

    #[inline(always)]
    fn debuff(ref self: Character, buff: Buff) {
        // [Effect] Apply debuff
        self.health -= buff.health;
        self.attack -= buff.attack;
        self.absorb -= buff.absorb;
    }

    #[inline(always)]
    fn stun(ref self: Character, stun: u8) -> u8 {
        // [Effect] Apply stun
        self.stun += stun;
        stun
    }

    #[inline(always)]
    fn take_damage(ref self: Character, mut damage: u8) -> u8 {
        // [Effect] Apply the damage to the character
        if damage > 0 {
            damage -= Math::min(damage, self.absorb);
            self.absorb = 0;
        }
        damage = Math::min(damage, self.health);
        self.health -= damage;
        damage
    }

    #[inline(always)]
    fn is_dead(self: Character) -> bool {
        self.health == 0
    }

    #[inline(always)]
    fn nullify(ref self: Character) {
        self.role = Role::None.into();
    }

    #[inline(always)]
    fn merge(ref self: Character, ref to: Character) {
        to.xp();
        self.nullify();
    }
}

impl ZeroableCharacter of core::Zeroable<Character> {
    #[inline(always)]
    fn zero() -> Character {
        Character {
            player_id: Zeroable::zero(),
            team_id: 0,
            id: 0,
            role: 0,
            item: 0,
            xp: 0,
            level: 0,
            health: 0,
            attack: 0,
            absorb: 0,
            stun: 0,
        }
    }

    #[inline(always)]
    fn is_zero(self: Character) -> bool {
        Role::None == self.role.into()
    }

    #[inline(always)]
    fn is_non_zero(self: Character) -> bool {
        !self.is_zero()
    }
}

impl ZeroableBuff of core::Zeroable<Buff> {
    #[inline(always)]
    fn zero() -> Buff {
        Buff { health: 0, attack: 0, absorb: 0, }
    }

    #[inline(always)]
    fn is_zero(self: Buff) -> bool {
        self.health == 0 && self.attack == 0 && self.absorb == 0
    }

    #[inline(always)]
    fn is_non_zero(self: Buff) -> bool {
        !self.is_zero()
    }
}

impl PartialEqCharacter of PartialEq<Character> {
    #[inline(always)]
    fn eq(lhs: @Character, rhs: @Character) -> bool {
        lhs.id == rhs.id
    }

    #[inline(always)]
    fn ne(lhs: @Character, rhs: @Character) -> bool {
        lhs.id != rhs.id
    }
}

#[generate_trait]
impl CharacterAssert of AssertTrait {
    #[inline(always)]
    fn assert_exists(self: Character) {
        assert(self.is_non_zero(), errors::CHARACTER_NOT_EXIST);
    }

    #[inline(always)]
    fn assert_not_exists(self: Character) {
        assert(self.is_zero(), errors::CHARACTER_ALREADY_EXIST);
    }

    #[inline(always)]
    fn assert_is_levelable(self: Character) {
        assert(self.level < MAX_LEVEL, errors::CHARACTER_NOT_LEVELABLE);
    }
}
