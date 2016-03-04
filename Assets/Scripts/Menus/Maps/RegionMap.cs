using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

public class RegionMap : MonoBehaviour {

    Animator animator;
    public GameObject currentSelected;
    public GameObject quitDialogue;
    public bool headingToWorldMap;

    void Start()
    {
        animator = GetComponent<Animator>();
        TransitionToRegionMap();
        headingToWorldMap = false;
    }

    void Update()
    {
        if (GameControl.gameControl.reSelectMapObject == true)
        {
            EventSystem.current.SetSelectedGameObject(GameObject.Find(currentSelected.name.ToString()), null);
            GameControl.gameControl.reSelectMapObject = false;
        }

        if (!GameControl.gameControl.AnyOpenMenus() && quitDialogue.activeSelf == false)
        {
            currentSelected = EventSystem.current.currentSelectedGameObject;

            if (Input.GetButtonDown("Cancel"))
            {
                OpenQuitDialogue();
                headingToWorldMap = true;
            }
        }
    }

    //Call landing zone at the end of the transition animation.
    public void LandingZone()
    {
        if (LevelManager.levelManager.lastRegionLoaded == 0)
        {
            EventSystem.current.SetSelectedGameObject(GameObject.Find("Level 01"), null);
        }
        else if (LevelManager.levelManager.lastRegionLoaded == 1)
        {

        }
        EventSystem.current.SetSelectedGameObject(GameObject.Find("Level 01"), null);
        GameControl.gameControl.playerHasControl = true;
    }

    public void TransitionToRegionMap()
    {
        animator.Play("Transition In");
    }

    public void TransitionFromRegionMap()
    {
        GameControl.gameControl.playerHasControl = false;
        CloseQuitDialogue();
        animator.Play("Transition Out");
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

    public void SelectLevel()
    {
        if (headingToWorldMap)
        {
            LevelManager.levelManager.LoadLevel("World Map");
        }
        else
        {
            LevelManager.levelManager.LoadLevel(currentSelected.name.ToString());

        }
    }

    public void OpenQuitDialogue()
    {
        quitDialogue.SetActive(true);
        headingToWorldMap = false;
        EventSystem.current.SetSelectedGameObject(quitDialogue.transform.GetChild(4).gameObject);
    }

    public void CloseQuitDialogue()
    {
        quitDialogue.SetActive(false);
        EventSystem.current.SetSelectedGameObject(GameObject.Find(currentSelected.name.ToString()));
    }
}
