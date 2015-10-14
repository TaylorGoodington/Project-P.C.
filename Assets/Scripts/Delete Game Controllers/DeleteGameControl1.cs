using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class DeleteGameControl1 : MonoBehaviour {
	
	
	// Update is called once per frame
	void Update () {
		DeleteFile1Available ();
	}
	
	public void DeleteFile1Available () {
		if (File.Exists(Application.persistentDataPath + "/playerInfo1.dat")) {
			Button activeButton = this.GetComponent<Button>();
			activeButton.interactable = true;
			
			Text file1Text = this.GetComponent<Text>();
			file1Text.text = "something's here";
		} else {
			Button activeButton1 = this.GetComponent<Button>();
			activeButton1.interactable = false;
			
			Text file1Text = this.GetComponent<Text>();
			file1Text.text = "New Game 1";
		}
	}
}
