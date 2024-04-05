using UnityEngine;
using UnityEngine.UI; // Utilisé pour accéder aux composants UI
using TMPro;

public class PlayerInfoUI : MonoBehaviour
{
    // Références aux éléments UI Text pour les valeurs du joueur
     public TextMeshProUGUI txtLife;
    public TextMeshProUGUI txtGold;
    public TextMeshProUGUI txtTrophy;

    // Variables pour stocker les valeurs du joueur
    private int playerLives;
    private int playerGold;
    private int playerTrophies;

    void Start()
    {
        // Initialiser les valeurs du joueur
        playerLives = 5; // Mettez ici la valeur initiale de la vie
        playerGold = 30; // Mettez ici la valeur initiale de l'or
        playerTrophies = 0; // Mettez ici la valeur initiale des trophées

        // Mettre à jour l'affichage UI
        UpdateUI();
    }

    // Méthode pour mettre à jour les valeurs du joueur et l'interface utilisateur
    public void UpdatePlayerStats(int lives, int gold, int trophies)
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
    public void AddLife(int amount)
    {
        playerLives += amount;
        UpdateUI();
    }

    public void AddGold(int amount)
    {
        playerGold += amount;
        UpdateUI();
    }

    public void AddTrophy(int amount)
    {
        playerTrophies += amount;
        UpdateUI();
    }

    // Ajoutez d'autres méthodes selon les besoins de votre jeu pour gérer la perte de vies, dépenses d'or, etc.
}