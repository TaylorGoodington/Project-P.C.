using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Collections.Generic;

public class MainMenuControl : MonoBehaviour {

	public GameObject expandedGameSelection;
	public GameObject mainMenuCanvas;
	public GameObject deleteGameVerification;
	
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
			playerLevelText.text = PlayerPrefsManager.GetFile1PlayerLevel().ToString();
			
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
			playerLevelText.text = PlayerPrefsManager.GetFile2PlayerLevel().ToString();
			
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
			playerLevelText.text = PlayerPrefsManager.GetFile3PlayerLevel().ToString();
			
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
	
	//I dont think I need this here due to explicit button paths.
//	public void TurnOffAllButtons () {
//		foreach (Transform child in mainMenuCanvas.transform) {
//			if (child.GetComponent<Button>()) {
//				child.GetComponent<Button>().interactable = false;
//			}
//		}
//	}
//	
//	public void TurnOnAllButtons () {
//		foreach (Transform child in mainMenuCanvas.transform) {
//			if (child.GetComponent<Button>()) {
//				child.GetComponent<Button>().interactable = true;
//			}
//		}
//	}
	
	public void ExpandGameSelection (int fileNumber) {
		playerSoundEffects.PlaySoundEffect(playerSoundEffects.SoundEffectToArrayInt(PlayerSoundEffects.SoundEffect.MenuConfirm));
		
		PlayerPrefsManager.SetGameFile(fileNumber);
		
		GameObject gameSlot = GameObject.FindGameObjectWithTag("Game " + fileNumber);
		
		GameObject expansion = Instantiate(expandedGameSelection, new Vector3(0, -200, 0), Quaternion.identity) as GameObject;
		expansion.transform.SetParent(gameSlot.transform, false);
		
//		TurnOffAllButtons();
		
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
	
	}
	
	
	public void DeleteGame (int fileNumber) {
		playerSoundEffects = GameObject.FindObjectOfType<PlayerSoundEffects>();
		playerSoundEffects.PlaySoundEffect(playerSoundEffects.SoundEffectToArrayInt(PlayerSoundEffects.SoundEffect.MenuNavigation));
		
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
			playerLevel.text = PlayerPrefsManager.GetFile1PlayerLevel().ToString();
		} else if (fileNumber == 2) {
			playerLevel.text = PlayerPrefsManager.GetFile2PlayerLevel().ToString();
		} else if (fileNumber == 3) {
			playerLevel.text = PlayerPrefsManager.GetFile3PlayerLevel().ToString();
		}
		EventSystem.current.SetSelectedGameObject(GameObject.Find("No"), null);
	}
	
	public void DeleteGameYes () {
		playerSoundEffects = GameObject.FindObjectOfType<PlayerSoundEffects>();
		playerSoundEffects.PlaySoundEffect(playerSoundEffects.SoundEffectToArrayInt(PlayerSoundEffects.SoundEffect.MenuConfirm));
		gameControl.DeleteGame (PlayerPrefsManager.GetGameFile());
		Destroy(GameObject.FindGameObjectWithTag("Delete Game Verify"));
		Destroy(GameObject.FindGameObjectWithTag("Expanded Game Select"));
		Invoke("RefreshGameLabels", 0.0001f);
		EventSystem.current.SetSelectedGameObject(GameObject.FindGameObjectWithTag("Game " + PlayerPrefsManager.GetGameFile()), null);
	}
	
	public void DeleteGameNo () {
		playerSoundEffects = GameObject.FindObjectOfType<PlayerSoundEffects>();
		playerSoundEffects.PlaySoundEffect(playerSoundEffects.SoundEffectToArrayInt(PlayerSoundEffects.SoundEffect.MenuNavigation));
		Destroy(GameObject.FindGameObjectWithTag("Delete Game Verify"));
		Debug.Log("");
		EventSystem.current.SetSelectedGameObject(GameObject.Find("Play"), null);
	}
	
	//TODO:
	//DONE....Write functions to for expanding the game slots (when you click they instantiate a second menu that appears below.
	//DONE....Write function that replaces "New Game" with appropriate info.
	//DONE....Wire up sounds.
	//DONE....write delete options function.
	//write copy options function.
	//Hook up "back" function.
}
