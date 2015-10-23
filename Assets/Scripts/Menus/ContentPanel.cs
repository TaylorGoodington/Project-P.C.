using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ContentPanel : MonoBehaviour {

	public void DeactivateInventory () {
		foreach (Transform child in transform) {
			child.GetComponent<Button>().interactable = false;
		}
	}
	
	public void ActivateInventory () {
		foreach (Transform child in transform) {
			child.GetComponent<Button>().interactable = true;
		}
	}
	
	public void RefreshInventory () {
		foreach (Transform child in transform) {
			if (child.gameObject.name == "Place Holder") {
			} else {
				Destroy (child.gameObject);
			}
		}
	}
}
