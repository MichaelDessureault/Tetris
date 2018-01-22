using TMPro;
using UnityEngine;
using System.Collections;

public class Score : MonoBehaviour {
    // Static for quick references 
    public static int score = 0; 
    public static int[] scorePerLine = new int[4] { 50, 125, 200, 400 };

    public TextMeshProUGUI scoreNumberText;

    private int previousScore = 0;

    private bool scoringEnabled = true;
    
    private void Start() {
        if (scoreNumberText == null) {
            Debug.LogWarning("ScoreText is not found, score will not be updated");
            scoringEnabled = false;
        }
        
        SceneController.Instance.BeforeSceneUnload += Save;
    }
    
    // Using the update loop to update the score GUI on it's own if the previousScore is not the same as the currentScore
    // this allows for other Scripts to only have to update the static score variable
    void Update () {
        if (!scoringEnabled)
            return;

        if (previousScore != score) {
            UpdateScoreGUI();
            previousScore = score;
        }
	}
    
    void UpdateScoreGUI () {
        scoreNumberText.text = score.ToString();
    }

    void Save() {
        if (GameManager.Instance.gameOver) {
            HighScores.Instance.TryToAddScore(score);
            SaveData.Save();
        }
    }
}
