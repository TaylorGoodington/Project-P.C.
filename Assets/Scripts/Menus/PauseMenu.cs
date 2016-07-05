using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

public class PauseMenu : MonoBehaviour
{
    public Dictionary<int, int> pauseMenuPath;
    int currentLevel;

    public GameObject pauseMenu;
    public GameObject[] level2;
    public GameObject[] level3;
    public GameObject[] level4;
    public GameObject rightMenu;

    [HideInInspector]
    public string path;
    public string funnel;

    void Start()
    {
        pauseMenuPath = new Dictionary<int, int>();
        pauseMenuPath.Add(1, 1);
        pauseMenuPath.Add(2, 1);
        pauseMenuPath.Add(3, 1);
        pauseMenuPath.Add(4, 1);
        currentLevel = 1;
        UpdateRightMenu();
    }

    void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            Back();
        }

        if (Input.GetButtonDown("Switch Profiles"))
        {
            for (int i = currentLevel; i > 1; i--)
            {
                Back();
            }
        }

        UpdateRightMenu();
    }

    public void Advance(int fork)
    {
        ClearRightMenuText();
        funnel = EventSystem.current.currentSelectedGameObject.name;
        if (currentLevel == 1)
        {
            pauseMenu.SetActive(false);
            level2[fork].SetActive(true);
            UpdateRightMenu();
            StartCoroutine(Selection(level2[fork].transform.GetChild(0).gameObject));
            currentLevel = 2;

            if (fork == 1)
            {
                path = "Equipment";
            }
            else if (fork == 2)
            {
                path = "Skills";
            }
        }
        else if (currentLevel == 2)
        {
            level2[pauseMenuPath[1]].SetActive(false);
            level3[fork].SetActive(true);
            UpdateRightMenu();
            StartCoroutine(Selection(level3[fork].transform.GetChild(0).gameObject));
            currentLevel = 3;
        }
        else if (currentLevel == 3)
        {
            level3[pauseMenuPath[2]].SetActive(false);
            level4[fork].SetActive(true);
            UpdateRightMenu();
            StartCoroutine(Selection(level4[fork].transform.GetChild(0).gameObject));
            currentLevel = 4;
        }
        pauseMenuPath[currentLevel] = fork;
    }

    IEnumerator Selection(GameObject item)
    {
        yield return new WaitForSeconds(0.01f);
        EventSystem.current.SetSelectedGameObject(item);
    }

    public void Back()
    {
        ClearRightMenuText();
        if (currentLevel == 4)
        {
            level4[pauseMenuPath[4]].SetActive(false);
            level3[pauseMenuPath[3]].SetActive(true);
            StartCoroutine(Selection(level3[pauseMenuPath[3]].transform.GetChild(pauseMenuPath[4] - 1).gameObject));
            currentLevel = 3;
        }
        else if (currentLevel == 3)
        {
            level3[pauseMenuPath[3]].SetActive(false);
            level2[pauseMenuPath[2]].SetActive(true);
            StartCoroutine(Selection(level2[pauseMenuPath[2]].transform.GetChild(pauseMenuPath[3] - 1).gameObject));
            currentLevel = 2;
        }
        else if (currentLevel == 2)
        {
            level2[pauseMenuPath[2]].SetActive(false);
            pauseMenu.SetActive(true);
            StartCoroutine(Selection(pauseMenu.transform.GetChild(pauseMenuPath[2] - 1).gameObject));
            currentLevel = 1;
        }
        else if (currentLevel == 1)
        {
            GetComponent<Animator>().Play("Close");
        }
    }

    void ClearRightMenuText ()
    {
        if (rightMenu.transform.GetChild(0).gameObject.activeSelf)
        {
            foreach (Transform child in rightMenu.transform.GetChild(0).gameObject.transform)
            {
                if (child.GetComponent<Text>())
                {
                    child.GetComponent<Text>().text = "";
                }
            }
        }
        else if (rightMenu.transform.GetChild(1).gameObject.activeSelf)
        {
            foreach (Transform child in rightMenu.transform.GetChild(1).gameObject.transform)
            {
                if (child.GetComponent<Text>())
                {
                    child.GetComponent<Text>().text = "";
                }
            }
        }
        else if (rightMenu.transform.GetChild(2).gameObject.activeSelf)
        {
            foreach (Transform child in rightMenu.transform.GetChild(2).gameObject.transform)
            {
                if (child.GetComponent<Text>())
                {
                    child.GetComponent<Text>().text = "";
                }
            }
        }
    }

    public void UpdateRightMenu()
    {
        #region Open Correct Menu
        if (transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.activeSelf == true ||
            transform.GetChild(0).gameObject.transform.GetChild(1).gameObject.activeSelf == true)
        {
            rightMenu.transform.GetChild(0).gameObject.SetActive(true);
            rightMenu.transform.GetChild(1).gameObject.SetActive(false);
            rightMenu.transform.GetChild(2).gameObject.SetActive(false);
        }
        else if (transform.GetChild(0).gameObject.transform.GetChild(2).gameObject.activeSelf == true)
        {
            rightMenu.transform.GetChild(0).gameObject.SetActive(false);
            rightMenu.transform.GetChild(1).gameObject.SetActive(true);
            rightMenu.transform.GetChild(2).gameObject.SetActive(false);
        }
        else if (path == "Equipment" && transform.GetChild(0).gameObject.transform.GetChild(3).gameObject.activeSelf == true)
        {
            rightMenu.transform.GetChild(0).gameObject.SetActive(false);
            rightMenu.transform.GetChild(1).gameObject.SetActive(false);
            rightMenu.transform.GetChild(2).gameObject.SetActive(true);
        }
        #endregion
        #region Player Info Menu
        if (rightMenu.transform.GetChild(0).gameObject.activeSelf == true)
        {
            GameObject.FindGameObjectWithTag("InfoSprite").GetComponent<PlayerMenuSprite>().InitializeSprites();
            Text playerLevel = rightMenu.transform.GetChild(0).transform.GetChild(1).GetComponent<Text>();
            Text playerExperience = rightMenu.transform.GetChild(0).transform.GetChild(2).GetComponent<Text>();
            Text weaponName = rightMenu.transform.GetChild(0).transform.GetChild(3).GetComponent<Text>();
            Text weaponTier = rightMenu.transform.GetChild(0).transform.GetChild(4).GetComponent<Text>();
            Text weaponLevel = rightMenu.transform.GetChild(0).transform.GetChild(5).GetComponent<Text>();
            Text equipmentName = rightMenu.transform.GetChild(0).transform.GetChild(6).GetComponent<Text>();
            Text equipmentLevel = rightMenu.transform.GetChild(0).transform.GetChild(7).GetComponent<Text>();
            Text strengthStat = rightMenu.transform.GetChild(0).transform.GetChild(8).GetComponent<Text>();
            Text defenseStat = rightMenu.transform.GetChild(0).transform.GetChild(9).GetComponent<Text>();
            Text intelligenceStat = rightMenu.transform.GetChild(0).transform.GetChild(10).GetComponent<Text>();
            Text agilityStat = rightMenu.transform.GetChild(0).transform.GetChild(11).GetComponent<Text>();
            Text healthStat = rightMenu.transform.GetChild(0).transform.GetChild(12).GetComponent<Text>();
            Text manaStat = rightMenu.transform.GetChild(0).transform.GetChild(13).GetComponent<Text>();

            int weaponID = (GameControl.gameControl.currentProfile == 1) ? GameControl.gameControl.profile1Weapon : GameControl.gameControl.profile2Weapon;
            int equipmentID = (GameControl.gameControl.currentProfile == 1) ? GameControl.gameControl.profile1Equipment : GameControl.gameControl.profile2Equipment;

            playerLevel.text = "level   " + GameControl.gameControl.playerLevel;
            playerExperience.text = "exp   " + GameControl.gameControl.xp + "   /   " + GameControl.gameControl.xpToLevel;
            weaponName.text = EquipmentDatabase.equipmentDatabase.equipment[weaponID].equipmentName;
            weaponTier.text = "tier   " + EquipmentDatabase.equipmentDatabase.equipment[weaponID].equipmentTier;
            weaponLevel.text = "level   " + EquipmentDatabase.equipmentDatabase.equipment[weaponID].equipmentPowerLevel;
            equipmentName.text = EquipmentDatabase.equipmentDatabase.equipment[equipmentID].equipmentName;
            equipmentLevel.text = "level   " + EquipmentDatabase.equipmentDatabase.equipment[equipmentID].equipmentPowerLevel;
            strengthStat.text = GameControl.gameControl.currentStrength.ToString();
            defenseStat.text = GameControl.gameControl.currentDefense.ToString();
            intelligenceStat.text = GameControl.gameControl.currentIntelligence.ToString();
            agilityStat.text = GameControl.gameControl.currentSpeed.ToString();
            healthStat.text = GameControl.gameControl.currentHealth.ToString();
            manaStat.text = GameControl.gameControl.currentMana.ToString();
        }
        #endregion
        #region Class Preview Menu
        else if (rightMenu.transform.GetChild(1).gameObject.activeSelf == true)
        {
            if (EventSystem.current.currentSelectedGameObject.transform.parent.name == "Weapons Menu")
            {
                Text className = rightMenu.transform.GetChild(1).transform.GetChild(1).GetComponent<Text>();
                Text classDescription = rightMenu.transform.GetChild(1).transform.GetChild(2).GetComponent<Text>();
                Text strengthStat = rightMenu.transform.GetChild(1).transform.GetChild(3).GetComponent<Text>();
                Text defenseStat = rightMenu.transform.GetChild(1).transform.GetChild(4).GetComponent<Text>();
                Text intelligenceStat = rightMenu.transform.GetChild(1).transform.GetChild(5).GetComponent<Text>();
                Text agilityStat = rightMenu.transform.GetChild(1).transform.GetChild(6).GetComponent<Text>();
                Text healthStat = rightMenu.transform.GetChild(1).transform.GetChild(7).GetComponent<Text>();
                Text manaStat = rightMenu.transform.GetChild(1).transform.GetChild(8).GetComponent<Text>();
                string classWeapon = EventSystem.current.currentSelectedGameObject.name;
                int classIndex;
                if (classWeapon == "Swords")
                {
                    classIndex = 0;
                }
                else if (classWeapon == "Axes")
                {
                    classIndex = 1;
                }
                else if (classWeapon == "Daggers")
                {
                    classIndex = 2;
                }
                else if (classWeapon == "Bows")
                {
                    classIndex = 3;
                }
                else if (classWeapon == "Staves")
                {
                    classIndex = 4;
                }
                else if (classWeapon == "Talismans")
                {
                    classIndex = 5;
                }
                else if (classWeapon == "Knuckles")
                {
                    classIndex = 6;
                }
                else
                {
                    classIndex = 7;
                }
                className.text = ClassesDatabase.classDatabase.classes[classIndex].className;
                classDescription.text = ClassesDatabase.classDatabase.classes[classIndex].classDescription;
                strengthStat.text = ClassesDatabase.classDatabase.classes[classIndex].strengthLevelBonus.ToString();
                defenseStat.text = ClassesDatabase.classDatabase.classes[classIndex].defenseLevelBonus.ToString();
                intelligenceStat.text = ClassesDatabase.classDatabase.classes[classIndex].intelligenceLevelBonus.ToString();
                agilityStat.text = ClassesDatabase.classDatabase.classes[classIndex].speedLevelBonus.ToString();
                healthStat.text = ClassesDatabase.classDatabase.classes[classIndex].healthLevelBonus.ToString();
                manaStat.text = ClassesDatabase.classDatabase.classes[classIndex].manaLevelBonus.ToString();
            }
        }
        #endregion
        #region Equipment Info
        else if (rightMenu.transform.GetChild(2).gameObject.activeSelf == true)
        {
            Text equipmentName = rightMenu.transform.GetChild(2).transform.GetChild(1).GetComponent<Text>();
            Text equipmentDescription = rightMenu.transform.GetChild(2).transform.GetChild(2).GetComponent<Text>();
            Text strengthStat = rightMenu.transform.GetChild(2).transform.GetChild(3).GetComponent<Text>();
            Text defenseStat = rightMenu.transform.GetChild(2).transform.GetChild(4).GetComponent<Text>();
            Text intelligenceStat = rightMenu.transform.GetChild(2).transform.GetChild(5).GetComponent<Text>();
            Text agilityStat = rightMenu.transform.GetChild(2).transform.GetChild(6).GetComponent<Text>();
            Text healthStat = rightMenu.transform.GetChild(2).transform.GetChild(7).GetComponent<Text>();
            Text manaStat = rightMenu.transform.GetChild(2).transform.GetChild(8).GetComponent<Text>();
            Text levelRequirement = rightMenu.transform.GetChild(2).transform.GetChild(9).GetComponent<Text>();

            if (EventSystem.current.currentSelectedGameObject.transform.childCount > 0)
            {
                int equipmentID;
                int.TryParse(EventSystem.current.currentSelectedGameObject.transform.GetChild(0).GetComponent<Text>().text, out equipmentID);
                int comparisonEquipment;
                if (EquipmentController.equipmentController.isEquipmentAWeapon)
                {
                    if (GameControl.gameControl.currentProfile == 1)
                    {
                        comparisonEquipment = GameControl.gameControl.profile1Weapon;
                    }
                    else
                    {
                        comparisonEquipment = GameControl.gameControl.profile2Weapon;
                    }
                }
                else
                {
                    if (GameControl.gameControl.currentProfile == 1)
                    {
                        comparisonEquipment = GameControl.gameControl.profile1Equipment;
                    }
                    else
                    {
                        comparisonEquipment = GameControl.gameControl.profile2Equipment;
                    }
                }

                equipmentName.text = EquipmentDatabase.equipmentDatabase.equipment[equipmentID].equipmentName;
                equipmentDescription.text = EquipmentDatabase.equipmentDatabase.equipment[equipmentID].equipmentDescription;
                levelRequirement.text = "level   requirement   " + EquipmentDatabase.equipmentDatabase.equipment[equipmentID].equipmentLevelRequirement;

                int strengthDifference = EquipmentDatabase.equipmentDatabase.equipment[equipmentID].equipmentStrength - EquipmentDatabase.equipmentDatabase.equipment[comparisonEquipment].equipmentStrength;
                if (strengthDifference > 0)
                {
                    strengthStat.text = strengthDifference.ToString();
                    strengthStat.color = Color.green;
                }
                else if (strengthDifference == 0)
                {
                    strengthStat.text = strengthDifference.ToString();
                    strengthStat.color = Color.white;
                }
                else
                {
                    strengthStat.text = Mathf.Abs(strengthDifference).ToString();
                    strengthStat.color = Color.red;
                }

                int defenseDifference = EquipmentDatabase.equipmentDatabase.equipment[equipmentID].equipmentDefense - EquipmentDatabase.equipmentDatabase.equipment[comparisonEquipment].equipmentDefense;
                if (defenseDifference > 0)
                {
                    defenseStat.text = defenseDifference.ToString();
                    defenseStat.color = Color.green;
                }
                else if (defenseDifference == 0)
                {
                    defenseStat.text = defenseDifference.ToString();
                    defenseStat.color = Color.white;
                }
                else
                {
                    defenseStat.text = Mathf.Abs(defenseDifference).ToString();
                    defenseStat.color = Color.red;
                }

                int intelligenceDifference = EquipmentDatabase.equipmentDatabase.equipment[equipmentID].equipmentIntelligence - EquipmentDatabase.equipmentDatabase.equipment[comparisonEquipment].equipmentIntelligence;
                if (intelligenceDifference > 0)
                {
                    intelligenceStat.text = intelligenceDifference.ToString();
                    intelligenceStat.color = Color.green;
                }
                else if (intelligenceDifference == 0)
                {
                    intelligenceStat.text = intelligenceDifference.ToString();
                    intelligenceStat.color = Color.white;
                }
                else
                {
                    intelligenceStat.text = Mathf.Abs(intelligenceDifference).ToString();
                    intelligenceStat.color = Color.red;
                }

                int agilityDifference = EquipmentDatabase.equipmentDatabase.equipment[equipmentID].equipmentSpeed - EquipmentDatabase.equipmentDatabase.equipment[comparisonEquipment].equipmentSpeed;
                if (agilityDifference > 0)
                {
                    agilityStat.text = agilityDifference.ToString();
                    agilityStat.color = Color.green;
                }
                else if (agilityDifference == 0)
                {
                    agilityStat.text = agilityDifference.ToString();
                    agilityStat.color = Color.white;
                }
                else
                {
                    agilityStat.text = Mathf.Abs(agilityDifference).ToString();
                    agilityStat.color = Color.red;
                }

                int healthDifference = EquipmentDatabase.equipmentDatabase.equipment[equipmentID].equipmentHealth - EquipmentDatabase.equipmentDatabase.equipment[comparisonEquipment].equipmentHealth;
                if (healthDifference > 0)
                {
                    healthStat.text = healthDifference.ToString();
                    healthStat.color = Color.green;
                }
                else if (healthDifference == 0)
                {
                    healthStat.text = healthDifference.ToString();
                    healthStat.color = Color.white;
                }
                else
                {
                    healthStat.text = Mathf.Abs(healthDifference).ToString();
                    healthStat.color = Color.red;
                }

                int manaDifference = EquipmentDatabase.equipmentDatabase.equipment[equipmentID].equipmentMana - EquipmentDatabase.equipmentDatabase.equipment[comparisonEquipment].equipmentMana;
                if (manaDifference > 0)
                {
                    manaStat.text = manaDifference.ToString();
                    manaStat.color = Color.green;
                }
                else if (manaDifference == 0)
                {
                    manaStat.text = manaDifference.ToString();
                    manaStat.color = Color.white;
                }
                else
                {
                    manaStat.text = Mathf.Abs(manaDifference).ToString();
                    manaStat.color = Color.red;
                }

                if (GameControl.gameControl.playerLevel >= EquipmentDatabase.equipmentDatabase.equipment[equipmentID].equipmentLevelRequirement)
                {
                    levelRequirement.color = Color.white;
                }
                else
                {
                    levelRequirement.color = Color.red;
                }
            }
        }
        #endregion
    }

    public void TurnOnPauseMenu()
    {
        pauseMenu.SetActive(true);
    }

    public void MakeFirstSelection()
    {
        GameObject equipment = GameObject.FindGameObjectWithTag("Pause Menu").transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).gameObject;
        EventSystem.current.SetSelectedGameObject(equipment);
    }

    public void CloseMenus()
    {
        foreach (Transform child in transform.GetChild(0).transform)
        {
            child.gameObject.SetActive(false);
        }
        foreach (Transform child in transform.GetChild(1).transform)
        {
            child.gameObject.SetActive(false);
        }
    }

    public void Destroy()
    {
        Destroy(this.gameObject);

        if (LevelManager.levelManager.inMapScenes == true)
        {
            GameControl.gameControl.reSelectMapObject = true;
        }
    }

    public void PlayerHasControl()
    {
        GameControl.gameControl.playerHasControl = true;
    }

    public void PlayerDoesntHaveControl()
    {
        GameControl.gameControl.playerHasControl = false;
    }
}