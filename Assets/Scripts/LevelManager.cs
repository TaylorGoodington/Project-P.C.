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

    int level01;
	
	void Start () {
		levelManager = GetComponent<LevelManager>();
        cameraManager = GameObject.FindGameObjectWithTag("Cameras");
        lastRegionLoaded = 0;
        InMapScenes = false;
        if (SceneManager.GetActiveScene().name == "_Splash") {
            Invoke("LoadNextLevel", 3f);
        }
        level01 = SceneManager.GetSceneByName("Level 01").buildIndex;
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
        if (SceneManager.GetActiveScene().name == "Main Menu")
        {
            lastRegionLoaded = 0;
        }
        else if (SceneManager.GetActiveScene().name == "Region 1")
        {
            lastRegionLoaded = 1;
        }
        else if (SceneManager.GetActiveScene().name == "Region 2")
        {
            lastRegionLoaded = 2;
        }
        else if (SceneManager.GetActiveScene().name == "Region 3")
        {
            lastRegionLoaded = 3;
        }
        else if (SceneManager.GetActiveScene().name == "Region 4")
        {
            lastRegionLoaded = 4;
        }
        else if (SceneManager.GetActiveScene().name == "Region 5")
        {
            lastRegionLoaded = 5;
        }
        else if (SceneManager.GetActiveScene().name == "Region 6")
        {
            lastRegionLoaded = 6;
        }
        else if (SceneManager.GetActiveScene().name == "Region 7")
        {
            lastRegionLoaded = 7;
        }
        else if (SceneManager.GetActiveScene().name == "Region 8")
        {
            lastRegionLoaded = 8;
        }
        else if (SceneManager.GetActiveScene().name == "Region 9")
        {
            lastRegionLoaded = 9;
        }
        else if (SceneManager.GetActiveScene().name == "Region 10")
        {
            lastRegionLoaded = 10;
        }
    }

    //Used for game levels only.
    public void InitializeLevel (int level)
    {
        Animator initializationAnimator = GameObject.FindGameObjectWithTag("UserInterface").GetComponent<Animator>();
        //run script to re calculate stats for the begining of a level.
        
        //If the level hasn't been played Get the animator to play the "story intro animation". Else, play the "normal intro animation". In GameControl.gameControl.levelScores I can check the bool if its been played.
        if (GameControl.gameControl.levelScores[level].hasLevelBeenPlayed == false)
        {
            //play story animation.
        }
        
        //play regular fade in transition.
        
        
        //Reset enemies defeated counter at the end of intro animations.
        //start time counter at the end of intro animation.
    }

    //Checks which level should be selected based on what the last level played was.
    public string BackToRegion ()
    {
        int region1FirstLevel = level01;
        if (lastLevelPlayed >= region1FirstLevel && lastLevelPlayed < (region1FirstLevel + 10))
        {
            return "Region 1";
        }
        else if (lastLevelPlayed >= (region1FirstLevel + 10) && lastLevelPlayed < (region1FirstLevel + 20))
        {
            return "Region 2";
        }
        else if (lastLevelPlayed >= (region1FirstLevel + 20) && lastLevelPlayed < (region1FirstLevel + 30))
        {
            return "Region 3";
        }
        else if (lastLevelPlayed >= (region1FirstLevel + 30) && lastLevelPlayed < (region1FirstLevel + 40))
        {
            return "Region 4";
        }
        else if (lastLevelPlayed >= (region1FirstLevel + 40) && lastLevelPlayed < (region1FirstLevel + 50))
        {
            return "Region 5";
        }
        else if (lastLevelPlayed >= (region1FirstLevel + 50) && lastLevelPlayed < (region1FirstLevel + 60))
        {
            return "Region 6";
        }
        else if (lastLevelPlayed >= (region1FirstLevel + 60) && lastLevelPlayed < (region1FirstLevel + 70))
        {
            return "Region 7";
        }
        else if (lastLevelPlayed >= (region1FirstLevel + 70) && lastLevelPlayed < (region1FirstLevel + 80))
        {
            return "Region 8";
        }
        else if (lastLevelPlayed >= (region1FirstLevel + 80) && lastLevelPlayed < (region1FirstLevel + 90))
        {
            return "Region 9";
        }
        else
        {
            return "Region 10";
        }
    }

    //This should be done now.
    void OnLevelWasLoaded (int level)
    {
        //Playable levels.
        if (level >= level01)
        {
            lastLevelPlayed = level;
            InMapScenes = false;
            cameraManager.transform.GetChild(0).gameObject.SetActive(true);
            cameraManager.transform.GetChild(1).gameObject.SetActive(false);
            InitializeLevel(level);
            GameControl.gameControl.levelScores[level].hasLevelBeenPlayed = true;
        }

        //Maps and Menus.
        else
        {
            LastRegionLoaded();
            cameraManager.transform.GetChild(0).gameObject.SetActive(false);
            cameraManager.transform.GetChild(1).gameObject.SetActive(true);
            InMapScenes = true;
        }
    }
}

