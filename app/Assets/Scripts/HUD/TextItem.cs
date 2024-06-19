using UnityEngine;
using TMPro;

public class TxtItem : MonoBehaviour
{
    private TextMeshProUGUI textMesh;

    private void Awake()
    {
        textMesh = GetComponent<TextMeshProUGUI>();
    }

    public void UpdateText(string description)
    {
        if (textMesh != null)
        {
            textMesh.text = description;
        }
        else
        {
            Debug.LogError("TextMeshProUGUI component is not assigned properly.");
        }
    }

    public void HideShowText(bool isShow)
    {
        if (textMesh != null)
        {
            textMesh.enabled = isShow;
        }
        else
        {
            Debug.LogError("TextMeshProUGUI component is not assigned properly.");
        }
    }
}
