using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

public class RegionMap : MonoBehaviour {

    Animator animator;
    public GameObject currentSelected;
    bool headingToWorldMap;

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

        if (!GameControl.gameControl.AnyOpenMenus())
        {
            currentSelected = EventSystem.current.currentSelectedGameObject;

            if (Input.GetButtonDown("Cancel"))
            {
                headingToWorldMap = true;

                //There needs to be an are you sure you want to quit dialogue.
                TransitionFromRegionMap();
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
    }

    public void TransitionToRegionMap()
    {
        animator.Play("Transition In");
    }

    public void TransitionFromRegionMap()
    {
        animator.Play("Transition Out");
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
}
