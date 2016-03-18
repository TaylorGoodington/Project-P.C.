using UnityEngine;
using System.Collections;

[System.Serializable]
public class LevelXP {

    public int levelID;
    public int experienceToLevel;

    public LevelXP (int iD, int exp)
    {
        levelID = iD;
        experienceToLevel = exp;
    }
}
