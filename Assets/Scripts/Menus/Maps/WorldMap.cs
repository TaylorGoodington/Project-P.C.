using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

public class WorldMap : MonoBehaviour {

    Animator animator;
    public GameObject currentSelected;
    public GameObject quitDialogue;
    bool headingToTitleScene;

	void Start () {
        animator = GetComponent<Animator>();
        TransitionToWorldMap();
        headingToTitleScene = false;
	}

    void Update ()
    {
        //Find some way to use the cancel button to close the quit dialogue.
        if (quitDialogue.activeSelf == true)
        {
            if (Input.GetButtonDown("Cancel"))
            {
                headingToTitleScene = false;
                CloseQuitDialogue();
                Debug.Log("what");
            }
        }

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
    }

    //Called by the animator
    public void FadeMusicOut ()
    {
        MusicManager.musicManager.fadeMusicOut = true;
    }

    //Called by the animator
    public void FadeMusicIn ()
    {
        MusicManager.musicManager.fadeMusicIn = true;
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
            EventSystem.current.SetSelectedGameObject(GameObject.FindGameObjectWithTag("Region 1"), null);
        }
        else if (LevelManager.levelManager.lastRegionLoaded == 2)
        {
            EventSystem.current.SetSelectedGameObject(GameObject.FindGameObjectWithTag("Region 2"), null);
        }
        else if (LevelManager.levelManager.lastRegionLoaded == 3)
        {
            EventSystem.current.SetSelectedGameObject(GameObject.FindGameObjectWithTag("Region 3"), null);
        }
        else if (LevelManager.levelManager.lastRegionLoaded == 4)
        {
            EventSystem.current.SetSelectedGameObject(GameObject.FindGameObjectWithTag("Region 4"), null);
        }
        else if (LevelManager.levelManager.lastRegionLoaded == 5)
        {
            EventSystem.current.SetSelectedGameObject(GameObject.FindGameObjectWithTag("Region 5"), null);
        }
        else if (LevelManager.levelManager.lastRegionLoaded == 6)
        {
            EventSystem.current.SetSelectedGameObject(GameObject.FindGameObjectWithTag("Region 6"), null);
        }
        else if (LevelManager.levelManager.lastRegionLoaded == 7)
        {
            EventSystem.current.SetSelectedGameObject(GameObject.FindGameObjectWithTag("Region 7"), null);
        }
        else if (LevelManager.levelManager.lastRegionLoaded == 8)
        {
            EventSystem.current.SetSelectedGameObject(GameObject.FindGameObjectWithTag("Region 8"), null);
        }
        else if (LevelManager.levelManager.lastRegionLoaded == 9)
        {
            EventSystem.current.SetSelectedGameObject(GameObject.FindGameObjectWithTag("Region 9"), null);
        }
        else if (LevelManager.levelManager.lastRegionLoaded == 10)
        {
            EventSystem.current.SetSelectedGameObject(GameObject.FindGameObjectWithTag("Region 10"), null);
        }
        GameControl.gameControl.playerHasControl = true;
    }

    public void TransitionToWorldMap ()
    {
        animator.Play("Transition In");
    }

    public void TransitionFromWorldMap ()
    {
        GameControl.gameControl.playerHasControl = false;
        quitDialogue.SetActive(false);
        animator.Play("Transition Out");
    }

    //Called from the end of the transition animation.
    public void SelectRegion ()
    {
        if (headingToTitleScene)
        {
            GameControl.gameControl.Save();
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
