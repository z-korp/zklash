import { Button } from "@/ui/elements/button";
import background from "/assets/bg-desert.png";
import logo from "/assets/enter_logo.png";
import warriorBlue from "/assets/Warrior_Blue.png";
import archerBlue from "/assets/Archer_Blue.png";
import pawnBlue from "/assets/Pawn_Blue.png";
import barrelRed from "/assets/Barrel_Red.png";
import tntRed from "/assets/TNT_Red.png";
import torchRed from "/assets/Torch_Red.png";
import castleTitle from "/assets/Castle_Title.png";
import { SpriteAnimator } from "react-sprite-animator";

export const Loading = ({
  enter,
  setEnter,
}: {
  enter: boolean;
  setEnter: (state: boolean) => void;
}) => {
  return (
    <div className="w-full h-dvh flex justify-center items-center">
      {/* Background */}
      <div className="absolute inset-0 overflow-hidden">
        <div
          className="absolute inset-0 bg-cover bg-center opacity-50 animate-zoom-in-out"
          style={{ backgroundImage: `url('${background}')` }}
        />
      </div>

      <div className="absolute md:top-1/4 top-1/6 left-1/2 -translate-x-1/2 -translate-y-1/2 text-9xl p-4 rounded-lg">
        <div
          className="w-96 h-96 bg-contain bg-no-repeat bg-center"
          style={{ backgroundImage: `url('${castleTitle}')` }}
        ></div>
      </div>
      {/* Logo */}
      <div className="absolute md:top-1/2 top-1:3 left-1/2 -translate-x-1/2 -translate-y-1/2 flex justify-center items-center w-full h-20">
        <SpriteAnimator
          sprite={warriorBlue}
          width={192}
          height={192}
          fps={10}
          className={` ${enter && "animate-bounce "}`}
        />
        <SpriteAnimator
          sprite={archerBlue}
          width={192}
          height={192}
          fps={10}
          frameCount={6}
          className={` ${enter && "animate-bounce "}`}
        />
        <SpriteAnimator
          sprite={pawnBlue}
          width={192}
          height={192}
          fps={10}
          className={` ${enter && "animate-bounce "}`}
        />
        <div className="scale-x-[-1]">
          <SpriteAnimator
            sprite={tntRed}
            width={192}
            height={192}
            fps={10}
            frameCount={6}
            className={`${enter && " animate-bounce "}`}
          />
        </div>
        <div className="scale-x-[-1]">
          <SpriteAnimator
            sprite={torchRed}
            width={192}
            height={192}
            fps={10}
            className={`${enter && " animate-bounce "}`}
          />
        </div>
        <div className="scale-x-[-1] w-[192px] h-[192px] flex justify-center items-center">
          <SpriteAnimator
            sprite={barrelRed}
            width={128}
            height={128}
            fps={10}
            wrapAfter={1}
            className={`${enter && " animate-bounce "}`}
          />
        </div>
      </div>

      {/* Enter Button */}
      <div
        className={`absolute bottom-1/4 left-1/2 -translate-x-1/2 -translate-y-1/2 flex justify-center items-center z-[2000] ${enter && "hidden"}`}
      >
        <Button
          onClick={() => setEnter(true)}
          className="text-4xl"
          variant="default"
          size={"xl"}
        >
          Enter
        </Button>
      </div>
    </div>
  );
};
