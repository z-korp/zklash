import UnityLoader from "../containers/UnityLoader";
import { SpriteAnimator } from "react-sprite-animator";
import archerBlue from "/assets/Archer_Blue.png";
import towerRed from "/assets/Wood_Tower_Red.png";
import towerBlue from "/assets/Tower_Blue.png";

const Home = () => {
  return (
    <>
      <div className="absolute flex flex-col justify-center items-center left-0 top-0 gap-1">
        <div className="flex justify-center items-center w-[256px]">
          <SpriteAnimator
            sprite={towerBlue}
            width={128}
            height={256}
            fps={10}
          />
        </div>
        <div className="flex justify-center items-center w-[256px]">
          <SpriteAnimator
            sprite={towerBlue}
            width={128}
            height={256}
            fps={10}
          />
        </div>
        <div className="flex justify-center items-center w-[256px]">
          <SpriteAnimator
            sprite={towerBlue}
            width={128}
            height={256}
            fps={10}
          />
        </div>
      </div>
      <div className="absolute flex flex-col justify-center items-center right-0 top-0 gap-1">
        <div className="flex justify-center items-center h-[256px]">
          <SpriteAnimator sprite={towerRed} width={256} height={192} fps={10} />
        </div>
        <div className="flex justify-center items-center h-[256px]">
          <SpriteAnimator sprite={towerRed} width={256} height={192} fps={10} />
        </div>
        <div className="flex justify-center items-center h-[256px]">
          <SpriteAnimator sprite={towerRed} width={256} height={192} fps={10} />
        </div>
      </div>
      <div className="relative w-screen">
        <div className="relative flex flex-col grow items-center justify-start">
          <div className="absolute top-0 flex flex-col items-center w-full max-w-4xl mx-auto">
            <UnityLoader />
          </div>
        </div>
      </div>
    </>
  );
};

export default Home;
