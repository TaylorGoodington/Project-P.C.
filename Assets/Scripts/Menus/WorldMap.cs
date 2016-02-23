using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

public class WorldMap : MonoBehaviour {

    Animator animator;

	void Start () {
        animator = transform.GetChild(2).GetComponent<Animator>();
        TransitionToWorldMap();
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
        animator.Play("Transition Out");
    }
}
