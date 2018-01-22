using UnityEngine;
using UnityEngine.Audio;

public class Settings : MonoBehaviour {
    public AudioMixer audioMixer;
    
    public GameObject mainMenuPanel;
    public GameObject optionsPanel;

    public void ResetHighScores () {
        HighScores.Instance.Reset();
    }

	public void SetVolume (float volume) {
        audioMixer.SetFloat("volume", volume);
	}

    public void MainMenu () {
        optionsPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }
}
