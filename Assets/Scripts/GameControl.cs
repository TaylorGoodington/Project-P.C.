using UnityEngine;
using System.Collections;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class GameControl : MonoBehaviour {

	public static GameControl gameControl;
	private LevelManager levelManager;
	
	
	public string playerName;
	public int gameProgress;
	public int playerLevel;
	public int baseStrength;
	public int baseDefense;
	public int baseSpeed;
	public int baseIntelligence;
	public int baseHealth;
	public int baseMana;

	void Awake () {
		DontDestroyOnLoad (gameObject);
	}
	
	
	//**************************************************************************************
	//Save Section
	//**************************************************************************************
	
	
	public void Save () {
		BinaryFormatter binaryFormatter = new BinaryFormatter ();
		FileStream saveFile = File.Create (Application.persistentDataPath + "/playerInfo" + PlayerPrefsManager.GetGameFile() + ".dat"); //Its possible gameNumber might need to be based to the mehod.
		
		PlayerData playerData = new PlayerData ();
		
		playerData.playerName = playerName;
		playerData.gameProgress = gameProgress;
		playerData.playerLevel = playerLevel;
		playerData.baseStrength = baseStrength;
		playerData.baseDefense = baseDefense;
		playerData.baseSpeed = baseSpeed;
		playerData.baseIntelligence = baseIntelligence;
		playerData.baseHealth = baseHealth;
		playerData.baseMana = baseMana;
		
		binaryFormatter.Serialize (saveFile, playerData);
		saveFile.Close();
	}
	
	public void SaveFile1 () {
		PlayerPrefsManager.SetGameFile(1);
		PlayerPrefsManager.SetFile1PlayerName(playerName);
		PlayerPrefsManager.SetFile1LevelProgress(gameProgress);
		Save ();
	}
	
	public void SaveFile2 () {
		PlayerPrefsManager.SetGameFile(2);
		PlayerPrefsManager.SetFile2PlayerName(playerName);
		PlayerPrefsManager.SetFile2LevelProgress(gameProgress);
		Save ();
	}
	
	public void SaveFile3 () {
		PlayerPrefsManager.SetGameFile(3);
		PlayerPrefsManager.SetFile3PlayerName(playerName);
		PlayerPrefsManager.SetFile3LevelProgress(gameProgress);
		Save ();
	}
	
	
	//**************************************************************************************
	//Load Section
	//**************************************************************************************
	
	
	public void Load () {
		if(File.Exists(Application.persistentDataPath + "/playerInfo" + PlayerPrefsManager.GetGameFile() + ".dat")); {
			BinaryFormatter binaryFormatter = new BinaryFormatter();
			FileStream saveFile = File.Open (Application.persistentDataPath + "/playerInfo" + PlayerPrefsManager.GetGameFile() + ".dat", FileMode.Open);
			PlayerData playerData = (PlayerData) binaryFormatter.Deserialize(saveFile);
			
			
			playerName = playerData.playerName;
			gameProgress = playerData.gameProgress;
			playerLevel = playerData.playerLevel;
			baseStrength = playerData.baseStrength;
			baseDefense = playerData.baseDefense;
			baseSpeed = playerData.baseSpeed;
			baseIntelligence = playerData.baseIntelligence;
			baseHealth = playerData.baseHealth;
			baseMana = playerData.baseMana;
			
			saveFile.Close(); //I wonder if this should go at the end of the method...
		}
	}
	
	public void LoadFile1 () {
		PlayerPrefsManager.SetGameFile(1);
		Load ();
		//Need to add code here for loading the appropriate region scene.
	}
	
	public void LoadFile2 () {
		PlayerPrefsManager.SetGameFile(2);
		Load ();
		//Need to add code here for loading the appropriate region scene.
	}
	
	public void LoadFile3 () {
		PlayerPrefsManager.SetGameFile(3);
		Load ();
		//Need to add code here for loading the appropriate region scene.
	}
	
	
	//**************************************************************************************
	//New Game Section
	//**************************************************************************************
	
	
	public void NewGame1 () {
		//do some stuff about initializing here
		playerName = "Taylor";
		gameProgress = 27;
		playerLevel = 27;
		baseStrength = 1;
		baseDefense = 2;
		baseSpeed = 3;
		baseIntelligence = 4;
		baseHealth = 5;
		baseMana = 6;
		
		SaveFile1 ();
	}
	
	public void NewGame2 () {
		//do some stuff about initializing here
		playerName = "Lee";
		gameProgress = 9;
		playerLevel = 9;
		baseStrength = 5;
		baseDefense = 5;
		baseSpeed = 5;
		baseIntelligence = 5;
		baseHealth = 5;
		baseMana = 5;
		
		SaveFile2 ();
	}
	
	public void NewGame3 () {
		//do some stuff about initializing here
		playerName = "Katelyn";
		gameProgress = 24;
		playerLevel = 24;
		baseStrength = 7;
		baseDefense = 7;
		baseSpeed = 7;
		baseIntelligence = 7;
		baseHealth = 7;
		baseMana = 7;
		
		SaveFile3 ();
	}
	
	
	//**************************************************************************************
	//Delete Game Section
	//**************************************************************************************
	
	public void ToDeleteMenu () {
		PlayerPrefsManager.SetDeleteEntryPoint (Application.loadedLevelName);
	}
	
	public void DeleteGame1 () {
		//add pop up for making sure....
		File.Delete (Application.persistentDataPath + "/playerInfo1.dat");
		PlayerPrefsManager.SetFile1PlayerName("");
		PlayerPrefsManager.SetFile1LevelProgress(0);
	}
	
	public void DeleteGame2 () {
		//add pop up for making sure....
		File.Delete (Application.persistentDataPath + "/playerInfo2.dat");
		PlayerPrefsManager.SetFile2PlayerName("");
		PlayerPrefsManager.SetFile2LevelProgress(0);
	}
	
	public void DeleteGame3 () {
		//add pop up for making sure....
		File.Delete (Application.persistentDataPath + "/playerInfo3.dat");
		PlayerPrefsManager.SetFile3PlayerName("");
		PlayerPrefsManager.SetFile3LevelProgress(0);
	}
	
	
	
}

[Serializable]
class PlayerData {
	public string playerName;
	public int gameProgress;
	public int playerLevel;
	public int baseStrength;
	public int baseDefense;
	public int baseSpeed;
	public int baseIntelligence;
	public int baseHealth;
	public int baseMana;
}
