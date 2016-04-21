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
    public Skills selectedSkill;
    public bool activatingAbility;

    void Start() {
        skillsController = GetComponent<SkillsController>();
        skillsDatabase = GetComponent<SkillsDatabase>();
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
            ActivateAbility(skillID);
            
        }
    }

    public void PassiveAbilityBonuses ()
    {
        //Look through the aquired skills list for Passives.
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
            if (index == profile1SlottedSkills.Count)
            {
                index = 1;
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

    public IEnumerator ActivateAbility (int skillID)
    {
        activeSkills.Add(skillsDatabase.skills[skillID]);
        activatingAbility = true;
        //play animation on player.
        float duration = skillsDatabase.skills[skillID].skillDuration;
        yield return new WaitForSeconds(duration);
        activeSkills.Remove(skillsDatabase.skills[skillID]);
    }
}