// Constants

const TWO_POW_8: u128 = 256;

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
        (Packer::pack(result), removed)
    }

    fn add(mut packed: u128, value: u8, index: u8) -> u128 {
        let mut result: Array<u8> = array![];
        let mut idx = 0;
        let mut added = false;
        loop {
            if packed == 0 {
                break;
            }
            let current: u8 = (packed % TWO_POW_8).try_into().unwrap();
            if idx == index {
                result.append(value);
                added = true;
            }
            result.append(current);
            idx += 1;
            packed /= TWO_POW_8;
        };
        if !added {
            result.append(value);
        }
        Packer::pack(result)
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
