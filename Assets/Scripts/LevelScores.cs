using UnityEngine;

[System.Serializable]
public class LevelScores {

    public int levelNumber;
    public bool hasLevelBeenPlayed;
    public float fastestLevelClearTime;
    public int enemiesDefeated;

    public LevelScores (int level, bool levelPlayed, float fastestTime, int enemies)
    {
        levelNumber = level;
        hasLevelBeenPlayed = levelPlayed;
        fastestLevelClearTime = fastestTime;
        enemiesDefeated = enemies;
    }

    public LevelScores ()
    {

    }
}
