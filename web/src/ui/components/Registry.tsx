import * as React from "react";
import {
  Carousel,
  CarouselContent,
  CarouselNext,
  CarouselPrevious,
} from "@/ui/elements/carousel";
import League from "./League";

import silver_01 from "/assets/silver_01.png";
import silver_02 from "/assets/silver_02.png";
import silver_03 from "/assets/silver_03.png";
import silver_elite from "/assets/silver_elite.png";
import silver_master from "/assets/silver_master.png";
import gold_nova_01 from "/assets/gold_nova_01.png";
import gold_nova_02 from "/assets/gold_nova_02.png";
import gold_nova_03 from "/assets/gold_nova_03.png";
import gold_nova_master from "/assets/gold_nova_master.png";
import master_guardian_01 from "/assets/master_guardian_01.png";
import master_guardian_02 from "/assets/master_guardian_02.png";
import master_guardian_elite from "/assets/master_guardian_elite.png";
import distinguished_master_guardian from "/assets/distinguished_master_guardian.png";
import legendary_eagle from "/assets/legendary_eagle.png";
import legendary_eagle_master from "/assets/legendary_eagle_master.png";
import supreme_master_first_class from "/assets/supreme_master_first_class.png";
import the_global_elite from "/assets/the_global_elite.png";
import unranked from "/assets/unranked.png";

export function getTitle(index: number) {
  switch (index) {
    case 0:
      return "Silver I";
    case 1:
      return "Silver II";
    case 2:
      return "Silver III";
    case 3:
      return "Silver Elite";
    case 4:
      return "Silver Master";
    case 5:
      return "Gold Nova I";
    case 6:
      return "Gold Nova II";
    case 7:
      return "Gold Nova III";
    case 8:
      return "Gold Nova Master";
    case 9:
      return "Master Guardian I";
    case 10:
      return "Master Guardian II";
    case 11:
      return "Master Guardian Elite";
    case 12:
      return "Distinguished Master Guardian";
    case 13:
      return "Legendary Eagle";
    case 14:
      return "Legendary Eagle Master";
    case 15:
      return "Supreme Master First Class";
    case 16:
      return "The Global Elite";
    default:
      return "Unranked";
  }
}

export function getImage(index: number) {
  switch (index) {
    case 0:
      return silver_01;
    case 1:
      return silver_02;
    case 2:
      return silver_03;
    case 3:
      return silver_elite;
    case 4:
      return silver_master;
    case 5:
      return gold_nova_01;
    case 6:
      return gold_nova_02;
    case 7:
      return gold_nova_03;
    case 8:
      return gold_nova_master;
    case 9:
      return master_guardian_01;
    case 10:
      return master_guardian_02;
    case 11:
      return master_guardian_elite;
    case 12:
      return distinguished_master_guardian;
    case 13:
      return legendary_eagle;
    case 14:
      return legendary_eagle_master;
    case 15:
      return supreme_master_first_class;
    case 16:
      return the_global_elite;
    default:
      return unranked;
  }
}

interface RegistryProps {
  registry: number;
}

const Registry = React.memo((props: RegistryProps) => {
  const { registry } = props;
  return (
    <div className="flex justify-center items-center">
      <Carousel
        className="w-full max-w-xs md:max-w-2xl lg:max-w-4xl xl:max-w-6xl"
        orientation={"horizontal"}
        opts={{ dragFree: true }}
      >
        <CarouselContent className="flex gap-4 mx-4">
          {Array.from({ length: 15 }).map((_, index) => (
            <div key={index} className="px-1">
              <League
                index={index}
                registry={registry}
                title={getTitle(index)}
                image={getImage(index)}
              />
            </div>
          ))}
        </CarouselContent>
        <CarouselPrevious />
        <CarouselNext />
      </Carousel>
    </div>
  );
});

export default Registry;
