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
            GameManager.Instance.TriggerCreatePlayAsync(playerName);

            SceneManager.LoadScene("ShopScene");
        }
    }
}
