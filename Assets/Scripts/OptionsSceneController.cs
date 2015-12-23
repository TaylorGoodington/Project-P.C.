using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class OptionsSceneController : MonoBehaviour {

	public Slider musicVolumeSlider;
	public Slider soundEffectsSlider;
	
	private MusicManager musicManager;
	private PlayerSoundEffects playerSoundEffects;
	private LevelManager levelManager;
	

	// Use this for initialization
	void Start () {
		musicManager = FindObjectOfType<MusicManager>();
		playerSoundEffects = FindObjectOfType<PlayerSoundEffects>();
		levelManager = FindObjectOfType<LevelManager>();
		EventSystem.current.SetSelectedGameObject(GameObject.FindGameObjectWithTag("Music Volume Slider"), null);
		
		musicVolumeSlider = transform.GetChild (3).GetComponent<Slider>();
		musicVolumeSlider.value = PlayerPrefsManager.GetMasterMusicVolume();
		soundEffectsSlider = transform.GetChild (5).GetComponent<Slider>();
		soundEffectsSlider.value = PlayerPrefsManager.GetMasterEffectsVolume();
		
		GameControl gameControl = GameObject.FindObjectOfType<GameControl>();
		gameControl.mainMenuLevel = 3;
	}
	
	// Update is called once per frame
	void Update () {
		musicManager.ChangeVolume (musicVolumeSlider.value);
		playerSoundEffects.ChangeVolume(soundEffectsSlider.value);
	}
	
	public void SaveAndExit () {
		PlayerPrefsManager.SetMasterMusicVolume (musicVolumeSlider.value);
		PlayerPrefsManager.SetMasterEffectsVolume (soundEffectsSlider.value);
		levelManager.LoadLevel ("Main Menu");
	}
	
	public void SetDefaults () {
		musicVolumeSlider.value = 0.25f;
		soundEffectsSlider.value = 0.18f;
	}
	
}
