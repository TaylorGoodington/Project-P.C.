using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class SelectionMenu : MonoBehaviour {

    string path;
    string funnel;
    List<Skills> inventorySkillsList;
    List<Equipment> inventoryEquipmentList;
    List<Skills> skillsList;
    List<Equipment> equipmentList;

	void Start () {
        inventorySkillsList = GameControl.gameControl.acquiredSkills;
        inventoryEquipmentList = GameControl.gameControl.equipmentInventoryList;

        skillsList = new List<Skills>();
        equipmentList = new List<Equipment>();
    }

    public void ReRunList ()
    {
        inventorySkillsList = GameControl.gameControl.acquiredSkills;
        inventoryEquipmentList = GameControl.gameControl.equipmentInventoryList;

        skillsList = new List<Skills>();
        equipmentList = new List<Equipment>();

        path = GameObject.FindGameObjectWithTag("Pause Menu").GetComponent<PauseMenu>().path;
        funnel = GameObject.FindGameObjectWithTag("Pause Menu").GetComponent<PauseMenu>().funnel;

        DefineSelectionList();

        PopulateSelectionMenu();
    }

    public void DefineSelectionList ()
    {
        if (path == "Equipment")
        {
            if (funnel == "Equipment")
            {
                foreach (Equipment armor in inventoryEquipmentList)
                {
                    if (EquipmentController.Equipmentcontroller.IsArmorEquipable(armor.equipmentName))
                    {
                        equipmentList.Add(armor);
                    }
                }
            }
            else
            {
                foreach (Equipment weapon in inventoryEquipmentList)
                {
                    if (weapon.equipmentType.ToString() == funnel)
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

    public void PopulateSelectionMenu ()
    {
        int index = 0;

        #region Equipment Section
        if (path == "Equipment")
        {
            #region Armor
            if (funnel == "Equipment")
            {
                foreach (Transform child in transform)
                {
                    //Checks if the Equipment is equipped
                    if (GameControl.gameControl.currentProfile == 1)
                    {
                        if (GameControl.gameControl.profile1Equipment == equipmentList[index].equipmentID)
                        {
                            child.GetComponent<Button>().interactable = false;
                        }
                    }
                    else
                    {
                        if (GameControl.gameControl.profile2Equipment == equipmentList[index].equipmentID)
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
                        child.GetComponent<Text>().text = equipmentList[index].equipmentName;
                        child.transform.GetChild(0).GetComponent<Text>().text = equipmentList[index].equipmentID.ToString();
                    }

                    index++;
                }
            }
            #endregion

            #region Weapons
            else
            {
                foreach (Transform child in transform)
                {
                    //Checks if the Weapon is equipped
                    if (GameControl.gameControl.currentProfile == 1)
                    {
                        if (GameControl.gameControl.profile1Weapon == equipmentList[index].equipmentID)
                        {
                            child.GetComponent<Button>().interactable = false;
                        }
                    }
                    else
                    {
                        if (GameControl.gameControl.profile2Weapon == equipmentList[index].equipmentID)
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
                        child.GetComponent<Text>().text = equipmentList[index].equipmentName;
                        child.transform.GetChild(0).GetComponent<Text>().text = equipmentList[index].equipmentID.ToString();
                    }

                    index++;
                }
            }
            #endregion
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
}