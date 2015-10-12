using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class Continue : MonoBehaviour {

	

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(File.Exists(Application.persistentDataPath + "/playerInfo1.dat") || File.Exists(Application.persistentDataPath + "/playerInfo2.dat") || File.Exists(Application.persistentDataPath + "/playerInfo2.dat")) {
			Button continueButton = GetComponent<Button>();
			continueButton.interactable = true;
			gameObject.SetActive(true);
		} else {
			Button continueButton = GetComponent<Button>();
			continueButton.interactable = false;
			gameObject.SetActive(false);
		}
	}
}
