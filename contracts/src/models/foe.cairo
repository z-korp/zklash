// Internal imports

use zklash::constants::ZERO;
use zklash::models::character::Character;
use zklash::types::item::Item;
use zklash::types::role::{Role, RoleTrait};
use zklash::types::phase::Phase;

#[derive(Model, Copy, Drop, Serde)]
struct Foe {
    #[key]
    registry_id: u32,
    #[key]
    squad_id: u32,
    #[key]
    id: u8,
    role: u8,
    item: u8,
    level: u8,
    health: u8,
    attack: u8,
    absorb: u8,
    stun: u8,
}

impl FoeIntoCharacter of Into<Foe, Character> {
    #[inline(always)]
    fn into(self: Foe) -> Character {
        Character {
            player_id: ZERO(),
            team_id: self.squad_id,
            id: self.id,
            role: self.role,
            item: self.item,
            xp: 0,
            level: self.level,
            health: self.health,
            attack: self.attack,
            absorb: self.absorb,
            stun: self.stun,
        }
    }
}

#[generate_trait]
impl FoeImpl of FoeTrait {
    #[inline(always)]
    fn new(registry_id: u32, squad_id: u32, id: u8, role: Role, level: u8, item: Item) -> Foe {
        Foe {
            registry_id,
            squad_id,
            id,
            role: role.into(),
            item: item.into(),
            level,
            health: role.health(Phase::OnHire, level),
            attack: role.attack(Phase::OnHire, level),
            absorb: role.absorb(Phase::OnHire, level),
            stun: 0,
        }
    }

    #[inline(always)]
    fn from(character: Character, registry_id: u32, squad_id: u32, id: u8) -> Foe {
        Foe {
            registry_id,
            squad_id,
            id,
            role: character.role,
            item: character.item,
            level: character.level,
            health: character.health,
            attack: character.attack,
            absorb: character.absorb,
            stun: character.stun,
        }
    }
}
