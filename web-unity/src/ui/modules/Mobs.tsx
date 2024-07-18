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
import { Mob, RoleType } from "@/game/types/mob";
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

export const Mobs = () => {
  const isMdOrLarger = useMediaQuery({ query: "(min-width: 768px)" });

  const roleTypes: RoleType[] = Object.values(RoleType).filter(
    (role) => role !== RoleType.None,
  ) as RoleType[];

  return (
    <Drawer handleOnly={true}>
      <DrawerTrigger asChild>
        <Button variant="outline">Mobs</Button>
      </DrawerTrigger>
      <DrawerContent>
        <div className="w-full max-w-sm md:max-w-full m-auto pb-4">
          <DrawerHeader>
            <DrawerTitle className="text-center text-2xl">
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

  const mob = useMemo(() => new Mob(role), [role]);

  return (
    <div className="flex flex-col justify-center items-center gap-2 p-2 pb-4 border rounded-2xl">
      <div className="flex flex-col items-center justify-center mb-2 ">
        <div className="text-2xl">{mob.value}</div>
        <div className="flex items-center">
          <div className="text-center">{`Health ${mob.getHealth()}`}</div>
          <img src={mob.getImage()} className="w-32 h-32 pixelated" />

          <div className="text-center">{`Attack ${mob.getDamage()}`}</div>
        </div>

        <div className="h-[48px]">{mob.getTalent(level)}</div>
      </div>
      <div className="flex justify-center items-center gap-2">
        Lvl
        <Pagination>
          <PaginationContent>
            {[1, 2, 3].map((lvl, index) => (
              <PaginationItem key={index}>
                <PaginationLink
                  className={`${level === lvl && "opacity-80"}`}
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
