using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class NewGame : MonoBehaviour
{
    public TMP_InputField inputFieldTMP;

    public void OnStartNewGameButtonPressed()
    {
        // Ensure GameManager.Instance is not null before calling
        if (GameManager.Instance != null)
        {
            string playerName = inputFieldTMP.text;
            StartCoroutine(CreateSpawnAndLoadScene(playerName));
        }
    }

    private IEnumerator CreateSpawnAndLoadScene(string playerName)
    {
        // Start the game creation process using the GameManager's TriggerCreatePlay coroutine
        yield return StartCoroutine(GameManager.Instance.TriggerCreateAndSpawn(playerName));

        // After creation is complete, load the next scene
        SceneManager.LoadScene("Shop");
    }
}
