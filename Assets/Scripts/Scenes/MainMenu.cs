using UnityEngine;

public class MainMenu : MonoBehaviour {

    public GameObject mainMenuPanel;
    public GameObject optionsPanel;
    public GameObject highScoresPanel;

    private void Start() {
        Time.timeScale = 1;
    }
    public void Play () {
        SceneController.Instance.LoadScene(Scenes.game);
    }

    public void HighScores () {
        SaveData.Load();

        mainMenuPanel.SetActive(false);
        highScoresPanel.SetActive(true);
	}

    public void Options () {
        mainMenuPanel.SetActive(false);
        optionsPanel.SetActive(true);
    }
}
