using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelManager : MonoBehaviour {

    public const int KMaxLevel = 15;
    public static int linesLeft = 5;
    
    public TextMeshProUGUI levelNumerText;
    public TextMeshProUGUI linesLeftNumberText;

    private int[] linesPerLevel = new int[KMaxLevel] {
        5, 10, 15, 20, 25, 30, 35, 40, 45, 50, 55, 60, 65, 70, 75
    };

    private float[] levelDropSpeeds = new float[KMaxLevel] {
        0.8f,0.75f,0.7f,0.65f,0.6f,0.55f,0.5f,0.45f,0.4f,0.35f,0.3f,0.25f,0.2f,0.15f,0.1f
    };

    // Defaults are levelDropSpeed * 16 + 1, with a minimum of 2.6f
    private float[] levelForceNextShapeTime = new float[KMaxLevel] {
        13.8f, 13f, 12.2f, 11.4f, 10.6f, 9.8f, 9f,8.2f, 7.4f, 6.6f, 5.8f, 5f, 4.2f, 3.4f, 2.6f
    };

    private int currentLevel = 1;
    private bool levelManagerDisabled = false;
    private int previousLinesLeft = 5;

    private void Start() {
        if (levelNumerText == null || linesLeftNumberText == null)
            levelManagerDisabled = true;
    }

    // Using the update loop to update the level GUI on it's own if the previousLinesLeft is not the same as the current linesLeft
    // this allows for other Scripts to only have to update the static linesLeft variable
    private void Update() {
        if (levelManagerDisabled)
            return;

        if (previousLinesLeft != linesLeft) {
            UpdateLevelGUI();
            previousLinesLeft = linesLeft;
        }
    }

    public void UpdateLevelGUI () {
        if (linesLeft <= 0)
            UpdateLevel();
        else
            linesLeftNumberText.text = linesLeft.ToString();
    }

    void UpdateLevel() {
        currentLevel++;

        if (currentLevel == KMaxLevel + 1) {
            linesLeftNumberText.text = "0";
            GameManager.Instance.GameOver();
        } else {
            levelNumerText.text = currentLevel.ToString();

            linesLeft += linesPerLevel[currentLevel - 1];
            linesLeftNumberText.text = linesLeft.ToString();

            Movement.downSpeed = levelDropSpeeds[currentLevel];
            Movement.forceNextShapeTime = levelForceNextShapeTime[currentLevel];
        }
    }
}
