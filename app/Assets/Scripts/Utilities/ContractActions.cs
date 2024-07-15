using UnityEngine;
using Dojo;
using Dojo.Starknet;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;
using System.Collections;

public class ContractActions : MonoBehaviour
{
    public static ContractActions instance;
    private List<FieldElement> transactionHashes = new List<FieldElement>();

    public EventsFetcher eventsFetcher;

    [SerializeField] WorldManagerData dojoConfig;

    [SerializeField] MarketSystem marketSystem;
    [SerializeField] BattleSystem battleSystem;

    private GameManager gameManager;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one instance of BattleActions");
            return;
        }

        instance = this;
    }

    void Start()
    {
        gameManager = GameManager.Instance;
    }

    void Update()
    {
        if (GameManager.Instance != null && gameManager == null)
        {
            gameManager = GameManager.Instance;
        }
    }

    public async void TriggerEquip(byte character_id, uint index)
    {
        Debug.Log("TriggerEquip");
        Account currentBurner = gameManager.burnerManager.CurrentBurner;

        var playerEntity = PlayerData.Instance.playerEntity;
        var player = gameManager.worldManager.Entity(playerEntity).GetComponent<Player>();
        Debug.Log($"Player team_id: {player.team_count}");

        var txHash = await marketSystem.Equip(currentBurner, dojoConfig.worldAddress, player.team_count, character_id, index);
        Debug.Log($"[Equip] Transaction Hash: {txHash.Hex()}");

        transactionHashes.Add(txHash);
    }

    public async void TriggerHire(uint index)
    {
        Debug.Log("TriggerHire");
        Account currentBurner = gameManager.burnerManager.CurrentBurner;

        var playerEntity = PlayerData.Instance.playerEntity;
        var player = gameManager.worldManager.Entity(playerEntity).GetComponent<Player>();
        Debug.Log($"Player team_id: {player.team_count}");

        var txHash = await marketSystem.Hire(currentBurner, dojoConfig.worldAddress, player.team_count, index);
        Debug.Log($"[Hire] Transaction Hash: {txHash.Hex()}");

        transactionHashes.Add(txHash);
    }

    public async void TriggerReroll()
    {
        Debug.Log("TriggerReroll");
        Account currentBurner = gameManager.burnerManager.CurrentBurner;

        var playerEntity = PlayerData.Instance.playerEntity;
        var player = gameManager.worldManager.Entity(playerEntity).GetComponent<Player>();
        Debug.Log($"Player team_id: {player.team_count}");

        var txHash = await marketSystem.Reroll(currentBurner, dojoConfig.worldAddress, player.team_count);
        Debug.Log($"[Reroll] Transaction Hash: {txHash.Hex()}");

        transactionHashes.Add(txHash);
    }

    public async void TriggerMerge(uint from_id, uint to_id)
    {
        Debug.Log("TriggerMerge");
        Account currentBurner = gameManager.burnerManager.CurrentBurner;

        var playerEntity = PlayerData.Instance.playerEntity;
        var player = gameManager.worldManager.Entity(playerEntity).GetComponent<Player>();
        Debug.Log($"Player team_id: {player.team_count}");

        var txHash = await marketSystem.Merge(currentBurner, dojoConfig.worldAddress, player.team_count, from_id, to_id);
        Debug.Log($"[Merge] Transaction Hash: {txHash.Hex()}");

        transactionHashes.Add(txHash);
    }

    public async void TriggerMergeFromShop(uint character_id, uint index)
    {
        Debug.Log("TriggerMergeFromShop");
        Account currentBurner = gameManager.burnerManager.CurrentBurner;

        var playerEntity = PlayerData.Instance.playerEntity;
        var player = gameManager.worldManager.Entity(playerEntity).GetComponent<Player>();
        Debug.Log($"Player team_id: {player.team_count}");

        var txHash = await marketSystem.MergeFromShop(currentBurner, dojoConfig.worldAddress, player.team_count, character_id, index);
        Debug.Log($"[MergeFromShop] Transaction Hash: {txHash.Hex()}");

        transactionHashes.Add(txHash);
    }

    public async void TriggerSell(uint character_id)
    {
        Debug.Log("TriggerSell");
        Account currentBurner = gameManager.burnerManager.CurrentBurner;

        var playerEntity = PlayerData.Instance.playerEntity;
        var player = gameManager.worldManager.Entity(playerEntity).GetComponent<Player>();
        Debug.Log($"Player team_id: {player.team_count}");

        var txHash = await marketSystem.Sell(currentBurner, dojoConfig.worldAddress, player.team_count, character_id);
        Debug.Log($"[Sell] Transaction Hash: {txHash.Hex()}");

        transactionHashes.Add(txHash);
    }

    public IEnumerator WaitForAllTransactionsCoroutine()
    {
        yield return new WaitUntil(() => WaitForAllTransactions().IsCompleted);
    }

    public async Task WaitForAllTransactions()
    {
        Debug.Log("Waiting for all transactions to complete...");

        // Wait for all transaction hashes
        foreach (var txHash in transactionHashes)
        {
            await gameManager.provider.WaitForTransaction(txHash);
        }

        Debug.Log("All transactions are confirmed.");
        transactionHashes.Clear(); // Clear the list after all transactions are processed
    }

    public static string RemoveLeadingZerosFromHex(string hex)
    {
        if (string.IsNullOrEmpty(hex) || !hex.StartsWith("0x"))
        {
            throw new ArgumentException("The input string is not a valid hexadecimal number.", nameof(hex));
        }

        // Remove the "0x" prefix, trim leading zeros, and then re-add the "0x" prefix.
        return "0x" + hex.Substring(2).TrimStart('0');
    }

    public async void TriggerStartBattle()
    {
        Debug.Log("wwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwww");
        Debug.Log("TriggerStartBattle");
        Debug.Log("GameManager: " + gameManager);
        Debug.Log("BurnerManager: " + gameManager.burnerManager);
        Account currentBurner = gameManager.burnerManager.CurrentBurner;

        var playerEntity = PlayerData.Instance.playerEntity;
        var player = gameManager.worldManager.Entity(playerEntity).GetComponent<Player>();
        Debug.Log($"Player team_id: {player.team_count}");

        /*foreach (var characterEntity in PlayerData.Instance.characterEntities) {
            Debug.Log($"characterEntity: {characterEntity}");
            var character = gameManager.worldManager.Entity(characterEntity).GetComponent<Character>();
            Debug.Log($"Character id: {character.id}");
        }*/
        /*GameObject[] mobs = GameObject.FindGameObjectsWithTag("Mob");
        List<ElementData> mobDataList = new List<ElementData>();
        foreach (var mob in mobs)
        {
            Debug.Log($"mob: {mob}");
            ElementData data = mob.GetComponent<ElementData>();
            if (data != null && data.index != -1) // Filter out mobs with index == -1
            {
                mobDataList.Add(data);
            }
        }

        // Order the list by the index of ElementData
        var orderedMobDataList = mobDataList.OrderBy(data => data.index).ToList();*/

        // Extract the entity property from each ElementData in order
        /*List<string> orderedEntities = new List<string>();
        foreach (var data in orderedMobDataList)
        {
            var entity = VillageData.Instance.Spots[data.index].EntityContained;
            // FIXME: what is a string entity?
            // orderedEntities.Add(entity);
            Debug.Log($"Ordered Mob Entity: {entity}, Index: {data.index}");
        }

        // Now, orderedEntities contains the entities of each mob, ordered and filtered as required
        Debug.Log("Entities ordered and collected.");

        foreach (var entity in orderedEntities)
        {
            Debug.Log($"entity: {entity}");
            var character = gameManager.worldManager.Entity(entity).GetComponent<Character>();
            Debug.Log($"Character id: {character.id}");
        }
        Debug.Log("MMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMM");

        // Instantiate a StringBuilder to hold our concatenated hex string.
        StringBuilder hexStringBuilder = new StringBuilder();

        foreach (var entity in orderedEntities)
        {
            Debug.Log($"entity: {entity}");
            var character = gameManager.worldManager.Entity(entity).GetComponent<Character>();
            Debug.Log($"Character id: {character.id}");

            // Convert the ID to hex and append to the StringBuilder
            hexStringBuilder.AppendFormat("{0:X2}", character.id); // Assuming character.id is an int
        }

        // After all IDs have been processed, convert the StringBuilder to a string.
        string hexString = hexStringBuilder.ToString();
        uint hexNumber = Convert.ToUInt32(hexString, 16);
        Debug.Log($"Concatenated Hex String: {hexString}");

        var txHash = await battleSystem.StartBattle(currentBurner, dojoConfig.worldAddress, player.team_count, hexNumber);
        Debug.Log($"[Hire] Transaction Hash: {txHash.Hex()}");

        await gameManager.provider.WaitForTransaction(txHash);
        await Task.Delay(1000);

        string playerId = player.id.Hex().ToString();

        List<Hit> hitEventDetails = new List<Hit>();
        List<Fighter> fighterEventDetails = new List<Fighter>();
        List<Stun> stunEventDetails = new List<Stun>();
        List<Absorb> absorbEventDetails = new List<Absorb>();
        List<Usage> usageEventDetails = new List<Usage>();
        List<Talent> talentEventDetails = new List<Talent>();

        EventParser parser = new EventParser();

        // Fetch and process "Hit" events
        var hitEvents = await eventsFetcher.FetchEventsOnce(new string[] { "0x33f1adaeb6b7468c983c7285a0776514bd4bc3082362e9ead4211d605daf6fa", RemoveLeadingZerosFromHex(playerId), "0x1", "0x0" });
        foreach (var eventNode in hitEvents)
        {
            // Process each hit event here and store details in hitEventDetails if necessary
            var ret = parser.ProcessNode(eventNode.node.id, eventNode.node.keys, eventNode.node.data, eventNode.node.createdAt, eventNode.node.transactionHash);
            if (ret is Hit hitEventDetail) // Check if ret is of type Hit and cast it
            {
                hitEventDetails.Add(hitEventDetail);
            }
        }

        var stunEvents = await eventsFetcher.FetchEventsOnce(new string[] { "0x2726959cf68d0c5db668fc83c0d2dba1219eac773ca2dcbc132751349bc56b1", RemoveLeadingZerosFromHex(playerId), "0x1", "0x0" });
        foreach (var eventNode in stunEvents)
        {
            // Process each stun event here and store details in stunEventDetails if necessary
            var ret = parser.ProcessNode(eventNode.node.id, eventNode.node.keys, eventNode.node.data, eventNode.node.createdAt, eventNode.node.transactionHash);
            if (ret is Stun stunEventDetail) // Check if ret is of type Hit and cast it
            {
                stunEventDetails.Add(stunEventDetail);
            }
        }

        var absorbEvents = await eventsFetcher.FetchEventsOnce(new string[] { "0x1fa8f09733b3238e24214d0467ad01fb18a7a487aeee6341fb65b8a65a9f0ec", RemoveLeadingZerosFromHex(playerId), "0x1", "0x0" });
        foreach (var eventNode in absorbEvents)
        {
            // Process each absorb event here and store details in absorbEventDetails if necessary
            var ret = parser.ProcessNode(eventNode.node.id, eventNode.node.keys, eventNode.node.data, eventNode.node.createdAt, eventNode.node.transactionHash);
            if (ret is Absorb absorbEventDetail) // Check if ret is of type Hit and cast it
            {
                absorbEventDetails.Add(absorbEventDetail);
            }
        }

        var usageEvents = await eventsFetcher.FetchEventsOnce(new string[] { "0x31dc5bb49c81bb56051cf4df0b97da231ab1dc494fd966e87d79dccf76f4244", RemoveLeadingZerosFromHex(playerId), "0x1", "0x0" });
        foreach (var eventNode in usageEvents)
        {
            var ret = parser.ProcessNode(eventNode.node.id, eventNode.node.keys, eventNode.node.data, eventNode.node.createdAt, eventNode.node.transactionHash);
            if (ret is Usage usageEventDetail) // Check if ret is of type Hit and cast it
            {
                usageEventDetails.Add(usageEventDetail);
            }
        }

        var talentEvents = await eventsFetcher.FetchEventsOnce(new string[] { "0x24adf676f72d49020e56880b277e37210699f6b1c3822f9401e727754aa8a49", RemoveLeadingZerosFromHex(playerId), "0x1", "0x0" });
        foreach (var eventNode in talentEvents)
        {
            var ret = parser.ProcessNode(eventNode.node.id, eventNode.node.keys, eventNode.node.data, eventNode.node.createdAt, eventNode.node.transactionHash);
            if (ret is Talent talentEventDetail) // Check if ret is of type Hit and cast it
            {
                talentEventDetails.Add(talentEventDetail);
            }
        }

        var fighterEvents = await eventsFetcher.FetchEventsOnce(new string[] { "0x2e716cf114cb4ac634249799a5c2f6d92d29e1ffbabe1b53fd81dd04a93343d", RemoveLeadingZerosFromHex(playerId), "0x1", "0x0" });
        foreach (var eventNode in fighterEvents)
        {
            var ret = parser.ProcessNode(eventNode.node.id, eventNode.node.keys, eventNode.node.data, eventNode.node.createdAt, eventNode.node.transactionHash);
            if (ret is Fighter fighterEventDetail) // Check if ret is of type Hit and cast it
            {
                fighterEventDetails.Add(fighterEventDetail);
            }
        }

        Debug.Log("Battle events fetched and processed.");
        Debug.Log("Hit events: " + hitEventDetails.Count);
        Debug.Log("Stun events: " + stunEventDetails.Count);
        Debug.Log("Absorb events: " + absorbEventDetails.Count);
        Debug.Log("Usage events: " + usageEventDetails.Count);
        Debug.Log("Talent events: " + talentEventDetails.Count);
        Debug.Log("Fighter events: " + fighterEventDetails.Count);

        VillageData.Instance.hitEventDetails = hitEventDetails;
        VillageData.Instance.fighterEventDetails = fighterEventDetails;
        VillageData.Instance.stunEventDetails = stunEventDetails;
        VillageData.Instance.absorbEventDetails = absorbEventDetails;
        VillageData.Instance.usageEventDetails = usageEventDetails;
        VillageData.Instance.talentEventDetails = talentEventDetails;*/

        // Display CombatScene
        SceneManager.LoadScene("Battle");
    }
}