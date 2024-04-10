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

    void Start()
    {
        playerLives = 0;
        playerGold = 0;
        playerTrophies = 0;
    }

    void Update()
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

    // Méthode pour mettre à jour les valeurs du joueur et l'interface utilisateur
    public void UpdatePlayerStats(uint lives, uint gold, uint trophies)
    {
        playerLives = lives;
        playerGold = gold;
        playerTrophies = trophies;

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
}