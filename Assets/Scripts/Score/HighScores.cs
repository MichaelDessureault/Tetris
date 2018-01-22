using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HighScores {

    #region singleton
    private static HighScores _instance;
    public static HighScores Instance {
        get {
            if (_instance == null)
                _instance = new HighScores();
            return _instance;
        }
        set {
            _instance = value;
        }
    }
    #endregion

    private const int KMaxScores = 3;

    public List<int> scoreList = new List<int>() { 0, 0, 0 };
    
    public void TryToAddScore (int score) {
        for (int i = 0; i < scoreList.Count; i++) {
            if (score > scoreList[i]) {
                scoreList.Insert(i, score);
                break;
            }
        }

        if (scoreList.Count > KMaxScores) {
            scoreList.RemoveAt(KMaxScores);
        }
    }

    public void Reset () {
        scoreList.Clear();
        scoreList = new List<int>() { 0, 0, 0 };
        SaveData.Save();
    }

    public string ToJsonString () {
        return JsonUtility.ToJson(this);
    }
}
