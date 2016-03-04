using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour {

    public static MusicManager musicManager;

	public int currentTrack = 0;

	public AudioClip[] levelMusicChangeArray;
	private AudioSource audioSource;
    public bool endOfLevel;
    public bool fadeMusicOut;
    public bool fadeMusicIn;

    void Awake () {
		//DontDestroyOnLoad (gameObject);
	}
	
	void Start () {
		audioSource = GetComponent<AudioSource>();
        musicManager = GetComponent<MusicManager>();
        endOfLevel = false;
        fadeMusicOut = false;
        fadeMusicIn = false;

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

        if (fadeMusicOut)
        {
            FadeMusicOut();
        }

        if (fadeMusicIn)
        {
            FadeMusicIn();
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

    public void FadeMusicIn ()
    {
        float currentVolume = PlayerPrefsManager.GetMasterMusicVolume();
        float fadeTime = .75f;
        bool fadeIn = true;
        if (fadeIn)
        {
            audioSource.volume += (currentVolume / fadeTime) * Time.deltaTime;
        }

        if (audioSource.volume >= currentVolume)
        {
            audioSource.volume = currentVolume;
            fadeIn = false;
            fadeMusicIn = false;
        }
    }

    public void FadeMusicOut()
    {
        float currentVolume = PlayerPrefsManager.GetMasterMusicVolume();
        float fadeOutTime = .75f;
        bool fadeOut = true;
        if (fadeOut)
        {
            audioSource.volume -= (currentVolume / fadeOutTime) * Time.deltaTime;
        }

        if (audioSource.volume <= 0)
        {
            fadeOut = false;
            fadeMusicOut = false;
        }
    }

    public int LevelTrack () {
		if (SceneManager.GetActiveScene().name == "Start" || SceneManager.GetActiveScene().name == "Main Menu" ||
            SceneManager.GetActiveScene().name == "01b Options" || SceneManager.GetActiveScene().name == "Extras") {
		return 1;
		} else if (SceneManager.GetActiveScene().name == "Level 01" || SceneManager.GetActiveScene().name == "Test Level 2") {
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
        else if (SceneManager.GetActiveScene().name == "World Map")
        {
            return 6;
        }
        else {
			return 0;
		}
	}
	
	public void ChangeVolume (float volume) {
		audioSource.volume = volume;
	}
}
