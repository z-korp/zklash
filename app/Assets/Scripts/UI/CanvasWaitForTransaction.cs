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
        if (txHash.Length > 16)
        {
            string shortenedTxHash = $"{txHash.Substring(0, 8)}...{txHash.Substring(txHash.Length - 8)}";
            txtHashText.text = shortenedTxHash;
        }
        else
        {
            txtHashText.text = txHash;
        }
    }

    public void ToggleCanvas(bool isActive)
    {
        waitForTransactionPanel.SetActive(isActive);
    }

    public bool IsCanvasActive()
    {
        return waitForTransactionPanel.activeSelf;
    }
}