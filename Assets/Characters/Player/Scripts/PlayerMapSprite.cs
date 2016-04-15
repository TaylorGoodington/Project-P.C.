﻿using UnityEngine;

public class PlayerMapSprite : MonoBehaviour {

    public SpriteRenderer body;
    public Sprite[] bodySprites;
    public SpriteRenderer equipment;
    public Sprite[] equipmentSprites;
    public SpriteRenderer hair;
    public Sprite[] hairSprites;
    public SpriteRenderer weapon;
    public Sprite[] weaponSprites;

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
        equipment.sprite = equipmentSprites[GameControl.gameControl.equippedEquipmentIndex - 1];
        hair.sprite = hairSprites[GameControl.gameControl.hairIndex - 1];
        weapon.sprite = weaponSprites[GameControl.gameControl.equippedWeapon - 1];
    }
}