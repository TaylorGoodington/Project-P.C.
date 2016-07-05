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
	
	public GameObject pauseMenu;

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
    public int maxCombos;
    public int playThroughNumber;
    public int currency;

	public List<Equipment> equipmentInventoryList;
    public List<LevelScores> levelScores;

    //Next section is for profile selecting.
    [Range(1, 2)]
    public int currentProfile;
    public List<Skills> acquiredSkills;
    public Skills selectedSkill;
    public List<Skills> profile1SlottedSkills;
    public List<Skills> profile2SlottedSkills;
    public int profile1Equipment;
    public int profile2Equipment;
    public int profile1Weapon;
    public int profile2Weapon;
    
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

    #region Menu Levels
    //Menu Levels are incremented by functions that dive deeper into the respective menu and are decremented by the back function.
    public int weaponEvolutionMenuLevel = 0;
    public int mainMenuLevel = 0;
    public int mainMenuDeleteLevel = 0;
    public int mainMenuCopyLevel = 0;
    public int ladyDeathMenu = 0;
    #endregion
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
        endOfLevel = false;
        dying = false;

		weaponEvolutionMenuLevel = 0;
		mainMenuLevel = 0;
		mainMenuDeleteLevel = 0;
		mainMenuCopyLevel = 0;
        ladyDeathMenu = 0;
        currentProfile = 1;
		
		PlayerSoundEffects.playerSoundEffects.ChangeVolume(PlayerPrefsManager.GetMasterEffectsVolume());
		MusicManager.musicManager.ChangeVolume(PlayerPrefsManager.GetMasterMusicVolume());
    }
	
	void Update () {
        if (playerHasControl)
        {
            #region Pause
            if (Input.GetButtonDown("Open Pause Menu"))
            {
                //makes opening pause menu impossible from the listed scenes, need to add a few for the world/region menus and cutscenes....might be easier to say when instead of when cant.
                if (SceneManager.GetActiveScene().name == "Company Logo" || SceneManager.GetActiveScene().name == "Start" ||
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
                        if (LevelManager.levelManager.inMapScenes == true)
                        {
                            reSelectMapObject = true;
                        }
                    }
                    else
                    {
                        OpenPauseMenu();
                    }
                }
            }
            #endregion

            //for level testing...
            if (Input.GetKeyDown(KeyCode.L))
            {
                LoadFile(1);
                LevelManager.levelManager.LoadLevel("The Pit Intro");
            }

            //for Equipment Testing
            if (Input.GetKeyDown(KeyCode.E))
            {
                equipmentInventoryList.Add(EquipmentDatabase.equipmentDatabase.equipment[9]);
                equipmentInventoryList.Add(EquipmentDatabase.equipmentDatabase.equipment[13]);
                equipmentInventoryList.Add(EquipmentDatabase.equipmentDatabase.equipment[16]);
                equipmentInventoryList.Add(EquipmentDatabase.equipmentDatabase.equipment[22]);
                equipmentInventoryList.Add(EquipmentDatabase.equipmentDatabase.equipment[50]);
                equipmentInventoryList.Add(EquipmentDatabase.equipmentDatabase.equipment[166]);
                equipmentInventoryList.Add(EquipmentDatabase.equipmentDatabase.equipment[176]);
            }

            //upgrade equipment testing
            if (Input.GetButtonDown("Switch Profiles"))
            {
                playerHasControl = false;
                //play some animation that transitions profiles.
            }

            if (Input.GetKeyDown(KeyCode.U))
            {
                //playerLevel = 7;
                currency = 10000;
                EquipmentController.equipmentController.UpgradeEquipment(1);
            }

            #region Back Button
            //"Back" function.
            if (Input.GetButtonDown("Cancel"))
            {
                //Main Menu.
                if (mainMenuLevel > 0)
                {
                    GameObject.FindGameObjectWithTag("Main Menu").GetComponent<MainMenuControl>().OpenPreviousMainMenu(mainMenuLevel);
                    mainMenuLevel--;
                }
                //Copy Branch
                if (mainMenuCopyLevel > 0)
                {
                    GameObject.FindGameObjectWithTag("Main Menu").GetComponent<MainMenuControl>().OpenPreviousCopyMenu(mainMenuCopyLevel);
                    mainMenuCopyLevel--;
                }
                //Delete Branch
                if (mainMenuDeleteLevel > 0)
                {
                    GameObject.FindGameObjectWithTag("Main Menu").GetComponent<MainMenuControl>().OpenPreviousDeleteMenu(mainMenuDeleteLevel);
                    mainMenuDeleteLevel--;
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
                    //equipmentInventory.GetComponent<EquipmentInventory>().OpenPreviousWeaponMenu(weaponEvolutionMenuLevel);
                    weaponEvolutionMenuLevel--;
                }
            }
            #endregion

            #region Equipment Switching for testing
            //TODO TESTING FOR SWITCHING GEAR...MODIFY FOR ACTUAL USE LATER.
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                SkillsController.skillsController.ChangingEquipmentOrPerks();
                //changing the equipment.

                if (profile1Equipment == 411)
                {
                    //playerClass = 1;
                    profile1Weapon = 1;
                    profile1Equipment = 401;
                    skinColorIndex = 2;
                    hairIndex = 2;
                }
                else if (profile1Equipment == 401)
                {
                    //playerClass = 3;
                    profile1Weapon = 151;
                    profile1Equipment = 428;
                    skinColorIndex = 1;
                    hairIndex = 1;
                }
                else if (profile1Equipment == 428)
                {
                    //playerClass = 2;
                    profile1Weapon = 351;
                    profile1Equipment = 433;
                    skinColorIndex = 2;
                    hairIndex = 3;
                }
                else if (profile1Equipment == 433)
                {
                    //playerClass = 4;
                    profile1Weapon = 301;
                    profile1Equipment = 411;
                    skinColorIndex = 3;
                    hairIndex = 4;
                }

                //Adjusts the max combo.
                maxCombos = EquipmentDatabase.equipmentDatabase.equipment[profile1Weapon].maxCombos;

                //Adjusts Current Class.
                CurrentClass();

                //Updates Equipment
                //EquipmentInventory.equipmentInventory.UpdateEquippedStats();

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

    //TODO Update Perk Web Skills
    //Called from profile switching animation.
    void SwitchProfiles ()
    {
        if (currentProfile == 1)
        {
            currentProfile = 2;
            selectedSkill = profile2SlottedSkills[0];
        }
        else
        {
            currentProfile = 1;
            selectedSkill = profile1SlottedSkills[0];
        }

        //Updates Stats
        UpdateEquippedStats(true);

        SkillsController.skillsController.ChangingEquipmentOrPerks();

        //Updates Skills
        SkillsController.skillsController.UpdateAcquiredAndActiveSkillsLists();

        //Update Perk Web Skills.
        SkillsController.skillsController.UpdatePerkWebSkills();

        //Adjusts the max combo.
        EquipmentController.equipmentController.UpdateMaxCombos();

        //Adjusts Current Class.
        CurrentClass();
        
        SkillsController.skillsController.DoneChangingEquipmentOrPerks();
        playerHasControl = true;
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
		int menuIndex = weaponEvolutionMenuLevel + mainMenuLevel + mainMenuDeleteLevel + mainMenuCopyLevel + ladyDeathMenu;
		if (menuIndex > 0 || GameObject.FindGameObjectWithTag("Pause Menu"))
        {
			return true;
		}
        else
        {
            return false;
		}
	}
	
	public void OpenPauseMenu ()
    {
		Instantiate(pauseMenu);
	}
	
	public void ClosePauseMenu ()
    {
		GameObject.FindGameObjectWithTag("Pause Menu").GetComponent<Animator>().Play("Close");
	}

    public void LevelUpStats ()
    {
        baseStrength += ClassesDatabase.classDatabase.classes[playerClass].strengthLevelBonus;
        baseDefense += ClassesDatabase.classDatabase.classes[playerClass].defenseLevelBonus;
        baseSpeed += ClassesDatabase.classDatabase.classes[playerClass].speedLevelBonus;
        baseIntelligence += ClassesDatabase.classDatabase.classes[playerClass].intelligenceLevelBonus;
        baseHealth += ClassesDatabase.classDatabase.classes[playerClass].healthLevelBonus;
        baseMana += ClassesDatabase.classDatabase.classes[playerClass].manaLevelBonus;

        UpdateEquippedStats(false);
        SkillsController.skillsController.ChangingEquipmentOrPerks();
        SkillsController.skillsController.UpdateAcquiredAndActiveSkillsLists();
        SkillsController.skillsController.DoneChangingEquipmentOrPerks();
    }

    public void UpdateEquippedStats(bool equipmentChange)
    {
        hpRatio = (float)hp / currentHealth;
        mpRatio = (float)mp / currentMana;

        if (currentProfile == 1)
        {
            currentStrength = baseStrength + EquipmentDatabase.equipmentDatabase.equipment[profile1Weapon].equipmentStrength +
                          EquipmentDatabase.equipmentDatabase.equipment[profile1Equipment].equipmentStrength;
            currentDefense = baseDefense + EquipmentDatabase.equipmentDatabase.equipment[profile1Weapon].equipmentDefense +
                             EquipmentDatabase.equipmentDatabase.equipment[profile1Equipment].equipmentDefense;
            currentSpeed = baseSpeed + EquipmentDatabase.equipmentDatabase.equipment[profile1Weapon].equipmentSpeed +
                           EquipmentDatabase.equipmentDatabase.equipment[profile1Equipment].equipmentSpeed;
            currentIntelligence = baseIntelligence + EquipmentDatabase.equipmentDatabase.equipment[profile1Weapon].equipmentIntelligence +
                                  EquipmentDatabase.equipmentDatabase.equipment[profile1Equipment].equipmentIntelligence;
            currentHealth = baseHealth + EquipmentDatabase.equipmentDatabase.equipment[profile1Weapon].equipmentHealth +
                            EquipmentDatabase.equipmentDatabase.equipment[profile1Equipment].equipmentHealth;
            currentMana = baseMana + EquipmentDatabase.equipmentDatabase.equipment[profile1Weapon].equipmentMana +
                          EquipmentDatabase.equipmentDatabase.equipment[profile1Equipment].equipmentMana;
        }
        else
        {
            currentStrength = baseStrength + EquipmentDatabase.equipmentDatabase.equipment[profile2Weapon].equipmentStrength +
                          EquipmentDatabase.equipmentDatabase.equipment[profile2Equipment].equipmentStrength;
            currentDefense = baseDefense + EquipmentDatabase.equipmentDatabase.equipment[profile2Weapon].equipmentDefense +
                             EquipmentDatabase.equipmentDatabase.equipment[profile2Equipment].equipmentDefense;
            currentSpeed = baseSpeed + EquipmentDatabase.equipmentDatabase.equipment[profile2Weapon].equipmentSpeed +
                           EquipmentDatabase.equipmentDatabase.equipment[profile2Equipment].equipmentSpeed;
            currentIntelligence = baseIntelligence + EquipmentDatabase.equipmentDatabase.equipment[profile2Weapon].equipmentIntelligence +
                                  EquipmentDatabase.equipmentDatabase.equipment[profile2Equipment].equipmentIntelligence;
            currentHealth = baseHealth + EquipmentDatabase.equipmentDatabase.equipment[profile2Weapon].equipmentHealth +
                            EquipmentDatabase.equipmentDatabase.equipment[profile2Equipment].equipmentHealth;
            currentMana = baseMana + EquipmentDatabase.equipmentDatabase.equipment[profile2Weapon].equipmentMana +
                          EquipmentDatabase.equipmentDatabase.equipment[profile2Equipment].equipmentMana;
        }

        CalculateHealthAndMana(equipmentChange);
    }

    public void CalculateHealthAndMana(bool equipmentChange)
    {
        if (equipmentChange)
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

    public void CurrentClass () {
		string weaponClass = EquipmentDatabase.equipmentDatabase.equipment [profile1Weapon].equipmentType.ToString();
		if (weaponClass == "Sword") {
			int classIndex = 0;
			playerClass = classIndex;
		} else if (weaponClass == "Axe") {
			int classIndex = 1;
			playerClass = classIndex;
		} else if (weaponClass == "Dagger") {
			int classIndex = 2;
			playerClass = classIndex;
		} else if (weaponClass == "Bow") {
			int classIndex = 3;
			playerClass = classIndex;
		} else if (weaponClass == "Staff") {
			int classIndex = 4;
			playerClass = classIndex;
		} else if (weaponClass == "Talisman") {
			int classIndex = 5;
			playerClass = classIndex;
		} else if (weaponClass == "Knuckles") {
			int classIndex = 6;
			playerClass = classIndex;
		} else {
			int classIndex = 7;
			playerClass = classIndex;
		}
	}
	
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
        
        playerData.equipmentInventoryList = equipmentInventoryList;
		playerData.levelScores = levelScores;
        playerData.currency = currency;
        playerData.currentProfile = currentProfile;
        playerData.acquiredSkills = acquiredSkills;
        playerData.selectedSkill = selectedSkill;
        playerData.profile1SlottedSkills = profile1SlottedSkills;
        playerData.profile2SlottedSkills = profile2SlottedSkills;
        playerData.profile1Equipment = profile1Equipment;
        playerData.profile2Equipment = profile2Equipment;
        playerData.profile1Weapon = profile1Weapon;
        playerData.profile2Weapon = profile2Weapon;

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
            PlayerPrefsManager.SetFile1PlayerLevel(playerLevel);
			Save ();
		}
        else if (fileNumber == 2)
        {
			PlayerPrefsManager.SetGameFile(2);
			PlayerPrefsManager.SetFile2PlayerName(playerName);
			PlayerPrefsManager.SetFile2LevelProgress(gameProgress);
            PlayerPrefsManager.SetFile2PlayerLevel(playerLevel);
            Save ();
		}
        else
        {
			PlayerPrefsManager.SetGameFile(3);
			PlayerPrefsManager.SetFile3PlayerName(playerName);
			PlayerPrefsManager.SetFile3LevelProgress(gameProgress);
            PlayerPrefsManager.SetFile3PlayerLevel(playerLevel);
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

            currentStrength = playerData.currentStrength;
            currentDefense = playerData.currentDefense;
            currentSpeed = playerData.currentSpeed;
            currentIntelligence = playerData.currentIntelligence;
            currentHealth = playerData.currentHealth;
            currentMana = playerData.currentMana;
            

            hairIndex = playerData.hairIndex;
            skinColorIndex = playerData.skinColorIndex;

            playThroughNumber = playerData.playThroughNumber;
            equipmentInventoryList = playerData.equipmentInventoryList;
            levelScores = playerData.levelScores;

            currency = playerData.currency;
            currentProfile = playerData.currentProfile;
            acquiredSkills = playerData.acquiredSkills;
            selectedSkill = playerData.selectedSkill;
            profile1SlottedSkills = playerData.profile1SlottedSkills;
            profile2SlottedSkills = playerData.profile2SlottedSkills;
            profile1Equipment = playerData.profile1Equipment;
            profile2Equipment = playerData.profile2Equipment;
            profile1Weapon = playerData.profile1Weapon;
            profile2Weapon = playerData.profile2Weapon;

            UpdateEquippedStats(false);
            xpToLevel = ExperienceToLevel.experienceToLevel.levels[playerLevel].experienceToLevel;

            //TODO Remove after testing.
            LoadSkills();

            saveFile.Close();          
        }
	}

    private void LoadSkills()
    {
        //Change these lines to actually load the data.
        acquiredSkills.Add(SkillsDatabase.skillsDatabase.skills[0]);
        acquiredSkills.Add(SkillsDatabase.skillsDatabase.skills[1]);
        acquiredSkills.Add(SkillsDatabase.skillsDatabase.skills[4]);
        acquiredSkills.Add(SkillsDatabase.skillsDatabase.skills[3]);
        profile1SlottedSkills.Add(SkillsDatabase.skillsDatabase.skills[0]);
        profile1SlottedSkills.Add(SkillsDatabase.skillsDatabase.skills[1]);
        profile1SlottedSkills.Add(SkillsDatabase.skillsDatabase.skills[4]);
        profile1SlottedSkills.Add(SkillsDatabase.skillsDatabase.skills[3]);
        selectedSkill = SkillsDatabase.skillsDatabase.skills[0];

        //Keep this lines.
        SkillsController.skillsController.selectedSkill = selectedSkill;
    }
    
    public void NewGame (int fileNumber) {
        ResetNewGameData();
		
		playerName = "";
		gameProgress = 0;

		playerLevel = 1;
		baseStrength = 1;
		baseDefense = 1;
		baseSpeed = 1;
		baseIntelligence = 1;
		baseHealth = 1;
		baseMana = 1;

        playerClass = 0;
        profile1Weapon = 1;
        profile1Equipment = 401;
        skinColorIndex = 1;
        hairIndex = 1;

        xpToLevel = ExperienceToLevel.experienceToLevel.levels[playerLevel].experienceToLevel;
        UpdateEquippedStats(false);

        AddLevelScores();
        currency = 0;
        //TODO Remove after testing.
        LoadSkills();
        EquipmentController.equipmentController.InitializeNewGameEquipment();

        SaveFile(fileNumber);
	}

    public void ResetNewGameData ()
    {
        levelScores.Clear();
        equipmentInventoryList.Clear();
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
    
	public List<Equipment> equipmentInventoryList;
    public List<LevelScores> levelScores;
    public int currency;
    public int currentProfile;
    public List<Skills> acquiredSkills;
    public Skills selectedSkill;
    public List<Skills> profile1SlottedSkills;
    public List<Skills> profile2SlottedSkills;
    public int profile1Equipment;
    public int profile2Equipment;
    public int profile1Weapon;
    public int profile2Weapon;
}