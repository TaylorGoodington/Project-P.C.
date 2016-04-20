using UnityEngine;
using UnityEngine.EventSystems;

public class StartAnimator : MonoBehaviour {

    public MainMenuControl startScript;

	public void MainMenu ()
    {
        startScript.MainMenuSceneStart();
    }

    //Called by the animator
    public void SelectGameobject ()
    {
        EventSystem.current.SetSelectedGameObject(GameObject.Find("Start"), null);
        GameControl.gameControl.playerHasControl = true;
    }
}
