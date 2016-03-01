using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Collections.Generic;

public class GameControl : MonoBehaviour {

	public static GameControl gameControl;
    public static GameControl thisObject;
	
	//item inventory menu for instantiating.
	public GameObject itemInventoryMenu;
    public GameObject equipmentBaseMenu;
    public GameObject equipmentSlotMenu;
	public GameObject pauseMenu;
	public PlayerSoundEffects playerSoundEffects;
	public MusicManager musicManager;
	
	//item inventory script access.
	public GameObject itemInventory;
	
	// equipment inventory script access.
	public GameObject equipmentInventory;
	public EquipmentDatabase equipmentDatabase;
	
	public ClassesDatabase classesDatabase;	
	
	public MainMenuControl mainMenuControl;

    public bool playerHasControl;
	
	//Player Data//
	
	public string playerName;
	
	public int gameProgress;
	
	public int playerLevel;
	
	public int playerClass;
	
	public int baseStrength;
	public int baseDefense;
	public int baseSpeed;
	public int baseIntelligence;
	public int baseHealth;
	public int baseMana;
//	this needs to be saved.
	public int hp;
	public int mp;
	
	public int currentStrength;
	public int currentDefense;
	public int currentSpeed;
	public int currentIntelligence;
	public int currentHealth;
	public int currentMana;
	
	public List<Items> itemInventoryList;
	public List<Equipment> equipmentInventoryList;

//	these need to be saved.
	public List<Equipment> weaponsList;
    public List<LevelScores> levelScores;

	public int equippedHead;
	public int equippedChest;
	public int equippedPants;
	public int equippedFeet;
	
	public int equippedWeapon;
    //	this needs to be saved.
    public int activeItem;
	public int availableEvolutions;
	
	//Menu Levels are incremented by functions that dive deeper into the respective menu and are decremented by the back function.
	public int equipmentMenuLevel = 0;
	public int weaponEvolutionMenuLevel = 0;
	public int pauseMenuLevel = 0;
	public int itemsMenuLevel = 0;
	public int mainMenuLevel = 0;
	public int mainMenuDeleteLevel = 0;
	public int mainMenuCopyLevel = 0;
	
//		this needs to be saved.
	//Next section is for profile selecting.
	public int currentProfile;
	
	public bool reSelectMapObject;

	void Awake () {
        if (thisObject != null)
        {
            DestroyImmediate(gameObject);
            return;
        }
        thisObject = this;
		DontDestroyOnLoad (transform.root.gameObject);
	}
	
	void Start () {
		gameControl = GetComponent<GameControl>();
        reSelectMapObject = false;

        playerHasControl = true;
	
		equipmentMenuLevel = 0;
		weaponEvolutionMenuLevel = 0;
		pauseMenuLevel = 0;
		itemsMenuLevel = 0;
		mainMenuLevel = 0;
		mainMenuDeleteLevel = 0;
		mainMenuCopyLevel = 0;
		
		playerClass = 7;
		equippedWeapon = 80;
		equippedHead = 5;
		equippedChest = 6;
		equippedPants = 7;
		equippedFeet = 8;
		
		playerSoundEffects.ChangeVolume(PlayerPrefsManager.GetMasterEffectsVolume());
		musicManager.ChangeVolume(PlayerPrefsManager.GetMasterMusicVolume());
        //REMOVE AFTER TESTING.
        //AddLevelScores();
	}
	
	void Update () {
        if (playerHasControl)
        {
            if (Input.GetButtonDown("Open Pause Menu"))
            {
                //makes opening pause menu impossible from the listed scenes, need to add a few for the world/region menus and cutscenes....might be easier to say when instead of when cant.
                if (SceneManager.GetActiveScene().name == "_Splash" || SceneManager.GetActiveScene().name == "Start" ||
                    SceneManager.GetActiveScene().name == "Main Menu" || SceneManager.GetActiveScene().name == "01b Options" || SceneManager.GetActiveScene().name == "Extras")
                {
                }
                else {
                    if (GameObject.FindGameObjectWithTag("Pause Menu"))
                    {
                        ClosePauseMenu();
                        pauseMenuLevel = 0;
                        if (LevelManager.levelManager.InMapScenes == true)
                        {
                            reSelectMapObject = true;
                        }
                    }
                    else {
                        OpenPauseMenu();
                        pauseMenuLevel = 1;
                    }
                }
            }

            if (Input.GetButtonDown("Item Cycle"))
            {
                //Inventory.inventory.CycleActiveItems(Input.GetAxisRaw("Item Cycle"));
            }


            if (Input.GetKeyDown(KeyCode.E))
            {
                //temp way to add info to the weapons list.
                weaponsList.Add(equipmentDatabase.equipment[10]);
                weaponsList.Add(equipmentDatabase.equipment[20]);
                weaponsList.Add(equipmentDatabase.equipment[30]);
                weaponsList.Add(equipmentDatabase.equipment[40]);
                weaponsList.Add(equipmentDatabase.equipment[50]);
                weaponsList.Add(equipmentDatabase.equipment[60]);
                weaponsList.Add(equipmentDatabase.equipment[70]);
                equipmentInventory.GetComponent<EquipmentInventory>().AddTempData();
                itemInventory.GetComponent<Inventory>().AddTempData();
            }


            //"Back" function.
            if (Input.GetButtonDown("Cancel"))
            {
                //Main Menu.
                if (mainMenuLevel > 0)
                {
                    mainMenuControl.GetComponent<MainMenuControl>().OpenPreviousMainMenu(mainMenuLevel);
                    mainMenuLevel--;
                }
                //Copy Branch
                if (mainMenuCopyLevel > 0)
                {
                    mainMenuControl.GetComponent<MainMenuControl>().OpenPreviousCopyMenu(mainMenuCopyLevel);
                    mainMenuCopyLevel--;
                }
                //Delete Branch
                if (mainMenuDeleteLevel > 0)
                {
                    mainMenuControl.GetComponent<MainMenuControl>().OpenPreviousDeleteMenu(mainMenuDeleteLevel);
                    mainMenuDeleteLevel--;
                }


                //Pause Menu.
                if (pauseMenuLevel > 0)
                {
                    ClosePauseMenu();
                    pauseMenuLevel--;
                    if (LevelManager.levelManager.InMapScenes == true)
                    {
                        reSelectMapObject = true;
                    }
                }
                //Items Menu.
                if (itemsMenuLevel > 0)
                {
                    itemInventory.GetComponent<Inventory>().OpenPreviousWeaponMenu(itemsMenuLevel);
                    itemsMenuLevel--;
                }
                //Equipment Menu.
                if (equipmentMenuLevel > 0)
                {
                    equipmentInventory.GetComponent<EquipmentInventory>().OpenPreviousEquipmentMenu(equipmentMenuLevel);
                    equipmentMenuLevel--;
                }


                //Weapon Evolution Menu.
                if (weaponEvolutionMenuLevel > 0)
                {
                    equipmentInventory.GetComponent<EquipmentInventory>().OpenPreviousWeaponMenu(weaponEvolutionMenuLevel);
                    weaponEvolutionMenuLevel--;
                }
            }

            if (Input.GetKeyDown(KeyCode.W))
            {
                equipmentInventory.GetComponent<EquipmentInventory>().OpenWeaponEvolutionMenu();
            }
        }
	}

    //This is really only need for new games but for now we can call it at the begining of every game and just overwrite as needed.
    void AddLevelScores ()
    {
        int numberOfGameLevels = 100;
        for (int i = 0; i < numberOfGameLevels; i++)
        {
            levelScores.Add(LevelScoresDatabase.levelScoresDatabase.levelScores[i]);
        }
    }
	
	public bool AnyOpenMenus () {
		int menuIndex = equipmentMenuLevel + weaponEvolutionMenuLevel + pauseMenuLevel + itemsMenuLevel + mainMenuLevel + mainMenuDeleteLevel + mainMenuCopyLevel;
		if (menuIndex > 0) {
			return true;
		} else {
			return false;
		}
	}
	
	public void OpenPauseMenu () {
		Instantiate(pauseMenu);
	}
	
	public void ClosePauseMenu () {
		Destroy(GameObject.FindGameObjectWithTag("Pause Menu"));
	}

	
	//I dont like the way this works, I would rather do a find based on the equipped weapon material...
	public void CurrentClass () {
		string weaponClass = equipmentDatabase.equipment [equippedWeapon].equipmentMaterial.ToString();
		if (weaponClass == "Soldier") {
			int classIndex = 0;
			playerClass = classIndex;
		} else if (weaponClass == "Berserker") {
			int classIndex = 1;
			playerClass = classIndex;
		} else if (weaponClass == "Rogue") {
			int classIndex = 2;
			playerClass = classIndex;
		} else if (weaponClass == "Ranger") {
			int classIndex = 3;
			playerClass = classIndex;
		} else if (weaponClass == "Wizard") {
			int classIndex = 4;
			playerClass = classIndex;
		} else if (weaponClass == "Sorcerer") {
			int classIndex = 5;
			playerClass = classIndex;
		} else if (weaponClass == "Monk") {
			int classIndex = 6;
			playerClass = classIndex;
		} else {
			int classIndex = 7;
			playerClass = classIndex;
		}
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
	
	public void SaveFile (int fileNumber) {
		if (fileNumber == 1) {
			PlayerPrefsManager.SetGameFile(1);
			PlayerPrefsManager.SetFile1PlayerName(playerName);
			PlayerPrefsManager.SetFile1LevelProgress(gameProgress);
			Save ();
		} else if (fileNumber == 2) {
			PlayerPrefsManager.SetGameFile(2);
			PlayerPrefsManager.SetFile2PlayerName(playerName);
			PlayerPrefsManager.SetFile2LevelProgress(gameProgress);
			Save ();
		} else {
			PlayerPrefsManager.SetGameFile(3);
			PlayerPrefsManager.SetFile3PlayerName(playerName);
			PlayerPrefsManager.SetFile3LevelProgress(gameProgress);
			Save ();
		}
	}
	
	
	//**************************************************************************************
	//Load Section
	//**************************************************************************************
	
	
	public void LoadFile (int fileNumber) {	
		if(File.Exists(Application.persistentDataPath + "/playerInfo" + fileNumber + ".dat")) {
			BinaryFormatter binaryFormatter = new BinaryFormatter();
			FileStream saveFile = File.Open (Application.persistentDataPath + "/playerInfo" + fileNumber + ".dat", FileMode.Open);
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
		if (fileNumber == 1) {
			PlayerPrefsManager.SetGameFile(1);
			PlayerPrefsManager.SetFile1PlayerName(playerName);
			PlayerPrefsManager.SetFile1LevelProgress(gameProgress);
			Save ();
		} else if (fileNumber == 2) {
			PlayerPrefsManager.SetGameFile(2);
			PlayerPrefsManager.SetFile2PlayerName(playerName);
			PlayerPrefsManager.SetFile2LevelProgress(gameProgress);
			Save ();
		} else {
			PlayerPrefsManager.SetGameFile(3);
			PlayerPrefsManager.SetFile3PlayerName(playerName);
			PlayerPrefsManager.SetFile3LevelProgress(gameProgress);
			Save ();
		}
	}
	
//	public void LoadFile1 () {
//		PlayerPrefsManager.SetGameFile(1);
//		Load ();
//		//Need to add code here for loading the appropriate region scene.
//	}
//	
//	public void LoadFile2 () {
//		PlayerPrefsManager.SetGameFile(2);
//		Load ();
//		//Need to add code here for loading the appropriate region scene.
//	}
//	
//	public void LoadFile3 () {
//		PlayerPrefsManager.SetGameFile(3);
//		Load ();
//		//Need to add code here for loading the appropriate region scene.
//	}
	
	
	//**************************************************************************************
	//New Game Section
	//**************************************************************************************
	
	public void NewGame (int fileNumber) {
		//do some stuff about initializing here
		
		//remove this stuff at some point.
		playerName = "Taylor";
		gameProgress = 27;
		playerLevel = 27;
		baseStrength = 1;
		baseDefense = 2;
		baseSpeed = 3;
		baseIntelligence = 4;
		baseHealth = 5;
		baseMana = 6;
        AddLevelScores();
		
		SaveFile(fileNumber);
	}
	
	
//	public void NewGame1 () {	
//		//do some stuff about initializing here
//		playerName = "Taylor";
//		gameProgress = 27;
//		playerLevel = 27;
//		baseStrength = 1;
//		baseDefense = 2;
//		baseSpeed = 3;
//		baseIntelligence = 4;
//		baseHealth = 5;
//		baseMana = 6;
//		
//		SaveFile1 ();
//	}
//	
//	public void NewGame2 () {
//		//do some stuff about initializing here
//		playerName = "Lee";
//		gameProgress = 9;
//		playerLevel = 9;
//		baseStrength = 5;
//		baseDefense = 5;
//		baseSpeed = 5;
//		baseIntelligence = 5;
//		baseHealth = 5;
//		baseMana = 5;
//		
//		SaveFile2 ();
//	}
//	
//	public void NewGame3 () {
//		//do some stuff about initializing here
//		playerName = "Katelyn";
//		gameProgress = 24;
//		playerLevel = 24;
//		baseStrength = 7;
//		baseDefense = 7;
//		baseSpeed = 7;
//		baseIntelligence = 7;
//		baseHealth = 7;
//		baseMana = 7;
//		
//		SaveFile3 ();
//	}
	
	
	//**************************************************************************************
	//Delete Game Section
	//**************************************************************************************
	
//	public void ToDeleteMenu () {
//		PlayerPrefsManager.SetDeleteEntryPoint (Application.loadedLevelName);
//	}

	public void DeleteGame (int fileNumber) {
		//add pop up for making sure....
		File.Delete (Application.persistentDataPath + "/playerInfo" + fileNumber + ".dat");
	
		if (fileNumber == 1) {
			PlayerPrefsManager.SetFile1PlayerName("");
			PlayerPrefsManager.SetFile1LevelProgress(0);
			File.Delete (Application.persistentDataPath + "/playerInfo1.dat");
		} else if (fileNumber == 2) {
			PlayerPrefsManager.SetFile2PlayerName("");
			PlayerPrefsManager.SetFile2LevelProgress(0);
			File.Delete (Application.persistentDataPath + "/playerInfo2.dat");
		} else {
			PlayerPrefsManager.SetFile3PlayerName("");
			PlayerPrefsManager.SetFile3LevelProgress(0);
			File.Delete (Application.persistentDataPath + "/playerInfo3.dat");
		}
	}
	
//	public void DeleteGame1 () {
//		//add pop up for making sure....
//		File.Delete (Application.persistentDataPath + "/playerInfo1.dat");
//		PlayerPrefsManager.SetFile1PlayerName("");
//		PlayerPrefsManager.SetFile1LevelProgress(0);
//	}
//	
//	public void DeleteGame2 () {
//		//add pop up for making sure....
//		File.Delete (Application.persistentDataPath + "/playerInfo2.dat");
//		PlayerPrefsManager.SetFile2PlayerName("");
//		PlayerPrefsManager.SetFile2LevelProgress(0);
//	}
//	
//	public void DeleteGame3 () {
//		//add pop up for making sure....
//		File.Delete (Application.persistentDataPath + "/playerInfo3.dat");
//		PlayerPrefsManager.SetFile3PlayerName("");
//		PlayerPrefsManager.SetFile3LevelProgress(0);
//	}
	
	
	
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
