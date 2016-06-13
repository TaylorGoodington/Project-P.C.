using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class EquipmentController: MonoBehaviour
{
    #region Variables
    public static EquipmentController equipmentController;
    public bool proceedWithUpgradeAnyway;

    Dictionary<int, int> upgradeCost;
    List<string> soldierArmor;
    List<string> berserkerArmor;
    List<string> rogueArmor;
    List<string> rangerArmor;
    List<string> wizardArmor;
    List<string> sorcerorArmor;
    List<string> monkArmor;
    List<string> paladinArmor;
    public bool isEquipmentAWeapon;
    #endregion

    void Start ()
    {
        equipmentController = GetComponent<EquipmentController>();
        InitializeClassArmorList();
        InitializeEquipmentUpgradeCosts();
        proceedWithUpgradeAnyway = false;
    }

    //TODO Update Costs
    void InitializeEquipmentUpgradeCosts()
    {
        upgradeCost = new Dictionary<int, int>();
        upgradeCost.Add(1, 100);
        upgradeCost.Add(2, 100);
        upgradeCost.Add(3, 100);
        upgradeCost.Add(4, 100);
        upgradeCost.Add(5, 100);
        upgradeCost.Add(6, 100);
        upgradeCost.Add(7, 100);
        upgradeCost.Add(8, 100);
        upgradeCost.Add(9, 100);
    }

    void InitializeClassArmorList ()
    {
        #region Soldier
        soldierArmor = new List<string>();
        soldierArmor.Add("Pit Rags");
        soldierArmor.Add("Plate Armor");
        soldierArmor.Add("Noble Cuirass");
        soldierArmor.Add("Dueling Carapace");
        #endregion
        #region Berserker
        berserkerArmor = new List<string>();
        berserkerArmor.Add("Pit Rags");
        berserkerArmor.Add("Plate Armor");
        berserkerArmor.Add("Chain Mail");
        berserkerArmor.Add("Wild Mantle");
        #endregion
        #region Rogue
        rogueArmor = new List<string>();
        rogueArmor.Add("Pit Rags");
        rogueArmor.Add("Leather Armor");
        rogueArmor.Add("Chain Mail");
        rogueArmor.Add("Dueling Carapace");
        #endregion
        #region Ranger
        rangerArmor = new List<string>();
        rangerArmor.Add("Pit Rags");
        rangerArmor.Add("Leather Armor");
        rangerArmor.Add("Mystic Cowl");
        rangerArmor.Add("Wild Mantle");
        #endregion
        #region Wizard
        wizardArmor = new List<string>();
        wizardArmor.Add("Pit Rags");
        wizardArmor.Add("Arcane Cloak");
        wizardArmor.Add("Mystic Cowl");
        wizardArmor.Add("Hallowed Robe");
        #endregion
        #region Sorceror
        sorcerorArmor = new List<string>();
        sorcerorArmor.Add("Pit Rags");
        sorcerorArmor.Add("Arcane Cloak");
        sorcerorArmor.Add("Mystic Cowl");
        sorcerorArmor.Add("Martial Gi");
        #endregion
        #region Monk
        monkArmor = new List<string>();
        monkArmor.Add("Pit Rags");
        monkArmor.Add("Leather Armor");
        monkArmor.Add("Chain Mail");
        monkArmor.Add("Martial Gi");
        #endregion
        #region Paladin
        paladinArmor = new List<string>();
        paladinArmor.Add("Pit Rags");
        paladinArmor.Add("Plate Armor");
        paladinArmor.Add("Noble Cuirass");
        paladinArmor.Add("Hallowed Robe");
        #endregion
    }

    public void InitializeNewGameEquipment ()
    {
        //Armor
        GameControl.gameControl.equipmentInventoryList.Add(EquipmentDatabase.equipmentDatabase.equipment[401]);
        GameControl.gameControl.equipmentInventoryList.Add(EquipmentDatabase.equipmentDatabase.equipment[411]);
        GameControl.gameControl.equipmentInventoryList.Add(EquipmentDatabase.equipmentDatabase.equipment[421]);
        GameControl.gameControl.equipmentInventoryList.Add(EquipmentDatabase.equipmentDatabase.equipment[431]);
        GameControl.gameControl.equipmentInventoryList.Add(EquipmentDatabase.equipmentDatabase.equipment[441]);
        GameControl.gameControl.equipmentInventoryList.Add(EquipmentDatabase.equipmentDatabase.equipment[451]);
        GameControl.gameControl.equipmentInventoryList.Add(EquipmentDatabase.equipmentDatabase.equipment[461]);
        GameControl.gameControl.equipmentInventoryList.Add(EquipmentDatabase.equipmentDatabase.equipment[471]);
        GameControl.gameControl.equipmentInventoryList.Add(EquipmentDatabase.equipmentDatabase.equipment[481]);
        GameControl.gameControl.equipmentInventoryList.Add(EquipmentDatabase.equipmentDatabase.equipment[491]);
        GameControl.gameControl.equipmentInventoryList.Add(EquipmentDatabase.equipmentDatabase.equipment[501]);

        //Weapons
        GameControl.gameControl.equipmentInventoryList.Add(EquipmentDatabase.equipmentDatabase.equipment[1]);
        GameControl.gameControl.equipmentInventoryList.Add(EquipmentDatabase.equipmentDatabase.equipment[51]);
        GameControl.gameControl.equipmentInventoryList.Add(EquipmentDatabase.equipmentDatabase.equipment[101]);
        GameControl.gameControl.equipmentInventoryList.Add(EquipmentDatabase.equipmentDatabase.equipment[151]);
        GameControl.gameControl.equipmentInventoryList.Add(EquipmentDatabase.equipmentDatabase.equipment[201]);
        GameControl.gameControl.equipmentInventoryList.Add(EquipmentDatabase.equipmentDatabase.equipment[251]);
        GameControl.gameControl.equipmentInventoryList.Add(EquipmentDatabase.equipmentDatabase.equipment[301]);
        GameControl.gameControl.equipmentInventoryList.Add(EquipmentDatabase.equipmentDatabase.equipment[351]);
    }

    public bool IsArmorEquipable (string armor)
    {
        int playerClass = GameControl.gameControl.playerClass;
        List<string> classArmorList = new List<string>();

        #region Class Armor List
        if (playerClass == 0)
        {
            classArmorList = soldierArmor;
        }
        else if (playerClass == 1)
        {
            classArmorList = berserkerArmor;
        }
        else if (playerClass == 2)
        {
            classArmorList = rogueArmor;
        }
        else if (playerClass == 3)
        {
            classArmorList = rangerArmor;
        }
        else if (playerClass == 4)
        {
            classArmorList = wizardArmor;
        }
        else if (playerClass == 5)
        {
            classArmorList = sorcerorArmor;
        }
        else if (playerClass == 6)
        {
            classArmorList = monkArmor;
        }
        else if (playerClass == 7)
        {
            classArmorList = paladinArmor;
        }
        #endregion

        if (classArmorList.Contains(armor))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    //TODO Animation for upgrading
    public void UpgradeEquipment (int equipmentID)
    {
        int currency = GameControl.gameControl.currency;
        int cost = upgradeCost[EquipmentDatabase.equipmentDatabase.equipment[equipmentID].equipmentPowerLevel];
        int profile = GameControl.gameControl.currentProfile;

        if (IsEquipmentUpgradeable(equipmentID) && cost <= currency)
        {
            if (equipmentID == GameControl.gameControl.profile1Equipment || equipmentID == GameControl.gameControl.profile1Weapon ||
                equipmentID == GameControl.gameControl.profile2Equipment || equipmentID == GameControl.gameControl.profile2Weapon)
            {
                if (EquipmentLevelRequirementMet(equipmentID + 1))
                {
                    #region Upgrade Equipment
                    GameControl.gameControl.equipmentInventoryList.Remove(EquipmentDatabase.equipmentDatabase.equipment[equipmentID]);
                    GameControl.gameControl.equipmentInventoryList.Add(EquipmentDatabase.equipmentDatabase.equipment[equipmentID + 1]);
                    #endregion
                    if (GameControl.gameControl.profile1Equipment == equipmentID)
                    {
                        GameControl.gameControl.profile1Equipment = equipmentID + 1;
                    }
                    else if (GameControl.gameControl.profile1Weapon == equipmentID)
                    {
                        GameControl.gameControl.profile1Weapon = equipmentID + 1;
                    }
                    else if (GameControl.gameControl.profile2Equipment == equipmentID)
                    {
                        GameControl.gameControl.profile2Equipment = equipmentID + 1;
                    }
                    else if (GameControl.gameControl.profile2Weapon == equipmentID)
                    {
                        GameControl.gameControl.profile2Weapon = equipmentID + 1;
                    }
                }
                else
                {
                    if (!proceedWithUpgradeAnyway)
                    {
                        //present message showing that the equipment wont be equipable once upgraded and the next best piece of equipment will be automatically selected.
                        //just turning on the message from pause prefab.

                        //add listener to the button on the message with the equipment ID.
                        //Button playButton = expansion.transform.GetChild(1).GetComponent<Button>();
                        //playButton.onClick.AddListener(() => GameObject.FindObjectOfType<MainMenuControl>().PlayGame(fileNumber));

                        proceedWithUpgradeAnyway = true;
                    }
                    else
                    {
                        #region Upgrade Equipment
                        GameControl.gameControl.equipmentInventoryList.Remove(EquipmentDatabase.equipmentDatabase.equipment[equipmentID]);
                        GameControl.gameControl.equipmentInventoryList.Add(EquipmentDatabase.equipmentDatabase.equipment[equipmentID + 1]);
                        #endregion
                        //switch out equipped equipment for last that meets level requirement.
                        proceedWithUpgradeAnyway = false;
                    }
                }
            }
            GameControl.gameControl.UpdateEquippedStats(true);
            //Play satisfying animation for upgrade!
        }
        else
        {
            PlayerSoundEffects.playerSoundEffects.PlaySoundEffect(PlayerSoundEffects.playerSoundEffects.SoundEffectToArrayInt(PlayerSoundEffects.SoundEffect.MenuUnable));
        }
    }

    public void IsEquipmentAWeapon(int equipmentID)
    {
        Equipment.EquipmentType equipmentType = EquipmentDatabase.equipmentDatabase.equipment[equipmentID].equipmentType;
        if (equipmentType == Equipment.EquipmentType.Cloth || equipmentType == Equipment.EquipmentType.Leather ||
            equipmentType == Equipment.EquipmentType.Chainmail || equipmentType == Equipment.EquipmentType.Platemail)
        {
            isEquipmentAWeapon = false;
        }
        else
        {
            isEquipmentAWeapon = true;
        }
    }

    public void ChangeEquipmentFromMenu ()
    {
        int equipmentID;
        int.TryParse(EventSystem.current.currentSelectedGameObject.transform.GetChild(0).GetComponent<Text>().text, out equipmentID);
        SwitchEquipment(equipmentID);
        transform.parent.GetComponent<SelectionMenu>().ReRunList();
    }

    //TODO Update Acquired and Active Skills list
    public void SwitchEquipment (int equipmentID)
    {        
        if (EquipmentLevelRequirementMet(equipmentID))
        {
            int profile = GameControl.gameControl.currentProfile;
            IsEquipmentAWeapon(equipmentID);

            if (profile == 1)
            {
                if (isEquipmentAWeapon)
                {
                    GameControl.gameControl.profile1Weapon = equipmentID;

                    if (!IsArmorEquipable(EquipmentDatabase.equipmentDatabase.equipment[GameControl.gameControl.profile1Equipment].equipmentName))
                    {
                        foreach (Equipment equipment in GameControl.gameControl.equipmentInventoryList)
                        {
                            if (equipment.equipmentName == "Pit Rags")
                            {
                                GameControl.gameControl.profile1Equipment = equipment.equipmentID;
                            }
                        }
                    }
                }
                else
                {
                    GameControl.gameControl.profile1Equipment = equipmentID;
                }
            }
            else if (profile == 2)
            {
                if (isEquipmentAWeapon)
                {
                    GameControl.gameControl.profile2Weapon = equipmentID;

                    if (!IsArmorEquipable(EquipmentDatabase.equipmentDatabase.equipment[GameControl.gameControl.profile2Equipment].equipmentName))
                    {
                        foreach (Equipment equipment in GameControl.gameControl.equipmentInventoryList)
                        {
                            if (equipment.equipmentName == "Pit Rags")
                            {
                                GameControl.gameControl.profile2Equipment = equipment.equipmentID;
                            }
                        }
                    }
                }
                else
                {
                    GameControl.gameControl.profile2Equipment = equipmentID;
                }
            }

            //TODO Update Acquired and Active Skills list
            GameControl.gameControl.UpdateEquippedStats(true);
        }
        else
        {
            PlayerSoundEffects.playerSoundEffects.PlaySoundEffect(PlayerSoundEffects.playerSoundEffects.SoundEffectToArrayInt(PlayerSoundEffects.SoundEffect.MenuUnable));
        }
    }

    public bool IsEquipmentUpgradeable (int equipmentID)
    {
        string nameBeforeUpgrade = EquipmentDatabase.equipmentDatabase.equipment[equipmentID].equipmentName;
        string nameAfterUpgrade = EquipmentDatabase.equipmentDatabase.equipment[(equipmentID + 1)].equipmentName;

        if (nameBeforeUpgrade == nameAfterUpgrade)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool EquipmentLevelRequirementMet (int equipmentID)
    {
        int levelRequirement = EquipmentDatabase.equipmentDatabase.equipment[equipmentID].equipmentLevelRequirement;

        if (GameControl.gameControl.playerLevel >= levelRequirement)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}