using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

public class WorldMap : MonoBehaviour {

    Animator animator;
    GameObject currentSelected;
    public GameObject quitDialogue;
    bool headingToTitleScene;

	void Start () {
        animator = GetComponent<Animator>();
        TransitionToWorldMap();
        headingToTitleScene = false;
	}

    void Update ()
    {
        if (GameControl.gameControl.reSelectMapObject == true)
        {
            EventSystem.current.SetSelectedGameObject(GameObject.Find(currentSelected.name.ToString()));
            GameControl.gameControl.reSelectMapObject = false;
        }

        if (!GameControl.gameControl.AnyOpenMenus() && quitDialogue.activeSelf == false)
        {
            currentSelected = EventSystem.current.currentSelectedGameObject;

            if (Input.GetButtonDown("Cancel")) {
                headingToTitleScene = true;
                OpenQuitDialogue();
            }
        }

        //Find some way to use the cancel button to close the quit dialogue.
        if (1 == 2)
        {
            if (Input.GetButtonDown("Cancel"))
            {
                headingToTitleScene = false;
                CloseQuitDialogue();
                Debug.Log("what");
            }
        }
    }

    //Call landing zone at the end of the transition animation.
    public void LandingZone ()
    {
        if (LevelManager.levelManager.lastRegionLoaded == 0)
        {
            EventSystem.current.SetSelectedGameObject(GameObject.FindGameObjectWithTag("The Pit"), null);
        }
        else if (LevelManager.levelManager.lastRegionLoaded == 1)
        {

        }
        EventSystem.current.SetSelectedGameObject(GameObject.FindGameObjectWithTag("The Pit"), null);
    }

    public void TransitionToWorldMap ()
    {
        animator.Play("Transition In");
    }

    public void TransitionFromWorldMap ()
    {
        quitDialogue.SetActive(false);
        animator.Play("Transition Out");
    }

    //needs to grab from the variable recording the selected object...
    public void SelectRegion ()
    {
        if (headingToTitleScene)
        {
            LevelManager.levelManager.LoadLevel("Start");
        }
        else
        {
            LevelManager.levelManager.LoadLevel(currentSelected.name.ToString());
        }
    }

    public void OpenQuitDialogue ()
    {
        quitDialogue.SetActive(true);
        EventSystem.current.SetSelectedGameObject(quitDialogue.transform.GetChild(4).gameObject);
    }

    public void CloseQuitDialogue ()
    {
        quitDialogue.SetActive(false);
        headingToTitleScene = false;
        EventSystem.current.SetSelectedGameObject(GameObject.Find(currentSelected.name.ToString()));
    }
}
