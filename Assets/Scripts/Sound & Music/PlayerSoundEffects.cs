using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;


public class PlayerSoundEffects : MonoBehaviour {
	
	
	public AudioClip[] playerSoundEffects;
	private AudioSource audioSource;
	public string lastObjectName;
	
	void Awake () {
		DontDestroyOnLoad (gameObject);
	}
	
	void Start () {
		audioSource = GetComponent<AudioSource>();
		lastObjectName = "Start";
	}
	
	void Update () {
	// Feeling good about this now.
		if (Application.loadedLevel > 0) {
			if (IsSelectedObjectDifferent() == true && ((Input.GetButtonDown("Horizontal") || Input.GetAxis("Horizontal") != 0) || 
														(Input.GetButtonDown("Vertical") || Input.GetAxis("Vertical") != 0)) && 
														 Input.GetButtonDown("Submit") == false) {
				PlaySoundEffect(SoundEffectToArrayInt(SoundEffect.MenuNavigation));
			}
		}
	}
	
	public enum SoundEffect {
		MenuNavigation,
		MenuConfirm,
		MenuUnable
	}
	
	//The int returned matches the array number in the player sound effect Game Object.
	public int SoundEffectToArrayInt (SoundEffect effect) {
		if (effect == SoundEffect.MenuNavigation) {
			return 1;
		} else if (effect == SoundEffect.MenuConfirm) {
			return 2;
		} else if (effect == SoundEffect.MenuUnable) {
			return 3;
		}
		return 0;
	}
	
	public void PlaySoundEffect (int sound) {
		AudioClip soundEffect = playerSoundEffects[sound];
		audioSource = GetComponent<AudioSource>();
		if (soundEffect) {
			audioSource.clip = soundEffect;
			audioSource.loop = false;
			audioSource.Play();
		}
	}
	
	public void ChangeVolume (float volume) {
		audioSource.volume = volume;
	}
	
	
	public bool IsSelectedObjectDifferent () {
		if (lastObjectName == EventSystem.current.currentSelectedGameObject.name) {
			return false;
		} else {
			lastObjectName = EventSystem.current.currentSelectedGameObject.name;
			return true;
		}
	}
	
}
