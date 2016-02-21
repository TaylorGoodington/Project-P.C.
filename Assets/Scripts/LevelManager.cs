using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class LevelManager : MonoBehaviour {

	public static LevelManager levelManager;

	public float autoLoadNextLevelAfter;
	
	void Start () {
		levelManager = GetComponent<LevelManager>();
        if (SceneManager.GetActiveScene().name == "_Splash") {
            Invoke("LoadNextLevel", 3f);
        }
	}

	public void LoadLevel (string name) {
		SceneManager.LoadScene (name);
	}

	public void QuitRequest () {
		Application.Quit ();
	}
	
	public void LevelBack () {
		SceneManager.LoadScene (PlayerPrefsManager.GetDeleteEntryPoint());
	}

    public void LoadNextLevel() {
        int sceneBuildIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(sceneBuildIndex + 1);
    }

    public void EndOfLevel ()
    {
        GameControl.gameControl.playerHasControl = false;
        MusicManager.musicManager.LevelVictoryMusic();
        Text clearText = GameObject.FindGameObjectWithTag("Level Clear Text").GetComponent<Text>();
        clearText.enabled = true;
    }
}

