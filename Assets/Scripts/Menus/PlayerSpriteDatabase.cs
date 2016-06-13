using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerSpriteDatabase : MonoBehaviour
{
    public static PlayerSpriteDatabase playerSpriteDatabase;

    public Sprite[] bodySprites;
    public Sprite[] equipmentSprites;
    public Sprite[] hairSprites;
    public Sprite[] weaponSprites;
    int weaponID;
    int armorID;
    public int weaponIndex;
    public int equipmentIndex;

    void Start ()
    {
        playerSpriteDatabase = GetComponent<PlayerSpriteDatabase>();
    }

    public void AssignEquipmentID (bool inPauseMenu)
    {
        if (inPauseMenu)
        {
            int equipmentID;
            int.TryParse(EventSystem.current.currentSelectedGameObject.transform.GetChild(0).GetComponent<Text>().text, out equipmentID);
            weaponID = equipmentID;
            armorID = equipmentID;
        }
        else
        {
            armorID = (GameControl.gameControl.currentProfile == 1) ? GameControl.gameControl.profile1Equipment : GameControl.gameControl.profile2Equipment;
            weaponID = (GameControl.gameControl.currentProfile == 1) ? GameControl.gameControl.profile1Weapon : GameControl.gameControl.profile2Weapon;
        }
    }
    
    public void AssignEquipmentIndex()
    {
        if (EquipmentDatabase.equipmentDatabase.equipment[armorID].equipmentName == "Pit Rags")
        {
            equipmentIndex = 0;
        }
        else if (EquipmentDatabase.equipmentDatabase.equipment[armorID].equipmentName == "Arcane Cloak")
        {
            equipmentIndex = 1;
        }
        else if (EquipmentDatabase.equipmentDatabase.equipment[armorID].equipmentName == "Leather Armor")
        {
            equipmentIndex = 2;
        }
        else if (EquipmentDatabase.equipmentDatabase.equipment[armorID].equipmentName == "Plate Armor")
        {
            equipmentIndex = 3;
        }
        else if (EquipmentDatabase.equipmentDatabase.equipment[armorID].equipmentName == "Chain Mail")
        {
            equipmentIndex = 4;
        }
        else if (EquipmentDatabase.equipmentDatabase.equipment[armorID].equipmentName == "Mystic Cowl")
        {
            equipmentIndex = 5;
        }
        else if (EquipmentDatabase.equipmentDatabase.equipment[armorID].equipmentName == "Noble Cuirass")
        {
            equipmentIndex = 6;
        }
        else if (EquipmentDatabase.equipmentDatabase.equipment[armorID].equipmentName == "Dueling Carapace")
        {
            equipmentIndex = 7;
        }
        else if (EquipmentDatabase.equipmentDatabase.equipment[armorID].equipmentName == "Martial Gi")
        {
            equipmentIndex = 8;
        }
        else if (EquipmentDatabase.equipmentDatabase.equipment[armorID].equipmentName == "Hallowed Robe")
        {
            equipmentIndex = 9;
        }
        else if (EquipmentDatabase.equipmentDatabase.equipment[armorID].equipmentName == "Wild Mantle")
        {
            equipmentIndex = 10;
        }
    }

    //TODO Update
    public void AssignWeaponIndex()
    {
        string weaponType = EquipmentDatabase.equipmentDatabase.equipment[weaponID].equipmentType.ToString();
        int weaponNumber = EquipmentDatabase.equipmentDatabase.equipment[weaponID].equipmentTier;
        string weaponTypeAndNumber = weaponType + weaponNumber;
        #region Swords
        if (weaponTypeAndNumber == "Sword1")
        {
            weaponIndex = 0;
        }
        #endregion
        #region Staves
        else if (weaponTypeAndNumber == "Staff1")
        {
            weaponIndex = 10;
        }
        #endregion
        #region Bows
        else if (weaponTypeAndNumber == "Bow1")
        {
            weaponIndex = 20;
        }
        #endregion
        #region Polearms
        else if (weaponTypeAndNumber == "Polearm1")
        {
            weaponIndex = 30;
        }
        #endregion
    }
}