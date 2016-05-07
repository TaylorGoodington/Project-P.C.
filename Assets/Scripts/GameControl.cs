using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class GameControl : MonoBehaviour {
    #region Variables
    public static GameControl gameControl;
    static GameControl thisObject;
	
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
    public int currentStrength;
    public int currentDefense;
    public int currentSpeed;
    public int currentIntelligence;
    public int currentHealth;
    public int currentMana;
    public int hp;
	public int mp;
    public int xp;
    public int xpToLevel;
    public int hairIndex;
    public int skinColorIndex;
    public int equippedEquipmentIndex;
    public int equippedWeapon;
    public int maxCombos;
    public int playThroughNumber;
	
	public List<Items> itemInventoryList;
	public List<Equipment> equipmentInventoryList;
	public List<Equipment> weaponsList;
    public List<LevelScores> levelScores;

    //remove later.
	public int equippedHead;
	public int equippedChest;
	public int equippedPants;
	public int equippedFeet;
    public int activeItem;
	public int availableEvolutions;

    #region Menu Levels
    //Menu Levels are incremented by functions that dive deeper into the respective menu and are decremented by the back function.
    public int equipmentMenuLevel = 0;
	public int weaponEvolutionMenuLevel = 0;
	public int pauseMenuLevel = 0;
	public int itemsMenuLevel = 0;
	public int mainMenuLevel = 0;
	public int mainMenuDeleteLevel = 0;
	public int mainMenuCopyLevel = 0;
    public int ladyDeathMenu = 0;
    #endregion

    //this needs to be saved.
    //Next section is for profile selecting.
    [Range(1, 2)]
    public int currentProfile;
    public List<Skills> acquiredSkills;
    public List<Skills> profile1SlottedSkills;
    public List<Skills> profile2SlottedSkills;
    public Skills selectedSkill;

    public bool reSelectMapObject;

    public float hpRatio;
    public float mpRatio;

    public int levelCap = 40;

    [HideInInspector]
    public bool endOfLevel;
    [HideInInspector]
    public bool startFromLoad;
    [HideInInspector]
    public bool dying;
    #endregion

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
        startFromLoad = true;
        playerHasControl = true;
	
		equipmentMenuLevel = 0;
		weaponEvolutionMenuLevel = 0;
		pauseMenuLevel = 0;
		itemsMenuLevel = 0;
		mainMenuLevel = 0;
		mainMenuDeleteLevel = 0;
		mainMenuCopyLevel = 0;
        ladyDeathMenu = 0;
        endOfLevel = false;
        dying = false;
        currentProfile = 1;
		
		playerSoundEffects.ChangeVolume(PlayerPrefsManager.GetMasterEffectsVolume());
		musicManager.ChangeVolume(PlayerPrefsManager.GetMasterMusicVolume());
    }
	
	void Update () {
        if (playerHasControl)
        {
            #region Pause
            if (Input.GetButtonDown("Open Pause Menu"))
            {
                //makes opening pause menu impossible from the listed scenes, need to add a few for the world/region menus and cutscenes....might be easier to say when instead of when cant.
                if (SceneManager.GetActiveScene().name == "_Splash" || SceneManager.GetActiveScene().name == "Start" ||
                    SceneManager.GetActiveScene().name == "Main Menu" || SceneManager.GetActiveScene().name == "01b Options" || SceneManager.GetActiveScene().name == "Extras"
                    || GameObject.FindGameObjectWithTag("Back Dialogue"))
                {
                    //Don't open the pause menu.
                }
                else
                {
                    if (AnyOpenMenus())
                    {
                        ClosePauseMenu();
                        //MAKE A WAY TO CLOSE ALL PAUSE RELATED MENUS.
                        pauseMenuLevel = 0;
                        if (LevelManager.levelManager.inMapScenes == true)
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
            #endregion

            //for level testing...
            if (Input.GetKeyDown(KeyCode.L))
            {
                LevelManager.levelManager.LoadLevel("Level 11");
            }

            #region Back Button
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
                    if (LevelManager.levelManager.inMapScenes == true)
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
                //Lady Death Menu.
                if (ladyDeathMenu > 0)
                {
                    GameObject.FindGameObjectWithTag("Lady Death").GetComponent<LadyDeath>().OpenPreviousMenu();
                    ladyDeathMenu--;
                }
                //Weapon Evolution Menu.
                if (weaponEvolutionMenuLevel > 0)
                {
                    equipmentInventory.GetComponent<EquipmentInventory>().OpenPreviousWeaponMenu(weaponEvolutionMenuLevel);
                    weaponEvolutionMenuLevel--;
                }
            }
            #endregion

            //TODO TESTING THE PIT INTO LEVEL...
            if (Input.GetKeyDown(KeyCode.O))
            {
                LevelManager.levelManager.LoadLevel("Level 68");
            }

            #region Equipment Switching for testing
            //TODO TESTING FOR SWITCHING GEAR...MODIFY FOR ACTUAL USE LATER.
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                SkillsController.skillsController.ChangingEquipmentOrPerks();
                //changing the equipment.
                if (equippedEquipmentIndex < 4)
                {
                    equippedEquipmentIndex++;
                }
                else
                {
                    equippedEquipmentIndex = 1;
                }

                //changes the rest of the stuff.
                if (equippedEquipmentIndex == 1)
                {
                    //playerClass = 1;
                    equippedWeapon = 10;
                    equippedEquipmentIndex = 1;
                    skinColorIndex = 2;
                    hairIndex = 2;
                }
                else if (equippedEquipmentIndex == 2)
                {
                    //playerClass = 3;
                    equippedWeapon = 40;
                    equippedEquipmentIndex = 2;
                    skinColorIndex = 1;
                    hairIndex = 1;
                }
                else if (equippedEquipmentIndex == 3)
                {
                    //playerClass = 2;
                    equippedWeapon = 80;
                    equippedEquipmentIndex = 3;
                    skinColorIndex = 2;
                    hairIndex = 3;
                }
                else if (equippedEquipmentIndex == 4)
                {
                    //playerClass = 4;
                    equippedWeapon = 70;
                    equippedEquipmentIndex = 4;
                    skinColorIndex = 3;
                    hairIndex = 4;
                }

                //Adjusts the max combo.
                maxCombos = EquipmentDatabase.equipmentDatabase.equipment[equippedWeapon].maxCombos;

                //Adjusts Current Class.
                CurrentClass();

                //Updates Equipment
                EquipmentInventory.equipmentInventory.UpdateEquippedStats();

                //Update Perks

                //Update Aquired Skills List

                SkillsController.skillsController.DoneChangingEquipmentOrPerks();
            }
            #endregion

            #region Camera Sweep
            if (Input.GetKeyDown(KeyCode.P))
            {
                if (!LevelManager.levelManager.inMapScenes)
                {
                    GameObject levelCamera = GameObject.FindGameObjectWithTag("Cameras").transform.GetChild(0).gameObject;
                    GameObject panCamera = GameObject.FindGameObjectWithTag("Level").transform.GetChild(0).gameObject;
                    if (levelCamera.activeInHierarchy == true)
                    {
                        levelCamera.SetActive(false);
                        panCamera.SetActive(true);
                    }
                    else
                    {
                        levelCamera.SetActive(true);
                        panCamera.SetActive(false);
                    }
                }
            }
            #endregion
        }
        else
        {
            EventSystem.current.SetSelectedGameObject(null);
        }

        #region MP/HP Adjustments
        //HP adjuster...
        if (hp < 0)
        {
            hp = 0;
        }
        if (hp > currentHealth * 10)
        {
            hp = currentHealth * 10;
        }

        //MP Adjster...
        if (mp < 0)
        {
            mp = 0;
        }
        if (mp > currentMana * 10)
        {
            mp = currentMana * 10;
        }
        #endregion
    }

    //This is really only needed for new games but for now we can call it at the begining of every game and just overwrite as needed.
    public void AddLevelScores ()
    {
        int numberOfGameLevels = 101;
        for (int i = 0; i < numberOfGameLevels; i++)
        {
            levelScores.Add(LevelScoresDatabase.levelScoresDatabase.levelScores[i]);
        }
    }
	
	public bool AnyOpenMenus () {
		int menuIndex = equipmentMenuLevel + weaponEvolutionMenuLevel + pauseMenuLevel + itemsMenuLevel + mainMenuLevel + mainMenuDeleteLevel + mainMenuCopyLevel + ladyDeathMenu;
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

    public void LevelUpStats ()
    {
        baseStrength += ClassesDatabase.classDatabase.classes[playerClass].strengthLevelBonus;
        baseDefense += ClassesDatabase.classDatabase.classes[playerClass].defenseLevelBonus;
        baseSpeed += ClassesDatabase.classDatabase.classes[playerClass].speedLevelBonus;
        baseIntelligence += ClassesDatabase.classDatabase.classes[playerClass].intelligenceLevelBonus;
        baseHealth += ClassesDatabase.classDatabase.classes[playerClass].healthLevelBonus;
        baseMana += ClassesDatabase.classDatabase.classes[playerClass].manaLevelBonus;

        EquipmentInventory.equipmentInventory.UpdateEquippedStats();

        CalculateHealthAndMana(false);
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

    //HP & MP
    public void CalculateHealthAndMana (bool resetFromEquipment)
    {
        if (resetFromEquipment)
        {
            hp = Mathf.RoundToInt(currentHealth * hpRatio);
            mp = Mathf.RoundToInt(currentMana * mpRatio);
        }
        else
        {
            hp = currentHealth * 10;
            mp = currentMana * 10;
        }
    }
	
	//Save Section
	public void Save () {
		BinaryFormatter binaryFormatter = new BinaryFormatter ();
		FileStream saveFile = File.Create (Application.persistentDataPath + "/playerInfo" + PlayerPrefsManager.GetGameFile() + ".dat");
		
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
        playerData.currentStrength = currentStrength;
        playerData.currentDefense = currentDefense;
        playerData.currentSpeed = currentSpeed;
        playerData.currentIntelligence = currentIntelligence;
        playerData.currentHealth = currentHealth;
        playerData.currentMana = currentMana;

        playerData.hairIndex = hairIndex;
        playerData.skinColorIndex = skinColorIndex;

        playerData.playThroughNumber = playThroughNumber;
        playerData.equippedWeapon = equippedWeapon;
        playerData.equippedEquipmentIndex = equippedEquipmentIndex;

        playerData.weaponsList = weaponsList;
        playerData.equipmentInventoryList = equipmentInventoryList;
		playerData.levelScores = levelScores;

        playerData.currentProfile = currentProfile;
        playerData.acquiredSkills = acquiredSkills;
        playerData.profile1SlottedSkills = profile1SlottedSkills;
        playerData.profile2SlottedSkills = profile2SlottedSkills;
        playerData.selectedSkill = selectedSkill;

		//Remove after equipment revamp.
		playerData.itemInventoryList = itemInventoryList;
		playerData.equippedHead = equippedHead;
		playerData.equippedChest = equippedChest;
		playerData.equippedPants = equippedPants;
		playerData.equippedFeet = equippedFeet;
        //*****************************

        binaryFormatter.Serialize (saveFile, playerData);
		saveFile.Close();
	}
	
	public void SaveFile (int fileNumber)
    {
		if (fileNumber == 1)
        {
			PlayerPrefsManager.SetGameFile(1);
			PlayerPrefsManager.SetFile1PlayerName(playerName);
			PlayerPrefsManager.SetFile1LevelProgress(gameProgress);
			Save ();
		}
        else if (fileNumber == 2)
        {
			PlayerPrefsManager.SetGameFile(2);
			PlayerPrefsManager.SetFile2PlayerName(playerName);
			PlayerPrefsManager.SetFile2LevelProgress(gameProgress);
			Save ();
		}
        else
        {
			PlayerPrefsManager.SetGameFile(3);
			PlayerPrefsManager.SetFile3PlayerName(playerName);
			PlayerPrefsManager.SetFile3LevelProgress(gameProgress);
			Save ();
		}
	}
	
	public void LoadFile (int fileNumber) {	
		if(File.Exists(Application.persistentDataPath + "/playerInfo" + fileNumber + ".dat"))
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream saveFile = File.Open(Application.persistentDataPath + "/playerInfo" + fileNumber + ".dat", FileMode.Open);
            PlayerData playerData = (PlayerData)binaryFormatter.Deserialize(saveFile);
            

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

            //ToDo needs to be saved
            equippedEquipmentIndex = 1;
            skinColorIndex = 1;
            hairIndex = 1;

            equippedWeapon = playerData.equippedWeapon;

            levelScores = playerData.levelScores;

            EquipmentInventory.equipmentInventory.UpdateEquippedStats();
            xpToLevel = ExperienceToLevel.experienceToLevel.levels[playerLevel].experienceToLevel;

            //TODO Remove after testing.
            LoadSkills();

            saveFile.Close(); //I wonder if this should go at the end of the method...            
        }

        if (fileNumber == 1)
        {
			PlayerPrefsManager.SetGameFile(1);
			PlayerPrefsManager.SetFile1PlayerName(playerName);
			PlayerPrefsManager.SetFile1LevelProgress(gameProgress);
			Save ();
		}
        else if (fileNumber == 2)
        {
			PlayerPrefsManager.SetGameFile(2);
			PlayerPrefsManager.SetFile2PlayerName(playerName);
			PlayerPrefsManager.SetFile2LevelProgress(gameProgress);
			Save ();
		}
        else
        {
			PlayerPrefsManager.SetGameFile(3);
			PlayerPrefsManager.SetFile3PlayerName(playerName);
			PlayerPrefsManager.SetFile3LevelProgress(gameProgress);
			Save ();
		}
	}

    private void LoadSkills()
    {
        //Change these lines to actually load the data.
        acquiredSkills.Add(SkillsDatabase.skillsDatabase.skills[4]);
        acquiredSkills.Add(SkillsDatabase.skillsDatabase.skills[1]);
        acquiredSkills.Add(SkillsDatabase.skillsDatabase.skills[2]);
        acquiredSkills.Add(SkillsDatabase.skillsDatabase.skills[3]);
        profile1SlottedSkills.Add(SkillsDatabase.skillsDatabase.skills[4]);
        profile1SlottedSkills.Add(SkillsDatabase.skillsDatabase.skills[1]);
        profile1SlottedSkills.Add(SkillsDatabase.skillsDatabase.skills[2]);
        profile1SlottedSkills.Add(SkillsDatabase.skillsDatabase.skills[3]);
        selectedSkill = SkillsDatabase.skillsDatabase.skills[4];

        //Keep these lines.
        SkillsController.skillsController.acquiredSkills = acquiredSkills;
        SkillsController.skillsController.profile1SlottedSkills = profile1SlottedSkills;
        SkillsController.skillsController.selectedSkill = selectedSkill;
    }
    
    public void NewGame (int fileNumber) {
        //do some stuff about initializing here
        ResetNewGameData();
		
		//remove this stuff at some point.
		playerName = "Taylor" + fileNumber;
		gameProgress = 27;

		playerLevel = 0;
		baseStrength = 1;
		baseDefense = 1;
		baseSpeed = 1;
		baseIntelligence = 1;
		baseHealth = 1;
		baseMana = 1;

        currentStrength = 1;
        currentDefense = 1;
        currentSpeed = 1;
        currentIntelligence = 1;
        currentHealth = 1;
        currentMana = 1;

        playerClass = 6;
        equippedWeapon = 10;
        equippedHead = 1;
        equippedChest = 2;
        equippedPants = 3;
        equippedFeet = 4;

        //ToDo needs to be saved
        equippedEquipmentIndex = 1;
        skinColorIndex = 2;
        hairIndex = 2;
        maxCombos = 3;

        xpToLevel = ExperienceToLevel.experienceToLevel.levels[playerLevel].experienceToLevel;
        EquipmentInventory.equipmentInventory.UpdateEquippedStats();
        //ToDo move this to after intro story stuff...
        weaponsList.Add(equipmentDatabase.equipment[50]);
        weaponsList.Add(equipmentDatabase.equipment[20]);
        weaponsList.Add(equipmentDatabase.equipment[30]);
        weaponsList.Add(equipmentDatabase.equipment[40]);
        weaponsList.Add(equipmentDatabase.equipment[80]);
        weaponsList.Add(equipmentDatabase.equipment[60]);
        weaponsList.Add(equipmentDatabase.equipment[70]);

        AddLevelScores();

        //TODO Remove after testing.
        LoadSkills();

        SaveFile(fileNumber);
	}

    public void ResetNewGameData ()
    {
        levelScores.Clear();
        equipmentInventoryList.Clear();
        weaponsList.Clear();
        acquiredSkills.Clear();
        profile1SlottedSkills.Clear();
        profile2SlottedSkills.Clear();
    }
	
	public void DeleteGame (int fileNumber) {
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
    public int currentStrength;
    public int currentDefense;
    public int currentSpeed;
    public int currentIntelligence;
    public int currentHealth;
    public int currentMana;

    public int hairIndex;
    public int skinColorIndex;

    public int playThroughNumber;
    public int equippedWeapon;
    public int equippedEquipmentIndex;

    public List<Equipment> weaponsList;
	public List<Equipment> equipmentInventoryList;
    public List<LevelScores> levelScores;

    public int currentProfile;
    public List<Skills> acquiredSkills;
    public List<Skills> profile1SlottedSkills;
    public List<Skills> profile2SlottedSkills;
    public Skills selectedSkill;

    //TODO 
    #region This needs to be removed after equipment gets revamped.
    public List<Items> itemInventoryList;
    public int equippedHead;
	public int equippedChest;
	public int equippedPants;
	public int equippedFeet;
    #endregion
}