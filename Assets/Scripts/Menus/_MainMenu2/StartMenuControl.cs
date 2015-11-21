using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class StartMenuControl : MonoBehaviour {

	// Use this for initialization
	void Start () {
		EventSystem.current.SetSelectedGameObject(GameObject.Find("Start"),null);
	}
	
	public void StartGame () {
		PlayerSoundEffects sound = GameObject.FindGameObjectWithTag("Player Sound Effects").GetComponent<PlayerSoundEffects>();
		sound.PlaySoundEffect(sound.SoundEffectToArrayInt(PlayerSoundEffects.SoundEffect.MenuConfirm));
		Application.LoadLevel("Main Menu");
	}
	
	public void QuitGame () {
		PlayerSoundEffects sound = GameObject.FindGameObjectWithTag("Player Sound Effects").GetComponent<PlayerSoundEffects>();
		sound.PlaySoundEffect(sound.SoundEffectToArrayInt(PlayerSoundEffects.SoundEffect.MenuConfirm));
		Application.Quit();
	}
}
