using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;


public class PauseMenu : MonoBehaviour
{
    public static bool paused = false;

    [SerializeField]
    GameObject pauseMenuUI;

    [SerializeField]
    private TextMeshProUGUI killsTracker;

    private LevelController levelController;





    public void SetLevelController(LevelController controller)
    {
        levelController = controller;
        //levelController.KillUpdate += UpdateKillTracker;
    }


    /*
    private void UpdateKillTracker(int kills)
    {
        killsTracker.text = "Kill Tracker: " + kills;
        if(kills > RemoteStatsManager.Instance.TotalKills)
        {
            RemoteStatsManager.Instance.SetKillsScore(kills);
        }
    }
    */

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
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
        Debug.Log("Quit");
    }
}
