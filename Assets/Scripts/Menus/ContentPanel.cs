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
	
	public void DeleteInventory () {
		foreach (Transform child in transform) {
			if (child.gameObject.name == "Place Holder" || child.gameObject.tag == "Equipped Slot" ) {
			} else {
				Destroy (child.gameObject);
			}
		}
	}
}
