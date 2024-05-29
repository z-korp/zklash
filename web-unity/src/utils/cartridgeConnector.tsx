import { Connector } from '@starknet-react/core';
import CartridgeConnector from '@cartridge/connector';

const cartridgeConnector = new CartridgeConnector([
  {
    target: import.meta.env.VITE_PUBLIC_FEE_TOKEN_ADDRESS,
    method: 'approve',
  },
  {
    target: import.meta.env.VITE_PUBLIC_ACCOUNT_CLASS_HASH,
    method: 'initialize',
  },
  {
    target: import.meta.env.VITE_PUBLIC_ACCOUNT_CLASS_HASH,
    method: 'create',
  },
]) as never as Connector;

export default cartridgeConnector;
