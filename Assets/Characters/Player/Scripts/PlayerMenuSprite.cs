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
        spriteDatabase.AssignEquipmentID(false);
        spriteDatabase.AssignEquipmentIndex();
        spriteDatabase.AssignWeaponIndex();
        body.sprite = spriteDatabase.bodySprites[GameControl.gameControl.skinColorIndex - 1];
        hair.sprite = spriteDatabase.hairSprites[GameControl.gameControl.hairIndex - 1];
        equipment.sprite = spriteDatabase.equipmentSprites[spriteDatabase.equipmentIndex];
        weapon.sprite = spriteDatabase.weaponSprites[spriteDatabase.weaponIndex];
    }

    public void UpdatePlayerSprite ()
    {
        int equipmentID;
        int.TryParse(EventSystem.current.currentSelectedGameObject.transform.GetChild(0).GetComponent<Text>().text, out equipmentID);

        EquipmentController.Equipmentcontroller.IsEquipmentAWeapon(equipmentID);
        if (EquipmentController.Equipmentcontroller.isEquipmentAWeapon)
        {
            spriteDatabase.AssignWeaponIndex();
            weapon.sprite = spriteDatabase.weaponSprites[spriteDatabase.weaponIndex];
        }
        else
        {
            spriteDatabase.AssignEquipmentIndex();
            equipment.sprite = spriteDatabase.equipmentSprites[spriteDatabase.equipmentIndex];
        }
    }
}