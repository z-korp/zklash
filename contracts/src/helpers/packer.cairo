mod errors {
    const PACKER_INDEX_OUT_OF_BOUNDS: felt252 = 'Packer: index out of bounds';
}

#[generate_trait]
impl Packer<
    T,
    +Into<u8, T>,
    +Into<u16, T>,
    +TryInto<T, u8>,
    +NumericLiteral<T>,
    +PartialEq<T>,
    +Zeroable<T>,
    +Rem<T>,
    +Mul<T>,
    +AddEq<T>,
    +MulEq<T>,
    +DivEq<T>,
    +Drop<T>,
    +Copy<T>
> of PackerTrait<T> {
    fn unpack(mut packed: T) -> Array<u8> {
        let mut result: Array<u8> = array![];
        let modulo: T = 256_u16.into();
        loop {
            if packed.is_zero() {
                break;
            }
            let value: u8 = (packed % modulo).try_into().unwrap();
            result.append(value);
            packed /= modulo;
        };
        result
    }

    fn remove(mut packed: T, index: u8) -> (T, u8) {
        // [Compute] Loop over the packed value and remove the value at the given index
        let mut removed = false;
        let mut removed_value: u8 = 0;
        let mut result: Array<u8> = array![];
        let mut idx = 0;
        let modulo: T = 256_u16.into();
        loop {
            if packed.is_zero() {
                break;
            }
            let value: u8 = (packed % modulo).try_into().unwrap();
            if idx != index {
                result.append(value);
            } else {
                result.append(0);
                removed_value = value;
                removed = true;
            }
            idx += 1;
            packed /= modulo;
        };
        // [Check] Index not out of bounds
        assert(removed, errors::PACKER_INDEX_OUT_OF_BOUNDS);
        // [Return] The new packed value and the removed value
        (Packer::pack(result), removed_value)
    }

    fn pack(mut unpacked: Array<u8>) -> T {
        let mut result: T = Zeroable::zero();
        let mut modulo: T = 256_u16.into();
        let mut offset: T = 1_u16.into();
        loop {
            match unpacked.pop_front() {
                Option::Some(value) => {
                    result += offset.into() * value.into();
                    offset *= modulo;
                },
                Option::None => { break; }
            }
        };
        result
    }
}
