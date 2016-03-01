using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class LevelManager : MonoBehaviour {

	public static LevelManager levelManager;

    public int lastRegionLoaded;

    public int lastLevelPlayed;

	public float autoLoadNextLevelAfter;

    private GameObject cameraManager;

    public bool InMapScenes;
	
	void Start () {
		levelManager = GetComponent<LevelManager>();
        cameraManager = GameObject.FindGameObjectWithTag("Cameras");
        lastRegionLoaded = 0;
        InMapScenes = false;
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
        Animator initializationAnimator = GameObject.FindGameObjectWithTag("UserInterface").GetComponent<Animator>();
        GameControl.gameControl.playerHasControl = false;
        MusicManager.musicManager.LevelVictoryMusic();
        Text clearText = GameObject.FindGameObjectWithTag("Level Clear Text").GetComponent<Text>();
        clearText.enabled = true;
        //have animator play "exit level transition". The end of the animation should call "CallBackToRegion".
        //set level score in GameControl.gameControl.levelScores, remember to reference the fact that there are more scenes in the build than just the playable levels.
    }


    public void LastRegionLoaded ()
    {
        //Once I add all the scenes to the build I will make a list here to reference.
        int sceneBuildIndex = SceneManager.GetActiveScene().buildIndex;

        //Dont add the world map to this list.
        if (sceneBuildIndex == 5)
        {
            lastRegionLoaded = 0;
        }
    }


    //Used for game levels only.
    public void InitializeLevel ()
    {
        Animator initializationAnimator = GameObject.FindGameObjectWithTag("UserInterface").GetComponent<Animator>();
        //run script to re calculate stats for the begining of a level.
        //If the level hasn't been played Get the animator to play the "story intro animation". Else, play the "normal intro animation". In GameControl.gameControl.levelScores I can check the bool if its been played.
        //Reset enemies defeated counter at the end of intro animations.
        //start time counter at the end of intro animation.
    }


    //Checks which level should be selected based on what the last level played was.
    public void BackToRegion ()
    {
        if (lastLevelPlayed >= 10 && lastLevelPlayed < 20)
        {
            LoadLevel("Region 1");
        }
    }


    void OnLevelWasLoaded (int level)
    {
        LastRegionLoaded();

        //Playable levels.
        if (level > 6)
        {
            InitializeLevel();
            lastLevelPlayed = level;
            InMapScenes = false;
            cameraManager.transform.GetChild(0).gameObject.SetActive(true);
            cameraManager.transform.GetChild(1).gameObject.SetActive(false);
        }

        //Maps and Menus.
        else
        {
            cameraManager.transform.GetChild(0).gameObject.SetActive(false);
            cameraManager.transform.GetChild(1).gameObject.SetActive(true);
            InMapScenes = true;
        }
    }
}

