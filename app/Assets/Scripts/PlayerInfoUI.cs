using UnityEngine;
using UnityEngine.UI; // Utilisé pour accéder aux composants UI
using TMPro;

public class PlayerInfoUI : MonoBehaviour
{
    public TextMeshProUGUI txtLife;
    public TextMeshProUGUI txtGold;
    public TextMeshProUGUI txtTrophy;

    private uint playerLives;
    private uint playerGold;
    private uint playerTrophies;

    public static PlayerInfoUI instance;

    void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one instance of PlayerInfo found!");
            return;
        }
        instance = this;
    }

    void Start()
    {
        playerLives = 0;
        playerGold = 0;
        playerTrophies = 0;

        EventManager.OnRefreshPlayerStats += RefreshPlayerStats;
    }

    void OnDestroy()
    {
        EventManager.OnRefreshPlayerStats -= RefreshPlayerStats;
    }

    void Update()
    {
        RefreshPlayerStats();
    }

    public void RefreshPlayerStats()
    {
        if (GameManager.Instance != null)
        {
            var teamEntity = PlayerData.Instance.teamEntity;
            if (teamEntity != null)
            {
                var team = GameManager.Instance.worldManager.Entity(teamEntity).GetComponent<Team>();
                if (team != null)
                {
                    UpdatePlayerStats(team.health, team.gold, team.level);
                }
            }
        }
    }

    public void UpdatePlayerStats(uint lives, uint gold, uint trophies)
    {
        playerLives = lives;
        playerGold = gold;
        playerTrophies = trophies - 1;

        UpdateUI();
    }

    // Méthode pour mettre à jour l'interface utilisateur avec les valeurs actuelles
    void UpdateUI()
    {
        txtLife.text = playerLives.ToString();
        txtGold.text = playerGold.ToString();
        txtTrophy.text = playerTrophies.ToString();
    }

    // Exemples de méthodes pour modifier les valeurs, à appeler depuis d'autres scripts ou événements
    public void AddLife(uint amount)
    {
        playerLives += amount;
        UpdateUI();
    }

    public void AddGold(uint amount)
    {
        playerGold += amount;
        UpdateUI();
    }

    public void AddTrophy(uint amount)
    {
        playerTrophies += amount;
        UpdateUI();
    }

    public uint getLifes()
    {
        return playerLives;
    }

    public uint getTrophies()
    {
        return playerTrophies;
    }
}