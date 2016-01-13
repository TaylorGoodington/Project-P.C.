using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class SkillsController : MonoBehaviour {

    public static SkillsController skillsController;
    private SkillsDatabase skillsDatabase;
    public List<Skills> acquiredSkills;
    public List<Skills> activeSkills;

	void Start () {
        skillsController = GetComponent<SkillsController>();
        skillsDatabase = GetComponent<SkillsDatabase>();
	}
	
	void Update () {
	}

    //this is for sorting any list by group number....Not sure if it will work.
    static int SortByGroupNumber(Skills skill1, Skills skill2) {
        return skill1.groupNumber.CompareTo(skill2.groupNumber);
    }

    public void ActivateCurrentPhaseAbilities(Skills.TriggerPhase triggerPhase)
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
        int randomNumber = Random.Range(0, 100);
        if (randomNumber > triggerRate) {
            activeSkills.Add(skillsDatabase.skills[skillID]);
        }
    }

    public void ActivatePassiveAbility() { }

    //I think skills should have another key that allows me to add skills of the same together in the active skills list. So an example could be
    // dodge 1, 2, & 3 which have different names and different IDs, but the same group number. So the method will check if a skill in that group
    // exists and the trigger rates.

    //I need to have a few different damage methods. The first will be a damage that can be avoided, the second will be a damage that cant be avoided.
}
