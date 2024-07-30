import * as React from "react";
import { useMemo } from "react";

import { Card, CardContent, CardFooter, CardHeader } from "@/ui/elements/card";
import Slot from "./Slot";
import { getEntityIdFromKeys } from "@dojoengine/utils";
import { Entity, Has, HasValue } from "@dojoengine/recs";
import { useComponentValue, useEntityQuery } from "@dojoengine/react";
import { useDojo } from "@/dojo/useDojo";

interface LeagueProps {
  registry: number;
  index: number;
  title: string;
  image: string;
}

const League = React.memo((props: LeagueProps) => {
  const { registry, index, title, image } = props;
  const {
    setup: {
      clientModels: {
        models: { League: LeagueModel, Slot: SlotModel },
        classes: { League: LeagueClass },
      },
    },
  } = useDojo();

  const leagueKey = useMemo(() => {
    return getEntityIdFromKeys([BigInt(registry), BigInt(index + 1)]) as Entity;
  }, [registry, index]);
  const leagueComponent = useComponentValue(LeagueModel, leagueKey);

  const league = useMemo(() => {
    return leagueComponent ? new LeagueClass(leagueComponent) : null;
  }, [leagueComponent]);

  const slotKeys = useEntityQuery([
    Has(SlotModel),
    HasValue(SlotModel, { league_id: index + 1 }),
  ]);

  const leagueSize = useMemo(() => {
    return league ? league.size : 0;
  }, [league]);

  return (
    <Card>
      <CardHeader className="m-2 w-40 h-16 relative overflow-hidden rounded">
        <div
          className="absolute inset-0 bg-cover bg-center flex justify-center items-center"
          style={{
            backgroundImage: `url('${image}')`,
            transform: "scale(1.1)",
          }}
        ></div>
      </CardHeader>
      <CardContent className="flex flex-col items-center justify-between p-4">
        {slotKeys.map((entity, index) => (
          <div key={index} className="py-1 w-full">
            <Slot
              registry={registry}
              entity={entity}
              title={`0x${index}`}
              rating={0}
              size={leagueSize}
            />
          </div>
        ))}
      </CardContent>
      <CardFooter className="flex justify-between"></CardFooter>
    </Card>
  );
});

export default League;
