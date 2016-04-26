using UnityEngine;
using System.Collections.Generic;

public class SkillsDatabase : MonoBehaviour {

    public static SkillsDatabase skillsDatabase;
    public List<Skills> skills;

    void Start () {
        skillsDatabase = GetComponent<SkillsDatabase>();

        //skills.Add(new Skills (0, 0, "none", "none", Skills.RequiredStat.Agility, 0, Skills.RequiredWeapon.None, Skills.TriggerPhase.OnCast, Skills.AnimationType.Ability, 0, 0, false, 0, 0, 0, 0, 0));
        skills.Add(new Skills(0, 0, "Eldritch Blast", "Eldritch Blast", Skills.RequiredStat.Intelligence, 10, Skills.RequiredWeapon.None, Skills.TriggerPhase.OnCast, Skills.AnimationType.Ability, "Foreground", true, 10, 5, true, 0, 1f, 1.25f, 0f, 0, 0f, 0, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 15));
        skills.Add(new Skills(1, 1, "Strength Buff", "Strength Buff", Skills.RequiredStat.Strength, 10, Skills.RequiredWeapon.Sword, Skills.TriggerPhase.OnCast, Skills.AnimationType.Buff, "Background", false, 5, 30, true, 15, 1f, 1f, 0f, 0, 0f, 0, 0f, 2f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0));
        skills.Add(new Skills(2, 2, "Heal", "Heal", Skills.RequiredStat.Defense, 10, Skills.RequiredWeapon.None, Skills.TriggerPhase.OnCast, Skills.AnimationType.Buff, "Foreground", false, 10, 30, false, 0, 1f, 1f, 0f, 0, 0f, 0, 0f, 0f, 0f, 0f, 0f, 0f, 0.25f, 0f, 0f, 0f, 0f, 0f, 0));
        skills.Add(new Skills(3, 3, "Dash", "Dash", Skills.RequiredStat.Agility, 10, Skills.RequiredWeapon.None, Skills.TriggerPhase.OnCast, Skills.AnimationType.MovementAbility, "Background", false, 5, 15, true, 0, 1f, 1f, 0f, 0, 0f, 0, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 150));
    }
}