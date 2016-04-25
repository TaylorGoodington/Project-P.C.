using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class SkillsController : MonoBehaviour {

    public static SkillsController skillsController;
    private SkillsDatabase skillsDatabase;
    public List<Skills> acquiredSkills;
    public List<Skills> activeSkills;
    public List<Skills> profile1SlottedSkills;
    public List<Skills> profile2SlottedSkills;
    public Dictionary<int, float> cooldownList;
    public Skills selectedSkill;
    public bool activatingAbility;
    public int skillBeingActivated;
    public GameObject[] projectileList;
    public Dictionary<int, bool> talliedSkills;
    float healthModifier;
    float manaModifier;
    float strengthModifier;
    float speedModifier;
    float intelligenceModifier;
    float defenseModifier;
    bool switchingClasses;

    void Start() {
        skillsController = GetComponent<SkillsController>();
        skillsDatabase = GetComponent<SkillsDatabase>();
        cooldownList = new Dictionary<int, float>();
        talliedSkills = new Dictionary<int, bool>();
        healthModifier = 1;
        manaModifier = 1;
        strengthModifier = 1;
        speedModifier = 1;
        intelligenceModifier = 1;
        defenseModifier = 1;
        switchingClasses = false;
    }

    void Update ()
    {
        UpdateCoolDownList();
        if (!switchingClasses)
        {
            AmendStatsFromActiveSkills();
        }
    }

    //this is for sorting any list by group number....Not sure if it will work.
    static int SortByGroupNumber(Skills skill1, Skills skill2) {
        return skill1.groupNumber.CompareTo(skill2.groupNumber);
    }

    public void ClearAttackingCombatTriggeredAbilitiesFromList()
    {
        foreach (Skills skill in activeSkills)
        {
            if (skill.skillDuration == 0 && (skill.triggerPhase == Skills.TriggerPhase.Attacking || skill.triggerPhase == Skills.TriggerPhase.Hitting || 
                                             skill.triggerPhase == Skills.TriggerPhase.DealingDamage))
            {
                activeSkills.Remove(skill);
            }
        }
    }

    public void ActivatePlayerAbilities(Skills.TriggerPhase triggerPhase)
    {
        int currentGroupNumber = 0;
        int triggeringSkill = 0;
        float triggeringSkillRate = 0;

        acquiredSkills.Sort(SortByGroupNumber);
        foreach (Skills skill in acquiredSkills)
        {
            if (skill.triggerPhase == triggerPhase)
            {
                //the first if statement needs to check if the ability is the last in the list. If it is check the if below.
                if (skill.skillID == activeSkills.Last().skillID)
                {
                    //New skill, new group number.
                    if (!activeSkills.Contains(skill) && skill.groupNumber != currentGroupNumber)
                    {
                        CheckIfSkillTriggers(triggeringSkill, triggeringSkillRate);

                        currentGroupNumber = skill.groupNumber;
                        triggeringSkill = skill.skillID;
                        triggeringSkillRate = skill.triggerRate;

                        CheckIfSkillTriggers(triggeringSkill, triggeringSkillRate);
                    }
                    //New skill, same group number.
                    else if (!activeSkills.Contains(skill) && skill.groupNumber == currentGroupNumber)
                    {
                        triggeringSkillRate += skill.triggerRate;
                        CheckIfSkillTriggers(triggeringSkill, triggeringSkillRate);
                    }
                }
                else {

                    //New skill, new group number.
                    if (!activeSkills.Contains(skill) && skill.groupNumber != currentGroupNumber)
                    {
                        CheckIfSkillTriggers(triggeringSkill, triggeringSkillRate);

                        currentGroupNumber = skill.groupNumber;
                        triggeringSkill = skill.skillID;
                        triggeringSkillRate = skill.triggerRate;
                    }
                    //New skill, same group number.
                    else if (!activeSkills.Contains(skill) && skill.groupNumber == currentGroupNumber)
                    {
                        triggeringSkillRate += skill.triggerRate;
                    }
                    else
                    {
                        CheckIfSkillTriggers(triggeringSkill, triggeringSkillRate);

                        currentGroupNumber = 0;
                        triggeringSkill = 0;
                        triggeringSkillRate = 0;
                    }
                }
            }
        }
    }

    public void CheckIfSkillTriggers(int skillID, float triggerRate) {
        int randomNumber = Random.Range(0, 101);
        if (randomNumber > triggerRate) {
            ActivateAbilityTest(skillID);
        }
    }

    public void ClassSwitch ()
    {
        switchingClasses = true;
        foreach (Skills skill in activeSkills)
        {
            ReduceModifiers(skill);
        }
        talliedSkills.Clear();
    }

    public void DoneSwitchingClasses ()
    {
        switchingClasses = false;
    }

    public void AmendStatsFromActiveSkills ()
    {
        foreach (Skills skill in activeSkills)
        {
            int key = skill.skillID;
            if (talliedSkills.ContainsKey(key))
            {
                if (talliedSkills[key] == false)
                {
                    IncreaseModifiers(skill);
                    GameControl.gameControl.hp += (int)((skill.hpIncrease) * (GameControl.gameControl.currentHealth * 10));
                    GameControl.gameControl.mp += (int)((skill.mpIncrease) * (GameControl.gameControl.currentMana * 10));
                    talliedSkills[key] = true;
                }
            }
            else
            {
                IncreaseModifiers(skill);
                GameControl.gameControl.hp += (int)((skill.hpIncrease) * (GameControl.gameControl.currentHealth * 10));
                GameControl.gameControl.mp += (int)((skill.mpIncrease) * (GameControl.gameControl.currentMana * 10));
                talliedSkills.Add(key, true);
            }
        }
    }

    public void IncreaseModifiers (Skills skill)
    {
        healthModifier += skill.healthIncrease;
        manaModifier += skill.manaIncrease;
        strengthModifier += skill.strengthincrease;
        intelligenceModifier += skill.intelligenceIncrease;
        speedModifier += skill.speedIncrease;
        defenseModifier += skill.defenseIncrease;

        GameControl.gameControl.currentDefense = Mathf.RoundToInt(GameControl.gameControl.currentDefense * defenseModifier);
        GameControl.gameControl.currentHealth = Mathf.RoundToInt(GameControl.gameControl.currentHealth * healthModifier);
        GameControl.gameControl.currentIntelligence = Mathf.RoundToInt(GameControl.gameControl.currentIntelligence * intelligenceModifier);
        GameControl.gameControl.currentMana = Mathf.RoundToInt(GameControl.gameControl.currentMana * manaModifier);
        GameControl.gameControl.currentSpeed = Mathf.RoundToInt(GameControl.gameControl.currentSpeed * speedModifier);
        GameControl.gameControl.currentStrength = Mathf.RoundToInt(GameControl.gameControl.currentStrength * strengthModifier);
    }

    public void ReduceModifiers (Skills skill)
    {
        GameControl.gameControl.currentDefense = Mathf.RoundToInt(GameControl.gameControl.currentDefense / defenseModifier);
        GameControl.gameControl.currentHealth = Mathf.RoundToInt(GameControl.gameControl.currentHealth / healthModifier);
        GameControl.gameControl.currentIntelligence = Mathf.RoundToInt(GameControl.gameControl.currentIntelligence / intelligenceModifier);
        GameControl.gameControl.currentMana = Mathf.RoundToInt(GameControl.gameControl.currentMana / manaModifier);
        GameControl.gameControl.currentSpeed = Mathf.RoundToInt(GameControl.gameControl.currentSpeed / speedModifier);
        GameControl.gameControl.currentStrength = Mathf.RoundToInt(GameControl.gameControl.currentStrength / strengthModifier);

        healthModifier -= skill.healthIncrease;
        manaModifier -= skill.manaIncrease;
        strengthModifier -= skill.strengthincrease;
        intelligenceModifier -= skill.intelligenceIncrease;
        speedModifier -= skill.speedIncrease;
        defenseModifier -= skill.defenseIncrease;

        GameControl.gameControl.currentDefense = Mathf.RoundToInt(GameControl.gameControl.currentDefense * defenseModifier);
        GameControl.gameControl.currentHealth = Mathf.RoundToInt(GameControl.gameControl.currentHealth * healthModifier);
        GameControl.gameControl.currentIntelligence = Mathf.RoundToInt(GameControl.gameControl.currentIntelligence * intelligenceModifier);
        GameControl.gameControl.currentMana = Mathf.RoundToInt(GameControl.gameControl.currentMana * manaModifier);
        GameControl.gameControl.currentSpeed = Mathf.RoundToInt(GameControl.gameControl.currentSpeed * speedModifier);
        GameControl.gameControl.currentStrength = Mathf.RoundToInt(GameControl.gameControl.currentStrength * strengthModifier);
    }

    public void LevelEndAbilityListCleaning ()
    {
        foreach (Skills skill in activeSkills)
        {
            if (skill.triggerPhase != Skills.TriggerPhase.Passive)
            {
                activeSkills.Remove(skill);
            }
        }
        
        cooldownList = new Dictionary<int, float>();
    }

    public void ActivateEnemyAbilities (Skills.TriggerPhase trigger, Collider2D collider)
    {
        int currentGroupNumber = 0;
        int triggeringSkill = 0;
        float triggeringSkillRate = 0;
        List<Skills> acquiredSkillsList = collider.GetComponent<EnemyStats>().acquiredSkillsList;
        List<Skills> activeSkillsList = collider.GetComponent<EnemyStats>().activeSkillsList;

        acquiredSkillsList.Sort(SortByGroupNumber);
        foreach (Skills skill in acquiredSkillsList)
        {
            if (skill.triggerPhase == trigger)
            {
                //the first if statement needs to check if the ability is the last in the list. If it is check the if below.
                if (skill.skillID == activeSkillsList.Last().skillID)
                {
                    //New skill, new group number.
                    if (!activeSkillsList.Contains(skill) && skill.groupNumber != currentGroupNumber)
                    {
                        CheckIfSkillTriggers(triggeringSkill, triggeringSkillRate);

                        currentGroupNumber = skill.groupNumber;
                        triggeringSkill = skill.skillID;
                        triggeringSkillRate = skill.triggerRate;

                        CheckIfSkillTriggers(triggeringSkill, triggeringSkillRate);
                    }
                    //New skill, same group number.
                    else if (!activeSkillsList.Contains(skill) && skill.groupNumber == currentGroupNumber)
                    {
                        triggeringSkillRate += skill.triggerRate;
                        CheckIfSkillTriggers(triggeringSkill, triggeringSkillRate);
                    }
                }
                else {

                    //New skill, new group number.
                    if (!activeSkillsList.Contains(skill) && skill.groupNumber != currentGroupNumber)
                    {
                        CheckIfSkillTriggers(triggeringSkill, triggeringSkillRate);

                        currentGroupNumber = skill.groupNumber;
                        triggeringSkill = skill.skillID;
                        triggeringSkillRate = skill.triggerRate;
                    }
                    //New skill, same group number.
                    else if (!activeSkillsList.Contains(skill) && skill.groupNumber == currentGroupNumber)
                    {
                        triggeringSkillRate += skill.triggerRate;
                    }
                    else
                    {
                        CheckIfSkillTriggers(triggeringSkill, triggeringSkillRate);

                        currentGroupNumber = 0;
                        triggeringSkill = 0;
                        triggeringSkillRate = 0;
                    }
                }
            }
        }
    }

    public void NextSlottedAbility ()
    {
        if (GameControl.gameControl.currentProfile == 1)
        {
            int index = profile1SlottedSkills.IndexOf(selectedSkill);
            if (index == (profile1SlottedSkills.Count - 1))
            {
                index = 0;
            }
            else
            {
                index++;
            }
            selectedSkill = profile1SlottedSkills[index];
        }
        else
        {
            int index = profile2SlottedSkills.IndexOf(selectedSkill);
            if (index == profile2SlottedSkills.Count)
            {
                index = 1;
            }
            else
            {
                index++;
            }
            selectedSkill = profile2SlottedSkills[index];
        }
    }

    public void CallActivateAbility (int skillID)
    {
        StartCoroutine("ActivateAbility", skillID);
    }

    public void ActivateAbilityTest (int skillID)
    {
        if ((skillsDatabase.skills[skillID].requiredWeapon.ToString() == "None" ||
            skillsDatabase.skills[skillID].requiredWeapon.ToString() == EquipmentDatabase.equipmentDatabase.equipment[GameControl.gameControl.equippedWeapon].equipmentType.ToString()) &&
            cooldownList.ContainsKey(skillID) == false &&
            GameControl.gameControl.mp >= skillsDatabase.skills[skillID].manaCost)
        {
            activatingAbility = true;
            skillBeingActivated = skillID;
        }
        else
        {
            activatingAbility = false;
            GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().callingActivateAbility = false;
        }
    }

    public IEnumerator ActivateAbility (int skillID)
    {
        if (!skillsDatabase.skills[skillID].interuptable)
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().uninterupatble = true;
        }
        GameControl.gameControl.mp -= (int)skillsDatabase.skills[skillID].manaCost;
        activeSkills.Add(skillsDatabase.skills[skillID]);
        talliedSkills.Add(skillID, false);
        AmendStatsFromActiveSkills();
        cooldownList.Add(skillID, skillsDatabase.skills[skillID].cooldown);
        float duration = skillsDatabase.skills[skillID].skillDuration;
        if (duration == 0)
        {
            activeSkills.Remove(skillsDatabase.skills[skillID]);
            talliedSkills.Remove(skillID);
            ReduceModifiers(skillsDatabase.skills[skillID]);
        }
        else
        {
            yield return new WaitForSeconds(duration);
            activeSkills.Remove(skillsDatabase.skills[skillID]);
            talliedSkills.Remove(skillID);
            ReduceModifiers(skillsDatabase.skills[skillID]);
            GameObject.FindGameObjectWithTag("UserInterface").GetComponent<UserInterface>().activeBuffs.Remove(skillsDatabase.skills[skillID]);
        }
    }

    public void UpdateCoolDownList ()
    {
        for (int i = 0; i < cooldownList.Count; i++)
        {
            var key = cooldownList.ElementAt(i);
            int itemKey = key.Key;
            if (cooldownList[itemKey] <= 0)
            {
                cooldownList.Remove(itemKey);
            }
            else
            {
                cooldownList[itemKey] -= Time.deltaTime;
            }
        } 
    }

    public void FireProjectile ()
    {
        Instantiate(projectileList[selectedSkill.skillID], GameObject.FindGameObjectWithTag("Player").transform.position, Quaternion.identity);
    }
}