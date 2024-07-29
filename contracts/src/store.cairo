//! Store struct and component management methods.

// Core imports

use core::debug::PrintTrait;

// Straknet imports

use starknet::ContractAddress;

// Dojo imports

use dojo::world::{IWorldDispatcher, IWorldDispatcherTrait};

// Models imports

use zklash::models::index::{Character, Player, Shop, Team, Slot, Squad, League, Foe, Registry};
use zklash::models::slot::SlotTrait;
use zklash::models::league::LeagueTrait;
use zklash::models::foe::FoeTrait;
use zklash::models::character::CharacterTrait;
use zklash::models::foe::FoeIntoCharacter;


/// Store struct.
#[derive(Copy, Drop)]
struct Store {
    world: IWorldDispatcher,
}

/// Implementation of the `StoreTrait` trait for the `Store` struct.
#[generate_trait]
impl StoreImpl of StoreTrait {
    // Getters

    #[inline(always)]
    fn new(world: IWorldDispatcher) -> Store {
        Store { world: world }
    }

    #[inline(always)]
    fn player(self: Store, player_id: felt252) -> Player {
        get!(self.world, player_id, (Player))
    }

    #[inline(always)]
    fn shop(self: Store, player_id: felt252, team_id: u32) -> Shop {
        get!(self.world, (player_id, team_id), (Shop))
    }

    #[inline(always)]
    fn team(self: Store, player_id: felt252, team_id: u32) -> Team {
        get!(self.world, (player_id, team_id), (Team))
    }

    #[inline(always)]
    fn character(self: Store, player_id: felt252, team_id: u32, character_id: u8) -> Character {
        get!(self.world, (player_id, team_id, character_id), (Character))
    }

    #[inline(always)]
    fn registry(self: Store, registry_id: u32,) -> Registry {
        get!(self.world, registry_id, (Registry))
    }

    #[inline(always)]
    fn league(self: Store, registry_id: u32, league_id: u8) -> League {
        get!(self.world, (registry_id, league_id), (League))
    }

    #[inline(always)]
    fn slot(self: Store, registry_id: u32, league_id: u8, index: u32) -> Slot {
        get!(self.world, (registry_id, league_id, index), (Slot))
    }

    #[inline(always)]
    fn squad(self: Store, registry_id: u32, squad_id: u32) -> Squad {
        get!(self.world, (registry_id, squad_id), (Squad))
    }

    #[inline(always)]
    fn foe(self: Store, registry_id: u32, squad_id: u32, foe_id: u8) -> Foe {
        get!(self.world, (registry_id, squad_id, foe_id), (Foe))
    }

    fn characters(self: Store, squad: Squad) -> Array<Character> {
        let mut foes: Array<Character> = array![];
        let mut index: u8 = 0;
        loop {
            if index == squad.size {
                break;
            }
            index += 1;
            let foe: Foe = self.foe(squad.registry_id, squad.id, index);
            let character: Character = foe.into();
            foes.append(character);
        };
        foes
    }

    // Setters

    #[inline(always)]
    fn set_player(self: Store, player: Player) {
        set!(self.world, (player))
    }

    #[inline(always)]
    fn set_shop(self: Store, shop: Shop) {
        set!(self.world, (shop))
    }

    #[inline(always)]
    fn set_team(self: Store, team: Team) {
        set!(self.world, (team))
    }

    #[inline(always)]
    fn set_character(self: Store, character: Character) {
        set!(self.world, (character))
    }

    #[inline(always)]
    fn set_registry(self: Store, registry: Registry) {
        set!(self.world, (registry))
    }

    #[inline(always)]
    fn set_league(self: Store, league: League) {
        set!(self.world, (league))
    }

    #[inline(always)]
    fn set_slot(self: Store, slot: Slot) {
        set!(self.world, (slot))
    }

    #[inline(always)]
    fn set_squad(self: Store, squad: Squad) {
        set!(self.world, (squad))
    }

    #[inline(always)]
    fn set_foe(self: Store, foe: Foe) {
        set!(self.world, (foe))
    }

    #[inline(always)]
    fn remove_squad_slot(self: Store, squad: Squad) {
        // [Effect] Replace the slot with the last slot if needed
        let league = self.league(squad.registry_id, squad.league_id);
        let mut squad_slot = self.slot(squad.registry_id, squad.league_id, squad.index);
        let mut last_slot = self.slot(squad.registry_id, squad.league_id, league.size - 1);
        if last_slot.index != squad_slot.index {
            let mut last_squad = self.squad(squad.registry_id, last_slot.squad_id);
            let last_slot_index = last_slot.index;
            last_slot.index = squad_slot.index;
            last_squad.index = squad_slot.index;
            squad_slot.index = last_slot_index;
            self.set_squad(last_squad);
            self.set_slot(last_slot);
        }
        // [Effect] Remove the last slot
        squad_slot.nullify();
        self.set_slot(squad_slot);
    }

    fn set_foes(self: Store, ref foes: Array<Foe>) {
        loop {
            match foes.pop_front() {
                Option::Some(foe) => { self.set_foe(foe); },
                Option::None => { break; },
            };
        };
    }

    fn set_character_foes(
        self: Store, registry_id: u32, squad_id: u32, mut characters: Span<Character>
    ) {
        let mut index: u8 = 0;
        loop {
            match characters.pop_front() {
                Option::Some(character) => {
                    index += 1;
                    let foe: Foe = FoeTrait::from(*character, registry_id, squad_id, index);
                    self.set_foe(foe);
                },
                Option::None => { break; },
            };
        };
    }
}
