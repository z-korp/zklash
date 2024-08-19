import React, { useEffect, useMemo, useState } from "react";
import ReactDOM from "react-dom/client";
import App from "./App.tsx";
import { setup, SetupResult } from "./dojo/setup.ts";
import { DojoProvider } from "./dojo/context.tsx";
import { dojoConfig } from "../dojo.config.ts";
import { Loading } from "./ui/screens/Loading.tsx";

import "./index.css";

const root = ReactDOM.createRoot(
  document.getElementById("root") as HTMLElement,
);

function Main() {
  const [setupResult, setSetupResult] = useState<SetupResult | null>(null);
  const [ready, setReady] = useState(false);
  const [enter, setEnter] = useState(false);

  const loading = useMemo(
    () => !enter || !setupResult || !ready,
    [enter, setupResult, ready],
  );

  useEffect(() => {
    async function initialize() {
      const result = await setup(dojoConfig() as any);
      setSetupResult(result);
    }

    initialize();
  }, [enter]);

  useEffect(() => {
    if (!enter) return;
    setTimeout(() => setReady(true), 2000);
  }, [enter]);

  return (
    <React.StrictMode>
      {!loading && setupResult ? (
        <DojoProvider value={setupResult}>
          <div className="relative flex flex-col h-screen w-screen">
            <App />
          </div>
        </DojoProvider>
      ) : (
        <Loading enter={enter} setEnter={setEnter} />
      )}
    </React.StrictMode>
  );
}

root.render(<Main />);
