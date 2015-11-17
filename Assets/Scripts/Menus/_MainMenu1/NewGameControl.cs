using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class NewGameControl : MonoBehaviour {

	// Use this for initialization
	void Start () {
		if (!File.Exists(Application.persistentDataPath + "/playerInfo1.dat")) {
			EventSystem.current.SetSelectedGameObject(GameObject.Find("New Game 1"));
		} else if (!File.Exists(Application.persistentDataPath + "/playerInfo2.dat")) {
			EventSystem.current.SetSelectedGameObject(GameObject.Find("New Game 2"));
		} else if (!File.Exists(Application.persistentDataPath + "/playerInfo3.dat")) {
			EventSystem.current.SetSelectedGameObject(GameObject.Find("New Game 3"));
		}
	}
}
