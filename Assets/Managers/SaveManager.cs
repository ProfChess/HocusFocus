using System.Collections.Generic;
using System.IO;
using UnityEngine;


[System.Serializable]
public class SaveData
{
    public string startingScene;                    //Most recent save point
    public Vector2 spawnLocation;                   //Where to start in scene
    public float HpUpgrades;                        //Amount of Hp upgrade books found
    public float ManaUpgrades;                      //Amound of mana upgrade books found
    public bool dashFound;                          //T/F if player found dash
    public bool jumpFound;                          //T/F if player found jump
    public bool teleFound;                          //T/F if player found tele
    public List<string> playerItems;                //List of items player has collected
    public List<FastTravelPoint> fastTravelPoints;  //List of Travel Points
}


public static class SaveManager
{
    private static string _SaveFilePath = Path.Combine(Application.persistentDataPath, "SaveData.json");

    public static void SaveGame(SaveData data)
    {
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(_SaveFilePath, json);
        Debug.Log("Game Saved");
    }

    public static SaveData LoadGame()
    {
        if (File.Exists(_SaveFilePath))
        {
            string json = File.ReadAllText(_SaveFilePath);
            SaveData data = JsonUtility.FromJson<SaveData>(json);
            Debug.Log("Game Loaded");
            Debug.Log(_SaveFilePath);
            return data;
        }

        else
        {
            Debug.Log("Save File Not Found");
            return null;
        }
    }


}
