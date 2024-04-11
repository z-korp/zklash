using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;
    // Une variable static est une variable accessible depuis une autre classe
    public GameObject settingsMenuUI;
    public static bool gameIsPaused = false;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (gameIsPaused)
            {
                Resume();
            }
            else
            {
                Paused();
            }
        }
    }

    public void ToggleMenu()
    {
        if (gameIsPaused)
        {
            Resume();
        }
        else
        {
            Paused();
        }
    }

    public void Paused()
    {
        // Activer notre menu de pause
        pauseMenuUI.SetActive(true);
        // Arreter le temps
        //Time.timeScale = 0;
        // Changer le statut du jeu 
        gameIsPaused = true;

    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        //Time.timeScale = 1;
        gameIsPaused = false;

    }

    public void LoadMainMenu()
    {
        Resume();
        SceneManager.LoadScene("MainMenu");
    }

    public void OpenSettingsMenu()
    {
        Debug.Log("Open settings menu");
        settingsMenuUI.SetActive(true);
    }

    public void CloseSettingsMenu()
    {
        settingsMenuUI.SetActive(false);
    }

}
