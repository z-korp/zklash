import React, { useCallback, useEffect, useMemo, useState } from "react";
import { useDojo } from "@/dojo/useDojo";
import { useEntityQuery } from "@dojoengine/react";
import { Has, getComponentValue } from "@dojoengine/recs";
import {
  Dialog,
  DialogContent,
  DialogHeader,
  DialogTitle,
  DialogTrigger,
  DialogDescription,
} from "@/ui/elements/dialog";
import { Button } from "@/ui/elements/button";
import { Squad } from "@/dojo/game/models/squad";
import {
  Pagination,
  PaginationContent,
  PaginationItem,
  PaginationLink,
  PaginationNext,
  PaginationPrevious,
} from "@/ui/elements/pagination";
import { useMediaQuery } from "react-responsive";

const SQUADS_PER_PAGE = 5;
const MAX_PAGE_COUNT = 5;

function getTop20UniqueNames(dataArray: any[]) {
  const sortedArray = dataArray.sort((a, b) => b.rating - a.rating);
  const nameMap = new Map();

  sortedArray.forEach((item) => {
    const name = item.name;
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
      clientModels: {
        models: { Squad: SquadModel },
        classes: { Squad: SquadClass },
      },
    },
  } = useDojo();

  const [squads, setSquads] = useState<Squad[]>([]);
  const [page, setPage] = useState<number>(1);
  const [pageCount, setPageCount] = useState<number>(0);

  const squadKeys = useEntityQuery([Has(SquadModel)]);
  useEffect(() => {
    if (!squadKeys) return;
    const newSquads: Squad[] = [];
    squadKeys.forEach((key) => {
      const squadComponent = getComponentValue(SquadModel, key);
      if (!squadComponent) return;
      newSquads.push(new SquadClass(squadComponent));
    });
    setSquads(newSquads);
  }, [SquadModel, squadKeys]);

  const topSquads = useMemo(() => getTop20UniqueNames(squads), [squads]);

  useEffect(() => {
    const rem = Math.floor(topSquads.length / (SQUADS_PER_PAGE + 1)) + 1;
    setPageCount(rem);
  }, [topSquads]);

  const { start, end } = useMemo(() => {
    const start = (page - 1) * SQUADS_PER_PAGE;
    const end = start + SQUADS_PER_PAGE;
    return { start, end };
  }, [page]);

  const handlePrevious = useCallback(() => {
    if (page === 1) return;
    setPage((prev) => prev - 1);
  }, [page]);

  const handleNext = useCallback(() => {
    if (page === Math.min(pageCount, MAX_PAGE_COUNT)) return;
    setPage((prev) => prev + 1);
  }, [page, pageCount]);

  const isSmallScreen = useMediaQuery({ query: "(min-width: 640px)" });

  return (
    <Dialog>
      <DialogTrigger asChild>
        <Button variant="red">Leaderboard</Button>
      </DialogTrigger>
      <DialogContent className="border-2 border-black">
        <DialogHeader>
          <DialogTitle className="font-vinque text-4xl">
            Leaderboard
          </DialogTitle>
          <DialogDescription className="text-xl">
            Top players by ELO rating
          </DialogDescription>
        </DialogHeader>
        <div className="flex flex-col gap-2 h-[152px]">
          {topSquads.slice(start, end).map((squad, index) => (
            <div className="flex justify-between" key={index}>
              <p>{squad.name}</p>
              <p>{squad.rating}</p>
            </div>
          ))}
        </div>
        <Pagination className="mt-5">
          <PaginationContent>
            <PaginationItem>
              <PaginationPrevious
                className={`${page === 1 && "opacity-50"}`}
                onClick={handlePrevious}
              />
            </PaginationItem>
            {isSmallScreen &&
              Array.from({ length: Math.min(pageCount, MAX_PAGE_COUNT) }).map(
                (_, index) => (
                  <PaginationItem key={index}>
                    <PaginationLink
                      isActive={index + 1 === page}
                      onClick={() => setPage(index + 1)}
                    >
                      {index + 1}
                    </PaginationLink>
                  </PaginationItem>
                ),
              )}
            <PaginationItem>
              <PaginationNext
                className={`${
                  page === Math.min(pageCount, MAX_PAGE_COUNT) && "opacity-50"
                }`}
                onClick={handleNext}
              />
            </PaginationItem>
          </PaginationContent>
        </Pagination>
      </DialogContent>
    </Dialog>
  );
};
