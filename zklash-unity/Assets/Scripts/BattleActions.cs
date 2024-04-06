using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Dojo;
using Dojo.Starknet;

public class BattleActions : MonoBehaviour
{
    [SerializeField] WorldManagerData dojoConfig;

    [SerializeField] MarketSystem marketSystem;

    private GameManager gameManager;

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

    public async void TriggerHire()
    {
        Debug.Log("TriggerHire");
        Debug.Log("GameManager: " + gameManager);
        Debug.Log("BurnerManager: " + gameManager.burnerManager);
        Account currentBurner = gameManager.burnerManager.CurrentBurner;

        var playerEntity = gameManager.playerEntity;
        var player = gameManager.worldManager.Entity(playerEntity).GetComponent<Player>();
        Debug.Log($"Player team_id: {player.team_count}");

        Debug.Log($"Current Burner: {currentBurner.Address.Hex()}");
        Debug.Log($"dojoConfig: {dojoConfig.worldAddress}");
        var txHash = await marketSystem.Hire(currentBurner, dojoConfig.worldAddress, player.team_count, 0);
        Debug.Log($"[Hire] Transaction Hash: {txHash.Hex()}");
    } 
}
