using UnityEngine;

public class CombatEngine : MonoBehaviour {

    //ToDo Talk about players having dodge & parry type skills. Add the reduction in taking damage to the engine.
	
	public static CombatEngine combatEngine;
    public int attackDamage;
    public float critRate;
    [HideInInspector]
    public int maxCombos;
    public int comboCount;
    public bool runComboClock;
    public float comboWindow;
    public float comboCountDown;

    private float maxIntelligence = 100; //ToDo UPDATE AT SOME POINT
    private float maxNakedCritRate = 25;

    public int enemyFaceDirection;
    public int enemyKnockBackForce;
    public int enemyKnockBackDirection;

    void Start () {
		combatEngine = GetComponent<CombatEngine>();
        comboCount = 1;
        runComboClock = false;

        comboWindow = 0.25f;
        comboCountDown = comboWindow;
    }

    //Update used to increment combos.
    void Update ()
    {
        maxCombos = GameControl.gameControl.maxCombos;

        if (runComboClock)
        {
            RunComboClock();
        }
    }

    public void RunComboClock ()
    {
        if (comboCountDown > 0)
        {
            comboCountDown -= Time.deltaTime;
        }
        else if (GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().attackLaunched == false)
        {
            runComboClock = false;
            comboCountDown = comboWindow;
            comboCount = 1;
        }
    }

    public void CalculateSecondaryStats() {
        float critModifier = 0;

        foreach (Skills skill in SkillsController.skillsController.activeSkills) {
            critModifier += skill.critModifier;
        }
        critRate = ((GameControl.gameControl.currentIntelligence / maxIntelligence) * maxNakedCritRate) + (critModifier * 100);
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
        }

        if ((attackDamage - enemyDefense) > 0)
        {
            collider.gameObject.GetComponent<EnemyStats>().hP -= (attackDamage - enemyDefense);
            collider.GetComponent<EnemyBase>().DisplayDamageReceived(attackDamage);
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool DealingDamageToPlayerPhase (Collider2D collider, int damage)
    {
        int playerDefense = GameControl.gameControl.currentDefense;
        int enemyAttackDamage = damage;

        SkillsController.skillsController.ActivatePlayerAbilities(Skills.TriggerPhase.BeingDamaged);
        SkillsController.skillsController.ActivateEnemyAbilities(Skills.TriggerPhase.DealingDamage, collider);

        if ((enemyAttackDamage - playerDefense) > 0)
        {
            GameControl.gameControl.hp -= (enemyAttackDamage - playerDefense);
            return true;
        }
        else
        {
            GameControl.gameControl.hp -= collider.GetComponent<EnemyStats>().minimumDamage;
            return true;
        }
    }
	
	public void AttackingEnemies (Collider2D collider)
    {
        if (AttackingPhase(collider, Attacker.Player))
        {
            if (HittingPhase(collider, Attacker.Player))
            {
                if (DealingDamageToEnemyPhase(collider))
                {
                    
                }
            }
        }
        else
        {
            //at the end we should clear the active skills list for anything the triggered, that doesnt have a duration.
            SkillsController.skillsController.ClearAttackingCombatTriggeredAbilitiesFromList();
        }
	}

    //The collider being passed here is actually the enemy attacker, since we would have no other way of knowing which one it was.
    public void AttackingPlayer (Collider2D collider, int damage)
    {
        if (AttackingPhase(collider, Attacker.Enemy))
        {
            if (HittingPhase(collider, Attacker.Enemy))
            {
                if (DealingDamageToPlayerPhase(collider, damage))
                {
                    //Hazard Response.
                    if (collider.GetComponent<Hazard>())
                    {
                        GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().flinching = true;
                        collider.GetComponent<Hazard>().SendMessage("KnockBack");
                    }

                    //Conventional Enemy.
                    else
                    {
                        enemyFaceDirection = collider.GetComponent<Controller2D>().collisions.faceDir;
                        enemyKnockBackForce = collider.GetComponent<EnemyStats>().knockbackForce;
                        GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().flinching = true;
                        GameObject.FindGameObjectWithTag("Player").SendMessage("Knockback");
                    }
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
