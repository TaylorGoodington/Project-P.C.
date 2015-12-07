using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;


public class LevelManager : MonoBehaviour {

	public float autoLoadNextLevelAfter;
	
	void Start () {
		if (Application.loadedLevel == 0) {
			if (autoLoadNextLevelAfter <= 0) {
			}else {
				Invoke ("LoadNextLevel",autoLoadNextLevelAfter);
			}
		}
	}

	public void LoadLevel (string name) {
		Application.LoadLevel (name);
	}

	public void QuitRequest () {
		Application.Quit ();
	}
	
	public void LoadNextLevel () {
		Application.LoadLevel(Application.loadedLevel + 1);
	}
	
	public void LevelBack () {
		Application.LoadLevel(PlayerPrefsManager.GetDeleteEntryPoint());
	}
}

