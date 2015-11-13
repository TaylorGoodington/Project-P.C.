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
