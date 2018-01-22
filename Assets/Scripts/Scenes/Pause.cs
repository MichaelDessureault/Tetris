using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Pause : MonoBehaviour {

    public Button ResumeButton;
    public TextMeshProUGUI mainText;
    public TextMeshProUGUI scoreText;
    
	void OnEnable () {
        Time.timeScale = 0;
	}

    void OnDisable () {
        Time.timeScale = 1;
    }
	
    public void SetMainText(string text) {
        mainText.text = text.ToUpper();
    }

    public void DisplayScore (int score) {
        scoreText.text = "FINAL SCORE: " + score.ToString();
    }

    public void Resume () {
        gameObject.SetActive(false);
    }

    public void MainMenu () {
        SceneController.Instance.LoadScene(Scenes.mainmenu);
    }
}
