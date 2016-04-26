using UnityEngine;

[System.Serializable]
public class Skills
{
    public int skillID;
    public int groupNumber;
    public string skillName;
    public string skillDescription;
    public Sprite skillIcon;
    public RequiredStat requiredStatName;
    public int requiredStatValue;
    public RequiredWeapon requiredWeapon;
    public TriggerPhase triggerPhase;
    public AnimationType animationType;
    public string locationInScene;
    public bool projectileFired;
    public float manaCost;
    public float cooldown;
    public bool interuptable;
    public int skillDuration;
    public float triggerRate;
    public float damageDelt;
    public float enemyStateRate;
    public int enemyStateInflicted;
    public float playerStateRate;
    public int playerStateInflicted;
    public float damageIncrease;
    public float strengthincrease;
    public float defenseIncrease;
    public float speedIncrease;
    public float intelligenceIncrease;
    public float healthIncrease;
    public float hpIncrease;
    public float hp5Increase;
    public float manaIncrease;
    public float mpIncrease;
    public float mp5Increase;
    public float critModifier;
    public int knockbackForce;

    public enum TriggerPhase
    {
        Attacking,
        Hitting,
        DealingDamage,
        BeingAttacked,
        BeingHit,
        BeingDamaged,
        OnCast,
        Passive
    }

    public enum RequiredStat {
        None,
        Strength,
        Defense,
        Agility,
        Intelligence
    }

    public enum RequiredWeapon
    {
        Sword,
        Axe,
        Dagger,
        Bow,
        Fist,
        Talisman,
        Staff,
        Polearm,
        None
    }

    public enum AnimationType
    {
        Ability,
        MovementAbility,
        Buff,
        Ultimate,
        None
    }

    public Skills (int id,int groupID, string name, string description, RequiredStat statName, int statValue, RequiredWeapon weapon, 
                   TriggerPhase phase, AnimationType animation, string location, bool projectile, float cost, float cd, bool interupt, int duration, float trigRate,
                   float damage, float enemySTRate, int enemyST, float playerSTRate, int playerST,
                   float damageUp, float strengthUp, float defenseUp, float speedUp, float intelligenceUp, float healthUp, 
                   float hpUp, float hp5Up, float manaUp, float mpUp, float mp5Up, float crit, int knockback) {
        skillID = id;
        groupNumber = groupID;
        skillName = name;
        skillDescription = description;
        requiredStatName = statName;
        requiredStatValue = statValue;
        requiredWeapon = weapon;
        triggerPhase = phase;
        animationType = animation;
        locationInScene = location;
        projectileFired = projectile;
        manaCost = cost;
        cooldown = cd;
        interuptable = interupt;
        skillDuration = duration;
        triggerRate = trigRate;
        damageDelt = damage;
        enemyStateRate = enemySTRate;
        enemyStateInflicted = enemyST;
        playerStateRate = playerSTRate;
        playerStateInflicted = playerST;
        damageIncrease = damageUp;
        strengthincrease = strengthUp;
        defenseIncrease = defenseUp;
        speedIncrease = speedUp;
        intelligenceIncrease = intelligenceUp;
        healthIncrease = healthUp;
        hpIncrease = hpUp;
        hp5Increase = hp5Up;
        manaIncrease = manaUp;
        mpIncrease = mpUp;
        mp5Increase = mp5Up;
        critModifier = crit;
        knockbackForce = knockback;
    }

    public Skills()
    {

    }
}
