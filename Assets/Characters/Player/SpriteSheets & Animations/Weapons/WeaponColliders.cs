using UnityEngine;
using System.Collections.Generic;

public class WeaponColliders : MonoBehaviour {

    public GameObject sprite;
    private Dictionary<string, GameObject> weaponCollidersList;
    private string activatedCollider;

    void Start()
    {
        weaponCollidersList = new Dictionary<string, GameObject>();
        InitializeColliderList();
    }

    public void ActivateWeaponCollider(bool grounded)
    {
        var equipmentType = EquipmentDatabase.equipmentDatabase.equipment[GameControl.gameControl.profile1Weapon].equipmentType;
        var equipmentTier = EquipmentDatabase.equipmentDatabase.equipment[GameControl.gameControl.profile1Weapon].equipmentTier;

        if (grounded)
        {
            activatedCollider = equipmentType + " " + equipmentTier + "." + CombatEngine.combatEngine.comboCount;
        }
        else
        {
            activatedCollider = equipmentType + " " + equipmentTier + ".0";
        }

        CheckForFlip(weaponCollidersList[activatedCollider]);
        weaponCollidersList[activatedCollider].GetComponent<Collider2D>().enabled = true;
    }

    public void DisableActiveCollider()
    {
        if (activatedCollider != null && weaponCollidersList[activatedCollider].GetComponent<Collider2D>().enabled)
        {
            weaponCollidersList[activatedCollider].GetComponent<Collider2D>().enabled = false;
        }
    }

    void CheckForFlip (GameObject desiredCollider)
    {
        if (sprite.GetComponent<SpriteRenderer>().flipX)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }

    void InitializeColliderList ()
    {
        foreach (Transform child in transform)
        {
            weaponCollidersList.Add(child.name, child.gameObject);
            child.GetComponent<Collider2D>().enabled = false;
        }
    }
}