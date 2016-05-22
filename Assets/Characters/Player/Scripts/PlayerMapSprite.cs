using UnityEngine;

public class PlayerMapSprite : MonoBehaviour {

    public SpriteRenderer body;
    public Sprite[] bodySprites;
    public SpriteRenderer equipment;
    public Sprite[] equipmentSprites;
    int equipmentIndex;
    public SpriteRenderer hair;
    public Sprite[] hairSprites;
    public SpriteRenderer weapon;
    public Sprite[] weaponSprites;
    int weaponIndex;

    void Start ()
    {
        UpdateSprites();
    }

    void Update()
    {
        if (!GameControl.gameControl.AnyOpenMenus())
        {
            UpdateSprites();
        }
    }

    public void SetPosition(Vector3 position)
    {
        transform.position = position;
    }

    void UpdateSprites()
    {
        body.sprite = bodySprites[GameControl.gameControl.skinColorIndex - 1];
        hair.sprite = hairSprites[GameControl.gameControl.hairIndex - 1];
        AssignEquipmentIndex();
        equipment.sprite = equipmentSprites[equipmentIndex];
        AssignWeaponIndex();
        weapon.sprite = weaponSprites[weaponIndex];
    }

    void AssignEquipmentIndex ()
    {
        if (EquipmentDatabase.equipmentDatabase.equipment[GameControl.gameControl.equippedEquipmentIndex].equipmentName == "Pit Rags")
        {
            equipmentIndex = 0;
        }
        else if (EquipmentDatabase.equipmentDatabase.equipment[GameControl.gameControl.equippedEquipmentIndex].equipmentName == "Wizard Robes")
        {
            equipmentIndex = 1;
        }
        else if (EquipmentDatabase.equipmentDatabase.equipment[GameControl.gameControl.equippedEquipmentIndex].equipmentName == "Leather Armor")
        {
            equipmentIndex = 2;
        }
        else if (EquipmentDatabase.equipmentDatabase.equipment[GameControl.gameControl.equippedEquipmentIndex].equipmentName == "Plate Armor")
        {
            equipmentIndex = 3;
        }
    }

    void AssignWeaponIndex ()
    {
        string weaponType = EquipmentDatabase.equipmentDatabase.equipment[GameControl.gameControl.equippedWeapon].equipmentType.ToString();
        int weaponNumber = EquipmentDatabase.equipmentDatabase.equipment[GameControl.gameControl.equippedWeapon].equipmentTier;
        string weaponTypeAndNumber = weaponType + weaponNumber;
        if (weaponTypeAndNumber == "Sword1")
        {
            weaponIndex = 0;
        }
        else if (weaponTypeAndNumber == "Staff1")
        {
            weaponIndex = 10;
        }
        else if (weaponTypeAndNumber == "Bow1")
        {
            weaponIndex = 20;
        }
        else if (weaponTypeAndNumber == "Polearm1")
        {
            weaponIndex = 30;
        }
    }
}