// Constants

const TWO_POW_8: u128 = 256;

// Errors

mod errors {
    const PACKER_INDEX_OUT_OF_BOUNDS: felt252 = 'Packer: index out of bounds';
}

#[generate_trait]
impl Packer of PackerTrait {
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
        // [Compute] Loop over the packed value and remove the value at the given index
        let mut removed = false;
        let mut removed_value: u8 = 0;
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
                removed_value = value;
                removed = true;
            }
            idx += 1;
            packed /= TWO_POW_8;
        };
        // [Check] Index not out of bounds
        assert(removed, errors::PACKER_INDEX_OUT_OF_BOUNDS);
        // [Return] The new packed value and the removed value
        (Packer::pack(result), removed_value)
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
