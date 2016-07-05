using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PlayerMenuSprite : MonoBehaviour
{
    public Image body;
    public Image equipment;
    public Image hair;
    public Image weapon;
    PlayerSpriteDatabase spriteDatabase;

    void Start()
    {
        spriteDatabase = PlayerSpriteDatabase.playerSpriteDatabase;
        InitializeSprites();
    }

    public void InitializeSprites()
    {
        PlayerSpriteDatabase.playerSpriteDatabase.AssignEquipmentID(false);
        PlayerSpriteDatabase.playerSpriteDatabase.AssignEquipmentIndex();
        PlayerSpriteDatabase.playerSpriteDatabase.AssignWeaponIndex();
        body.sprite = PlayerSpriteDatabase.playerSpriteDatabase.bodySprites[GameControl.gameControl.skinColorIndex - 1];
        hair.sprite = PlayerSpriteDatabase.playerSpriteDatabase.hairSprites[GameControl.gameControl.hairIndex - 1];
        equipment.sprite = PlayerSpriteDatabase.playerSpriteDatabase.equipmentSprites[PlayerSpriteDatabase.playerSpriteDatabase.equipmentIndex];
        weapon.sprite = PlayerSpriteDatabase.playerSpriteDatabase.weaponSprites[PlayerSpriteDatabase.playerSpriteDatabase.weaponIndex];
    }

    //Called by event triggers on the selection menu.
    public void UpdatePlayerSprite()
    {
        int equipmentID;
        int.TryParse(EventSystem.current.currentSelectedGameObject.transform.GetChild(0).GetComponent<Text>().text, out equipmentID);
        body.sprite = spriteDatabase.bodySprites[GameControl.gameControl.skinColorIndex - 1];
        hair.sprite = spriteDatabase.hairSprites[GameControl.gameControl.hairIndex - 1];
        spriteDatabase.AssignEquipmentID(true);
        EquipmentController.equipmentController.IsEquipmentAWeapon(equipmentID);
        if (EquipmentController.equipmentController.isEquipmentAWeapon)
        {
            spriteDatabase.AssignWeaponIndex();
            weapon.sprite = spriteDatabase.weaponSprites[spriteDatabase.weaponIndex];
            spriteDatabase.AssignEquipmentID(false);
            spriteDatabase.AssignEquipmentIndex();
            equipment.sprite = spriteDatabase.equipmentSprites[spriteDatabase.equipmentIndex];
        }
        else
        {
            spriteDatabase.AssignEquipmentIndex();
            equipment.sprite = spriteDatabase.equipmentSprites[spriteDatabase.equipmentIndex];
            spriteDatabase.AssignEquipmentID(false);
            spriteDatabase.AssignWeaponIndex();
            weapon.sprite = spriteDatabase.weaponSprites[spriteDatabase.weaponIndex];
        }
    }

    //TODO Update
    //Called by event triggers in the weapon menu
    public void UpdateClassExample()
    {
        if (EventSystem.current.currentSelectedGameObject.name == "Swords")
        {
            body.sprite = spriteDatabase.bodySprites[1];
            hair.sprite = spriteDatabase.hairSprites[1];
            equipment.sprite = spriteDatabase.equipmentSprites[1];
            weapon.sprite = spriteDatabase.weaponSprites[0];
        }
        else if (EventSystem.current.currentSelectedGameObject.name == "Axes")
        {
            body.sprite = spriteDatabase.bodySprites[1];
            hair.sprite = spriteDatabase.hairSprites[1];
            equipment.sprite = spriteDatabase.equipmentSprites[1];
            weapon.sprite = spriteDatabase.weaponSprites[0];
        }
        else if (EventSystem.current.currentSelectedGameObject.name == "Daggers")
        {
            body.sprite = spriteDatabase.bodySprites[1];
            hair.sprite = spriteDatabase.hairSprites[1];
            equipment.sprite = spriteDatabase.equipmentSprites[1];
            weapon.sprite = spriteDatabase.weaponSprites[0];
        }
        else if (EventSystem.current.currentSelectedGameObject.name == "Bows")
        {
            body.sprite = spriteDatabase.bodySprites[1];
            hair.sprite = spriteDatabase.hairSprites[1];
            equipment.sprite = spriteDatabase.equipmentSprites[1];
            weapon.sprite = spriteDatabase.weaponSprites[0];
        }
        else if (EventSystem.current.currentSelectedGameObject.name == "Staves")
        {
            body.sprite = spriteDatabase.bodySprites[1];
            hair.sprite = spriteDatabase.hairSprites[1];
            equipment.sprite = spriteDatabase.equipmentSprites[1];
            weapon.sprite = spriteDatabase.weaponSprites[0];
        }
        else if (EventSystem.current.currentSelectedGameObject.name == "Talismans")
        {
            body.sprite = spriteDatabase.bodySprites[1];
            hair.sprite = spriteDatabase.hairSprites[1];
            equipment.sprite = spriteDatabase.equipmentSprites[1];
            weapon.sprite = spriteDatabase.weaponSprites[0];
        }
        else if (EventSystem.current.currentSelectedGameObject.name == "Knuckles")
        {
            body.sprite = spriteDatabase.bodySprites[1];
            hair.sprite = spriteDatabase.hairSprites[1];
            equipment.sprite = spriteDatabase.equipmentSprites[1];
            weapon.sprite = spriteDatabase.weaponSprites[0];
        }
        else if (EventSystem.current.currentSelectedGameObject.name == "Polearms")
        {
            body.sprite = spriteDatabase.bodySprites[1];
            hair.sprite = spriteDatabase.hairSprites[1];
            equipment.sprite = spriteDatabase.equipmentSprites[1];
            weapon.sprite = spriteDatabase.weaponSprites[0];
        }
    }
}