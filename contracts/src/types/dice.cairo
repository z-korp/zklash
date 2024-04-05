//! Dice struct and methods for random dice rolls.

// Core imports

use core::poseidon::PoseidonTrait;
use core::hash::HashStateTrait;
use core::traits::Into;

/// Dice struct.
#[derive(Drop)]
struct Dice {
    face_count: u8,
    seed: felt252,
    nonce: felt252,
}

#[generate_trait]
impl DiceImpl of DiceTrait {
    #[inline(always)]
    fn new(face_count: u8, seed: felt252) -> Dice {
        Dice { face_count, seed, nonce: 0 }
    }

    #[inline(always)]
    fn roll(ref self: Dice) -> u8 {
        let mut state = PoseidonTrait::new();
        state = state.update(self.seed);
        state = state.update(self.nonce);
        self.nonce += 1;
        let random: u256 = state.finalize().into();
        (random % self.face_count.into() + 1).try_into().unwrap()
    }
}
