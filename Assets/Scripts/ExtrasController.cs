using UnityEngine;
using UnityEngine.EventSystems;

public class ExtrasController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		GameControl gameControl = GameObject.FindObjectOfType<GameControl>();
		gameControl.mainMenuLevel = 3;
		
		EventSystem.current.SetSelectedGameObject(this.transform.GetChild(2).gameObject, null);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
