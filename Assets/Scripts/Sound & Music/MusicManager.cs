using UnityEngine;
using System.Collections;

public class MusicManager : MonoBehaviour {

	public int currentTrack = 0;

	public AudioClip[] levelMusicChangeArray;
	private AudioSource audioSource;

	void Awake () {
		DontDestroyOnLoad (gameObject);
	}
	
	void Start () {
		audioSource = GetComponent<AudioSource>();
	}
	
	void Update () {
		if (LevelTrack() != currentTrack) {
			currentTrack = LevelTrack();
			PlayMusic(currentTrack);
		}
	}
	
	public void PlayMusic (int track) {
		AudioClip thisLevelMusic = levelMusicChangeArray[track];
		if (thisLevelMusic) {
			audioSource.clip = thisLevelMusic;
			audioSource.loop = true;
			audioSource.Play();
		}
		
	}
	
	public int LevelTrack () {
		if (Application.loadedLevelName == "Start" || Application.loadedLevelName == "Main Menu" || 
		    Application.loadedLevelName == "01b Options" || Application.loadedLevelName == "Extras") {
		return 1;
		} else if (Application.loadedLevelName == "Test Level" || Application.loadedLevelName == "Test Level 2") {
			return 2;
		} else {
			return 0;
		}
	}
	
	public void ChangeVolume (float volume) {
		audioSource.volume = volume;
	}
	
}
