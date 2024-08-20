import React, { useEffect, useState } from "react";
import { Unity, useUnityContext } from "react-unity-webgl";
import mapGrass from "/assets/map_grass.png";
import { faExpand } from "@fortawesome/free-solid-svg-icons";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";

const UnityLoader: React.FC = () => {
  const [isFullScreen, setIsFullScreen] = useState(false);
  const { unityProvider, requestFullscreen } = useUnityContext({
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

  useEffect(() => {
    const handleFullScreenChange = () => {
      setIsFullScreen(document.fullscreenElement !== null);
    };

    document.addEventListener("fullscreenchange", handleFullScreenChange);

    return () => {
      document.removeEventListener("fullscreenchange", handleFullScreenChange);
    };
  }, []);

  const handleFullScreen = () => {
    if (!isFullScreen) {
      requestFullscreen(true);
    }
  };

  return (
    <div
      className={`${isFullScreen ? "fixed inset-0 z-50" : "flex p-8 rounded-lg"}`}
      style={isFullScreen ? { backgroundColor: "black" } : {}}
    >
      <div
        style={{
          width: isFullScreen ? "100vw" : "960px",
          height: isFullScreen ? "100vh" : "600px",
          overflow: "hidden",
          display: "flex",
          justifyContent: "center",
          alignItems: "center",
        }}
        className={isFullScreen ? "" : "border-4 border-black rounded-lg"}
      >
        <Unity
          unityProvider={unityProvider}
          style={{ width: "100%", height: "100%" }}
          devicePixelRatio={window.devicePixelRatio}
        />
      </div>
      {!isFullScreen && (
        <button
          onClick={handleFullScreen}
          className="absolute bottom-6 -right-6 h-fit w-fit -translate-y-1/2 border-2 bg-white bg-opacity-30 hover:scale-110 border-white text-white font-bold px-2 rounded"
        >
          <FontAwesomeIcon icon={faExpand} size="xs" />
        </button>
      )}
    </div>
  );
};

export default UnityLoader;
