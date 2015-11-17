using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class StartMenuControl : MonoBehaviour {

	// Use this for initialization
	void Start () {
		EventSystem.current.SetSelectedGameObject(GameObject.Find("Start"),null);
	}
}
