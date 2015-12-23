using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

public class MainMenuControl : MonoBehaviour {

	public GameObject expandedGameSelection;
	public GameObject mainMenuCanvas;
	public GameObject deleteGameVerification;
	public GameObject overwriteGameVerification;
	public GameObject overwriteNewGameVerification;
	public GameObject overwriteSlots;
	
	//Need to be found at the begining.
	public GameControl gameControl;
	public PlayerSoundEffects playerSoundEffects;

	// Use this for initialization
	void Start () {
		mainMenuCanvas = GameObject.FindGameObjectWithTag("Main Menu");
		playerSoundEffects = GameObject.FindObjectOfType<PlayerSoundEffects>();
		gameControl = GameObject.FindObjectOfType<GameControl>();
		
		EventSystem.current.SetSelectedGameObject(GameObject.FindGameObjectWithTag("Game 1"),null);
		
		//I think I will have the system check for new display info here and after copy and delete.
		RefreshGameLabels ();
		
		gameControl.mainMenuLevel = 1;
	}
	
	// Update is called once per frame
	void Update () {
		
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
				//Need to make this lookup the actual level name someday.
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
	
	public void OpenPreviousMainMenu (int mainMenuLevel) {
		playerSoundEffects = GameObject.FindObjectOfType<PlayerSoundEffects>();
		playerSoundEffects.PlaySoundEffect(playerSoundEffects.SoundEffectToArrayInt(PlayerSoundEffects.SoundEffect.MenuNavigation));
		if (mainMenuLevel == 3) {
			SceneManager.LoadScene("Main Menu");
		} else if (mainMenuLevel == 2) {
			Destroy(GameObject.FindGameObjectWithTag("Expanded Game Select"));
			EventSystem.current.SetSelectedGameObject(GameObject.FindGameObjectWithTag("Game " + PlayerPrefsManager.GetGameFile()));
		} else if (mainMenuLevel == 1) {
            SceneManager.LoadScene("Start");
		}
	}
	
	public void OpenPreviousCopyMenu (int copyMenuLevel) {
		playerSoundEffects = GameObject.FindObjectOfType<PlayerSoundEffects>();
		playerSoundEffects.PlaySoundEffect(playerSoundEffects.SoundEffectToArrayInt(PlayerSoundEffects.SoundEffect.MenuNavigation));
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
		playerSoundEffects = GameObject.FindObjectOfType<PlayerSoundEffects>();
		playerSoundEffects.PlaySoundEffect(playerSoundEffects.SoundEffectToArrayInt(PlayerSoundEffects.SoundEffect.MenuNavigation));

		if (deleteMenuLevel == 1) {
			DeleteGameNo();
		}
	}
	
	public void ExpandGameSelection (int fileNumber) {
		playerSoundEffects.PlaySoundEffect(playerSoundEffects.SoundEffectToArrayInt(PlayerSoundEffects.SoundEffect.MenuConfirm));
		
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
		gameControl = GameObject.FindObjectOfType<GameControl>();
		gameControl.mainMenuLevel = 2;
	}
	
	
	public void PlayUnableNoise () {
		playerSoundEffects = GameObject.FindObjectOfType<PlayerSoundEffects>();
		playerSoundEffects.PlaySoundEffect(playerSoundEffects.SoundEffectToArrayInt(PlayerSoundEffects.SoundEffect.MenuUnable));
	}
	
	
	public void PlayGame (int fileNumber) {
		playerSoundEffects.PlaySoundEffect(playerSoundEffects.SoundEffectToArrayInt(PlayerSoundEffects.SoundEffect.MenuConfirm));
		
		if(File.Exists(Application.persistentDataPath + "/playerInfo" + fileNumber + ".dat")) {
			gameControl.LoadFile(fileNumber);
		} else {
			gameControl.NewGame(fileNumber);
		}
	}
	
	
	public void CopyGame (int fileNumber) {
		playerSoundEffects = GameObject.FindObjectOfType<PlayerSoundEffects>();
		playerSoundEffects.PlaySoundEffect(playerSoundEffects.SoundEffectToArrayInt(PlayerSoundEffects.SoundEffect.MenuConfirm));
		
		
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
		gameControl.mainMenuLevel = 0;
		gameControl.mainMenuCopyLevel = 1;
	}
	
	public void OverwriteNewGame (int fileNumber) {
		playerSoundEffects = GameObject.FindObjectOfType<PlayerSoundEffects>();
		playerSoundEffects.PlaySoundEffect(playerSoundEffects.SoundEffectToArrayInt(PlayerSoundEffects.SoundEffect.MenuNavigation));
		GameObject overwriteMenu = Instantiate(overwriteNewGameVerification);
		overwriteMenu.transform.SetParent(GameObject.FindGameObjectWithTag("Main Menu").transform, false);
		overwriteMenu.transform.GetChild(3).GetComponent<Button>().onClick.AddListener(() => GameObject.FindObjectOfType<MainMenuControl>().OverwriteNewGameYes(fileNumber));
		EventSystem.current.SetSelectedGameObject(overwriteMenu.transform.GetChild(3).gameObject);
		gameControl.mainMenuCopyLevel = 2;
	}

	void OverwriteNewGameYes (int fileNumber) {
		int gameFile = PlayerPrefsManager.GetGameFile ();
		gameControl.LoadFile (gameFile);
		gameControl.SaveFile (fileNumber);
		RefreshGameLabels ();
		
		Destroy (GameObject.FindGameObjectWithTag ("Overwrite New Game Verify"));
		Destroy (GameObject.FindGameObjectWithTag ("Overwrite Slots"));
		Debug.Log ("OverwriteNewGameYes");
		gameControl.mainMenuLevel = 1;
		gameControl.mainMenuCopyLevel = 0;
	}
	
	public void OverwriteNewGameNo () {
		Destroy (GameObject.FindGameObjectWithTag ("Overwrite New Game Verify"));
		Debug.Log ("OverwriteNewGameNo");
		EventSystem.current.SetSelectedGameObject(GameObject.FindGameObjectWithTag ("Overwrite Slots").transform.GetChild(1).gameObject, null);
		gameControl.mainMenuCopyLevel = 1;
	}
	
	public void OverwriteFile (int fileNumber) {
		playerSoundEffects = GameObject.FindObjectOfType<PlayerSoundEffects>();
		playerSoundEffects.PlaySoundEffect(playerSoundEffects.SoundEffectToArrayInt(PlayerSoundEffects.SoundEffect.MenuNavigation));
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
		gameControl.mainMenuCopyLevel = 2;
	}
	
	public void OverwriteFileYes (int fileNumber) {
		int gameFile = PlayerPrefsManager.GetGameFile ();
		gameControl.LoadFile (gameFile);
		gameControl.SaveFile (fileNumber);
		RefreshGameLabels ();
		
		Destroy (GameObject.FindGameObjectWithTag ("Overwrite Game Verify"));
		Destroy (GameObject.FindGameObjectWithTag ("Overwrite Slots"));
		Debug.Log ("OverwriteFileYes");
		gameControl.mainMenuLevel = 1;
		gameControl.mainMenuCopyLevel = 0;
	}
	
	public void OverwriteFileNo () {
		Destroy (GameObject.FindGameObjectWithTag ("Overwrite Game Verify"));
		Debug.Log ("OverwriteFileNo");
		EventSystem.current.SetSelectedGameObject(GameObject.FindGameObjectWithTag ("Overwrite Slots").transform.GetChild(1).gameObject, null);
		gameControl.mainMenuCopyLevel = 1;
	}
	
	
	public void DeleteGame (int fileNumber) {
		playerSoundEffects = GameObject.FindObjectOfType<PlayerSoundEffects>();
		playerSoundEffects.PlaySoundEffect(playerSoundEffects.SoundEffectToArrayInt(PlayerSoundEffects.SoundEffect.MenuNavigation));
		Destroy(GameObject.FindGameObjectWithTag("Expanded Game Select"));
		GameObject deleteCanvas = Instantiate(deleteGameVerification);
		deleteCanvas.transform.SetParent(mainMenuCanvas.transform, false);
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
		gameControl.mainMenuLevel = 0;
		gameControl.mainMenuDeleteLevel = 1;
	}
	
	public void DeleteGameYes () {
		playerSoundEffects = GameObject.FindObjectOfType<PlayerSoundEffects>();
		playerSoundEffects.PlaySoundEffect(playerSoundEffects.SoundEffectToArrayInt(PlayerSoundEffects.SoundEffect.MenuConfirm));
		gameControl.DeleteGame (PlayerPrefsManager.GetGameFile());
		Destroy(GameObject.FindGameObjectWithTag("Delete Game Verify"));
		Destroy(GameObject.FindGameObjectWithTag("Expanded Game Select"));
		Invoke("RefreshGameLabels", 0.0001f);
		EventSystem.current.SetSelectedGameObject(GameObject.FindGameObjectWithTag("Game " + PlayerPrefsManager.GetGameFile()), null);
		gameControl.mainMenuLevel = 1;
		gameControl.mainMenuDeleteLevel = 0;
	}
	
	public void DeleteGameNo () {
		playerSoundEffects = GameObject.FindObjectOfType<PlayerSoundEffects>();
		playerSoundEffects.PlaySoundEffect(playerSoundEffects.SoundEffectToArrayInt(PlayerSoundEffects.SoundEffect.MenuNavigation));
		Destroy(GameObject.FindGameObjectWithTag("Delete Game Verify"));
		Debug.Log("");
		ExpandGameSelection(PlayerPrefsManager.GetGameFile());
		gameControl.mainMenuDeleteLevel = 0;
	}
}
