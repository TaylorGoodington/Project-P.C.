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
            attackDamage = Mathf.RoundToInt(GameControl.gameControl.currentStrength);
        }
        else if (weaponClass == Equipment.EquipmentMaterial.Paladin || weaponClass == Equipment.EquipmentMaterial.Monk)
        {
            attackDamage = GameControl.gameControl.currentStrength;
        }
        else if (weaponClass == Equipment.EquipmentMaterial.Sorcerer || weaponClass == Equipment.EquipmentMaterial.Wizard)
        {
            attackDamage = GameControl.gameControl.currentStrength;
        }
        else if (weaponClass == Equipment.EquipmentMaterial.Ranger || weaponClass == Equipment.EquipmentMaterial.Rogue)
        {
            attackDamage = GameControl.gameControl.currentStrength;
        }
    }

    public bool AttackingPhase (Collider2D collider) {
        //float playerMissRate = 0; //We aren't using this right now.
        float enemyDodgeRate = 0; //this will at some point be set to the enemies dodge rate, accessed through the collider.

        

        SkillsController.skillsController.ActivateAttackingPhaseAbilities();

        int randomNumber = Random.Range(0, 100);

        if (randomNumber > (enemyDodgeRate) * 100 )
        {
            //Debug.Log(randomNumber);
            return true;
        }
        else {
            return false;
        }
    }
	
	public void AttackingEnemies (Collider2D collider) {
        if (AttackingPhase(collider))
        {
            Debug.Log("Phase one is successful!");
            //Debug.Log(collider.gameObject.name);
        }
        else
        {
            //call these at the begining of phase 3.
            CalculateAttackDamage();
            CalculateSecondaryStats();

            Debug.Log("We Missed!");
            //at the end we should clear the active skills list.
        }
	}
}
