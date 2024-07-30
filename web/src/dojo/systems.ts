import type { IWorld } from "./generated/contractSystems";
import { toast } from "sonner";
import * as SystemTypes from "./generated/contractSystems";
import { ClientModels } from "./models";
import { shortenHex } from "@dojoengine/utils";
import { Account } from "starknet";

export type SystemCalls = ReturnType<typeof systems>;

export function systems({
  client,
  clientModels,
}: {
  client: IWorld;
  clientModels: ClientModels;
}) {
  const TOAST_ID = "unique-id";

  const extractedMessage = (message: string) => {
    return message.match(/\('([^']+)'\)/)?.[1];
  };

  const isMdOrLarger = (): boolean => {
    return window.matchMedia("(min-width: 768px)").matches;
  };

  const isSmallHeight = (): boolean => {
    return window.matchMedia("(max-height: 768px)").matches;
  };

  const getToastAction = (transaction_hash: string) => {
    return {
      label: "View",
      onClick: () =>
        window.open(
          `https://worlds.dev/networks/slot/worlds/zklash/txs/${transaction_hash}`,
        ),
    };
  };

  const getToastPlacement = ():
    | "top-center"
    | "bottom-center"
    | "bottom-right" => {
    if (!isMdOrLarger()) {
      // if mobile
      return isSmallHeight() ? "top-center" : "bottom-center";
    }
    return "bottom-right";
  };

  const toastPlacement = getToastPlacement();

  const notify = (message: string, transaction: any) => {
    if (transaction.execution_status !== "REVERTED") {
      toast.success(message, {
        id: TOAST_ID,
        description: shortenHex(transaction.transaction_hash),
        action: getToastAction(transaction.transaction_hash),
        position: toastPlacement,
      });
    } else {
      toast.error(extractedMessage(transaction.revert_reason), {
        id: TOAST_ID,
        position: toastPlacement,
      });
    }
  };

  const handleTransaction = async (
    account: Account,
    action: () => Promise<{ transaction_hash: string }>,
    successMessage: string,
  ) => {
    toast.loading("Transaction in progress...", {
      id: TOAST_ID,
      position: toastPlacement,
    });
    try {
      const { transaction_hash } = await action();
      toast.loading("Transaction in progress...", {
        description: shortenHex(transaction_hash),
        action: getToastAction(transaction_hash),
        id: TOAST_ID,
        position: toastPlacement,
      });

      const transaction = await account.waitForTransaction(transaction_hash, {
        retryInterval: 100,
      });

      notify(successMessage, transaction);
    } catch (error: any) {
      toast.error(extractedMessage(error.message), { id: TOAST_ID });
    }
  };

  const create = async ({ account, ...props }: SystemTypes.Create) => {
    await handleTransaction(
      account,
      () => client.account.create({ account, ...props }),
      "Player has been created.",
    );
  };

  const spawn = async ({ account, ...props }: SystemTypes.Signer) => {
    await handleTransaction(
      account,
      () => client.account.spawn({ account, ...props }),
      "Player has been spawned.",
    );
  };

  const hydrate = async ({ account, ...props }: SystemTypes.Signer) => {
    await handleTransaction(
      account,
      () => client.battle.hydrate({ account, ...props }),
      "Game has been hydrated.",
    );
  };

  const start = async ({ account, ...props }: SystemTypes.Start) => {
    await handleTransaction(
      account,
      () => client.battle.start({ account, ...props }),
      "Battle has started.",
    );
  };

  const equip = async ({ account, ...props }: SystemTypes.Equip) => {
    await handleTransaction(
      account,
      () => client.market.equip({ account, ...props }),
      "Character has been equiped.",
    );
  };

  const hire = async ({ account, ...props }: SystemTypes.Hire) => {
    await handleTransaction(
      account,
      () => client.market.hire({ account, ...props }),
      "Character has been hired.",
    );
  };

  const merge = async ({ account, ...props }: SystemTypes.Merge) => {
    await handleTransaction(
      account,
      () => client.market.merge({ account, ...props }),
      "Characters have been merged.",
    );
  };

  const reroll = async ({ account, ...props }: SystemTypes.Reroll) => {
    await handleTransaction(
      account,
      () => client.market.reroll({ account, ...props }),
      "Player has rerolled.",
    );
  };

  const sell = async ({ account, ...props }: SystemTypes.Sell) => {
    await handleTransaction(
      account,
      () => client.market.sell({ account, ...props }),
      "Character has been sold.",
    );
  };

  const xp = async ({ account, ...props }: SystemTypes.XP) => {
    await handleTransaction(
      account,
      () => client.market.xp({ account, ...props }),
      "Character gained xp.",
    );
  };

  return {
    create,
    spawn,
    hydrate,
    start,
    equip,
    hire,
    merge,
    reroll,
    sell,
    xp,
  };
}
