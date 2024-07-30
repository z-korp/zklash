import { Button } from "@/ui/elements/button";

export const Loading = ({
  enter,
  setEnter,
}: {
  enter: boolean;
  setEnter: (state: boolean) => void;
}) => {
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
    </div>
  );
};
