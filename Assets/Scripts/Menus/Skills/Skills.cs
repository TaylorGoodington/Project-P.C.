using UnityEngine;
using System.Collections;

[System.Serializable]
public class Skills {

    //I think I will handle cast times the way I have for the attacking animation. 

    public int skillID;
    public int groupNumber;
    public string skillName;
    public string skillDescription;
    public Sprite skillIcon;
    public RequiredStat requiredStatName;
    public int requiredStatValue;
    public RequiredWeapon requiredWeapon;
    public AbilityType abilityType;
    public TriggerPhase triggerPhase;
    public float manaCost;
    public float cooldown;
    public bool interuptable;
    public int skillDuration;
    //only for passive skills, everything else is 1.
    public float triggerRate;
    public float critModifier;
    public float dodgeModifier;
    public float counterModifier;

    
    public enum AbilityType {
        Active,
        Passive
    }

    public enum TriggerPhase
    {
        Attacking,
        Hitting,
        DealingDamage,
        BeingAttacked,
        BeingHit,
        BeginDamaged,
        OnCast
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

    public Skills (int id,int groupID, string name, string description, RequiredStat statName, int statValue, RequiredWeapon weapon, AbilityType type, 
                   TriggerPhase phase, float cost, float cd, bool interupt, int duration, float rate, float crit, float dodge, float counter) {
        skillID = id;
        groupNumber = groupID;
        skillName = name;
        skillDescription = description;
        requiredStatName = statName;
        requiredStatValue = statValue;
        requiredWeapon = weapon;
        abilityType = type;
        triggerPhase = phase;
        manaCost = cost;
        cooldown = cd;
        interuptable = interupt;
        skillDuration = duration;
        triggerRate = rate;
        critModifier = crit;
        dodgeModifier = dodge;
        counterModifier = counter;
    }

    public Skills() {
    }

}
