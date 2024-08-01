import { Button } from "@/ui/elements/button";

export const Loading = ({
  enter,
  setEnter,
}: {
  enter: boolean;
  setEnter: (state: boolean) => void;
}) => {
  const customBackground = {
    "--s": "16px" /* control the size */,
    "--c1": "#e4844a",
    "--c2": "#0d6759",

    "--g":
      "radial-gradient(30% 50% at 30% 100%, #0000 66%, var(--c1) 67% 98%, #0000)",
    backgroundImage: `
          var(--g), 
          var(--g) calc(5 * var(--s)) calc(3 * var(--s)), 
          repeating-linear-gradient(90deg, var(--c1) 0 10%, var(--c2) 0 50%)`,
    backgroundSize: "calc(10 * var(--s)) calc(6 * var(--s))",
  };

  return (
    <div className="w-full h-screen flex justify-center items-center">
      {/* Background */}
      <div className="absolute inset-0 overflow-hidden">
        <div className="absolute inset-0 bg-cover bg-center opacity-50 animate-zoom-in-out" />
      </div>

      {/* Enter Button */}
      <div
        className={`absolute bottom-1/2 left-1/2 -translate-x-1/2 -translate-y-1/2 flex justify-center items-center z-[2000] ${enter && "hidden"}`}
      >
        <Button
          onClick={() => setEnter(true)}
          className="text-2xl"
          variant="default"
        >
          Enter
        </Button>
      </div>
      <div className="mt-8 border-2 border-black p-[2px] rounded-md bg-white drop-shadow-[0_4px_0px_rgba(255,0,0,0.4)]">
        <div
          className="border-2 border-black rounded-md px-2 py-1 bg-slate-800 text-white"
          style={{
            backgroundImage: `url("https://www.transparenttextures.com/patterns/natural-paper.png")`,
          }}
        >
          <p>Play</p>
        </div>
      </div>
      <Button variant="blue"> Hello </Button>
      <Button variant="green"> Hello </Button>
      <Button variant="red"> Hello </Button>
      <Button variant="yellow"> Hello </Button>
    </div>
  );
};
