using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Dojo;
using Dojo.Starknet;

public class ContractActions : MonoBehaviour
{
    public static ContractActions instance;

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

    public async void TriggerHire(uint index)
    {
        Debug.Log("TriggerHire");
        Debug.Log("GameManager: " + gameManager);
        Debug.Log("BurnerManager: " + gameManager.burnerManager);
        Account currentBurner = gameManager.burnerManager.CurrentBurner;

        var playerEntity = PlayerData.Instance.playerEntity;
        var player = gameManager.worldManager.Entity(playerEntity).GetComponent<Player>();
        Debug.Log($"Player team_id: {player.team_count}");

        Debug.Log($"Current Burner: {currentBurner.Address.Hex()}");
        Debug.Log($"dojoConfig: {dojoConfig.worldAddress}");
        var txHash = await marketSystem.Hire(currentBurner, dojoConfig.worldAddress, player.team_count, index);
        Debug.Log($"[Hire] Transaction Hash: {txHash.Hex()}");
    } 

    public async void TriggerStartBattle()
    {
        Debug.Log("TriggerStartBattle");
        Debug.Log("GameManager: " + gameManager);
        Debug.Log("BurnerManager: " + gameManager.burnerManager);
        Account currentBurner = gameManager.burnerManager.CurrentBurner;

        var playerEntity = PlayerData.Instance.playerEntity;
        var player = gameManager.worldManager.Entity(playerEntity).GetComponent<Player>();
        Debug.Log($"Player team_id: {player.team_count}");

        foreach (var characterEntity in PlayerData.Instance.characterEntities) {
            Debug.Log($"characterEntity: {characterEntity}");
            var character = gameManager.worldManager.Entity(characterEntity).GetComponent<Character>();
            Debug.Log($"Character id: {character.id}");
        }

        /*Debug.Log($"Current Burner: {currentBurner.Address.Hex()}");
        Debug.Log($"dojoConfig: {dojoConfig.worldAddress}");
        var order = 0;
        var txHash = await battleSystem.Hire(currentBurner, dojoConfig.worldAddress, player.team_count, order););
        Debug.Log($"[Hire] Transaction Hash: {txHash.Hex()}");*/
    } 
}
