import { AccountInterface } from "starknet";
import { ClientComponents } from "./createClientComponents";
import { ContractComponents } from "./generated/contractModels";
import type { IWorld } from "./generated/contractSystems";
import { shortString } from "starknet";
import { toast } from "sonner";

export type SystemCalls = ReturnType<typeof createSystemCalls>;

export function createSystemCalls(
  { client }: { client: IWorld },
  contractComponents: ContractComponents,
  { Player, Registry, League, Slot }: ClientComponents,
) {
  const extractedMessage = (message: string) => {
    return message.match(/\('([^']+)'\)/)?.[1];
  };

  const notify = (message: string, transaction: any) => {
    if (transaction.execution_status != "REVERTED") {
      toast.success(message);
    } else {
      toast.error(extractedMessage(transaction.revert_reason));
    }
  };

  const create = async (account: AccountInterface, playerName: string) => {
    try {
      const encoded = shortString.encodeShortString(playerName);
      const { transaction_hash } = await client.account.create({
        account,
        playerName: encoded,
      });

      notify(
        "Player has been created.",
        await account.waitForTransaction(transaction_hash, {
          retryInterval: 100,
        }),
      );
    } catch (e) {
      console.log(e);
    }
  };

  const spawn = async (account: AccountInterface) => {
    try {
      const { transaction_hash } = await client.account.spawn({
        account,
      });

      notify(
        "Player has been spawned.",
        await account.waitForTransaction(transaction_hash, {
          retryInterval: 100,
        }),
      );
    } catch (e) {
      console.log(e);
    }
  };

  const equip = async (
    account: AccountInterface,
    team_id: number,
    character_id: number,
    index: number,
  ) => {
    try {
      const { transaction_hash } = await client.market.equip({
        account,
        team_id,
        character_id,
        index,
      });

      notify(
        "Character has been equipped.",
        await account.waitForTransaction(transaction_hash, {
          retryInterval: 100,
        }),
      );
    } catch (e) {
      console.log(e);
    }
  };

  const hire = async (
    account: AccountInterface,
    team_id: number,
    index: number,
  ) => {
    try {
      const { transaction_hash } = await client.market.hire({
        account,
        team_id,
        index,
      });

      notify(
        "Character has been hired.",
        await account.waitForTransaction(transaction_hash, {
          retryInterval: 100,
        }),
      );
    } catch (e) {
      console.log(e);
    }
  };

  const xp = async (
    account: AccountInterface,
    team_id: number,
    character_id: number,
    index: number,
  ) => {
    try {
      const { transaction_hash } = await client.market.xp({
        account,
        team_id,
        character_id,
        index,
      });

      notify(
        "Character has been leveled up.",
        await account.waitForTransaction(transaction_hash, {
          retryInterval: 100,
        }),
      );
    } catch (e) {
      console.log(e);
    }
  };

  const merge = async (
    account: AccountInterface,
    team_id: number,
    from_id: number,
    to_id: number,
  ) => {
    try {
      const { transaction_hash } = await client.market.merge({
        account,
        team_id,
        from_id,
        to_id,
      });

      notify(
        "Character has been merged.",
        await account.waitForTransaction(transaction_hash, {
          retryInterval: 100,
        }),
      );
    } catch (e) {
      console.log(e);
    }
  };

  const sell = async (
    account: AccountInterface,
    team_id: number,
    character_id: number,
  ) => {
    try {
      const { transaction_hash } = await client.market.sell({
        account,
        team_id,
        character_id,
      });

      notify(
        "Character has been sold.",
        await account.waitForTransaction(transaction_hash, {
          retryInterval: 100,
        }),
      );
    } catch (e) {
      console.log(e);
    }
  };

  const reroll = async (account: AccountInterface, team_id: number) => {
    try {
      const { transaction_hash } = await client.market.reroll({
        account,
        team_id,
      });

      notify(
        "Team has been rerolled.",
        await account.waitForTransaction(transaction_hash, {
          retryInterval: 100,
        }),
      );
    } catch (e) {
      console.log(e);
    }
  };

  const hydrate = async (account: AccountInterface) => {
    try {
      const { transaction_hash } = await client.battle.hydrate({
        account,
      });

      notify(
        "Battle has been hydrated.",
        await account.waitForTransaction(transaction_hash, {
          retryInterval: 100,
        }),
      );
    } catch (e) {
      console.log(e);
    }
  };

  const start = async (
    account: AccountInterface,
    team_id: number,
    order: number,
  ) => {
    try {
      const { transaction_hash } = await client.battle.start({
        account,
        team_id,
        order,
      });

      notify(
        "Battle has been started.",
        await account.waitForTransaction(transaction_hash, {
          retryInterval: 100,
        }),
      );
    } catch (e) {
      console.log(e);
    }
  };

  return {
    create,
    spawn,
    equip,
    hire,
    xp,
    merge,
    sell,
    reroll,
    hydrate,
    start,
  };
}
