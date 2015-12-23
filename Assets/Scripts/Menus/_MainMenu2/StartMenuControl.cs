using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class StartMenuControl : MonoBehaviour {

	public GameControl gameControl;

	// Use this for initialization
	void Start () {
		EventSystem.current.SetSelectedGameObject(GameObject.Find("Start"),null);
		gameControl.mainMenuLevel = 0;
	}
	
	public void StartGame () {
		PlayerSoundEffects sound = GameObject.FindGameObjectWithTag("Player Sound Effects").GetComponent<PlayerSoundEffects>();
		sound.PlaySoundEffect(sound.SoundEffectToArrayInt(PlayerSoundEffects.SoundEffect.MenuConfirm));
		SceneManager.LoadScene("Main Menu");
	}
	
	public void QuitGame () {
		PlayerSoundEffects sound = GameObject.FindGameObjectWithTag("Player Sound Effects").GetComponent<PlayerSoundEffects>();
		sound.PlaySoundEffect(sound.SoundEffectToArrayInt(PlayerSoundEffects.SoundEffect.MenuConfirm));
		Application.Quit();
	}
}
