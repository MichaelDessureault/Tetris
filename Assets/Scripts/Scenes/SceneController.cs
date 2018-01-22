using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public enum Scenes {
    mainmenu,
    game
};

public class SceneController : MonoBehaviour
{
	#region singleton
	private static SceneController _instance;

	public static SceneController Instance {
		get { 
			if (_instance == null) {
				_instance = FindObjectOfType<SceneController> ();

				if (_instance == null) {
					_instance = new GameObject ("ScenesController").AddComponent<SceneController> () as SceneController;
				}
			}

			return _instance;
		}
	}
    #endregion

    private string[] sceneNames = new string[2] { "Main Menu", "Game" };

	public event Action BeforeSceneUnload;
	public event Action AfterSceneLoad;
    
	private bool isLoading = false;
    
	public void LoadScene (Scenes scene) {
        if (!isLoading)
            StartCoroutine (SwitchScene (sceneNames[(int) scene]));
	}

	private IEnumerator SwitchScene (string sceneName) {
        isLoading = true;

		if (BeforeSceneUnload != null)
			BeforeSceneUnload ();

        yield return SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);

		if (AfterSceneLoad != null)
			AfterSceneLoad ();

        isLoading = false;
	}
}

