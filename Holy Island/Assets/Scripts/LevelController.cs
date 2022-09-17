using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.Random;


public delegate void UIUpdate(int kills);

public class LevelController : MonoBehaviour
{
    //TODO: Find a better way of accessing the various enemy types. 
    //TODO: Move spawning to a script on spawners?
    //TODO: Fix the spawn rotating stuff

    private static string PERSISTENT_PATH => Application.persistentDataPath;

    [SerializeField]
    private GameObject enemyPrefab;
    [SerializeField]
    private GameObject bossPrefab;
    [SerializeField]
    GenerateLevel generator;

    public UIUpdate KillUpdate;


    private State seed;
    private List<EnemyController> enemies;
    private BossController Boss;
    private PlayerController player;

    private GameObject[] ItemPrefabs;


    public void Initialise(bool load)
    {

        enemies = new List<EnemyController>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        player.DeathFlag.AddListener(GameOver);

        ItemPrefabs = Resources.LoadAll<GameObject>("Items/Shop");

        if (load)
        {
            GetSeed();
            generator.Seed = seed;
            generator.GenerationFinished.AddListener(Load);
            generator.Generate();


        }
        else
        {
            seed = generator.Seed;
            generator.GenerationFinished.AddListener(Populate);
            generator.Generate();
        }
        player.isEnabled = true;
        //Debug.Log(GameObject.FindObjectsOfType<BossController>().Length);
       
    }

    private void Load()
    {
        DeserializePlayer();
        DeserializeEnemies();
        DeserializeBoss();
        generator.GenerationFinished.RemoveListener(Load);
    }

    private void Populate()
    {
        GameObject[] spawns = GameObject.FindGameObjectsWithTag("spawn");
        foreach (GameObject spawn in spawns)
        {
            GameObject newEnemy = Instantiate(enemyPrefab, spawn.transform);
            EnemyController enemyController = newEnemy.GetComponent<EnemyController>();
            enemyController.playerController = player;
            enemyController.DeathFlag.AddListener(EnemyDeath);
            enemies.Add(enemyController);
        }

        GameObject bossSpawn = GameObject.FindGameObjectWithTag("bossSpawn");
        GameObject boss = Instantiate(bossPrefab, bossSpawn.transform);
        Boss = boss.GetComponent<BossController>();
        Boss.playerController = player;
        Boss.DeathFlag.AddListener(EnemyDeath);
        Boss.BossRoom = bossSpawn.transform.GetComponentInParent<Room>();
        generator.GenerationFinished.RemoveListener(Populate);


        GameObject[] shopSpawns = GameObject.FindGameObjectsWithTag("shopSpawn");
        foreach(GameObject spawn in shopSpawns)
        {
            GameObject newItem = Instantiate(ItemPrefabs[Range(0, ItemPrefabs.Length)], spawn.transform);
        }

    }



    #region Serialization
    public void SerializeLevel()
    {
        SerializeSeed();
        SerializePlayer();
        if(enemies.Count != 0) SerializeEnemies();
        if(Boss != null) SerializeBoss();

    }

    private void SerializeSeed()
    {
        string json = JsonUtility.ToJson(seed);
        string path = PERSISTENT_PATH + @"/seed.json";

        if (!File.Exists(path)) File.Create(path).Dispose();
        File.WriteAllText(path, json);

    }

    private void SerializePlayer()
    {
        string json = JsonUtility.ToJson(player.playerData.Save());
        string path = PERSISTENT_PATH + @"/playerState.json";

        if (!File.Exists(path)) File.Create(path).Dispose();
        File.WriteAllText(path, json);
    }    

    private void SerializeBoss()
    {
        string json = JsonUtility.ToJson(Boss.Save());
        string path = PERSISTENT_PATH + @"/bossState.json";

        if (!File.Exists(path)) File.Create(path).Dispose();
        File.WriteAllText(path, json);
    }



    private void SerializeEnemies()
    {
        EnemyController.SaveData[] enemyDatas = new EnemyController.SaveData[enemies.Count];
        for (int i = 0; i < enemies.Count; i++)
        {
            enemyDatas[i] = enemies[i].Save();
        }
        EnemySaveData enemySaveData = new EnemySaveData();
        enemySaveData.data = enemyDatas;

        string json = JsonUtility.ToJson(enemySaveData);
        string path = PERSISTENT_PATH + @"/enemiesState.json";

        if (!File.Exists(path)) File.Create(path).Dispose();
        else 
        File.WriteAllText(path, json);

    }

    #endregion

    #region Deserialization

    public void DeserializeEnemies()
    {
        string path = PERSISTENT_PATH + @"/enemiesState.json";
        try
        {
            string json = File.ReadAllText(path);
            SpawnEnemies(JsonUtility.FromJson<EnemySaveData>(json).data);
        }
        catch (Exception e)
        {
            Debug.Log("Couldnt load json");
            Debug.Log(e);
        }

    }

    private void SpawnEnemies(IEnumerable<EnemyController.SaveData> data)
    {
        if (data == null) return;
        foreach (EnemyController.SaveData enemyData in data)
        {
            GameObject newEnemy = Instantiate(enemyPrefab, enemyData.position, Quaternion.Euler(enemyData.rotation));
            newEnemy.GetComponent<EnemyController>().SetData(enemyData);
            newEnemy.GetComponent<EnemyController>().playerController = player;
        }
    }

    public void DeserializePlayer()
    {
        string path = PERSISTENT_PATH + @"/playerState.json";
        try
        {
            string json = File.ReadAllText(path);
            PlayerData.SaveData playerData = JsonUtility.FromJson<PlayerData.SaveData>(json);
            player.playerData.SetData(playerData);
            //UpdateKillTracker();
        }
        catch (Exception e)
        {
            Debug.Log("Couldnt load json");
            Debug.Log(e);
        }

    }

    public void DeserializeBoss()
    {
        string path = PERSISTENT_PATH + @"/bossState.json";
        try
        {
            string json = File.ReadAllText(path);
            BossController.SaveData bossData = JsonUtility.FromJson<BossController.SaveData>(json);
            GameObject newBoss = Instantiate(bossPrefab, bossData.position, Quaternion.Euler(bossData.rotation));
            newBoss.GetComponent<BossController>().SetData(bossData);
            newBoss.GetComponent<BossController>().playerController = player;

        }
        catch (Exception e)
        {
            Debug.Log("Couldnt load json");
            Debug.Log(e);
        }
    }


    public void GetSeed()
    {
        string path = PERSISTENT_PATH + @"/seed.json";
        try
        {
            string json = File.ReadAllText(path);
            State seedState = JsonUtility.FromJson<State>(json);
            seed = seedState;
        }
        catch (Exception e)
        {
            Debug.Log("Couldnt load json");
            Debug.Log(e);
        }

    }

    #endregion


    private void EnemyDeath(EnemyController enemy)
    {
        enemies.Remove(enemy);
        //UpdateKillTracker();
    }

    private void UpdateKillTracker()
    {
        KillUpdate(player.playerData.Kills);
    }





    private void GameOver()
    {
        SceneManager.LoadScene(0);
    }


    [Serializable]
    public struct EnemySaveData
    {
        public EnemyController.SaveData[] data;
    }


}
