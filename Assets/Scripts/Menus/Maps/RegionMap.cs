using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

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

    //Call landing zone at the end of the transition in animation.
    public void LandingZone()
    {
        int lastLevelPlayed = (LevelManager.levelManager.lastLevelPlayed != 0) ? LevelManager.levelManager.lastLevelPlayed - LevelManager.levelManager.level01 + 1 : 1;
        string lastLevel = (lastLevelPlayed < 10)? "Level 0" + lastLevelPlayed : "Level " + lastLevelPlayed;
        
        if (SceneManager.GetActiveScene().name == "Region 1")
        {
            if (lastLevelPlayed >= 1 && lastLevelPlayed <= 10)
            {
                EventSystem.current.SetSelectedGameObject(GameObject.Find(lastLevel), null);
            }
            else
            {
                EventSystem.current.SetSelectedGameObject(GameObject.Find("Level 01"), null);
            }
        }
        else if (SceneManager.GetActiveScene().name == "Region 2")
        {
            if (lastLevelPlayed >= 11 && lastLevelPlayed <= 20)
            {
                EventSystem.current.SetSelectedGameObject(GameObject.Find(lastLevel), null);
            }
            else
            {
                EventSystem.current.SetSelectedGameObject(GameObject.Find("Level 11"), null);
            }
        }
        else if (SceneManager.GetActiveScene().name == "Region 3")
        {
            if (lastLevelPlayed >= 21 && lastLevelPlayed <= 30)
            {
                EventSystem.current.SetSelectedGameObject(GameObject.Find(lastLevel), null);
            }
            else
            {
                EventSystem.current.SetSelectedGameObject(GameObject.Find("Level 21"), null);
            }
        }
        else if (SceneManager.GetActiveScene().name == "Region 4")
        {
            if (lastLevelPlayed >= 31 && lastLevelPlayed <= 40)
            {
                EventSystem.current.SetSelectedGameObject(GameObject.Find(lastLevel), null);
            }
            else
            {
                EventSystem.current.SetSelectedGameObject(GameObject.Find("Level 31"), null);
            }
        }
        else if (SceneManager.GetActiveScene().name == "Region 5")
        {
            if (lastLevelPlayed >= 41 && lastLevelPlayed <= 50)
            {
                EventSystem.current.SetSelectedGameObject(GameObject.Find(lastLevel), null);
            }
            else
            {
                EventSystem.current.SetSelectedGameObject(GameObject.Find("Level 41"), null);
            }
        }
        else if (SceneManager.GetActiveScene().name == "Region 6")
        {
            if (lastLevelPlayed >= 51 && lastLevelPlayed <= 60)
            {
                EventSystem.current.SetSelectedGameObject(GameObject.Find(lastLevel), null);
            }
            else
            {
                EventSystem.current.SetSelectedGameObject(GameObject.Find("Level 51"), null);
            }
        }
        else if (SceneManager.GetActiveScene().name == "Region 7")
        {
            if (lastLevelPlayed >= 61 && lastLevelPlayed <= 70)
            {
                EventSystem.current.SetSelectedGameObject(GameObject.Find(lastLevel), null);
            }
            else
            {
                EventSystem.current.SetSelectedGameObject(GameObject.Find("Level 61"), null);
            }
        }
        else if (SceneManager.GetActiveScene().name == "Region 8")
        {
            if (lastLevelPlayed >= 71 && lastLevelPlayed <= 80)
            {
                EventSystem.current.SetSelectedGameObject(GameObject.Find(lastLevel), null);
            }
            else
            {
                EventSystem.current.SetSelectedGameObject(GameObject.Find("Level 71"), null);
            }
        }
        else if (SceneManager.GetActiveScene().name == "Region 9")
        {
            if (lastLevelPlayed >= 81 && lastLevelPlayed <= 90)
            {
                EventSystem.current.SetSelectedGameObject(GameObject.Find(lastLevel), null);
            }
            else
            {
                EventSystem.current.SetSelectedGameObject(GameObject.Find("Level 81"), null);
            }
        }
        else if (SceneManager.GetActiveScene().name == "Region 10")
        {
            if (lastLevelPlayed >= 91 && lastLevelPlayed <= 100)
            {
                EventSystem.current.SetSelectedGameObject(GameObject.Find(lastLevel), null);
            }
            else
            {
                EventSystem.current.SetSelectedGameObject(GameObject.Find("Level 91"), null);
            }
        }

        GameControl.gameControl.playerHasControl = true;
    }

    //Called by the animator
    public void TransitionToRegionMap()
    {
        animator.Play("Transition In");
    }

    //Called by the animator
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

    //Called by the animator
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
