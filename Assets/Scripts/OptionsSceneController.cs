using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class OptionsSceneController : MonoBehaviour {

	public Slider musicVolumeSlider;
	public Slider soundEffectsSlider;
	
	private MusicManager musicManager;
	private PlayerSoundEffects playerSoundEffects;
	private LevelManager levelManager;
	

	void Start () {
        musicManager = MusicManager.musicManager;
		playerSoundEffects = PlayerSoundEffects.playerSoundEffects;
		levelManager = LevelManager.levelManager;
		EventSystem.current.SetSelectedGameObject(GameObject.FindGameObjectWithTag("Music Volume Slider"), null);
		
		musicVolumeSlider = transform.GetChild (3).GetComponent<Slider>();
		musicVolumeSlider.value = PlayerPrefsManager.GetMasterMusicVolume();
		soundEffectsSlider = transform.GetChild (5).GetComponent<Slider>();
		soundEffectsSlider.value = PlayerPrefsManager.GetMasterEffectsVolume();
		
		GameControl.gameControl.mainMenuLevel = 3;
	}
	
	void Update () {
		musicManager.ChangeVolume (musicVolumeSlider.value);
		playerSoundEffects.ChangeVolume(soundEffectsSlider.value);
	}
	
	public void SaveAndExit () {
		PlayerPrefsManager.SetMasterMusicVolume (musicVolumeSlider.value);
		PlayerPrefsManager.SetMasterEffectsVolume (soundEffectsSlider.value);
		levelManager.LoadLevel ("Start");
	}
	
	public void SetDefaults () {
		musicVolumeSlider.value = 0.25f;
		soundEffectsSlider.value = 0.18f;
	}
	
}
