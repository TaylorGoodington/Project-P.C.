using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class MainMenu : MonoBehaviour {

	// Use this for initialization
	void Start () {
		if (File.Exists(Application.persistentDataPath + "/playerInfo1.dat") || File.Exists(Application.persistentDataPath + "/playerInfo2.dat") || File.Exists(Application.persistentDataPath + "/playerInfo2.dat")) {
			EventSystem.current.SetSelectedGameObject(GameObject.FindGameObjectWithTag("Continue"));
		} else {
			EventSystem.current.SetSelectedGameObject(GameObject.FindGameObjectWithTag("New Game"));
		}
	}	
}
