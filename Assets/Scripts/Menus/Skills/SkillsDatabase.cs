using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SkillsDatabase : MonoBehaviour {

    public static SkillsDatabase skillsDatabase;
    public List<Skills> skills;

    void Start () {
        skillsDatabase = GetComponent<SkillsDatabase>();

        skills.Add(new Skills (0, 0, "none", "none", Skills.RequiredStat.Agility, 0, Skills.RequiredWeapon.None, Skills.AbilityType.Active, Skills.TriggerPhase.OnCast, 0, 0, false, 0, 0, 0, 0, 0));
    }
}
