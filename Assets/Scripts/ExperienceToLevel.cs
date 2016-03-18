using UnityEngine;
using System.Collections.Generic;

public class ExperienceToLevel : MonoBehaviour {

    public static ExperienceToLevel experienceToLevel;
    public List<LevelXP> levels;

	void Start () {
        experienceToLevel = GetComponent<ExperienceToLevel>();

        levels.Add(new LevelXP(0, 1));
        levels.Add(new LevelXP(1, 50));
        levels.Add(new LevelXP(2, 100));
        levels.Add(new LevelXP(3, 150));
        levels.Add(new LevelXP(4, 200));
        levels.Add(new LevelXP(5, 250));
        levels.Add(new LevelXP(6, 300));
        levels.Add(new LevelXP(7, 350));
        levels.Add(new LevelXP(8, 400));
        levels.Add(new LevelXP(9, 450));
        levels.Add(new LevelXP(10, 500));
        levels.Add(new LevelXP(11, 550));
        levels.Add(new LevelXP(12, 600));
        levels.Add(new LevelXP(13, 650));
        levels.Add(new LevelXP(14, 700));
        levels.Add(new LevelXP(15, 750));
        levels.Add(new LevelXP(16, 800));
        levels.Add(new LevelXP(17, 850));
        levels.Add(new LevelXP(18, 900));
        levels.Add(new LevelXP(19, 950));
        levels.Add(new LevelXP(20, 1000));
        levels.Add(new LevelXP(21, 1050));
        levels.Add(new LevelXP(22, 1100));
        levels.Add(new LevelXP(23, 1150));
        levels.Add(new LevelXP(24, 1200));
        levels.Add(new LevelXP(25, 1250));
        levels.Add(new LevelXP(26, 1300));
        levels.Add(new LevelXP(27, 1350));
        levels.Add(new LevelXP(28, 1400));
        levels.Add(new LevelXP(29, 1450));
        levels.Add(new LevelXP(30, 1500));
        levels.Add(new LevelXP(31, 1550));
        levels.Add(new LevelXP(32, 1600));
        levels.Add(new LevelXP(33, 1650));
        levels.Add(new LevelXP(34, 1700));
        levels.Add(new LevelXP(35, 1750));
        levels.Add(new LevelXP(36, 1800));
        levels.Add(new LevelXP(37, 1850));
        levels.Add(new LevelXP(38, 1900));
        levels.Add(new LevelXP(39, 1950));
        levels.Add(new LevelXP(40, 2000));
    }
}
