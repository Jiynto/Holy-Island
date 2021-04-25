using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



public class LevelController : MonoBehaviour
{
    //TODO: Find a better way of accessing the various enemy types. 
    //TODO: Move spawning to a script on spawners?
    //TODO: Fix the spawn rotating stuff

    [SerializeField]
    private GameObject enemyPrefab;
    [SerializeField]
    private GameObject bossPrefab;

    private BossController Boss = null;
    private bool instantiate = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Initialise()
    {
        PlayerController playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        playerController.DeathFlag.AddListener(GameOver);
        GameObject[] spawns = GameObject.FindGameObjectsWithTag("spawn");
        foreach (GameObject spawn in spawns)
        {
            GameObject newEnemy = Instantiate(enemyPrefab, spawn.transform);
            newEnemy.GetComponent<EnemyController>().PlayerController = playerController;
        }
        GameObject bossSpawn = GameObject.FindGameObjectWithTag("bossSpawn");
        GameObject boss = Instantiate(bossPrefab, bossSpawn.transform);
        Boss = boss.GetComponent<BossController>();
        Boss.PlayerController = playerController;
        Boss.BossRoom = bossSpawn.transform.GetComponentInParent<Room>();

        Debug.Log(GameObject.FindObjectsOfType<BossController>().Length);
       
    }

    private void FixedUpdate()
    {


    }





    private void GameOver()
    {
        SceneManager.LoadScene(0);
    }



}
