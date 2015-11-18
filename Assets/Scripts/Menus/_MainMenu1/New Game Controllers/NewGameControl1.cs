//using UnityEngine;
//using System.Collections;
//using UnityEngine.UI;
//using System;
//using System.Runtime.Serialization.Formatters.Binary;
//using System.IO;
//
//public class NewGameControl1 : MonoBehaviour {
//
//
//
//	// Update is called once per frame
//	void Update () {
//		IsGameFile1Open ();
//	}
//	
//	public void IsGameFile1Open () {
//		if (File.Exists(Application.persistentDataPath + "/playerInfo1.dat")) {
//			Button activeButton1 = this.GetComponent<Button>();
//			activeButton1.onClick.RemoveAllListeners();
//				
//			Text file1Text = this.GetComponent<Text>();
//			file1Text.text = "something's here";
//		} else {
//			Button activeButton1 = this.GetComponent<Button>();
//			activeButton1.onClick.AddListener(() => GameObject.FindObjectOfType<GameControl>().NewGame1());
//				
//			Text file1Text = this.GetComponent<Text>();
//			file1Text.text = "New Game 1";
//		}
//	}
//}
