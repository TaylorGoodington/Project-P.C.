using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SkillsDatabase : MonoBehaviour {

    public List<Skills> skills;

    void Start () {
        skills.Add(new Skills (0, "none", "none", Skills.RequiredStat.Agility, 0, Skills.RequiredWeapon.None, Skills.AbilityType.Active, 0, 0, false, 0, 0));
    }
}
