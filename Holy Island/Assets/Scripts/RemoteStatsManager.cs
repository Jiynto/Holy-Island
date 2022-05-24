using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class KillStatResult
{
    public int KillsScore;
    public string code; // error code
    public string message; // error message
}


public class RemoteStatsManager : MonoBehaviour
{
    public static RemoteStatsManager Instance { get; private set; }

    private IEnumerator coroutineSend;
    private IEnumerator coroutineReceive;

    private const string APPLICATION_ID = "3850674B-6D1F-2621-FF35-4F2F62999100"; // you need to add your OWN id here!!
    private const string REST_SECRET_KEY = "A4044488-8FEF-403F-B847-5C7BFC79C4E2";

    private int totalKills;

    public int TotalKills { get { return totalKills; } }

    void Awake()
    {
        // force singleton instance
        if (Instance == null) { Instance = this; }
        else { Destroy(gameObject); }
        // do not destroy this object when we load the scene
        DontDestroyOnLoad(gameObject);
    }

    public void GetKillsScore(Action<int> onCompleteCallback)
    {
        coroutineReceive = GetKillsScoreCR(onCompleteCallback);
        StartCoroutine(coroutineReceive);
    }

    public void SetKillsScore(int kills)
    {
        coroutineSend = SetKillsScoreCR(kills);
        StartCoroutine(coroutineSend);
    }



    public IEnumerator GetKillsScoreCR(Action<int> onCompleteCallback)
    {


        //string strTableName = "NumberOfKills";
        //const string objectID = "F762A79B-B0A6-494E-9DD9-153F36E6899A";
        string url = "https://eu-api.backendless.com/3850674B-6D1F-2621-FF35-4F2F62999100/A4044488-8FEF-403F-B847-5C7BFC79C4E2/data/NumberOfKills/F762A79B-B0A6-494E-9DD9-153F36E6899A";
            
            /*= "https://eu-api.backendless.com/" +
                    APPLICATION_ID + "/" +
                    REST_SECRET_KEY +
                    "/data/" +
                    strTableName +
                    "/" +
                    objectID +
                    "'";*/

        UnityWebRequest webreq = UnityWebRequest.Get(url);

        webreq.SetRequestHeader("application-id", APPLICATION_ID);
        webreq.SetRequestHeader("secret-key", REST_SECRET_KEY);
        webreq.SetRequestHeader("application-type", "REST");

        yield return webreq.SendWebRequest();




        if (webreq.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.Log("ConnectionError");
        }
        else if (webreq.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.Log("ProtocolError");
        }
        else if (webreq.result == UnityWebRequest.Result.DataProcessingError)
        {
            Debug.Log("DataProcessingError");
        }
        else if (webreq.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Success");

            KillStatResult killStatData = JsonUtility.FromJson<KillStatResult>(webreq.downloadHandler.text);


            if (!string.IsNullOrEmpty(killStatData.code))
            {
                Debug.Log("Error:" + killStatData.code + " " + killStatData.message);
            }
            else
            {
                totalKills = killStatData.KillsScore;
                onCompleteCallback(killStatData.KillsScore);
            }
        }

    }

    public IEnumerator SetKillsScoreCR(int kills)
    {

        //string strTableName = "NumberOfKills";
        //const string objectID = "F762A79B-B0A6-494E-9DD9-153F36E6899A";
        string url = "https://eu-api.backendless.com/3850674B-6D1F-2621-FF35-4F2F62999100/A4044488-8FEF-403F-B847-5C7BFC79C4E2/data/NumberOfKills/F762A79B-B0A6-494E-9DD9-153F36E6899A";

        /*
        "https://eu-api.backendless.com/" +
                APPLICATION_ID + "/" +
                REST_SECRET_KEY +
                "/data/" +
                strTableName +
                "/" +
                objectID +
                "'";
        */

        string data = JsonUtility.ToJson(new KillStatResult { KillsScore = kills });
        UnityWebRequest webreq = UnityWebRequest.Put(url, data);

        webreq.SetRequestHeader("Content-Type", "application/json");
        webreq.SetRequestHeader("application-id", APPLICATION_ID);
        webreq.SetRequestHeader("secret-key", REST_SECRET_KEY);
        webreq.SetRequestHeader("application-type", "REST");

        yield return webreq.SendWebRequest();

        if (webreq.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.Log("ConnectionError");
        }
        else if (webreq.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.Log("ProtocolError");
        }
        else if (webreq.result == UnityWebRequest.Result.DataProcessingError)
        {
            Debug.Log("DataProcessingError");
        }
        else if (webreq.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Success");

            KillStatResult killStatData = JsonUtility.FromJson<KillStatResult>(webreq.downloadHandler.text);

            if (!string.IsNullOrEmpty(killStatData.code))
            {
                Debug.Log("Error:" + killStatData.code + " " + killStatData.message);
            }
            else
            {
                totalKills = kills;
            }
        }

        // TODO #10 - call the callback function
    }
}

