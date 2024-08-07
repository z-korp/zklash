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
import { ItemType, Item, Size } from "@/game/types/item";

export const Items = () => {
  const isMdOrLarger = useMediaQuery({ query: "(min-width: 768px)" });

  const itemTypes: ItemType[] = Object.values(ItemType).filter(
    (item) => item !== ItemType.None,
  ) as ItemType[];

  return (
    <Drawer handleOnly={true}>
      <DrawerTrigger asChild>
        <Button variant="outline">Items</Button>
      </DrawerTrigger>
      <DrawerContent>
        <div className="w-full max-w-sm md:max-w-full m-auto pb-4">
          <DrawerHeader>
            <DrawerTitle className="text-center text-2xl">
              Item collection
            </DrawerTitle>
          </DrawerHeader>

          <Carousel
            className="w-full"
            orientation={"horizontal"}
            opts={{ dragFree: isMdOrLarger }}
          >
            <CarouselContent className="flex items-end">
              {itemTypes.map((type, index) => (
                <CarouselItem
                  key={index}
                  className="sm:basis-1/2 md:basis-1/3 lg:basis-1/5 xl:basis-1/6"
                >
                  <Canvas itemType={type} />
                </CarouselItem>
              ))}
            </CarouselContent>
          </Carousel>
        </div>
      </DrawerContent>
    </Drawer>
  );
};

export const Canvas = ({ itemType }: { itemType: ItemType }) => {
  const [size, setSize] = useState<Size>(Size.Small);

  const item = useMemo(() => new Item(itemType), [itemType]);

  return (
    <div className="flex flex-col justify-center items-center gap-2 p-2 pb-4 border rounded-2xl">
      <div className="flex flex-col items-center justify-center mb-2 ">
        <div className="text-2xl">{item.value}</div>
        <img src={item.getImage(size)} className="w-32 h-32" />
        <div className="h-[48px]">{item.getTalent(size)}</div>
      </div>
      <div className="flex justify-center items-center gap-2">
        Size
        <Pagination>
          <PaginationContent>
            {[Size.Small, Size.Medium, Size.Large].map((s, index) => (
              <PaginationItem key={index}>
                {item.getTalent(s) !== "" && (
                  <PaginationLink
                    className={`${size === s && "opacity-80"} px-1`}
                    isActive={size === s}
                    onClick={() => setSize(s)}
                  >
                    {s}
                  </PaginationLink>
                )}
              </PaginationItem>
            ))}
          </PaginationContent>
        </Pagination>
      </div>
    </div>
  );
};
