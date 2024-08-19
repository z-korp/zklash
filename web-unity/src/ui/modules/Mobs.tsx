import {
  Drawer,
  DrawerContent,
  DrawerHeader,
  DrawerTitle,
  DrawerTrigger,
} from "@/ui/elements/drawer";
import { Button } from "@/ui/elements/button";
import { useMemo, useState } from "react";
import { useMediaQuery } from "react-responsive";
import {
  Carousel,
  CarouselContent,
  CarouselItem,
} from "@/ui/elements/carousel";
import {
  Pagination,
  PaginationContent,
  PaginationItem,
  PaginationLink,
} from "../elements/pagination";
import { Role, RoleType } from "@/dojo/game/types/role";

export const Mobs = () => {
  const isMdOrLarger = useMediaQuery({ query: "(min-width: 768px)" });

  const roleTypes: RoleType[] = Object.values(RoleType).filter(
    (role) => role !== RoleType.None,
  ) as RoleType[];

  return (
    <Drawer handleOnly={true}>
      <DrawerTrigger asChild>
        <Button variant="blue">Mobs</Button>
      </DrawerTrigger>
      <DrawerContent>
        <div className="w-full max-w-sm md:max-w-full m-auto pb-4 cursor-grab">
          <DrawerHeader>
            <DrawerTitle className="text-center text-4xl font-vinque">
              Mob collection
            </DrawerTitle>
          </DrawerHeader>

          <Carousel
            className="w-full"
            orientation={"horizontal"}
            opts={{ dragFree: isMdOrLarger }}
          >
            <CarouselContent className="flex items-end">
              {roleTypes.map((role, index) => (
                <CarouselItem
                  key={index}
                  className="sm:basis-1/2 md:basis-1/3 lg:basis-1/5 xl:basis-1/6"
                >
                  <Canvas role={role} />
                </CarouselItem>
              ))}
            </CarouselContent>
          </Carousel>
        </div>
      </DrawerContent>
    </Drawer>
  );
};

export const Canvas = ({ role }: { role: RoleType }) => {
  const [level, setLevel] = useState<number>(1);

  const mob = useMemo(() => new Role(role), [role]);

  return (
    <div
      className="flex flex-col justify-center items-center p-2 pb-4 border border-black rounded-2xl m-4 bg-secondary"
      style={{ boxShadow: `0 4px 0px black, 0 6px 0px black` }}
    >
      <div className="flex flex-col items-center justify-center border border-black p-2 rounded-lg bg-primary w-11/12">
        <div className="text-2xl font-vinque">{mob.value}</div>
        <div className="flex items-center">
          <div className="text-center">{`Health ${mob.getHealth()}`}</div>
          <img src={mob.getImage()} className="w-32 h-32 pixelated" />

          <div className="text-center">{`Attack ${mob.getDamage()}`}</div>
        </div>

        <div className="h-[48px] text-center">{mob.getTalent(level)}</div>
        <div className="mt-2">{`Cost: ${mob.getCost(level)} golds`}</div>
      </div>
      <div className="mt-4 flex justify-center items-center gap-2">
        Lvl
        <Pagination>
          <PaginationContent>
            {[1, 2, 3].map((lvl, index) => (
              <PaginationItem key={index}>
                <PaginationLink
                  className={`${level === lvl && "opacity-80"} cursor-pointer`}
                  isActive={level === lvl}
                  onClick={() => setLevel(lvl)}
                >
                  {lvl}
                </PaginationLink>
              </PaginationItem>
            ))}
          </PaginationContent>
        </Pagination>
      </div>
    </div>
  );
};
