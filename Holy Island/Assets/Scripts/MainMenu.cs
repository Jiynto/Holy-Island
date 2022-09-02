using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenu : MonoBehaviour
{


    [SerializeField]
    private TextMeshProUGUI totalKills;

    private void Start()
    {
        //RemoteStatsManager.Instance.GetKillsScore(UpdateUI);
    }

    private void UpdateUI(int kills)
    {
        if (kills > 0) totalKills.text = "Total Kills: " + kills + "!";
        else totalKills.text = "No Kills Yet!";
    }


    private void resetUI()
    {
        UpdateUI(0);
    }

    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1, LoadSceneMode.Single);
        PlayerPrefs.SetInt("load", 0);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void LoadGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1, LoadSceneMode.Single);
        PlayerPrefs.SetInt("load", 1);


    }
}
