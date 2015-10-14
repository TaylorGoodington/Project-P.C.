using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class NewGameControl2 : MonoBehaviour {
	
	
	
	// Update is called once per frame
	void Update () {
		IsGameFile2Open ();
	}
	
	public void IsGameFile2Open () {
		if (File.Exists(Application.persistentDataPath + "/playerInfo2.dat")) {
			Button activeButton2 = this.GetComponent<Button>();
			activeButton2.interactable = false;
			
			Text file2Text = this.GetComponent<Text>();
			file2Text.text = "something's here";
		} else {
			Button activeButton2 = this.GetComponent<Button>();
			activeButton2.interactable = true;
			
			Text file2Text = this.GetComponent<Text>();
			file2Text.text = "New Game 2";
		}
	}
}
