import React from 'react';
import ReactDOM from 'react-dom/client';
import App from './App.tsx';
import { Toaster } from '@/ui/elements/sonner.tsx';
import { setup } from './dojo/setup.ts';
import { DojoProvider } from './dojo/DojoContext.tsx';
import { dojoConfig } from '../dojoConfig.ts';
import { StarknetConfig, jsonRpcProvider } from '@starknet-react/core';
import { sepolia } from '@starknet-react/chains';

import './index.css';
import cartridgeConnector from './utils/cartridgeConnector.tsx';

function rpc() {
  return {
    nodeUrl: import.meta.env.VITE_PUBLIC_NODE_URL,
  };
}

async function init() {
  const rootElement = document.getElementById('root');
  if (!rootElement) throw new Error('React root not found');
  const root = ReactDOM.createRoot(rootElement as HTMLElement);

  const setupResult = await setup(dojoConfig);

  const chains = [sepolia];
  const connectors = [cartridgeConnector];

  root.render(
    <React.StrictMode>
      <StarknetConfig
        chains={chains}
        provider={jsonRpcProvider({ rpc })}
        connectors={connectors}
        autoConnect
      >
        <DojoProvider value={setupResult}>
          <App />
          <Toaster position="top-center" />
        </DojoProvider>
      </StarknetConfig>
    </React.StrictMode>
  );
}

init();
