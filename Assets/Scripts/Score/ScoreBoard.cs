using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreBoard : MonoBehaviour {
    public TextMeshProUGUI[] scoreListTextMeshes;

    public GameObject scoreBoardPanel;
    public GameObject mainMenuPanel;
    
    public void OnEnable() {
        Populate();
    }

    void Populate() {
        List<int> scoreList = HighScores.Instance.scoreList;
        if (scoreList.Count != 0) {
            for (int i = 0; i < scoreList.Count; i++) {
                 scoreListTextMeshes[i].text = (i + 1).ToString() + ". " + scoreList[i].ToString();
            }
        }
    }
     
    public void MainMenu () {
        scoreBoardPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }
}
