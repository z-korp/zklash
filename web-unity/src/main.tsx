import React from "react";
import ReactDOM from "react-dom/client";
import App from "./App.tsx";
import {
  StarknetConfig,
  argent,
  braavos,
  jsonRpcProvider,
  voyager,
} from "@starknet-react/core";
import { mainnet } from "@starknet-react/chains";
import { ThemeProvider } from "./ui/elements/theme-provider.tsx";
import { setup } from "./dojo/setup.ts";
import { DojoProvider } from "./dojo/DojoContext.tsx";
import { dojoConfig } from "../dojoConfig.ts";

import "./index.css";

function rpc() {
  return {
    nodeUrl: import.meta.env.VITE_PUBLIC_NODE_URL,
  };
}

async function init() {
  const rootElement = document.getElementById("root");
  if (!rootElement) throw new Error("React root not found");
  const root = ReactDOM.createRoot(rootElement as HTMLElement);
  const chains = [mainnet];
  const connectors = [argent(), braavos()];
  const provider = jsonRpcProvider({ rpc });

  const setupResult = await setup(dojoConfig);

  root.render(
    <React.StrictMode>
      <StarknetConfig
        chains={chains}
        provider={provider}
        connectors={connectors}
        explorer={voyager}
      >
        <DojoProvider value={setupResult}>
          <ThemeProvider
            defaultTheme="system"
            storageKey="vite-ui-theme-zklash"
          >
            <App />
          </ThemeProvider>
        </DojoProvider>
      </StarknetConfig>
    </React.StrictMode>,
  );
}

init();
