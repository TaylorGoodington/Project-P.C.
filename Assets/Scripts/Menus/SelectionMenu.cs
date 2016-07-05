using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class SelectionMenu : MonoBehaviour
{
    string path;
    string funnel;
    List<Skills> inventorySkillsList;
    List<Equipment> inventoryEquipmentList;
    List<Skills> skillsList;
    public List<Equipment> equipmentList;

    void Start()
    {
        inventorySkillsList = GameControl.gameControl.acquiredSkills;
        inventoryEquipmentList = GameControl.gameControl.equipmentInventoryList;
    }

    public void OnEnable()
    {
        ReRunList();
    }

    public void ReRunList()
    {
        inventorySkillsList = GameControl.gameControl.acquiredSkills;
        inventoryEquipmentList = GameControl.gameControl.equipmentInventoryList;

        path = GameObject.FindGameObjectWithTag("Pause Menu").GetComponent<PauseMenu>().path;
        funnel = GameObject.FindGameObjectWithTag("Pause Menu").GetComponent<PauseMenu>().funnel;

        DefineSelectionList();

        PopulateSelectionMenu();
    }

    public void DefineSelectionList()
    {
        skillsList = new List<Skills>();
        equipmentList = new List<Equipment>();

        if (path == "Equipment")
        {
            if (funnel == "Equipment")
            {
                foreach (Equipment armor in inventoryEquipmentList)
                {
                    if (EquipmentController.equipmentController.IsArmorEquipable(armor.equipmentName))
                    {
                        equipmentList.Add(armor);
                    }
                }
            }
            else
            {
                foreach (Equipment weapon in inventoryEquipmentList)
                {
                    string weaponName = weapon.equipmentType.ToString();
                    weaponName = (weaponName == "Staff") ? "Staves" : weaponName + "s";
                    if (weaponName == funnel)
                    {
                        equipmentList.Add(weapon);
                    }
                }
            }
        }
        else if (path == "Skills")
        {
            foreach (Skills skill in inventorySkillsList)
            {
                if (skill.requiredStatName.ToString() == funnel)
                {
                    skillsList.Add(skill);
                }
            }
        }
    }

    public void PopulateSelectionMenu()
    {
        int index = 0;

        #region Equipment Section
        if (path == "Equipment")
        {
            foreach (Transform child in transform)
            {
                if (index < equipmentList.Count)
                {
                    child.gameObject.SetActive(true);
                    child.GetComponent<Text>().text = equipmentList[index].equipmentName;
                    child.transform.GetChild(0).GetComponent<Text>().text = equipmentList[index].equipmentID.ToString();
                    Button playButton = child.GetComponent<Button>();
                    playButton.onClick.RemoveAllListeners();

                    //Checks if the Equipment is equipped
                    if (GameControl.gameControl.profile1Equipment == equipmentList[index].equipmentID ||
                        GameControl.gameControl.profile2Equipment == equipmentList[index].equipmentID ||
                        GameControl.gameControl.profile1Weapon == equipmentList[index].equipmentID ||
                        GameControl.gameControl.profile2Weapon == equipmentList[index].equipmentID)
                    {
                        playButton.onClick.AddListener(() => TestButton());
                        child.GetComponent<Text>().color = Color.grey;
                        //do something with equipped icon.
                    }
                    else if (GameControl.gameControl.playerLevel < equipmentList[index].equipmentLevelRequirement)
                    {
                        playButton.onClick.AddListener(() => TestButton());
                        child.GetComponent<Text>().color = Color.grey;
                    }
                    else
                    {
                        playButton.onClick.AddListener(() => EquipmentController.equipmentController.ChangeEquipmentFromMenu());
                        child.GetComponent<Text>().color = Color.white;
                    }
                }
                else
                {
                    child.gameObject.SetActive(false);
                }

                index++;
            }
        }
        #endregion

        #region Skills Section
        else if (path == "Skills")
        {
            foreach (Transform child in transform)
            {
                //Checks if the skill is equipped
                if (GameControl.gameControl.currentProfile == 1)
                {
                    if (GameControl.gameControl.profile1SlottedSkills.Contains(skillsList[index]))
                    {
                        child.GetComponent<Button>().interactable = false;
                    }
                }
                else
                {
                    if (GameControl.gameControl.profile2SlottedSkills.Contains(skillsList[index]))
                    {
                        child.GetComponent<Button>().interactable = false;
                    }
                }

                //Updates List if necessary
                if (index > skillsList.Count)
                {
                    child.gameObject.SetActive(false);
                }
                else
                {
                    child.GetComponent<Text>().text = skillsList[index].skillName;
                    child.transform.GetChild(0).GetComponent<Text>().text = skillsList[index].skillID.ToString();
                }

                index++;
            }
        }
        #endregion
    }

    //TODO Remove later
    public void TestButton ()
    {
        Debug.Log("Already Equipped");
    }
}