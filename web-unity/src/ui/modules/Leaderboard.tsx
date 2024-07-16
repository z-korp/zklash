import { useDojo } from "@/dojo/useDojo";
import { useEntityQuery } from "@dojoengine/react";
import { Has, getComponentValue } from "@dojoengine/recs";
import { useEffect, useState } from "react";
import {
  Dialog,
  DialogTrigger,
  DialogContent,
  DialogHeader,
  DialogTitle,
  DialogDescription,
} from "@/ui/elements/dialog";
import { Button } from "@/ui/elements/button";
import { shortString } from "starknet";

function getTop20UniqueNames(dataArray: any[]) {
  const sortedArray = dataArray.sort((a, b) => b.rating - a.rating);
  const nameMap = new Map();

  sortedArray.forEach((item) => {
    const name = shortString.decodeShortString(item.name);
    if (
      name !== "zKorp" &&
      (!nameMap.has(name) || item.rating > nameMap.get(name).rating)
    ) {
      nameMap.set(name, { name, rating: item.rating });
    }
  });

  const uniqueNamesArray = Array.from(nameMap.values()).slice(0, 20);
  return uniqueNamesArray;
}

export const Leaderboard = () => {
  const {
    setup: {
      clientComponents: { Squad: SquadModel },
    },
  } = useDojo();

  const [squads, setSquads] = useState<any[]>([]);

  const squadKeys = useEntityQuery([Has(SquadModel)]);
  useEffect(() => {
    if (!squadKeys) return;
    const newSquads: any[] = [];
    squadKeys.forEach((key) => {
      const squad = getComponentValue(SquadModel, key);
      if (!squad) return;
      newSquads.push(squad);
    });
    setSquads(newSquads);
  }, [SquadModel, squadKeys]);

  return (
    <Dialog>
      <DialogTrigger asChild>
        <Button variant="outline">Leaderboard</Button>
      </DialogTrigger>
      <DialogContent>
        <DialogHeader>
          <DialogTitle>Leaderboard</DialogTitle>
          <DialogDescription>Top players by ELO rating</DialogDescription>
        </DialogHeader>
        <div className="flex flex-col gap-2">
          {getTop20UniqueNames(squads).map((squad, index) => (
            <div className="flex justify-between" key={index}>
              <p>{squad.name}</p>
              <p>{squad.rating}</p>
            </div>
          ))}
        </div>
      </DialogContent>
    </Dialog>
  );
};
