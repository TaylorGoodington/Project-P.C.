using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class LevelManager : MonoBehaviour {

	public static LevelManager levelManager;

    public int lastRegionLoaded;

	public float autoLoadNextLevelAfter;
	
	void Start () {
		levelManager = GetComponent<LevelManager>();
        lastRegionLoaded = 0;
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

    //Gets the last scene loaded, if that scene was a region map or the starting scene we change the region index so the world map knows where to load the player.
    public void LastRegionLoaded ()
    {
        //Once I add all the scenes to the build I will make a list here to reference.
        int sceneBuildIndex = SceneManager.GetActiveScene().buildIndex;
        if (sceneBuildIndex == 5)
        {
            lastRegionLoaded = 0;
        }
    }

    void OnLevelWasLoaded (int level)
    {
        LastRegionLoaded();
    }
}

