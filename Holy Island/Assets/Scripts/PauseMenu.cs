using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PauseMenu : MonoBehaviour
{
    public static bool paused = false;

    [SerializeField]
    GameObject pauseMenuUI;

    public LevelController levelController;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if (paused)
            {
                Resume();
            }
            else
            {
                Pause();
            }

        }
    }
    private void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        paused = true;
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        paused = false;
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("LevelBase");
        Debug.Log("Restart");
    }

    public void SaveAndQuit()
    {
        levelController.SerializeLevel();
        SceneManager.LoadScene("MainMenu");
        Debug.Log("Quit");
    }
}
