using UnityEngine;
using System.Collections.Generic;

public class LevelScoresDatabase : MonoBehaviour {

    public List<LevelScores> levelScores;

    public static LevelScoresDatabase levelScoresDatabase;

    void Start()
    {
        levelScoresDatabase = GetComponent<LevelScoresDatabase>();

        levelScores.Add(new LevelScores(0, false, 0, 0));
    }
}
