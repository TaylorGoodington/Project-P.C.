using UnityEngine;
using System.Collections;

public class LevelsGained : MonoBehaviour {

    public static LevelsGained levelsGained;
    Animator animator;

    void Start()
    {
        levelsGained = GetComponent<LevelsGained>();
        animator = GetComponent<Animator>();
    }

    public void LevelsGainedAnimation()
    {
        animator.Play("LevelUp!");
    }

    public void PlayeLevelUpNoise()
    {
        PlayerSoundEffects.playerSoundEffects.PlaySoundEffect(2);
    }
}
