using TMPro;
using UnityEngine;

public class PlayerTextUpdater : MonoBehaviour
{
    private TextMeshProUGUI _txtPlayerName;
    private void Awake()
    {
        UpdatePlayerName();
    }

    public void UpdatePlayerName()
    {
        _txtPlayerName = GetComponent<TextMeshProUGUI>();
        if (_txtPlayerName != null)
        {
            if (_txtPlayerName.text != PlayerData.Instance.GetPlayerName())
                _txtPlayerName.text = PlayerData.Instance.GetPlayerName();
        }
    }
}
