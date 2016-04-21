using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Demo_ClassSelection : MonoBehaviour {

	public Text className;
    public Text classDescription;
    public GameObject Soldier;

	void Start ()
    {
        EventSystem.current.SetSelectedGameObject(Soldier);
	}

    void Update ()
    {
        if (EventSystem.current.currentSelectedGameObject.name == "Soldier")
        {
            className.text = "Soldier";
            classDescription.text = "the   sturdy   fighter" + System.Environment.NewLine + 
                                    "good   at   bringing   the   fight   to   the   enemy" + System.Environment.NewLine + 
                                    "great   at   taking   a   hit";
        }
        else if (EventSystem.current.currentSelectedGameObject.name == "Ranger")
        {
            className.text = "Ranger";
            classDescription.text = "the   precise   archer" + System.Environment.NewLine +
                                    "best   shot   at   the   county   fair   three   years   running" + System.Environment.NewLine +
                                    "second   best   in   square   dancing";
        }
        else if (EventSystem.current.currentSelectedGameObject.name == "Paladin")
        {
            className.text = "Paladin";
            classDescription.text = "the   unwavering   soul" + System.Environment.NewLine +
                                    "charges   into   danger   without   a   second   thought" + System.Environment.NewLine +
                                    "first   to   disapear   during   chores";
        }
        else
        {
            className.text = "Wizard";
            classDescription.text = "the   brilliant   magician" + System.Environment.NewLine +
                                    "the   staff   at   the   royal   academy   never   liked   surpirses" + System.Environment.NewLine +
                                    "especially   when   the   roof   of   the   banquet   hall   went   missing   one   night...";
        }
    }

    public void SelectSoldier ()
    {
        //set gamecontrol stuff
    }

    public void SelectRanger ()
    {

    }

    public void SelectPaladin()
    {

    }

    public void SelectWizard()
    {

    }
}