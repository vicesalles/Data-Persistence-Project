using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DataManager : MonoBehaviour
{

    public static DataManager Instance;

    public static string playerName = "";
    public static int playerScore = 0;
    public static SaveData savedData;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    
    [Serializable]
    public class SaveData{
        public string score;
        public string name;
    }

    public void setPlayerName(string newName)
    {
        playerName = newName;
    }

    public void setPlayerScore(int newScore)
    {
        playerScore = newScore;
    }

    public void navigateToGame()
    {
        if(playerName == "")
        {
            playerName = "Dude";
        }

        SceneManager.LoadScene(1);

    }

    public static void SaveGame(SaveData gameData)
    {      
       
        int newScore;
        int oldRecord;
     
        int.TryParse(savedData.score, out oldRecord);
        int.TryParse(gameData.score, out newScore);


        if (newScore > oldRecord)
        {
            string json = JsonUtility.ToJson(gameData);
            File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
        }       

    }

    public static SaveData LoadData()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);
            savedData = data;
            return data;
        }

        SaveData empty = new SaveData();
        empty.name = "Player";
        empty.score = "0";
        savedData = empty;
        return empty;
    }

    public static string LoadBestScore()
    {
        SaveData data = LoadData();
        return $"Best Score: {data.name}|{data.score}";
    }
}
