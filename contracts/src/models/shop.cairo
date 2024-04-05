// Starknet imports

use starknet::ContractAddress;

// Internal imports

use zklash::constants;
use zklash::types::dice::{Dice, DiceTrait};
use zklash::types::item::{Item, ITEM_COUNT};
use zklash::types::role::{Role, ROLE_COUNT};

// Constants

const TWO_POW_8: u128 = 256;

#[derive(Model, Copy, Drop, Serde)]
struct Shop {
    #[key]
    player_id: ContractAddress,
    #[key]
    team_id: u32,
    purchase_cost: u8,
    reroll_cost: u8,
    item_count: u8,
    items: u128,
    role_count: u8,
    roles: u128,
}

mod errors {
    const SHOP_NOT_EXIST: felt252 = 'Shop: does not exist';
    const SHOP_ALREADY_EXIST: felt252 = 'Shop: already exist';
}

#[generate_trait]
impl ShopImpl of ShopTrait {
    #[inline(always)]
    fn new(player_id: ContractAddress, team_id: u32, seed: felt252) -> Shop {
        // [Effect] Create the Shop and shuffle
        let mut shop = Shop {
            player_id,
            team_id,
            purchase_cost: constants::DEFAULT_SHOP_PURCHASE_COST,
            reroll_cost: constants::DEFAULT_SHOP_REROLL_COST,
            item_count: constants::DEFAULT_ITEM_COUNT,
            items: 0,
            role_count: constants::DEFAULT_ROLE_COUNT,
            roles: 0
        };
        shop.shuffle(seed);
        shop
    }

    #[inline(always)]
    fn shuffle(ref self: Shop, seed: felt252) {
        // [Effect] Shuffle the items and roles in the shop
        let mut dice: Dice = DiceTrait::new(ITEM_COUNT, seed);
        self.items = InternalTrait::gen(ref dice, self.item_count);
        dice.face_count = ROLE_COUNT;
        self.roles = InternalTrait::gen(ref dice, self.role_count);
    }

    #[inline(always)]
    fn purchase_item(ref self: Shop, index: u8) -> Item {
        // [Effect] Remove the item at index from the shop
        let (items, item) = InternalTrait::remove(self.items, index);
        self.items = items;
        // [Return] The purchased item
        item.into()
    }

    #[inline(always)]
    fn purchase_role(ref self: Shop, index: u8) -> Role {
        // [Effect] Remove the role at index from the shop
        let (roles, role) = InternalTrait::remove(self.roles, index);
        self.roles = roles;
        // [Return] The purchased role
        role.into()
    }

    fn items(self: Shop) -> Array<Item> {
        // [View] Get the items in the shop
        let mut items: Array<Item> = array![];
        let mut unpacked: Array<u8> = InternalTrait::unpack(self.items);
        loop {
            match unpacked.pop_front() {
                Option::Some(index) => {
                    let item: Item = index.into();
                    items.append(item);
                },
                Option::None => { break; }
            }
        };
        items
    }

    fn roles(self: Shop) -> Array<Role> {
        // [View] Get the items in the shop
        let mut roles: Array<Role> = array![];
        let mut unpacked: Array<u8> = InternalTrait::unpack(self.roles);
        loop {
            match unpacked.pop_front() {
                Option::Some(index) => {
                    let role: Role = index.into();
                    roles.append(role);
                },
                Option::None => { break; }
            }
        };
        roles
    }
}

#[generate_trait]
impl InternalImpl of InternalTrait {
    fn gen(ref dice: Dice, mut count: u8) -> u128 {
        let mut result: u128 = 0;
        loop {
            if count == 0 {
                break result;
            }
            count -= 1;
            result *= TWO_POW_8;
            result += dice.roll().into();
        }
    }

    fn unpack(mut packed: u128) -> Array<u8> {
        let mut result: Array<u8> = array![];
        loop {
            if packed == 0 {
                break;
            }
            let value: u8 = (packed % TWO_POW_8).try_into().unwrap();
            result.append(value);
            packed /= TWO_POW_8;
        };
        result
    }

    fn remove(mut packed: u128, index: u8) -> (u128, u8) {
        let mut removed: u8 = 0;
        let mut result: Array<u8> = array![];
        let mut idx = 0;
        loop {
            if packed == 0 {
                break;
            }
            let value: u8 = (packed % TWO_POW_8).try_into().unwrap();
            if idx != index {
                result.append(value);
            } else {
                removed = value;
            }
            idx += 1;
            packed /= TWO_POW_8;
        };
        (InternalTrait::pack(result), removed)
    }

    fn pack(mut unpacked: Array<u8>) -> u128 {
        let mut result: u128 = 0;
        loop {
            match unpacked.pop_front() {
                Option::Some(value) => {
                    result *= TWO_POW_8;
                    result += value.into();
                },
                Option::None => { break; }
            }
        };
        result
    }
}

#[generate_trait]
impl ShopAssert of AssertTrait {
    #[inline(always)]
    fn assert_exists(self: Shop) {
        assert(self.is_non_zero(), errors::SHOP_NOT_EXIST);
    }

    #[inline(always)]
    fn assert_not_exists(self: Shop) {
        assert(self.is_zero(), errors::SHOP_ALREADY_EXIST);
    }
}

impl ZeroableShopImpl of core::Zeroable<Shop> {
    #[inline(always)]
    fn zero() -> Shop {
        Shop {
            player_id: Zeroable::zero(),
            team_id: 0,
            purchase_cost: 0,
            reroll_cost: 0,
            item_count: 0,
            items: 0,
            role_count: 0,
            roles: 0
        }
    }

    #[inline(always)]
    fn is_zero(self: Shop) -> bool {
        0 == self.purchase_cost
    }

    #[inline(always)]
    fn is_non_zero(self: Shop) -> bool {
        !self.is_zero()
    }
}

