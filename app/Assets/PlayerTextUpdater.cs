using TMPro;
using UnityEngine;

public class PlayerTextUpdater : MonoBehaviour
{
    private TextMeshProUGUI _txtPlayerName;
    private void Awake()
    {
        _txtPlayerName = GetComponent<TextMeshProUGUI>();
        if (_txtPlayerName != null)
        {
            _txtPlayerName.text = PlayerData.Instance.GetPlayerName();
        }

    }
}
