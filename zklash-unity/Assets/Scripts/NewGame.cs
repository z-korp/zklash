using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NewGame : MonoBehaviour
{
    public TMP_InputField inputFieldTMP; 
    public Button startButton;

    void Start()
    {
        startButton.onClick.AddListener(StartNewGame);
    }

    public void StartNewGame()
    {
        string playerName = inputFieldTMP.text; // Récupérez le texte du champ de saisie
        Debug.Log("Démarrage d'une nouvelle partie pour le joueur : " + playerName);
        // Ici, ajoutez votre logique pour démarrer une nouvelle partie avec le nom du joueur
    }
}
