using UnityEngine;
using System.Collections;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Collections.Generic;

public class GameControl : MonoBehaviour {

	public static GameControl gameControl;
	
	//item inventory menu for instantiating.
	public GameObject itemInventoryMenu;
	public GameObject equipmentBaseMenu;
	public GameObject equipmentSlotMenu;
	
	//item inventory script access.
	public GameObject itemInventory;
	
	// equipment inventory script access.
	public GameObject equipmentInventory;
	
	public ClassesDatabase classesDatabase;	
	
	private LevelManager levelManager;
	
	//Player Data//
	
	public string playerName;
	
	public int gameProgress;
	
	public int playerLevel;
	
	public int playerClass;
	
	//I dont think i need the base stats, seems like i can handle everything with current stats.
	public int baseStrength;
	public int baseDefense;
	public int baseSpeed;
	public int baseIntelligence;
	public int baseHealth;
	public int baseMana;
	
	public int currentStrength;
	public int currentDefense;
	public int currentSpeed;
	public int currentIntelligence;
	public int currentHealth;
	public int currentMana;
	
	public List<Items> itemInventoryList;
	public List<Equipment> equipmentInventoryList;
//	public List<Weapons> weaponInventoryList;

	public int equippedHead;
	public int equippedChest;
	public int equippedPants;
	public int equippedFeet;
	
	public int equippedWeapon;
	
	

	void Awake () {
		DontDestroyOnLoad (gameObject);
	}
	
	void Start () {
		playerClass = 7;
		equippedWeapon = 9;
	}
	
	void Update () {
		if (Input.GetKeyDown(KeyCode.P)) {
			Instantiate(itemInventoryMenu);
			itemInventory.GetComponent<Inventory>().OpenItemMenu();
		} 
		
		if (Input.GetKeyDown(KeyCode.E)) {
			Instantiate(equipmentBaseMenu);
			equipmentInventory.GetComponent<EquipmentInventory>().OpenEquipmentBaseMenu();
		}
	}
	
	public void CurrentClass () {
		//write in logic that determines class based on equipped weapon.
	}
	
	
	//**************************************************************************************//
	//Save Section//
	//**************************************************************************************//
	
	
	public void Save () {
		BinaryFormatter binaryFormatter = new BinaryFormatter ();
		FileStream saveFile = File.Create (Application.persistentDataPath + "/playerInfo" + PlayerPrefsManager.GetGameFile() + ".dat"); //Its possible gameNumber might need to be pased to the mehod.
		
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
		
		playerData.itemInventoryList = itemInventoryList;
		playerData.equipmentInventoryList = equipmentInventoryList;
		
		playerData.currentStrength = currentStrength;
		playerData.currentDefense = currentDefense;
		playerData.currentSpeed = currentSpeed;
		playerData.currentIntelligence = currentIntelligence;
		playerData.currentHealth = currentHealth;
		playerData.currentMana = currentMana;
		
		playerData.equippedHead = equippedHead;
		playerData.equippedChest = equippedChest;
		playerData.equippedPants = equippedPants;
		playerData.equippedFeet = equippedFeet;
		
		playerData.equippedWeapon = equippedWeapon;
		
		
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
			
			itemInventoryList = playerData.itemInventoryList;
			equipmentInventoryList = playerData.equipmentInventoryList;
			
			currentStrength = playerData.currentStrength;
			currentDefense = playerData.currentDefense;
			currentSpeed = playerData.currentSpeed;
			currentIntelligence = playerData.currentIntelligence;
			currentHealth = playerData.currentHealth;
			currentMana = playerData.currentMana;
			
			equippedHead = playerData.equippedHead;
			equippedChest = playerData.equippedChest;
			equippedPants = playerData.equippedPants;
			equippedFeet = playerData.equippedFeet;
			
			equippedWeapon = playerData.equippedWeapon;
			
			
			
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
	
	public List<Items> itemInventoryList;
	public List<Equipment> equipmentInventoryList;
	//	public List<Weapons> weaponInventoryList;

	public int currentStrength;
	public int currentDefense;
	public int currentSpeed;
	public int currentIntelligence;
	public int currentHealth;
	public int currentMana;
	
	public int equippedHead;
	public int equippedChest;
	public int equippedPants;
	public int equippedFeet;
	
	public int equippedWeapon;
}
