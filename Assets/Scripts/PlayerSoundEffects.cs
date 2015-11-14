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
			return 0;
		} else if (effect == SoundEffect.MenuConfirm) {
			return 1;
		} else if (effect == SoundEffect.MenuUnable) {
			return 2;
		}
		return 10;
	}
	
	public void PlaySoundEffect (int sound) {
		AudioClip soundEffect = playerSoundEffects[sound];
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
