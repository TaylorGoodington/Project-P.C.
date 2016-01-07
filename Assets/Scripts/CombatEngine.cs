using UnityEngine;

public class CombatEngine : MonoBehaviour {
	
	public static CombatEngine combatEngine;
    public int attackDamage;
    public float critRate;
    public float dodgeRate;
    public float counterRate;
    public int maxCombos;
    public int comboCount;

    private float maxIntelligence = 100;
    private float maxNakedCritRate = 25;
    private float maxAgility = 100;
    private float maxNakedDodgeRate = 25;
    private float maxDefense = 100;
    private float maxNakedCounterRate = 25;

    void Start () {
		combatEngine = GetComponent<CombatEngine>();
	}
	
	void Update () {
	
	}

    public void CalculateSecondaryStats() {
        //I need to multiply these by some factor, then also check the active skill list for any spell that increases crit rate.
        float critModifier = 0;
        float dodgeModifier = 0;
        float counterModifier = 0;

        foreach (Skills skill in SkillsController.skillsController.activeSkills) {
            critModifier += skill.critModifier;
            dodgeModifier += skill.dodgeModifier;
            counterModifier = skill.counterModifier;
        }
        critRate = ((GameControl.gameControl.currentIntelligence / maxIntelligence) * maxNakedCritRate) + (critModifier * 100);
        dodgeRate = ((GameControl.gameControl.currentSpeed / maxAgility) * maxNakedDodgeRate) + (dodgeModifier * 100);
        counterRate = ((GameControl.gameControl.currentDefense / maxDefense) * maxNakedCounterRate) + (counterModifier * 100);
    }

    public void CalculateAttackDamage() {
        Equipment.EquipmentMaterial weaponClass = EquipmentDatabase.equipmentDatabase.equipment[GameControl.gameControl.equippedWeapon].equipmentMaterial;
        if (weaponClass == Equipment.EquipmentMaterial.Soldier || weaponClass == Equipment.EquipmentMaterial.Berserker)
        {
            attackDamage = Mathf.RoundToInt(GameControl.gameControl.currentStrength * 1.25f);
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

    public bool AttackingPhaseOneSucceeds (Collider2D collider) {
        if (1 == 1)
        {
            return true;
        }
        else {
            return false;
        }
    }
	
	public void AttackingEnemies (Collider2D collider) {
        if (AttackingPhaseOneSucceeds(collider))
        {
            Debug.Log("Phase one is successful!");
            Debug.Log(collider.gameObject.name);
            CalculateAttackDamage();
            CalculateSecondaryStats();
        }
        else
        {
            //before these we go through the phases of combat.
            CalculateAttackDamage();
            CalculateSecondaryStats();

            Debug.Log("We Missed!");
        }
	}
}
