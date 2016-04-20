using UnityEngine;
using UnityEngine.EventSystems;

public class MainMenuAnimator : MonoBehaviour {

    public MainMenuControl startScript;

    public void ToStartScene ()
    {
        startScript.StartSceneStart();
    }

    public void ToOptions ()
    {

    }

    public void ToExtras ()
    {

    }

    //Called by the animator
    public void SelectGameobject ()
    {
        EventSystem.current.SetSelectedGameObject(GameObject.FindGameObjectWithTag("Game 1"), null);
        GameControl.gameControl.playerHasControl = true;
    }
}