// Core imports

use core::Zeroable;
use core::NumericLiteral;

mod errors {
    const PACKER_ELEMENT_IS_MISSING: felt252 = 'Packer: element is missing';
    const PACKER_INDEX_OUT_OF_BOUNDS: felt252 = 'Packer: index out of bounds';
}

trait PackerTrait<T, U, V> {
    fn get(packed: T, index: u8, size: V) -> U;
    fn contains(packed: T, value: U, size: V) -> bool;
    fn unpack(packed: T, size: V) -> Array<U>;
    fn remove(packed: T, index: u8, size: V) -> (T, U);
    fn replace(packed: T, index: u8, size: V, value: U) -> T;
    fn pack(unpacked: Array<U>, size: V) -> T;
}

impl Packer<
    T,
    U,
    V,
    +Into<U, T>,
    +Into<u8, T>,
    +TryInto<T, U>,
    +NumericLiteral<T>,
    +PartialEq<T>,
    +Zeroable<T>,
    +Rem<T>,
    +Add<T>,
    +Mul<T>,
    +Div<T>,
    +Drop<T>,
    +Copy<T>,
    +PartialEq<U>,
    +Zeroable<U>,
    +Drop<U>,
    +Copy<U>,
    +Into<V, T>,
    +Drop<V>,
    +Copy<V>,
> of PackerTrait<T, U, V> {
    fn get(packed: T, index: u8, size: V) -> U {
        let unpacked: Array<U> = Self::unpack(packed, size);
        *unpacked.at(index.into())
    }

    fn contains(mut packed: T, value: U, size: V) -> bool {
        let modulo: T = size.into();
        let mut index = 0;
        loop {
            if packed.is_zero() {
                break false;
            }
            let raw: U = (packed % modulo).try_into().unwrap();
            if value == raw.into() {
                break true;
            }
            packed = packed / modulo;
            index += 1;
        }
    }

    fn unpack(mut packed: T, size: V) -> Array<U> {
        let mut result: Array<U> = array![];
        let modulo: T = size.into();
        let mut index = 0;
        loop {
            if packed.is_zero() {
                break;
            }
            let value: U = (packed % modulo).try_into().unwrap();
            result.append(value);
            packed = packed / modulo;
            index += 1;
        };

        result
    }

    fn remove(mut packed: T, index: u8, size: V) -> (T, U) {
        // [Compute] Loop over the packed value and replace the value at the given index with 0x0
        let mut removed = false;
        let mut removed_value: U = Zeroable::zero();
        let mut result: Array<U> = array![];
        let mut idx = 0;
        let modulo: T = size.into();
        loop {
            if packed.is_zero() {
                break;
            }
            let value: U = (packed % modulo).try_into().unwrap();
            if idx != index {
                result.append(value);
            } else {
                result.append(Zeroable::zero()); // Append zero instead of skipping
                removed_value = value;
                removed = true;
            }
            idx += 1;
            packed = packed / modulo;
        };
        assert(removed, errors::PACKER_INDEX_OUT_OF_BOUNDS);
        (Self::pack(result, size), removed_value)
    }

    fn replace(mut packed: T, index: u8, size: V, value: U) -> T {
        // [Compute] Loop over the packed value and replace the value at the given index
        let mut removed = false;
        let mut result: Array<U> = array![];
        let mut idx: u8 = 0;
        let modulo: T = size.into();
        loop {
            if packed.is_zero() {
                break;
            }
            let item: U = (packed % modulo).try_into().unwrap();
            if idx != index {
                result.append(item);
            } else {
                result.append(value);
                removed = true;
            }
            idx += 1;
            packed = packed / modulo;
        };
        // [Check] Index not out of bounds
        assert(removed, errors::PACKER_ELEMENT_IS_MISSING);
        // [Return] The new packed value and the removed value
        Self::pack(result, size)
    }

    fn pack(mut unpacked: Array<U>, size: V) -> T {
        let mut result: T = Zeroable::zero();
        let mut modulo: T = size.into();
        let mut offset: T = 1_u8.into();
        loop {
            match unpacked.pop_front() {
                Option::Some(value) => {
                    result = result + offset.into() * value.into();
                    if unpacked.is_empty() {
                        break;
                    }
                    offset = offset * modulo;
                },
                Option::None => { break; }
            }
        };

        result
    }
}

#[cfg(test)]
mod tests {
    // Core imports

    use core::debug::PrintTrait;

    // Local imports

    use super::Packer;


    #[test]
    fn test_packer_pack_and_unpack() {
        let values: Array<u8> = array![1, 2, 3, 4];
        let size: u16 = 256;
        let packed = Packer::<u32, u8, u16>::pack(values, size);
        let unpacked = Packer::<u32, u8, u16>::unpack(packed, size);
        assert_eq!(unpacked, array![1, 2, 3, 4]);
    }

    #[test]
    fn test_packer_get() {
        let packed: u32 = 0x04030201;
        let size: u16 = 256;
        assert_eq!(Packer::<u32, u8, u16>::get(packed, 0, size), 1);
        assert_eq!(Packer::<u32, u8, u16>::get(packed, 1, size), 2);
        assert_eq!(Packer::<u32, u8, u16>::get(packed, 2, size), 3);
        assert_eq!(Packer::<u32, u8, u16>::get(packed, 3, size), 4);
    }

    #[test]
    fn test_packer_contains() {
        let packed: u32 = 0x04030201;
        let size: u16 = 256;
        assert(Packer::<u32, u8, u16>::contains(packed, 1, size), 'Should contain 1');
        assert(Packer::<u32, u8, u16>::contains(packed, 4, size), 'Should contain 4');
        assert(!Packer::<u32, u8, u16>::contains(packed, 5, size), 'Should not contain 5');
    }

    #[test]
    fn test_packer_remove() {
        let packed: u32 = 0x04030201;
        let size: u16 = 256;
        let (new_packed, removed) = Packer::<u32, u8, u16>::remove(packed, 2, size);
        assert_eq!(new_packed, 0x04000201);
        assert_eq!(removed, 3);
    }

    #[test]
    fn test_packer_replace() {
        let packed: u32 = 0x04030201;
        let size: u16 = 256;
        let new_packed = Packer::<u32, u8, u16>::replace(packed, 2, size, 5);
        assert_eq!(new_packed, 0x04050201);
    }

    #[test]
    fn test_packer_with_larger_values() {
        let values: Array<u16> = array![1000, 2000, 3000];
        let size: u32 = 65536; // 2^16
        let packed = Packer::<u64, u16, u32>::pack(values, size);
        let unpacked = Packer::<u64, u16, u32>::unpack(packed, size);
        assert_eq!(unpacked, array![1000, 2000, 3000]);
    }

    #[test]
    #[should_panic(expected: ('Packer: index out of bounds',))]
    fn test_packer_remove_out_of_bounds() {
        let packed: u32 = 0x04030201;
        let size: u16 = 256;
        let _ = Packer::<u32, u8, u16>::remove(packed, 4, size);
    }

    #[test]
    #[should_panic(expected: ('Packer: element is missing',))]
    fn test_packer_replace_out_of_bounds() {
        let packed: u32 = 0x04030201;
        let size: u16 = 256;
        let _ = Packer::<u32, u8, u16>::replace(packed, 4, size, 5);
    }

    #[test]
    fn test_packer_with_maximum_values() {
        let values: Array<u8> = array![255, 255, 255, 255];
        let size: u16 = 256;
        let packed = Packer::<u32, u8, u16>::pack(values, size);
        assert_eq!(packed, 0xFFFFFFFF);
        let unpacked = Packer::<u32, u8, u16>::unpack(packed, size);
        assert_eq!(unpacked, array![255, 255, 255, 255]);
    }

    #[test]
    fn test_packer_with_zero_values() {
        let values: Array<u8> = array![0, 0, 0, 0];
        let size: u16 = 256;
        let packed = Packer::<u32, u8, u16>::pack(values, size);
        assert_eq!(packed, 0);
        let unpacked = Packer::<u32, u8, u16>::unpack(packed, size);
        assert_eq!(unpacked, array![]);
    }
}

