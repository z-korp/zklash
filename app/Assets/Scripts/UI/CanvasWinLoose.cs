using UnityEngine;
using UnityEngine.SceneManagement;

public class CanvasWinLoose : MonoBehaviour
{
    [SerializeField]
    private GameObject _winPanel, _loosePanel;
    public void OnClickGoBackToMenu()
    {
        SceneManager.LoadScene("ProfileScene");
    }

    public void ToggleWinPanel()
    {
        _winPanel.SetActive(!_winPanel.activeSelf);
    }

    public void ToggleLoosePanel()
    {
        _loosePanel.SetActive(!_loosePanel.activeSelf);
    }

}
