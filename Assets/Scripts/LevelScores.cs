using UnityEngine;

[System.Serializable]
public class LevelScores {

    public string levelName;
    public bool hasLevelBeenPlayed;
    public float fastestLevelClearTime;
    public int enemiesDefeated;

    public LevelScores (string level, bool levelPlayed, float fastestTime, int enemies)
    {
        levelName = level;
        hasLevelBeenPlayed = levelPlayed;
        fastestLevelClearTime = fastestTime;
        enemiesDefeated = enemies;
    }

    public LevelScores ()
    {

    }
}
