using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DataPersistenceManager : MonoBehaviour
{
    private static DataPersistenceManager instance;
    private const string SaveFileName = "playerdata.json";
    public static DataPersistenceManager Instance
    {
        get
        {
            return instance;
        }
    }

    private string SavePath
    {
        get
        {
            return Path.Combine(Application.persistentDataPath, SaveFileName);
        }
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.LogError("wtf there should only be one instance");
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
        LoadPlayerData();

        if (!File.Exists(SavePath))
        {
            SavePlayerData();
        }

        PlayerData.OnChanged += SavePlayerData;
    }

    private void OnDestroy()
    {
        PlayerData.OnChanged -= SavePlayerData;
    }

    public void SavePlayerData()
    {
        //YES I KNOW ITS NOT ENCRYPTED BUT IM TOO LAZY AND THIS GAME IS NOT THAT HARD 
        //GO MANIPULATE UR SAVE DATA OR SOMETING IDRC
        PlayerData.SaveData playerData = PlayerData.GetSaveData();
        string json = JsonUtility.ToJson(playerData, true);
        File.WriteAllText(SavePath, json);
    }

    public void LoadPlayerData()
    {
        if (!File.Exists(SavePath))
        {
            return;
        }

        string json = File.ReadAllText(SavePath);
        PlayerData.SaveData playerData = JsonUtility.FromJson<PlayerData.SaveData>(json);
        PlayerData.LoadSaveData(playerData);
    }
}
