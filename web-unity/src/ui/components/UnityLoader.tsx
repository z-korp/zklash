import React, { useEffect } from "react";
import { Unity, useUnityContext } from "react-unity-webgl";

const UnityLoader: React.FC = () => {
  const { unityProvider } = useUnityContext({
    loaderUrl: "Build/zKlash_webgl.loader.js",
    dataUrl: "Build/zKlash_webgl.data",
    frameworkUrl: "Build/zKlash_webgl.framework.js",
    codeUrl: "Build/zKlash_webgl.wasm",
  });

  useEffect(() => {
    const loadDojoScript = async () => {
      const script = document.createElement("script");
      script.src = "TemplateData/dojo.js/dojo_c.js";
      script.onload = async () => {
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
      };
      document.body.appendChild(script);

      return () => {
        document.body.removeChild(script);
      };
    };

    loadDojoScript();
  }, []);

  return (
    <div
      style={{
        width: "960px",
        height: "600px",
        overflow: "hidden",
        background: "black",
        display: "flex",
        justifyContent: "center",
        alignItems: "center",
      }}
    >
      <Unity
        unityProvider={unityProvider}
        style={{ width: "100%", height: "100%" }}
        devicePixelRatio={window.devicePixelRatio}
      />
    </div>
  );
};

export default UnityLoader;
