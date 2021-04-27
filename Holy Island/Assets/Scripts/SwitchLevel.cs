using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SwitchLevel : MonoBehaviour
{
    [SerializeField]
    PlayerController player;

    void Start()
    {
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
        Debug.Log("Level Loaded");
        Debug.Log(scene.name);
        Debug.Log(mode);
    }


}
