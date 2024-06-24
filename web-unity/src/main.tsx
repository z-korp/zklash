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

import "./index.css";
import { ThemeProvider } from "./ui/elements/theme-provider.tsx";

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

  root.render(
    <React.StrictMode>
      <StarknetConfig
        chains={chains}
        provider={provider}
        connectors={connectors}
        explorer={voyager}
        autoConnect
      >
        <ThemeProvider defaultTheme="system" storageKey="vite-ui-theme-zklash">
          <App />
        </ThemeProvider>
      </StarknetConfig>
    </React.StrictMode>,
  );
}

init();
