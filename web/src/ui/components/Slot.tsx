import * as React from "react";
import { useMemo } from "react";

import { Badge } from "@/ui/elements/badge";
import { Entity } from "@dojoengine/recs";
import { useComponentValue } from "@dojoengine/react";
import { useDojo } from "@/dojo/useDojo";
import { getEntityIdFromKeys } from "@dojoengine/utils";

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
      clientComponents: { Squad: SquadModel, Slot: SlotModel },
    },
  } = useDojo();

  const slot = useComponentValue(SlotModel, entity);

  const squadKey = useMemo(() => {
    return getEntityIdFromKeys([
      BigInt(registry),
      BigInt(slot?.squad_id || 0),
    ]) as Entity;
  }, [registry, slot]);

  const squad = useComponentValue(SquadModel, squadKey);

  const disabled = useMemo(() => {
    return slot ? slot.index >= size : true;
  }, [slot, size]);

  if (disabled) return null;

  return (
    <div className="flex justify-between w-full">
      <p>{squad?.id}</p>
      <Badge>{squad ? squad.rating : "?"}</Badge>
    </div>
  );
});

export default Slot;
