using UnityEngine;
using UnityEngine.SceneManagement;


public class LevelManager : MonoBehaviour {

	public static LevelManager levelManager;

    public int lastRegionLoaded;

    public int lastLevelPlayed;

	public float autoLoadNextLevelAfter;

    private GameObject cameraManager;

    public bool InMapScenes;

    //end of level info.
    public float levelTime;
    public int enemiesDefeated;
    public int BonusXP;

    public int level01;
	
	void Start () {
		levelManager = GetComponent<LevelManager>();
        cameraManager = GameObject.FindGameObjectWithTag("Cameras");
        lastRegionLoaded = 0;
        InMapScenes = false;
        if (SceneManager.GetActiveScene().name == "_Splash") {
            Invoke("LoadNextLevel", 3f);
        }
        //THIS NEEDS TO BE UPDATED MANUALLY
        level01 = 18;
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
        EquipmentInventory.equipmentInventory.UpdateEquippedStats();
        GameControl.gameControl.CalculateHealthAndMana(false);
        enemiesDefeated = 0;
        levelTime = 0;

        if (GameControl.gameControl.levelScores[level - level01 + 1].hasLevelBeenPlayed == false)
        {
            Debug.Log("First Time");
            GameControl.gameControl.levelScores[level - level01 + 1].hasLevelBeenPlayed = true;
            //play story animation.
            //Maybe this is a different scene that loads the movie textures?.
        }
        else
        {
            initializationAnimator.Play("LevelIntroTransition");
        }
    }

    //Checks which region should be selected based on what the last region loaded was.
    public void BackToRegion ()
    {
        GameControl.gameControl.Save();
        if (lastRegionLoaded == 1)
        {
            SceneManager.LoadScene("Region 1");
        }
        else if (lastRegionLoaded == 2)
        {
            SceneManager.LoadScene("Region 2");
        }
        else if (lastRegionLoaded == 3)
        {
            SceneManager.LoadScene("Region 3");
        }
        else if (lastRegionLoaded == 4)
        {
            SceneManager.LoadScene("Region 4");
        }
        else if (lastRegionLoaded == 5)
        {
            SceneManager.LoadScene("Region 5");
        }
        else if (lastRegionLoaded == 6)
        {
            SceneManager.LoadScene("Region 6");
        }
        else if (lastRegionLoaded == 7)
        {
            SceneManager.LoadScene("Region 7");
        }
        else if (lastRegionLoaded == 8)
        {
            SceneManager.LoadScene("Region 8");
        }
        else if (lastRegionLoaded == 9)
        {
            SceneManager.LoadScene("Region 9");
        }
        else if (lastRegionLoaded == 10)
        {
            SceneManager.LoadScene("Region 10");
        }
        //int region1FirstLevel = level01;
        //if (lastLevelPlayed >= region1FirstLevel && lastLevelPlayed < (region1FirstLevel + 10))
        //{
        //    SceneManager.LoadScene("Region 1");
        //}
        //else if (lastLevelPlayed >= (region1FirstLevel + 10) && lastLevelPlayed < (region1FirstLevel + 20))
        //{
        //    SceneManager.LoadScene("Region 2");
        //}
        //else if (lastLevelPlayed >= (region1FirstLevel + 20) && lastLevelPlayed < (region1FirstLevel + 30))
        //{
        //    SceneManager.LoadScene("Region 3");
        //}
        //else if (lastLevelPlayed >= (region1FirstLevel + 30) && lastLevelPlayed < (region1FirstLevel + 40))
        //{
        //    SceneManager.LoadScene("Region 4");
        //}
        //else if (lastLevelPlayed >= (region1FirstLevel + 40) && lastLevelPlayed < (region1FirstLevel + 50))
        //{
        //    SceneManager.LoadScene("Region 5");
        //}
        //else if (lastLevelPlayed >= (region1FirstLevel + 50) && lastLevelPlayed < (region1FirstLevel + 60))
        //{
        //    SceneManager.LoadScene("Region 6");
        //}
        //else if (lastLevelPlayed >= (region1FirstLevel + 60) && lastLevelPlayed < (region1FirstLevel + 70))
        //{
        //    SceneManager.LoadScene("Region 7");
        //}
        //else if (lastLevelPlayed >= (region1FirstLevel + 70) && lastLevelPlayed < (region1FirstLevel + 80))
        //{
        //    SceneManager.LoadScene("Region 8");
        //}
        //else if (lastLevelPlayed >= (region1FirstLevel + 80) && lastLevelPlayed < (region1FirstLevel + 90))
        //{
        //    SceneManager.LoadScene("Region 9");
        //}
        //else
        //{
        //    SceneManager.LoadScene("Region 10");
        //}
    }

    void OnLevelWasLoaded (int level)
    {
        //The Pit & Intro Pit
        if (level == level01 - 2 || level == level01 - 1)
        {
            lastLevelPlayed = level;
            InMapScenes = false;
            cameraManager.transform.GetChild(0).gameObject.SetActive(true);
            cameraManager.transform.GetChild(1).gameObject.SetActive(false);

            Animator initializationAnimator = GameObject.FindGameObjectWithTag("UserInterface").GetComponent<Animator>();
            EquipmentInventory.equipmentInventory.UpdateEquippedStats();
            GameControl.gameControl.CalculateHealthAndMana(false);
            enemiesDefeated = 0;
            levelTime = 0;
            initializationAnimator.Play("LevelIntroTransition");
            Debug.Log("pit");
        }
        //Playable levels.
        else if (level >= level01)
        {
            lastLevelPlayed = level;
            InMapScenes = false;
            cameraManager.transform.GetChild(0).gameObject.SetActive(true);
            cameraManager.transform.GetChild(1).gameObject.SetActive(false);
            InitializeLevel(level);
            Debug.Log("playable");
        }

        //Maps and Menus.
        else
        {
            LastRegionLoaded();
            cameraManager.transform.GetChild(0).gameObject.SetActive(false);
            cameraManager.transform.GetChild(1).gameObject.SetActive(true);
            InMapScenes = true;
            Debug.Log("maps");
        }
    }
}