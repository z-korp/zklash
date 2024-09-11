// Core imports

use core::debug::PrintTrait;

// Starknet imports

use starknet::ContractAddress;

// Internal imports

use zklash::models::index::Char;
use zklash::constants;
use zklash::helpers::math::Math;
use zklash::types::item::{Item, ItemTrait};
use zklash::types::role::{Role, RoleTrait};
use zklash::types::phase::Phase;

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
    const CHARACTER_NOT_EXIST: felt252 = 'Char: does not exist';
    const CHARACTER_ALREADY_EXIST: felt252 = 'Char: already exist';
    const CHARACTER_INVALID_ROLE: felt252 = 'Char: invalid role';
    const CHARACTER_NOT_LEVELABLE: felt252 = 'Char: not levelable';
}

#[generate_trait]
impl CharImpl of CharTrait {
    #[inline(always)]
    fn new(player_id: felt252, team_id: u32, id: u8, role: Role) -> Char {
        let level: u8 = 1;
        Char {
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
    fn from(id: u8, role: Role, level: u8, item: Item) -> Char {
        Char {
            player_id: core::Zeroable::zero(),
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
    fn health(ref self: Char) -> u8 {
        self.health
    }

    #[inline(always)]
    fn attack(ref self: Char) -> u8 {
        if self.stun > 0 {
            self.stun -= 1;
            return 0;
        }
        self.attack
    }

    #[inline(always)]
    fn absorb(ref self: Char) -> u8 {
        self.absorb
    }

    #[inline(always)]
    fn equip(ref self: Char, item: Item) {
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
    fn unequip(ref self: Char) {
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
    fn xp(ref self: Char) {
        // [Check] Char is levelable
        self.assert_is_levelable();
        // [Effect] Level up the character
        self.xp += 1;
        if self.xp == self.level + 1 {
            self.level += 1;
            self.xp = 0;
        };
    }

    #[inline(always)]
    fn talent(ref self: Char, phase: Phase, tick: u32) -> (u8, u8, Buff) {
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
    fn usage(ref self: Char, phase: Phase, tick: u32) -> u8 {
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
    fn buff(ref self: Char, buff: Buff) {
        // [Effect] Apply buff
        self.health += buff.health;
        self.attack += buff.attack;
        self.absorb += buff.absorb;
    }

    #[inline(always)]
    fn debuff(ref self: Char, buff: Buff) {
        // [Effect] Apply debuff
        self.health -= buff.health;
        self.attack -= buff.attack;
        self.absorb -= buff.absorb;
    }

    #[inline(always)]
    fn stun(ref self: Char, stun: u8) -> u8 {
        // [Effect] Apply stun
        self.stun += stun;
        stun
    }

    #[inline(always)]
    fn take_damage(ref self: Char, mut damage: u8) -> u8 {
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
    fn is_dead(self: Char) -> bool {
        self.health == 0
    }

    #[inline(always)]
    fn nullify(ref self: Char) {
        self.role = Role::None.into();
    }

    #[inline(always)]
    fn merge(ref self: Char, ref to: Char) {
        to.xp();
        self.nullify();
    }
}

impl ZeroableChar of core::Zeroable<Char> {
    #[inline(always)]
    fn zero() -> Char {
        Char {
            player_id: core::Zeroable::zero(),
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
    fn is_zero(self: Char) -> bool {
        Role::None == self.role.into()
    }

    #[inline(always)]
    fn is_non_zero(self: Char) -> bool {
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

impl PartialEqChar of PartialEq<Char> {
    #[inline(always)]
    fn eq(lhs: @Char, rhs: @Char) -> bool {
        lhs.id == rhs.id
    }

    #[inline(always)]
    fn ne(lhs: @Char, rhs: @Char) -> bool {
        lhs.id != rhs.id
    }
}

#[generate_trait]
impl CharAssert of AssertTrait {
    #[inline(always)]
    fn assert_exists(self: Char) {
        assert(self.is_non_zero(), errors::CHARACTER_NOT_EXIST);
    }

    #[inline(always)]
    fn assert_not_exists(self: Char) {
        assert(self.is_zero(), errors::CHARACTER_ALREADY_EXIST);
    }

    #[inline(always)]
    fn assert_is_levelable(self: Char) {
        assert(self.level < MAX_LEVEL, errors::CHARACTER_NOT_LEVELABLE);
    }
}
