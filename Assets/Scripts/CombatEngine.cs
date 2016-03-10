using UnityEngine;

public class CombatEngine : MonoBehaviour {
	
	public static CombatEngine combatEngine;
    public int attackDamage;
    public float critRate;
    //public float dodgeRate;
    //public float counterRate;
    public int maxCombos;
    public int comboCount;

    private float maxIntelligence = 100; //ToDo UPDATE AT SOME POINT
    private float maxNakedCritRate = 25;
    //private float maxAgility = 100;
    //private float maxNakedDodgeRate = 25;
    //private float maxDefense = 100;
    //private float maxNakedCounterRate = 25;

    void Start () {
		combatEngine = GetComponent<CombatEngine>();
	}

    public void CalculateSecondaryStats() {
        float critModifier = 0;
        //float dodgeModifier = 0;
        //float counterModifier = 0;

        foreach (Skills skill in SkillsController.skillsController.activeSkills) {
            critModifier += skill.critModifier;
            //dodgeModifier += skill.dodgeModifier;
            //counterModifier = skill.counterModifier;
        }
        critRate = ((GameControl.gameControl.currentIntelligence / maxIntelligence) * maxNakedCritRate) + (critModifier * 100);
        //dodgeRate = ((GameControl.gameControl.currentSpeed / maxAgility) * maxNakedDodgeRate) + (dodgeModifier * 100);
        //counterRate = ((GameControl.gameControl.currentDefense / maxDefense) * maxNakedCounterRate) + (counterModifier * 100);
    }

    public void CalculateAttackDamage() {
        Equipment.EquipmentMaterial weaponClass = EquipmentDatabase.equipmentDatabase.equipment[GameControl.gameControl.equippedWeapon].equipmentMaterial;
        if (weaponClass == Equipment.EquipmentMaterial.Soldier || weaponClass == Equipment.EquipmentMaterial.Berserker)
        {
            attackDamage = GameControl.gameControl.currentStrength;
        }
        else if (weaponClass == Equipment.EquipmentMaterial.Paladin || weaponClass == Equipment.EquipmentMaterial.Monk)
        {
            attackDamage = GameControl.gameControl.currentStrength;
        }
        else if (weaponClass == Equipment.EquipmentMaterial.Sorcerer || weaponClass == Equipment.EquipmentMaterial.Rogue)
        {
            attackDamage = GameControl.gameControl.currentStrength;
        }
        else if (weaponClass == Equipment.EquipmentMaterial.Ranger || weaponClass == Equipment.EquipmentMaterial.Wizard)
        {
            attackDamage = GameControl.gameControl.currentIntelligence;
        }
    }

    public bool AttackingPhase (Collider2D collider, Attacker attacker) {
        if (attacker == Attacker.Player)
        {
            SkillsController.skillsController.ActivatePlayerAbilities(Skills.TriggerPhase.Attacking);
            SkillsController.skillsController.ActivateEnemyAbilities(Skills.TriggerPhase.BeingAttacked, collider);
        }
        else
        {
            SkillsController.skillsController.ActivatePlayerAbilities(Skills.TriggerPhase.BeingAttacked);
            SkillsController.skillsController.ActivateEnemyAbilities(Skills.TriggerPhase.Attacking, collider);
        }
        return true;
    }

    public bool HittingPhase(Collider2D collider, Attacker attacker)
    {
        if (attacker == Attacker.Player)
        {
            SkillsController.skillsController.ActivatePlayerAbilities(Skills.TriggerPhase.Hitting);
            SkillsController.skillsController.ActivateEnemyAbilities(Skills.TriggerPhase.BeingHit, collider);
        }
        else
        {
            SkillsController.skillsController.ActivatePlayerAbilities(Skills.TriggerPhase.BeingHit);
            SkillsController.skillsController.ActivateEnemyAbilities(Skills.TriggerPhase.Hitting, collider);
        }
        return true;
    }

    public bool DealingDamageToEnemyPhase(Collider2D collider)
    {
        int enemyDefense = collider.gameObject.GetComponent<EnemyStats>().defense;

        SkillsController.skillsController.ActivatePlayerAbilities(Skills.TriggerPhase.DealingDamage);
        SkillsController.skillsController.ActivateEnemyAbilities(Skills.TriggerPhase.BeingDamaged, collider);

        CalculateAttackDamage();
        CalculateSecondaryStats();

        //Check for a crit.
        float randomNumber = Random.Range(0, 100);
        if (randomNumber <= critRate)
        {
            attackDamage = attackDamage * 2;
            Debug.Log("Critical Hit!");
        }

        if ((attackDamage - enemyDefense) > 0)
        {
            collider.gameObject.GetComponent<EnemyStats>().hP -= (attackDamage - enemyDefense);
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool DealingDamageToPlayerPhase (Collider2D collider)
    {
        int playerDefense = GameControl.gameControl.currentDefense;
        int enemyAttackDamage = collider.gameObject.GetComponent<EnemyStats>().attackDamage;

        SkillsController.skillsController.ActivatePlayerAbilities(Skills.TriggerPhase.BeingDamaged);
        SkillsController.skillsController.ActivateEnemyAbilities(Skills.TriggerPhase.DealingDamage, collider);

        if ((enemyAttackDamage - playerDefense) > 0)
        {
            GameControl.gameControl.hp -= (enemyAttackDamage - playerDefense);
            return true;
        }
        else
        {
            return false;
        }
    }
	
	public void AttackingEnemies (Collider2D collider) {
        if (AttackingPhase(collider, Attacker.Player))
        {
            Debug.Log("The Attack Has Hit!");
            if (HittingPhase(collider, Attacker.Player))
            {
                Debug.Log("The Hit Can Deal Damage!");
                if (DealingDamageToEnemyPhase(collider))
                {
                    Debug.Log("Damage Delt!");
                }
            }
        }
        else
        {
            Debug.Log("We Missed!");
            //at the end we should clear the active skills list for anything the triggered, that doesnt have a duration.
            SkillsController.skillsController.ClearAttackingCombatTriggeredAbilitiesFromList();
        }
	}

    //The collider being passed here is actually the enemy attacker, since we would have no other way of knowing which one it was.
    public void AttackingPlayer (Collider2D collider)
    {
        if (AttackingPhase(collider, Attacker.Enemy))
        {
            Debug.Log("The Attack Has Hit!");
            if (HittingPhase(collider, Attacker.Enemy))
            {
                Debug.Log("The Hit Can Deal Damage!");
                if (DealingDamageToPlayerPhase(collider))
                {
                    Debug.Log("Damage Delt!");
                }
            }
        }
    }

    public enum Attacker
    {
        Player,
        Enemy
    }
}
