using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class SkillsController : MonoBehaviour {

    public static SkillsController skillsController;
    private SkillsDatabase skillsDatabase;
    public List<Skills> acquiredSkills;
    public List<Skills> activeSkills;

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
            activeSkills.Add(skillsDatabase.skills[skillID]);
        }
    }

    public void ActivatePassiveAbility()
    {

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
}
