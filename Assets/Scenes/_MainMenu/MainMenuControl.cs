using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

public class MainMenuControl : MonoBehaviour {

	public GameObject expandedGameSelection;
	public GameObject deleteGameVerification;
	public GameObject overwriteGameVerification;
	public GameObject overwriteNewGameVerification;
	public GameObject overwriteSlots;
    public GameObject blackScreen;
    public GameObject startObjects;
    public GameObject mainMenuObjects;
    public GameObject optionsObjects;
    public GameObject extrasObject;
    
    Animator menuAnimator;
    Animator startAnimator;
    Animator mainMenuAnimator;
    Animator optionsAnimator;
    Animator extrasAnimator;

    void Start ()
    {
        startAnimator = startObjects.gameObject.GetComponent<Animator>();
        mainMenuAnimator = mainMenuObjects.gameObject.GetComponent<Animator>();
        optionsAnimator = optionsObjects.gameObject.GetComponent<Animator>();
        extrasAnimator = extrasObject.gameObject.GetComponent<Animator>();

        if (GameControl.gameControl.startFromLoad)
        {
            blackScreen.SetActive(true);
            menuAnimator = GetComponent<Animator>();
            startObjects.SetActive(true);
            mainMenuObjects.SetActive(false);
            menuAnimator.Play("Transition In");
            startAnimator.Play("TitleInStart");
            GameControl.gameControl.mainMenuLevel = -1;
            GameControl.gameControl.startFromLoad = false;
            GameControl.gameControl.ResetNewGameData();
        }
    }

    //Start Button
    public void ToMainMenu ()
    {
        PlayerSoundEffects.playerSoundEffects.PlaySoundEffect(PlayerSoundEffects.playerSoundEffects.SoundEffectToArrayInt(PlayerSoundEffects.SoundEffect.MenuConfirm));
        EventSystem.current.SetSelectedGameObject(null);
        startAnimator.Play("TitleOut");
    }

    //called by the animator at the end of title out.
    public void MainMenuSceneStart()
    {
        GameControl.gameControl.mainMenuLevel = 1;
        startObjects.SetActive(false);
        mainMenuObjects.SetActive(true);
        RefreshGameLabels();
        mainMenuAnimator.Play("ObjectsIn");
    }

    public void StartSceneStart ()
    {
        startObjects.SetActive(true);
        mainMenuObjects.SetActive(false);
        startAnimator.Play("TitleIn");
    }

    public void OptionsSceneStart ()
    {

    }

    public void ExtrasSceneStart ()
    {

    }

    public void QuitGame()
    {
        PlayerSoundEffects.playerSoundEffects.PlaySoundEffect(PlayerSoundEffects.playerSoundEffects.SoundEffectToArrayInt(PlayerSoundEffects.SoundEffect.MenuConfirm));
        Application.Quit();
    }

    public void RefreshGameLabels () {
		if (File.Exists (Application.persistentDataPath + "/playerInfo1.dat")) {
			GameObject gameSlot = GameObject.FindGameObjectWithTag ("Game 1");
			Text newGameText = gameSlot.transform.GetChild (1).GetComponent<Text> ();
			newGameText.enabled = false;
			
			Text playerNameText = gameSlot.transform.GetChild (2).GetComponent<Text> ();
			playerNameText.enabled = true;
			playerNameText.text = PlayerPrefsManager.GetFile1PlayerName();
			
			Text playerLevelText = gameSlot.transform.GetChild (3).GetComponent<Text> ();
			playerLevelText.enabled = true;
			playerLevelText.text = "Level: " + PlayerPrefsManager.GetFile1PlayerLevel().ToString();
			
			Text playerProgressText = gameSlot.transform.GetChild (5).GetComponent<Text> ();
			playerProgressText.enabled = true;
			if (PlayerPrefsManager.GetFile1LevelProgress() == 100) {
				playerProgressText.text = "Clear!";
			} else {
				//TODO Need to make this lookup the actual level name someday.
				playerProgressText.text = PlayerPrefsManager.GetFile1LevelProgress().ToString();
			}
		} else {
			GameObject gameSlot = GameObject.FindGameObjectWithTag ("Game 1");
			Text newGameText = gameSlot.transform.GetChild (1).GetComponent<Text> ();
			newGameText.enabled = true;
			
			Text playerNameText = gameSlot.transform.GetChild (2).GetComponent<Text> ();
			playerNameText.enabled = false;
			
			Text playerLevelText = gameSlot.transform.GetChild (3).GetComponent<Text> ();
			playerLevelText.enabled = false;
			
			Text playerProgressText = gameSlot.transform.GetChild (5).GetComponent<Text> ();
			playerProgressText.enabled = false;
		}
		
		if (File.Exists (Application.persistentDataPath + "/playerInfo2.dat")) {
			GameObject gameSlot = GameObject.FindGameObjectWithTag ("Game 2");
			Text newGameText = gameSlot.transform.GetChild (1).GetComponent<Text> ();
			newGameText.enabled = false;
			
			Text playerNameText = gameSlot.transform.GetChild (2).GetComponent<Text> ();
			playerNameText.enabled = true;
			playerNameText.text = PlayerPrefsManager.GetFile2PlayerName();
			
			Text playerLevelText = gameSlot.transform.GetChild (3).GetComponent<Text> ();
			playerLevelText.enabled = true;
			playerLevelText.text = "Level: " + PlayerPrefsManager.GetFile2PlayerLevel().ToString();
			
			Text playerProgressText = gameSlot.transform.GetChild (5).GetComponent<Text> ();
			playerProgressText.enabled = true;
			if (PlayerPrefsManager.GetFile2LevelProgress() == 100) {
				playerProgressText.text = "Clear!";
			} else {
				//Need to make this lookup the actual level name someday.
				playerProgressText.text = PlayerPrefsManager.GetFile2LevelProgress().ToString();
			}
		} else {
			GameObject gameSlot = GameObject.FindGameObjectWithTag ("Game 2");
			Text newGameText = gameSlot.transform.GetChild (1).GetComponent<Text> ();
			newGameText.enabled = true;
			
			Text playerNameText = gameSlot.transform.GetChild (2).GetComponent<Text> ();
			playerNameText.enabled = false;
			
			Text playerLevelText = gameSlot.transform.GetChild (3).GetComponent<Text> ();
			playerLevelText.enabled = false;
			
			Text playerProgressText = gameSlot.transform.GetChild (5).GetComponent<Text> ();
			playerProgressText.enabled = false;
		}
		
		if (File.Exists (Application.persistentDataPath + "/playerInfo3.dat")) {
			GameObject gameSlot = GameObject.FindGameObjectWithTag ("Game 3");
			Text newGameText = gameSlot.transform.GetChild (1).GetComponent<Text> ();
			newGameText.enabled = false;
			
			Text playerNameText = gameSlot.transform.GetChild (2).GetComponent<Text> ();
			playerNameText.enabled = true;
			playerNameText.text = PlayerPrefsManager.GetFile3PlayerName();
			
			Text playerLevelText = gameSlot.transform.GetChild (3).GetComponent<Text> ();
			playerLevelText.enabled = true;
			playerLevelText.text = "Level: " + PlayerPrefsManager.GetFile3PlayerLevel().ToString();
			
			Text playerProgressText = gameSlot.transform.GetChild (5).GetComponent<Text> ();
			playerProgressText.enabled = true;
			if (PlayerPrefsManager.GetFile3LevelProgress() == 100) {
				playerProgressText.text = "Clear!";
			} else {
				//Need to make this lookup the actual level name someday.
				playerProgressText.text = PlayerPrefsManager.GetFile3LevelProgress().ToString();
			}
		} else {
			GameObject gameSlot = GameObject.FindGameObjectWithTag ("Game 3");
			Text newGameText = gameSlot.transform.GetChild (1).GetComponent<Text> ();
			newGameText.enabled = true;
			
			Text playerNameText = gameSlot.transform.GetChild (2).GetComponent<Text> ();
			playerNameText.enabled = false;
			
			Text playerLevelText = gameSlot.transform.GetChild (3).GetComponent<Text> ();
			playerLevelText.enabled = false;
			
			Text playerProgressText = gameSlot.transform.GetChild (5).GetComponent<Text> ();
			playerProgressText.enabled = false;
		}
	}
	
	public void OpenPreviousMainMenu (int mainMenuLevel)
    {
        PlayerSoundEffects.playerSoundEffects.PlaySoundEffect(PlayerSoundEffects.playerSoundEffects.SoundEffectToArrayInt(PlayerSoundEffects.SoundEffect.MenuNavigation));
		if (mainMenuLevel == 3)
        {
            MainMenuSceneStart();
		}
        else if (mainMenuLevel == 2)
        {
			Destroy(GameObject.FindGameObjectWithTag("Expanded Game Select"));
			EventSystem.current.SetSelectedGameObject(GameObject.FindGameObjectWithTag("Game " + PlayerPrefsManager.GetGameFile()));
		}
        else if (mainMenuLevel == 1)
        {
            mainMenuAnimator.Play("ObjectsOutStart");
        }
	}
	
	public void OpenPreviousCopyMenu (int copyMenuLevel) {
        PlayerSoundEffects.playerSoundEffects.PlaySoundEffect(PlayerSoundEffects.playerSoundEffects.SoundEffectToArrayInt(PlayerSoundEffects.SoundEffect.MenuNavigation));
        if (copyMenuLevel == 2) {
			if (GameObject.FindGameObjectWithTag("Overwrite New Game Verify")) {
				Destroy(GameObject.FindGameObjectWithTag("Overwrite New Game Verify"));
			} else {
				Destroy(GameObject.FindGameObjectWithTag("Overwrite Game Verify"));
			}
			EventSystem.current.SetSelectedGameObject(GameObject.FindGameObjectWithTag("Overwrite Slots").transform.GetChild(1).gameObject, null);
		} else if (copyMenuLevel == 1) {
			Destroy(GameObject.FindGameObjectWithTag("Overwrite Slots"));
			Debug.Log ("");
			ExpandGameSelection(PlayerPrefsManager.GetGameFile());
		}
	}
	
	public void OpenPreviousDeleteMenu (int deleteMenuLevel) {
        PlayerSoundEffects.playerSoundEffects.PlaySoundEffect(PlayerSoundEffects.playerSoundEffects.SoundEffectToArrayInt(PlayerSoundEffects.SoundEffect.MenuNavigation));

        if (deleteMenuLevel == 1) {
			DeleteGameNo();
		}
	}
	
	public void ExpandGameSelection (int fileNumber) {
        PlayerSoundEffects.playerSoundEffects.PlaySoundEffect(PlayerSoundEffects.playerSoundEffects.SoundEffectToArrayInt(PlayerSoundEffects.SoundEffect.MenuConfirm));
		
		PlayerPrefsManager.SetGameFile(fileNumber);
		
		GameObject gameSlot = GameObject.FindGameObjectWithTag("Game " + fileNumber);
		
		GameObject expansion = Instantiate(expandedGameSelection, new Vector3(0, -200, 0), Quaternion.identity) as GameObject;
		expansion.transform.SetParent(gameSlot.transform, false);
				
		EventSystem.current.SetSelectedGameObject(GameObject.Find("Play"), null);
		
		//add listeners
		Button playButton = expansion.transform.GetChild(1).GetComponent<Button>();
		playButton.onClick.AddListener(() => GameObject.FindObjectOfType<MainMenuControl>().PlayGame(fileNumber));
		
		//shut these off if no game file
		if (File.Exists(Application.persistentDataPath + "/playerInfo" + fileNumber + ".dat")) {
			Button copyButton = expansion.transform.GetChild(2).GetComponent<Button>();
			copyButton.onClick.AddListener(() => GameObject.FindObjectOfType<MainMenuControl>().CopyGame(fileNumber));
			
			Button deleteButton = expansion.transform.GetChild(3).GetComponent<Button>();
			deleteButton.onClick.AddListener(() => GameObject.FindObjectOfType<MainMenuControl>().DeleteGame(fileNumber));
		} else {
			Button copyButton = expansion.transform.GetChild(2).GetComponent<Button>();
			copyButton.onClick.AddListener(() => GameObject.FindObjectOfType<MainMenuControl>().PlayUnableNoise());
			
			Button deleteButton = expansion.transform.GetChild(3).GetComponent<Button>();
			deleteButton.onClick.AddListener(() => GameObject.FindObjectOfType<MainMenuControl>().PlayUnableNoise());
		}
        GameControl.gameControl.mainMenuLevel = 2;
	}
	
	public void PlayUnableNoise () {
        PlayerSoundEffects.playerSoundEffects.PlaySoundEffect(PlayerSoundEffects.playerSoundEffects.SoundEffectToArrayInt(PlayerSoundEffects.SoundEffect.MenuUnable));
    }
	
	public void PlayGame (int fileNumber) {
        PlayerSoundEffects.playerSoundEffects.PlaySoundEffect(PlayerSoundEffects.playerSoundEffects.SoundEffectToArrayInt(PlayerSoundEffects.SoundEffect.MenuConfirm));
        GameControl.gameControl.playerHasControl = false;

        GameControl.gameControl.mainMenuLevel = 0;

        if (File.Exists(Application.persistentDataPath + "/playerInfo" + fileNumber + ".dat")) {
            GameControl.gameControl.LoadFile(fileNumber);
            menuAnimator.Play("TransitionOut");
		} else {
            GameControl.gameControl.NewGame(fileNumber);

            //Play intro cinematic.

            //Remove after testing....
            menuAnimator.Play("TransitionOut");
        }
	}

    //Called after transition out animation.
    public void LoadWorldMap ()
    {
        SceneManager.LoadScene("World Map");
    }
	
	public void CopyGame (int fileNumber) {
        PlayerSoundEffects.playerSoundEffects.PlaySoundEffect(PlayerSoundEffects.playerSoundEffects.SoundEffectToArrayInt(PlayerSoundEffects.SoundEffect.MenuConfirm));


        int lowNumber;
		if (fileNumber >= 2) {
			lowNumber = 1;
		} else {
			lowNumber = 2;
		}
		int highNumber;
		if (fileNumber == 3) {
			highNumber = 2;
		} else {
			highNumber = 3;
		}
				
		GameObject gameSlot = GameObject.FindGameObjectWithTag("Game " + fileNumber);
		
		Destroy(GameObject.FindGameObjectWithTag("Expanded Game Select"));
		
		GameObject expansion = Instantiate(overwriteSlots, new Vector3(0, -200, 0), Quaternion.identity) as GameObject;
		expansion.transform.SetParent(gameSlot.transform, false);
		
		Text lowSlot = expansion.transform.GetChild(1).GetComponent<Text>();
		lowSlot.text = "File " + lowNumber;
		
		Text highSlot = expansion.transform.GetChild(2).GetComponent<Text>();
		highSlot.text = "File " + highNumber;
		
		EventSystem.current.SetSelectedGameObject(expansion.transform.GetChild(1).gameObject, null);
		
		foreach (Transform child in expansion.transform) {
			if (child.GetComponent<Button>() == true) {
				if (child.name == "Low Slot") {
					if (File.Exists(Application.persistentDataPath + "/playerInfo" + lowNumber + ".dat")) {
						child.GetComponent<Button>().onClick.AddListener(() => GameObject.FindObjectOfType<MainMenuControl>().OverwriteFile(lowNumber));
					} else {
						child.GetComponent<Button>().onClick.AddListener(() => GameObject.FindObjectOfType<MainMenuControl>().OverwriteNewGame(lowNumber));
					}
				} else {
					if (File.Exists(Application.persistentDataPath + "/playerInfo" + highNumber + ".dat")) {
						child.GetComponent<Button>().onClick.AddListener(() => GameObject.FindObjectOfType<MainMenuControl>().OverwriteFile(highNumber));
					} else {
						child.GetComponent<Button>().onClick.AddListener(() => GameObject.FindObjectOfType<MainMenuControl>().OverwriteNewGame(highNumber));
					}
				}
			}
		}
		GameControl.gameControl.mainMenuLevel = 0;
        GameControl.gameControl.mainMenuCopyLevel = 1;
	}
	
	public void OverwriteNewGame (int fileNumber) {
        PlayerSoundEffects.playerSoundEffects.PlaySoundEffect(PlayerSoundEffects.playerSoundEffects.SoundEffectToArrayInt(PlayerSoundEffects.SoundEffect.MenuNavigation));
        GameObject overwriteMenu = Instantiate(overwriteNewGameVerification);
		overwriteMenu.transform.SetParent(GameObject.FindGameObjectWithTag("Main Menu").transform, false);
		overwriteMenu.transform.GetChild(3).GetComponent<Button>().onClick.AddListener(() => GameObject.FindObjectOfType<MainMenuControl>().OverwriteNewGameYes(fileNumber));
		EventSystem.current.SetSelectedGameObject(overwriteMenu.transform.GetChild(3).gameObject);
        GameControl.gameControl.mainMenuCopyLevel = 2;
	}

	void OverwriteNewGameYes (int fileNumber) {
		int gameFile = PlayerPrefsManager.GetGameFile ();
        GameControl.gameControl.LoadFile (gameFile);
        GameControl.gameControl.SaveFile (fileNumber);
		RefreshGameLabels ();
		
		Destroy (GameObject.FindGameObjectWithTag ("Overwrite New Game Verify"));
		Destroy (GameObject.FindGameObjectWithTag ("Overwrite Slots"));
		Debug.Log ("OverwriteNewGameYes");
        GameControl.gameControl.mainMenuLevel = 1;
        GameControl.gameControl.mainMenuCopyLevel = 0;
	}
	
	public void OverwriteNewGameNo () {
		Destroy (GameObject.FindGameObjectWithTag ("Overwrite New Game Verify"));
		Debug.Log ("OverwriteNewGameNo");
		EventSystem.current.SetSelectedGameObject(GameObject.FindGameObjectWithTag ("Overwrite Slots").transform.GetChild(1).gameObject, null);
        GameControl.gameControl.mainMenuCopyLevel = 1;
	}
	
	public void OverwriteFile (int fileNumber) {
        PlayerSoundEffects.playerSoundEffects.PlaySoundEffect(PlayerSoundEffects.playerSoundEffects.SoundEffectToArrayInt(PlayerSoundEffects.SoundEffect.MenuConfirm));
        GameObject overwriteMenu = Instantiate(overwriteGameVerification);
		overwriteMenu.transform.SetParent(GameObject.FindGameObjectWithTag("Main Menu").transform, false);
		
		if (fileNumber == 1) {
			overwriteMenu.transform.GetChild(2).GetComponent<Text>().text = PlayerPrefsManager.GetFile1PlayerName();
			overwriteMenu.transform.GetChild(3).GetComponent<Text>().text = "Level: " + PlayerPrefsManager.GetFile1PlayerLevel().ToString();
		} else if (fileNumber == 2) {
			overwriteMenu.transform.GetChild(2).GetComponent<Text>().text = PlayerPrefsManager.GetFile2PlayerName();
			overwriteMenu.transform.GetChild(3).GetComponent<Text>().text = "Level: " + PlayerPrefsManager.GetFile2PlayerLevel().ToString();
		} else {
			overwriteMenu.transform.GetChild(2).GetComponent<Text>().text = PlayerPrefsManager.GetFile3PlayerName();
			overwriteMenu.transform.GetChild(3).GetComponent<Text>().text = "Level: " + PlayerPrefsManager.GetFile3PlayerLevel().ToString();
		}
		overwriteMenu.transform.GetChild(4).GetComponent<Button>().onClick.AddListener(() => GameObject.FindObjectOfType<MainMenuControl>().OverwriteFileYes(fileNumber));
		EventSystem.current.SetSelectedGameObject(overwriteMenu.transform.GetChild(4).gameObject);
        GameControl.gameControl.mainMenuCopyLevel = 2;
	}
	
	public void OverwriteFileYes (int fileNumber) {
		int gameFile = PlayerPrefsManager.GetGameFile ();
        GameControl.gameControl.LoadFile (gameFile);
        GameControl.gameControl.SaveFile (fileNumber);
		RefreshGameLabels ();
		
		Destroy (GameObject.FindGameObjectWithTag ("Overwrite Game Verify"));
		Destroy (GameObject.FindGameObjectWithTag ("Overwrite Slots"));
		Debug.Log ("OverwriteFileYes");
        GameControl.gameControl.mainMenuLevel = 1;
        GameControl.gameControl.mainMenuCopyLevel = 0;
	}
	
	public void OverwriteFileNo () {
		Destroy (GameObject.FindGameObjectWithTag ("Overwrite Game Verify"));
		Debug.Log ("OverwriteFileNo");
		EventSystem.current.SetSelectedGameObject(GameObject.FindGameObjectWithTag ("Overwrite Slots").transform.GetChild(1).gameObject, null);
        GameControl.gameControl.mainMenuCopyLevel = 1;
	}

    //Called by the animator
    public void FadeMusicOut()
    {
        MusicManager.musicManager.fadeMusicOut = true;
    }
    
    //Called by the animator
    public void FadeMusicIn()
    {
        MusicManager.musicManager.fadeMusicIn = true;
    }

    public void DeleteGame (int fileNumber) {
        PlayerSoundEffects.playerSoundEffects.PlaySoundEffect(PlayerSoundEffects.playerSoundEffects.SoundEffectToArrayInt(PlayerSoundEffects.SoundEffect.MenuUnable));
        Destroy(GameObject.FindGameObjectWithTag("Expanded Game Select"));
		GameObject deleteCanvas = Instantiate(deleteGameVerification);
		//deleteCanvas.transform.SetParent(mainMenuCanvas.transform, false);
		Text playerName = deleteCanvas.transform.GetChild(2).GetComponent<Text>();
		if (fileNumber == 1) {
			playerName.text = PlayerPrefsManager.GetFile1PlayerName();
		} else if (fileNumber == 2) {
			playerName.text = PlayerPrefsManager.GetFile2PlayerName();
		} else if (fileNumber == 3) {
			playerName.text = PlayerPrefsManager.GetFile3PlayerName();
		}
		Text playerLevel = deleteCanvas.transform.GetChild(3).GetComponent<Text>();
		if (fileNumber == 1) {
			playerLevel.text = "Level: " + PlayerPrefsManager.GetFile1PlayerLevel().ToString();
		} else if (fileNumber == 2) {
			playerLevel.text = "Level: " + PlayerPrefsManager.GetFile2PlayerLevel().ToString();
		} else if (fileNumber == 3) {
			playerLevel.text = "Level: " + PlayerPrefsManager.GetFile3PlayerLevel().ToString();
		}
		EventSystem.current.SetSelectedGameObject(GameObject.Find("No"), null);
        GameControl.gameControl.mainMenuLevel = 0;
        GameControl.gameControl.mainMenuDeleteLevel = 1;
	}
	
	public void DeleteGameYes () {
        PlayerSoundEffects.playerSoundEffects.PlaySoundEffect(PlayerSoundEffects.playerSoundEffects.SoundEffectToArrayInt(PlayerSoundEffects.SoundEffect.MenuConfirm));
        GameControl.gameControl.DeleteGame (PlayerPrefsManager.GetGameFile());
		Destroy(GameObject.FindGameObjectWithTag("Delete Game Verify"));
		Destroy(GameObject.FindGameObjectWithTag("Expanded Game Select"));
		Invoke("RefreshGameLabels", 0.0001f);
		EventSystem.current.SetSelectedGameObject(GameObject.FindGameObjectWithTag("Game " + PlayerPrefsManager.GetGameFile()), null);
		GameControl.gameControl.mainMenuLevel = 1;
        GameControl.gameControl.mainMenuDeleteLevel = 0;
	}
	
	public void DeleteGameNo () {
        PlayerSoundEffects.playerSoundEffects.PlaySoundEffect(PlayerSoundEffects.playerSoundEffects.SoundEffectToArrayInt(PlayerSoundEffects.SoundEffect.MenuNavigation));
        Destroy(GameObject.FindGameObjectWithTag("Delete Game Verify"));
		Debug.Log("");
		ExpandGameSelection(PlayerPrefsManager.GetGameFile());
        GameControl.gameControl.mainMenuDeleteLevel = 0;
	}
}