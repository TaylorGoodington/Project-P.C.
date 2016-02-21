using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour {

    public static MusicManager musicManager;

	public int currentTrack = 0;

	public AudioClip[] levelMusicChangeArray;
	private AudioSource audioSource;
    public bool endOfLevel;

	void Awake () {
		//DontDestroyOnLoad (gameObject);
	}
	
	void Start () {
		audioSource = GetComponent<AudioSource>();
        musicManager = GetComponent<MusicManager>();
        endOfLevel = false;
	}
	
	void Update () {
		if (LevelTrack() != currentTrack) {
			currentTrack = LevelTrack();
			PlayMusic(currentTrack);
		}

        if (endOfLevel)
        {
            LevelVictoryMusic();
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

    public void LevelVictoryMusic ()
    {
        endOfLevel = true;
        float currentVolume = PlayerPrefsManager.GetMasterMusicVolume();
        float fadeOutTime = .75f;
        bool fadeOut = true;
        bool playVictory = false;
        if(fadeOut)
        {
            audioSource.volume -= (currentVolume / fadeOutTime) * Time.deltaTime;
            Debug.Log("fade");
        }

        if (audioSource.volume <= 0)
        {
            fadeOut = false;
            playVictory = true;
        }

        if (playVictory)
        {
            audioSource.volume = currentVolume;
            audioSource.clip = levelMusicChangeArray[5];
            audioSource.loop = false;
            audioSource.Play();
            playVictory = false;
            endOfLevel = false;
        }
    }
	
	public int LevelTrack () {
		if (SceneManager.GetActiveScene().name == "Start" || SceneManager.GetActiveScene().name == "Main Menu" ||
            SceneManager.GetActiveScene().name == "01b Options" || SceneManager.GetActiveScene().name == "Extras") {
		return 1;
		} else if (SceneManager.GetActiveScene().name == "Level 1-1" || SceneManager.GetActiveScene().name == "Test Level 2") {
			return 2;
        }
        else if (SceneManager.GetActiveScene().name == "Level 3-1")
        {
            return 3;
        }
        else if (SceneManager.GetActiveScene().name == "Level 4-1")
        {
            return 4;
        }
        else {
			return 0;
		}
	}
	
	public void ChangeVolume (float volume) {
		audioSource.volume = volume;
	}
	
}
