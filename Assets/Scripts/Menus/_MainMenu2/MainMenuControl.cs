using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class MainMenuControl : MonoBehaviour {

	// Use this for initialization
	void Start () {
		EventSystem.current.SetSelectedGameObject(GameObject.FindGameObjectWithTag("Game 1"),null);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	//TODO:
	//Write functions to for expanding the game slots (when you click they instantiate a second menu that appears below.
	//Write function that replaces "New Game" with appropriate info.
	//Wire up sounds.
	//write delete options function.
	//write copy options function.
}
