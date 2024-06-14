import React from "react";
import ReactDOM from "react-dom/client";
import App from "./App.tsx";
import { Toaster } from "@/ui/elements/sonner.tsx";
import "./index.css";
import { setup } from "./dojo/setup.ts";
import { DojoProvider } from "./dojo/DojoContext.tsx";
import { dojoConfig } from "../dojoConfig.ts";

async function init() {
  const rootElement = document.getElementById("root");
  if (!rootElement) throw new Error("React root not found");
  const root = ReactDOM.createRoot(rootElement as HTMLElement);

  const setupResult = await setup(dojoConfig);

  root.render(
    <React.StrictMode>
      <DojoProvider value={setupResult}>
        <App />
        <Toaster position="top-center" />
      </DojoProvider>
    </React.StrictMode>,
  );
}

init();
