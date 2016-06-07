using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class PauseMenu : MonoBehaviour {

    Dictionary<int, int> pauseMenuPath;
    int currentLevel;

    public GameObject pauseMenu;
    public GameObject[] level2;
    public GameObject[] level3;
    public GameObject[] level4;

    [HideInInspector] public string path;
    [HideInInspector] public string funnel;

    void Start () {
        pauseMenuPath = new Dictionary<int, int>();
        pauseMenuPath.Add(1, 1);
        pauseMenuPath.Add(2, 1);
        pauseMenuPath.Add(3, 1);
        currentLevel = 1;
	}

    void Update ()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            Back();
        }
    }
	
	public void Advance (int fork)
    {
        funnel = EventSystem.current.currentSelectedGameObject.name;
        if (currentLevel == 1)
        {
            pauseMenu.SetActive(false);
            level2[fork].SetActive(true);
            EventSystem.current.SetSelectedGameObject(level2[fork].transform.GetChild(0).gameObject);
            currentLevel = 2;

            if (fork == 1)
            {
                path = "Equipment";
            }
            else if (fork == 2)
            {
                path = "Skills";
            }
        }
        else if (currentLevel == 2)
        {
            level2[fork].SetActive(false);
            level3[fork].SetActive(true);
            EventSystem.current.SetSelectedGameObject(level3[fork].transform.GetChild(0).gameObject);
            currentLevel = 3;
        }
        else if (currentLevel == 3)
        {
            level3[fork].SetActive(false);
            level4[fork].SetActive(true);
            EventSystem.current.SetSelectedGameObject(level4[fork].transform.GetChild(0).gameObject);
            currentLevel = 4;
        }

        if (EventSystem.current.currentSelectedGameObject.transform.parent.gameObject.name == "Selections Menu")
        {
            transform.parent.GetComponent<SelectionMenu>().ReRunList();
        }

        pauseMenuPath[currentLevel] = fork;
    }

    public void Back ()
    {
        if (currentLevel == 4)
        {
            level4[pauseMenuPath[3]].SetActive(false);
            level3[pauseMenuPath[2]].SetActive(true);
            EventSystem.current.SetSelectedGameObject(level3[pauseMenuPath[3]].transform.GetChild(pauseMenuPath[3] - 1).gameObject);
            currentLevel = 3;
        }
        else if (currentLevel == 3)
        {
            level3[pauseMenuPath[2]].SetActive(false);
            level2[pauseMenuPath[1]].SetActive(true);
            EventSystem.current.SetSelectedGameObject(level2[pauseMenuPath[2]].transform.GetChild(pauseMenuPath[2] - 1).gameObject);
            currentLevel = 2;
        }
        else if (currentLevel == 2)
        {
            level2[pauseMenuPath[1]].SetActive(false);
            pauseMenu.SetActive(true);
            EventSystem.current.SetSelectedGameObject(pauseMenu.transform.GetChild(pauseMenuPath[1] - 1).gameObject);
            currentLevel = 1;
        }
        else if (currentLevel == 1)
        {
            GetComponent<Animator>().Play("Close");
        }
    }

    public void TurnOnPauseMenu ()
    {
        pauseMenu.SetActive(true);
    }

    public void MakeFirstSelection ()
    {
        GameObject equipment = GameObject.FindGameObjectWithTag("Pause Menu").transform.GetChild(2).transform.GetChild(0).gameObject;
        EventSystem.current.SetSelectedGameObject(equipment);
        GameObject equipment1 = GameObject.FindGameObjectWithTag("Pause Menu").transform.GetChild(2).transform.GetChild(1).gameObject;
        EventSystem.current.SetSelectedGameObject(equipment);
        EventSystem.current.SetSelectedGameObject(equipment);
    }

    public void CloseMenus ()
    {
        if (currentLevel == 4)
        {
            level4[pauseMenuPath[3]].SetActive(false);

        }
        else if (currentLevel == 3)
        {
            level3[pauseMenuPath[2]].SetActive(false);

        }
        else if (currentLevel == 2)
        {
            level2[pauseMenuPath[1]].SetActive(false);

        }
        else if (currentLevel == 1)
        {
            pauseMenu.SetActive(false);
        }
    }

    public void Destroy ()
    {
        Destroy(this.gameObject);

        if (LevelManager.levelManager.inMapScenes == true)
        {
            GameControl.gameControl.reSelectMapObject = true;
        }
    }

    public void PlayerHasControl ()
    {
        GameControl.gameControl.playerHasControl = true;
    }

    public void PlayerDoesntHaveControl ()
    {
        GameControl.gameControl.playerHasControl = false;
    }
}