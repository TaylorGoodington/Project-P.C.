using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UserInterface : MonoBehaviour {

    private Slider healthBar;
    private Text healthText;

    private Slider manaBar;
    private Text manaText;

    //private Image activeSkillIcon;
    public Text activeSkillCooldownTimer;
    public Image activeSkillShade;

    public Animator animator;

    public Text timeText;
    public Text enemiesDefeatedText;
    public Text bonusXPText;


    void Start()
    {
        healthBar = transform.GetChild(3).GetComponent<Slider>();
        healthText = healthBar.transform.GetChild(2).GetComponent<Text>();
        manaBar = transform.GetChild(4).GetComponent<Slider>();
        manaText = manaBar.transform.GetChild(2).GetComponent<Text>();
        //activeSkillIcon = transform.GetChild(3).transform.GetChild(1).GetComponent<Image>();
        activeSkillShade = transform.GetChild(2).transform.GetChild(3).GetComponent<Image>();
        activeSkillCooldownTimer = transform.GetChild(2).transform.GetChild(2).GetComponent<Text>();

        animator = GetComponent<Animator>();

        UpdateUIInfo();
    }

    void Update()
    {
        //this will move away at some point, its just for testing right now.....Maybe it can stay.
        UpdateUIInfo();

        //Level Timer.
        if (GameControl.gameControl.playerHasControl)
        {
            RunTheClock();
        }
    }

    public void UpdateUIInfo()
    {
        manaText.text = GameControl.gameControl.mp + " / " + GameControl.gameControl.currentMana;
        healthText.text = GameControl.gameControl.hp + " / " + GameControl.gameControl.currentHealth;


        //Mana Bar
        if (GameControl.gameControl.mp == 0)
        {
            manaBar.value = 0;
        }
        else
        {
            manaBar.value = GameControl.gameControl.currentMana / GameControl.gameControl.mp;
        }

        //Health Bar
        if (GameControl.gameControl.hp == 0)
        {
            healthBar.value = 0;
        }
        else
        {
            healthBar.value = GameControl.gameControl.currentHealth / GameControl.gameControl.hp;
        }
    }

    public void EndOfLevel ()
    {
        GameControl.gameControl.playerHasControl = false;
        animator.Play("EndOfLevel");

        float minutesOriginal = Mathf.Floor(LevelManager.levelManager.levelTime / 60);
        string minutes = Mathf.Floor(LevelManager.levelManager.levelTime / 60).ToString("00");
        float secondsOriginal = Mathf.Floor(LevelManager.levelManager.levelTime % 60);
        string seconds = Mathf.Floor(LevelManager.levelManager.levelTime % 60).ToString("00");
        string milliSeconds = Mathf.Floor((LevelManager.levelManager.levelTime - (minutesOriginal + secondsOriginal)) * 1000).ToString("000");

        timeText.text = minutes + ":" + seconds + "." + milliSeconds;
        enemiesDefeatedText.text = LevelManager.levelManager.enemiesDefeated.ToString();
        bonusXPText.text = (LevelManager.levelManager.lastLevelPlayed - LevelManager.levelManager.level01 + 1).ToString();

        //Level Clear Time.
        if (GameControl.gameControl.levelScores[LevelManager.levelManager.lastLevelPlayed - LevelManager.levelManager.level01 + 1].fastestLevelClearTime == 0)
        {
            GameControl.gameControl.levelScores[LevelManager.levelManager.lastLevelPlayed - LevelManager.levelManager.level01 + 1].fastestLevelClearTime = LevelManager.levelManager.levelTime;
        }
        else if (GameControl.gameControl.levelScores[LevelManager.levelManager.lastLevelPlayed - LevelManager.levelManager.level01 + 1].fastestLevelClearTime > LevelManager.levelManager.levelTime)
        {
            GameControl.gameControl.levelScores[LevelManager.levelManager.lastLevelPlayed - LevelManager.levelManager.level01 + 1].fastestLevelClearTime = LevelManager.levelManager.levelTime;
        }

        //Enemies Defeated.
        if (GameControl.gameControl.levelScores[LevelManager.levelManager.lastLevelPlayed - LevelManager.levelManager.level01 + 1].enemiesDefeated < LevelManager.levelManager.enemiesDefeated)
        {
            GameControl.gameControl.levelScores[LevelManager.levelManager.lastLevelPlayed - LevelManager.levelManager.level01 + 1].enemiesDefeated = LevelManager.levelManager.enemiesDefeated;
        }
    }

    //Used by the end of level animation to play Victory Music.
    public void LevelVictoryMusic ()
    {
        MusicManager.musicManager.LevelVictoryMusic();
    }

    //Used by the end of level animation to load the Region Map.
    public void BackToRegion ()
    {
        LevelManager.levelManager.BackToRegion();
    }

    //Called by the animator
    public void SetCameraTarget ()
    {
        CameraFollow.cameraFollow.UpdateTarget();
    }

    //Called by the animator
    public void FadeMusicOut()
    {
        MusicManager.musicManager.fadeMusicOut = true;
    }

    //Called by the animator
    public void FadeMusicIn()
    {
        MusicManager.musicManager.fadeMusicIn = true;
    }

    //Called from the end of the level intro animation.
    public void GiveThePlayerControl ()
    {
        GameControl.gameControl.playerHasControl = true;
    }

    //used in update to keep track of time.
    public void RunTheClock ()
    {
        LevelManager.levelManager.levelTime += Time.deltaTime;
    }
}