using UnityEngine;

public class LogoScene : MonoBehaviour {

	void Start () {
	
	}

    public void LoadStartMenu ()
    {
        LevelManager.levelManager.LoadLevel("Start");
    }
}
