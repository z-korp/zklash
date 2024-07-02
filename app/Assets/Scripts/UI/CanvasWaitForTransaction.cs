using UnityEngine;
using TMPro;
using System.Collections;

public class CanvasWaitForTransaction : MonoBehaviour
{
    public static CanvasWaitForTransaction Instance { get; private set; }

    [SerializeField] private GameObject waitForTransactionPanel;

    private TextMeshProUGUI txtHashText;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        txtHashText = waitForTransactionPanel.transform.Find("txtHash").GetComponent<TextMeshProUGUI>();
    }

    public void setTxHash(string txHash)
    {
        if (txHash.Length > 24)
        {
            string shortenedTxHash = $"{txHash.Substring(0, 12)}...{txHash.Substring(txHash.Length - 12)}";
            txtHashText.text = shortenedTxHash;
        }
        else
        {
            txtHashText.text = txHash;
        }
    }

    public void ToggleCanvas(bool isActive)
    {
        Debug.Log("XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX ToggleCanvasCoroutine");
        waitForTransactionPanel.SetActive(isActive);
    }
}