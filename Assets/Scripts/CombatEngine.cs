using UnityEngine;

public class CombatEngine : MonoBehaviour {
	
	public static CombatEngine combatEngine;
    public int attackDamage;

	void Start () {
		combatEngine = GetComponent<CombatEngine>();
	}
	
	void Update () {
	
	}

    public void CalculateAttackDamage() {
        Equipment.EquipmentMaterial weaponClass = EquipmentDatabase.equipmentDatabase.equipment[GameControl.gameControl.equippedWeapon].equipmentMaterial;
        if (weaponClass == Equipment.EquipmentMaterial.Soldier || weaponClass == Equipment.EquipmentMaterial.Berserker)
        {
            attackDamage = GameControl.gameControl.currentStrength;
        }
        else if (weaponClass == Equipment.EquipmentMaterial.Paladin || weaponClass == Equipment.EquipmentMaterial.Monk)
        {
            attackDamage = GameControl.gameControl.currentDefense;
        }
        else if (weaponClass == Equipment.EquipmentMaterial.Sorcerer || weaponClass == Equipment.EquipmentMaterial.Wizard)
        {
            attackDamage = GameControl.gameControl.currentIntelligence;
        }
        else if (weaponClass == Equipment.EquipmentMaterial.Ranger || weaponClass == Equipment.EquipmentMaterial.Rogue)
        {
            attackDamage = GameControl.gameControl.currentSpeed;
        }
    }
	
	public void Attacking (Collider2D collider) {
        CalculateAttackDamage();

		Debug.Log ("We're Attacking Now!");
        Debug.Log(collider.gameObject.name);
	}
}
