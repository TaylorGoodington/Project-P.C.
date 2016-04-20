using UnityEngine;
using UnityEngine.EventSystems;

public class ExtrasController : MonoBehaviour {

	void Start () {
        GameControl.gameControl.mainMenuLevel = 3;
		
		EventSystem.current.SetSelectedGameObject(this.transform.GetChild(2).gameObject, null);
	}
}
