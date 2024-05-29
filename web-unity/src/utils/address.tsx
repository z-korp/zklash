import { validateAndParseAddress } from 'starknet';

export const bigIntAddressToString = (address: bigint) => {
  return removeLeadingZeros(validateAndParseAddress(address));
};

export const shortAddress = (address: string, size = 4) => {
  return `${address.slice(0, size)}...${address.slice(-size)}`;
};

export const removeLeadingZeros = (address: string) => {
  // Check if the address starts with '0x' and then remove leading zeros from the hexadecimal part
  if (address.startsWith('0x')) {
    return '0x' + address.substring(2).replace(/^0+/, '');
  }
  // Return the original address if it doesn't start with '0x'
  return address;
};
