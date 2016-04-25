using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UserInterface : MonoBehaviour {

    //HUD
    public Slider healthBar;
    public Text healthText;
    public Slider manaBar;
    public Text manaText;
    public Image activeSkillIcon;
    public Text activeSkillCooldownTimer;
    public Image activeSkillShade;
    public Image interactionIndicator;
    public Image[] buffIcons;
    public List<Skills> activeBuffs;

    //Level End Components
    public Text timeText;
    public Text enemiesDefeatedText;
    public Text bonusXPText;
    
    //Combat Related
    public Text xpGained;
    public Image equipmentGainedIcon;
    public Text equipmentGainedText;
    public Text levelUpCount;

    [HideInInspector]
    public List<Equipment> receivedEquipment;
    [HideInInspector]
    public List<Items> receivedItems;
    [HideInInspector]
    public bool tallyingSpoils;
    private Animator animator;
    [HideInInspector]
    public bool showInteractableDisplay;
    

    void Start()
    {
        animator = GetComponent<Animator>();
        tallyingSpoils = false;
        showInteractableDisplay = false;
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

        if (GameControl.gameControl.xp >= GameControl.gameControl.xpToLevel)
        {
            LevelUp();
        }
        
        if (showInteractableDisplay)
        {
            interactionIndicator.enabled = true;
        }
        else
        {
            interactionIndicator.enabled = false;
        }
    }

    public void UpdateUIInfo()
    {
        manaText.text = GameControl.gameControl.mp + " / " + GameControl.gameControl.currentMana * 10;
        healthText.text = GameControl.gameControl.hp + " / " + GameControl.gameControl.currentHealth * 10;

        if (SkillsController.skillsController.selectedSkill != null)
        {
            activeSkillIcon.sprite = Resources.Load<Sprite>("Ability Icons/" + SkillsController.skillsController.selectedSkill.skillName);
        }

        //Skill Cooldown Shade & Timer
        if (SkillsController.skillsController.cooldownList.ContainsKey(SkillsController.skillsController.selectedSkill.skillID))
        {
            activeSkillShade.gameObject.SetActive(true);
            activeSkillCooldownTimer.gameObject.SetActive(true);
            int skillID = SkillsController.skillsController.selectedSkill.skillID;
            activeSkillCooldownTimer.text = Mathf.FloorToInt(SkillsController.skillsController.cooldownList[skillID]).ToString();
        }
        else
        {
            activeSkillShade.gameObject.SetActive(false);
            activeSkillCooldownTimer.gameObject.SetActive(false);
        }

        SetBuffIcons();

        //Mana Bar
        if (GameControl.gameControl.mp == 0)
        {
            manaBar.value = 0;
        }
        else
        {
            manaBar.value = (GameControl.gameControl.mp * 1f) / (GameControl.gameControl.currentMana * 10 * 1f);
        }

        //Health Bar
        if (GameControl.gameControl.hp == 0)
        {
            healthBar.value = 0;
        }
        else
        {
            healthBar.value = (GameControl.gameControl.hp * 1f) / (GameControl.gameControl.currentHealth * 10 * 1f);
        }
    }

    public void SetBuffIcons ()
    {
        //Sets up the active buffs list.
        foreach (Skills skill in SkillsController.skillsController.activeSkills)
        {
            if (skill.animationType == Skills.AnimationType.Buff)
            {
                if (!activeBuffs.Contains(skill) && skill.skillDuration > 0)
                {
                    activeBuffs.Add(skill);
                }
            }
        }

        foreach (var slot in buffIcons)
        {
            int index = System.Array.IndexOf(buffIcons, slot);
            if (activeBuffs.Count > 0)
            {
                if (index <= activeBuffs.Count - 1)
                {
                    buffIcons[index].sprite = Resources.Load<Sprite>("Ability Icons/" + activeBuffs[index].skillName);
                }
                else
                {
                    buffIcons[index].sprite = Resources.Load<Sprite>("Ability Icons/None");
                }
            }
            else
            {
                buffIcons[index].sprite = Resources.Load<Sprite>("Ability Icons/None");
            }
        }
    }

    public void EndOfLevel ()
    {
        SkillsController.skillsController.LevelEndAbilityListCleaning();
        GameControl.gameControl.playerHasControl = false;
        if (SceneManager.GetActiveScene().name == "The Pit" || SceneManager.GetActiveScene().name == "The Pit Intro")
        {
            animator.Play("PitEndOfLevel");
        }
        else
        {
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
    }

    //Used by the end of level animation to reset bool in player.
    public void ResetPlayerEndOfLevel ()
    {
        GameControl.gameControl.endOfLevel = false;
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

    public void ToWorldMap ()
    {
        LevelManager.levelManager.lastRegionLoaded = 0;
        SceneManager.LoadScene("World Map");
    }

    //Called by the animator
    public void SetCameraTarget ()
    {
        //GameObject player = GameObject.FindGameObjectWithTag("Player");
        GameObject camera = GameObject.FindGameObjectWithTag("MainCamera");
        CameraFollow.cameraFollow.UpdateTarget();
        camera.transform.position = new Vector3 (200, 100, -10);
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

    //Called when enemies are defeated.
    public void ReceiveXP (int xp)
    {
        if (GameControl.gameControl.playerLevel < GameControl.gameControl.levelCap)
        {
            xpGained.text = "+" + xp + " xp";
            XPGained.xpGained.XPGainedAnimation();
        }
    }

    public void CallReceiveEquipment()
    {
        StartCoroutine(ReceiveEquipment());
    }

    //Called when enemies are defeated.
    public IEnumerator ReceiveEquipment ()
    {
        foreach (Equipment equipment in receivedEquipment)
        {
            equipmentGainedIcon.sprite = Resources.Load<Sprite>("Equipment Icons/" + EquipmentDatabase.equipmentDatabase.equipment[equipment.equipmentID].equipmentName);
            equipmentGainedText.text = EquipmentDatabase.equipmentDatabase.equipment[equipment.equipmentID].equipmentName;
            EquipmentGained.equipmentGained.EquipmentGainedAnimation();
            if (GameControl.gameControl.equipmentInventoryList.Contains(EquipmentDatabase.equipmentDatabase.equipment[equipment.equipmentID]))
            {
                int index = GameControl.gameControl.equipmentInventoryList.IndexOf(EquipmentDatabase.equipmentDatabase.equipment[equipment.equipmentID]);
                GameControl.gameControl.equipmentInventoryList[index].quantity++;
            }
            else {
                GameControl.gameControl.equipmentInventoryList.Add(EquipmentDatabase.equipmentDatabase.equipment[equipment.equipmentID]);
            }
            yield return new WaitForSeconds(1.1f);
        }

        foreach (Items item in receivedItems)
        {
            equipmentGainedIcon.sprite = Resources.Load<Sprite>("Item Icons/" + ItemDatabase.itemDatabase.items[item.itemID].itemName);
            equipmentGainedText.text = ItemDatabase.itemDatabase.items[item.itemID].itemName;
            EquipmentGained.equipmentGained.EquipmentGainedAnimation();
            if (GameControl.gameControl.itemInventoryList.Contains(ItemDatabase.itemDatabase.items[item.itemID]))
            {
                int index = GameControl.gameControl.itemInventoryList.IndexOf(ItemDatabase.itemDatabase.items[item.itemID]);
                GameControl.gameControl.itemInventoryList[index].quantity++;
            }
            else {
                GameControl.gameControl.itemInventoryList.Add(ItemDatabase.itemDatabase.items[item.itemID]);
            }
            yield return new WaitForSeconds(1.1f);
        }

        GameObject.FindGameObjectWithTag("UserInterface").GetComponent<UserInterface>().receivedEquipment.Clear();
        GameObject.FindGameObjectWithTag("UserInterface").GetComponent<UserInterface>().receivedItems.Clear();
        tallyingSpoils = false;
    }

    public void LevelUp ()
    {
        if (GameControl.gameControl.playerLevel < GameControl.gameControl.levelCap) {
            int remainder = GameControl.gameControl.xp - GameControl.gameControl.xpToLevel;
            GameControl.gameControl.xp = remainder;
            GameControl.gameControl.playerLevel++;
            GameControl.gameControl.LevelUpStats();
            GameControl.gameControl.xpToLevel = ExperienceToLevel.experienceToLevel.levels[GameControl.gameControl.playerLevel].experienceToLevel;
            int levelUpCounter = 1;

            while (remainder >= ExperienceToLevel.experienceToLevel.levels[GameControl.gameControl.playerLevel].experienceToLevel)
            {
                remainder = GameControl.gameControl.xp - GameControl.gameControl.xpToLevel;
                GameControl.gameControl.xp = remainder;
                GameControl.gameControl.playerLevel++;
                GameControl.gameControl.LevelUpStats();
                levelUpCounter++;
                GameControl.gameControl.xpToLevel = ExperienceToLevel.experienceToLevel.levels[GameControl.gameControl.playerLevel].experienceToLevel;
            }
            levelUpCount.text = "+ " + levelUpCounter + " Level!";
            LevelsGained.levelsGained.LevelsGainedAnimation();
        }
    }

    public void DeathToPit ()
    {
        LevelManager.levelManager.LoadLevel("The Pit");
    }
}