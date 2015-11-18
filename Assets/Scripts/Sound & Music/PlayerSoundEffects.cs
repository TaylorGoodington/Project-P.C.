using UnityEngine;
using System.Collections;

public class PlayerSoundEffects : MonoBehaviour {
	
	
	public AudioClip[] playerSoundEffects;
	private AudioSource audioSource;
	
	void Awake () {
		DontDestroyOnLoad (gameObject);
	}
	
	void Start () {
		audioSource = GetComponent<AudioSource>();
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
	
}
