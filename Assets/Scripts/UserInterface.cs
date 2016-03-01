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


    void Start()
    {
        healthBar = transform.GetChild(3).GetComponent<Slider>();
        healthText = healthBar.transform.GetChild(2).GetComponent<Text>();
        manaBar = transform.GetChild(4).GetComponent<Slider>();
        manaText = manaBar.transform.GetChild(2).GetComponent<Text>();
        //activeSkillIcon = transform.GetChild(3).transform.GetChild(1).GetComponent<Image>();
        activeSkillShade = transform.GetChild(2).transform.GetChild(3).GetComponent<Image>();
        activeSkillCooldownTimer = transform.GetChild(2).transform.GetChild(2).GetComponent<Text>();

        UpdateUIInfo();
    }

    void Update()
    {
        //this will move away at some point, its just for testing right now.....Maybe it can stay.
        UpdateUIInfo();
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

    public void CallBackToRegion ()
    {
        LevelManager.levelManager.BackToRegion();
    }
}
