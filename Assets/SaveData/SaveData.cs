using UnityEngine;
using System.IO; 

public class SaveData {
    private const string highscoreJsonFile = "/HighScore.json";

    public static void Save () {
        SaveHighScores();
    }

    public static void Load () {
        LoadHighScores();
    }

    static void SaveHighScores() {
        try {
            string jsonString = HighScores.Instance.ToJsonString();
            File.WriteAllText(Application.streamingAssetsPath + highscoreJsonFile, jsonString);
        } catch (System.Exception) { }
    }

    static void LoadHighScores() {
        try {
            string jsonString = File.ReadAllText(Application.streamingAssetsPath + highscoreJsonFile);
            HighScores hs = JsonUtility.FromJson<HighScores>(jsonString);
            HighScores.Instance = hs;
        } catch (System.Exception) { }
    }
}
