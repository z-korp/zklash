using System.Collections;
using UnityEngine;
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
            StartCoroutine(CreateAndLoadScene(playerName));
        }
    }

    public void OnStartPlayAgainButtonPressed()
    {
        // Ensure GameManager.Instance is not null before calling
        if (GameManager.Instance != null)
        {
            string playerName = PlayerData.Instance.GetPlayerName();
            StartCoroutine(SpawnAndLoadScene(playerName));
        }
    }

    private IEnumerator CreateSpawnAndLoadScene(string playerName)
    {
        PlayerData.Instance.SetPlayerName(playerName);
        // Start the game creation process using the GameManager's ExecuteCreateAndSpawn coroutine
        yield return StartCoroutine(TxCoroutines.Instance.ExecuteCreateAndSpawn(playerName));

        // After creation is complete, load the next scene
        SceneManager.LoadScene("Shop");
    }

    private IEnumerator CreateAndLoadScene(string playerName)
    {
        PlayerData.Instance.SetPlayerName(playerName);
        // Start the game creation process using the GameManager's ExecuteCreateAndSpawn coroutine
        yield return StartCoroutine(TxCoroutines.Instance.ExecuteCreate(playerName));

        // After creation is complete, load the next scene
        SceneManager.LoadScene("ProfileScene");
    }

    private IEnumerator SpawnAndLoadScene(string playerName)
    {
        // Start the game creation process using the GameManager's ExecuteCreateAndSpawn coroutine
        yield return StartCoroutine(TxCoroutines.Instance.ExecuteSpawn());

        // After creation is complete, load the next scene
        SceneManager.LoadScene("Shop");
    }
}
