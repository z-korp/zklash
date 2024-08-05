import React, { useEffect } from "react";
import { Unity, useUnityContext } from "react-unity-webgl";

const UnityLoader: React.FC = () => {
  const { unityProvider } = useUnityContext({
    loaderUrl: "/unity/Build/zKlash_webgl.loader.js",
    dataUrl: "/unity/Build/zKlash_webgl.data",
    frameworkUrl: "/unity/Build/zKlash_webgl.framework.js",
    codeUrl: "/unity/Build/zKlash_webgl.wasm",
  });

  useEffect(() => {
    const loadScripts = async () => {
      const loadScript = (src: string, onLoad: () => void) => {
        return new Promise<void>((resolve, reject) => {
          const script = document.createElement("script");
          script.src = src;
          script.onload = () => {
            onLoad();
            resolve();
          };
          script.onerror = (error) => reject(error);
          document.body.appendChild(script);
        });
      };

      try {
        await loadScript("/unity/TemplateData/dojo.js/dojo_c.js", async () => {
          if (typeof wasm_bindgen !== "undefined") {
            try {
              await wasm_bindgen();
              console.log("wasm_bindgen initialized");
            } catch (error) {
              console.error("Error initializing wasm_bindgen", error);
            }
          } else {
            console.error("wasm_bindgen is not defined");
          }
        });

        await loadScript("/unity/TemplateData/starknet-5.24.3.js", () => {
          if (typeof starknetJs !== "undefined") {
            console.log("starknet-5.24.3.js loaded");
          } else {
            console.error("starknetJs is not defined");
          }
        });
      } catch (error) {
        console.error("Error loading scripts", error);
      }
    };

    loadScripts();
  }, []);

  return (
    <div
      className="border-4 border-black p-2 rounded-lg bg-white"
      style={{ boxShadow: `0 4px 0px black, 0 8px 0px black` }}
    >
      <div
        style={{
          width: "960px",
          height: "600px",
          overflow: "hidden",
          display: "flex",
          justifyContent: "center",
          alignItems: "center",
        }}
        className="border-4 border-black rounded-lg"
      >
        <Unity
          unityProvider={unityProvider}
          style={{ width: "100%", height: "100%" }}
          devicePixelRatio={window.devicePixelRatio}
        />
      </div>
    </div>
  );
};

export default UnityLoader;
