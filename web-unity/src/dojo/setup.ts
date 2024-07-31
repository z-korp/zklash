import { getSyncEntities } from "@dojoengine/state";
import { DojoConfig, DojoProvider } from "@dojoengine/core";
import * as torii from "@dojoengine/torii-client";
import { defineContractComponents } from "./generated/contractModels";
import { world } from "./world";
import { setupWorld } from "./generated/contractSystems";
import { Account, RpcProvider } from "starknet";
import { BurnerManager } from "@dojoengine/create-burner";
import { models } from "./models";
import { systems } from "./systems";

export type SetupResult = Awaited<ReturnType<typeof setup>>;

export async function setup({ ...config }: DojoConfig) {
  // torii client
  const toriiClient = await torii.createClient([], {
    rpcUrl: config.rpcUrl,
    toriiUrl: config.toriiUrl,
    relayUrl: "",
    worldAddress: config.manifest.world.address || "",
  });

  // create contract components
  const contractModels = defineContractComponents(world);

  // create client components
  const clientModels = models({ contractModels });

  // fetch all existing entities from torii
  // await getSyncEntities(toriiClient, contractModels as any, []);
  const sync = await getSyncEntities(
    toriiClient,
    contractModels as any,
    [],
    1000,
  );

  const client = await setupWorld(
    new DojoProvider(config.manifest, config.rpcUrl),
    config,
  );

  const rpcProvider = new RpcProvider({
    nodeUrl: config.rpcUrl,
  });

  const burnerManager = new BurnerManager({
    masterAccount: new Account(
      rpcProvider,
      config.masterAddress,
      config.masterPrivateKey,
    ),
    feeTokenAddress: config.feeTokenAddress,
    accountClassHash: config.accountClassHash,

    rpcProvider,
  });

  try {
    await burnerManager.init();
    if (burnerManager.list().length === 0) {
      await burnerManager.create();
    }
  } catch (e) {
    console.error(e);
  }

  return {
    client,
    clientModels,
    contractComponents: clientModels,
    systemCalls: systems({ client, clientModels }),
    config,
    world,
    burnerManager,
    rpcProvider,
    sync,
  };
}
