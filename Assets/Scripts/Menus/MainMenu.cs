using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MainMenu : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		// I could do an update based on an int, once i load the level i can set the update to set the selected object and switch the update off.
	}
	
	public void LoadMainMenu () {
		if (GameObject.FindGameObjectWithTag("Continue")) {
			EventSystem.current.SetSelectedGameObject(GameObject.FindGameObjectWithTag("Continue"));
		} else {
			EventSystem.current.SetSelectedGameObject(GameObject.FindGameObjectWithTag("New Game"));
		}
	}
		
}
