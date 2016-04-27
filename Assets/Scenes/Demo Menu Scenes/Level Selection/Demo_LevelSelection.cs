using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Demo_LevelSelection : MonoBehaviour {

    public Text classDescription;
    public SpriteRenderer selectedThumbnail;
    public Sprite[] thumbnails;
    public GameObject firstSelected;

    void Start()
    {
        EventSystem.current.SetSelectedGameObject(firstSelected);
    }

    void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            //play transition back to start scene.
        }

        if (EventSystem.current.currentSelectedGameObject.name == "Level 01")
        {
            selectedThumbnail.sprite = thumbnails[0];
            classDescription.text = "the   intro   level" + System.Environment.NewLine +
                                    "good   for   learning   basic   controls   while   still   having   fun";
        }
        else if (EventSystem.current.currentSelectedGameObject.name == "Level 21")
        {
            selectedThumbnail.sprite = thumbnails[1];
            classDescription.text = "the   middle   of   the   road   level" + System.Environment.NewLine +
                                    "deal   with   new   mechanics   and   enemy   types   with   harder   platforming";
        }
        else if (EventSystem.current.currentSelectedGameObject.name == "Level 31")
        {
            selectedThumbnail.sprite = thumbnails[2];
            classDescription.text = "the   challenging   level" + System.Environment.NewLine +
                                    "dissolving   platforms   and   moving   hazards   ratchet   up   the   difficulty";
        }
        else
        {
            selectedThumbnail.sprite = thumbnails[3];
            classDescription.text = "the   witch   of   the   wood" + System.Environment.NewLine +
                                    "are   you   prepared   to   lose?";
        }
    }

    public void LoadLevel ()
    {
        string level = EventSystem.current.currentSelectedGameObject.name;
        LevelManager.levelManager.LoadLevel(level);
    }
}