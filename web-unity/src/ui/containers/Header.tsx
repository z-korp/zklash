import { useCallback } from "react";
import { Separator } from "@/ui/elements/separator";
import { useNavigate } from "react-router-dom";
import { useMediaQuery } from "react-responsive";
import {
  Drawer,
  DrawerContent,
  DrawerHeader,
  DrawerTitle,
  DrawerTrigger,
} from "@/ui/elements/drawer";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faBars, faGear } from "@fortawesome/free-solid-svg-icons";
import Connect from "../components/Connect";
import {
  DropdownMenu,
  DropdownMenuTrigger,
  DropdownMenuContent,
  DropdownMenuLabel,
  DropdownMenuSeparator,
  DropdownMenuItem,
} from "@/ui/elements/dropdown-menu";
import { KATANA_ETH_CONTRACT_ADDRESS } from "@dojoengine/core";
import Balance from "../components/Balance";
import { useAccount } from "@starknet-react/core";
import { Button } from "../elements/button";
import { ModeToggle } from "../components/Theme";

export const Header = () => {
  const navigate = useNavigate();
  const account = useAccount();

  const isMdOrLarger = useMediaQuery({ query: "(min-width: 768px)" });

  return isMdOrLarger ? (
    <div>
      <div className="flex justify-center items-center p-4 flex-wrap md:justify-between">
        <div className="flex gap-8 items-center">
          <p
            onClick={() => navigate("/")}
            className="cursor-pointer text-4xl font-bold"
          >
            zKlash
          </p>
          <Button onClick={() => navigate("/rules")}>How to play?</Button>
        </div>

        <div className="flex flex-col gap-4 items-center md:flex-row">
          <Connect />
          <DropdownMenu>
            <DropdownMenuTrigger>
              <Button variant="outline" size="icon">
                <FontAwesomeIcon icon={faGear} />
              </Button>
            </DropdownMenuTrigger>
            <DropdownMenuContent>
              <DropdownMenuLabel>Sound</DropdownMenuLabel>
              <DropdownMenuSeparator />
              <DropdownMenuLabel>Account</DropdownMenuLabel>
              <DropdownMenuItem></DropdownMenuItem>
            </DropdownMenuContent>
          </DropdownMenu>
          <ModeToggle />
        </div>
      </div>
      <Separator />
    </div>
  ) : (
    <div>
      <div className="px-3 py-2 flex gap-3">
        <Drawer direction="left">
          <DrawerTrigger>
            <FontAwesomeIcon icon={faBars} size="xl" />
          </DrawerTrigger>
          <DrawerContent>
            <DrawerHeader className="mt-4">
              <DrawerTitle>Menu</DrawerTitle>
            </DrawerHeader>
            <div className="flex flex-col gap-4 p-4">
              <p className="text-2xl">Play</p>
              <p className="text-2xl">Leaderboard</p>
            </div>
            <div className="flex flex-col gap-5 p-4">
              {/*<div className="flex flex-col gap-2 items-center">
                <p className="self-start">Burner Account</p> <Account />
              </div>*/}

              <div className="flex flex-col gap-2 items-center">
                <p className="self-start">Account</p>
              </div>
            </div>
          </DrawerContent>
        </Drawer>
        <div className="w-full flex justify-between items-center">
          <p className="text-4xl font-bold">zKlash</p>
          {!!account.address ? (
            <div className="flex gap-2 items-center">
              <Balance
                address={account.address}
                token_address={KATANA_ETH_CONTRACT_ADDRESS}
              />
            </div>
          ) : (
            <Connect />
          )}
        </div>
      </div>
      <Separator />
    </div>
  );
};
