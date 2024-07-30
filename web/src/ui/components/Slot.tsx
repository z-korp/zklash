import * as React from "react";
import { useMemo } from "react";

import { Entity, Has, HasValue, getComponentValue } from "@dojoengine/recs";
import { useComponentValue, useEntityQuery } from "@dojoengine/react";
import { useDojo } from "@/dojo/useDojo";
import { getEntityIdFromKeys } from "@dojoengine/utils";
import { Button } from "@/ui/elements/button";
import {
  Dialog,
  DialogContent,
  DialogDescription,
  DialogHeader,
  DialogTitle,
  DialogTrigger,
} from "@/ui/elements/dialog";
import { Badge } from "../elements/badge";

interface SlotProps {
  registry: number;
  entity: Entity;
  title: string;
  rating: number;
  size: number;
}

const Slot = React.memo((props: SlotProps) => {
  const { registry, entity, title, rating, size } = props;
  const {
    setup: {
      clientModels: {
        models: { Squad: SquadModel, Slot: SlotModel, Foe: FoeModel },
        classes: { Squad: SquadClass, Foe: FoeClass },
      },
    },
  } = useDojo();

  const slot = useComponentValue(SlotModel, entity);

  const squadKey = useMemo(() => {
    return getEntityIdFromKeys([
      BigInt(registry),
      BigInt(slot?.squad_id || 0),
    ]) as Entity;
  }, [registry, slot]);

  const squadComponent = useComponentValue(SquadModel, squadKey);
  const squad = useMemo(() => {
    return squadComponent ? new SquadClass(squadComponent) : null;
  }, [squadComponent]);

  const foeKeys = useEntityQuery([
    Has(FoeModel),
    HasValue(FoeModel, { registry_id: registry, squad_id: squad?.id }),
  ]);
  const foes = useMemo(() => {
    return foeKeys.map((key) => {
      const component = getComponentValue(FoeModel, key);
      return component ? new FoeClass(component) : null;
    });
  }, [foeKeys]);

  const disabled = useMemo(() => {
    return slot ? slot.index >= size : true;
  }, [slot, size]);

  if (disabled || !squad || !foes) return null;

  return (
    <div className="flex justify-between w-full">
      <p>{squad.id}</p>
      <Dialog>
        <DialogTrigger asChild>
          <Button>{squad.rating}</Button>
        </DialogTrigger>
        <DialogContent className="sm:max-w-[425px]">
          <DialogHeader>
            <DialogTitle>{`Squad ${squad.id}`}</DialogTitle>
            <DialogDescription>Squad composition</DialogDescription>
          </DialogHeader>
          {foes.map((foe, index) => (
            <div className="flex gap-2" key={index}>
              {(foe && (
                <Badge className="grow">
                  <div className="flex flex-col">
                    <p>{`[${foe.id}] ${foe.role.getName()} Lv${foe.level}`}</p>
                    {(foe.item && (
                      <p className="text-xs">{`- ${foe.item.getName()}`}</p>
                    )) ||
                      null}
                  </div>
                </Badge>
              )) ||
                null}
            </div>
          ))}
        </DialogContent>
      </Dialog>
    </div>
  );
});

export default Slot;
