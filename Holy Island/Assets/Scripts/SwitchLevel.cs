using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class SwitchLevel : MonoBehaviour
{
    [SerializeField]
    PlayerController player;

    public bool load = false;

    void Start()
    {
        if(PlayerPrefs.GetInt("load") != 0)
        {
            load = true;
            PlayerPrefs.SetInt("load", 0);
        }
        player.LevelSwitch.AddListener(Switch);
        player.isEnabled = false;
        SceneManager.LoadScene("Level1", LoadSceneMode.Additive);
    }

    public void Switch()
    {
        player.isEnabled = false;
        Scene sceneToLoad = SceneManager.GetSceneByName("Level1");
        SceneManager.UnloadSceneAsync(sceneToLoad);
        SceneManager.LoadScene("Level1", LoadSceneMode.Additive);

    }

    void OnEnable()
    {
        //Tell our 'OnLevelFinishedLoading' function to start listening for a scene change as soon as this script is enabled.
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }

    void OnDisable()
    {
        //Tell our 'OnLevelFinishedLoading' function to stop listening for a scene change as soon as this script is disabled. Remember to always have an unsubscription for every delegate you subscribe to!
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    }

    void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        SceneManager.SetActiveScene(scene);
        player.transform.position = new Vector3(0, player.transform.position.y, 0);
        if(scene.name == "Level1")
        {
            PauseMenu pauseMenu = Object.FindObjectOfType<PauseMenu>();
            LevelController levelController = Object.FindObjectOfType<LevelController>();
            pauseMenu.SetLevelController(levelController);
            levelController.Initialise(load);
        }

        Debug.Log("Level Loaded");
        Debug.Log(scene.name);
        Debug.Log(mode);
    }


}
