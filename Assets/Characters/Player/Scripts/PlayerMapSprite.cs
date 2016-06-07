using UnityEngine;
using UnityEngine.UI;

public class PlayerMapSprite : MonoBehaviour
{
    public Image body;
    public Image equipment;
    public Image hair;
    public Image weapon;
    PlayerSpriteDatabase spriteDatabase;

    void Start ()
    {
        spriteDatabase = PlayerSpriteDatabase.playerSpriteDatabase;
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

    public void UpdateSprites()
    {
        body.sprite = spriteDatabase.bodySprites[GameControl.gameControl.skinColorIndex - 1];
        hair.sprite = spriteDatabase.hairSprites[GameControl.gameControl.hairIndex - 1];
        spriteDatabase.AssignEquipmentID(false);
        spriteDatabase.AssignEquipmentIndex();
        spriteDatabase.AssignWeaponIndex();
        equipment.sprite = spriteDatabase.equipmentSprites[spriteDatabase.equipmentIndex];
        weapon.sprite = spriteDatabase.weaponSprites[spriteDatabase.weaponIndex];
    }
}