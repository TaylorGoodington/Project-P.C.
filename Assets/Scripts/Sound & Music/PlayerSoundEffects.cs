using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;


public class PlayerSoundEffects : MonoBehaviour {
	
	
	public AudioClip[] playerSoundEffects;
	private AudioSource audioSource;
	public string lastObjectName;
	private bool hasVerticalAxisReset;
	private bool hasHorizontalAxisReset;
	
	void Awake () {
		DontDestroyOnLoad (gameObject);
	}
	
	void Start () {
		audioSource = GetComponent<AudioSource>();
		lastObjectName = "Start";
		hasVerticalAxisReset = true;
		hasHorizontalAxisReset = true;
	}
	
	void Update () {
		if (Application.loadedLevel > 0) {
			HasDirectionalAxisReset ();
			
			//I will handle all naviagtion noises here so I don't double noise.
			if (GameControl.gameControl.AnyOpenMenus() == true) {
				if (Input.GetButtonDown("Cancel")) {
					PlaySoundEffect(SoundEffectToArrayInt(SoundEffect.MenuNavigation));
				}
				
				if (IsSelectedObjectDifferent() == true && HasDirectionalInputBeenReceived() == true) {
					hasVerticalAxisReset = false;
					hasHorizontalAxisReset = false;
					PlaySoundEffect(SoundEffectToArrayInt(SoundEffect.MenuNavigation));
				}
			}
		}	
	}

	//Partners with HasDirectionalInputBeenReceived
	void HasDirectionalAxisReset () {
		if (Input.GetAxisRaw ("Vertical") < 0.2 && Input.GetAxisRaw ("Vertical") > -0.2 && hasVerticalAxisReset == false) {
			hasVerticalAxisReset = true;
		}
		if (Input.GetAxisRaw ("Horizontal") < 0.2 && Input.GetAxisRaw ("Horizontal") > -0.2 && hasHorizontalAxisReset == false) {
			hasHorizontalAxisReset = true;
		}
	}
	
	//Strong way to test if there was joystick movement.
	public bool HasDirectionalInputBeenReceived () {
		if ((Input.GetButtonDown("Vertical") || ((Input.GetAxisRaw("Vertical") > 0.2 && hasVerticalAxisReset == true) || 
		                                         (Input.GetAxisRaw("Vertical") < -0.2 && hasVerticalAxisReset == true))) || 
		    (Input.GetButtonDown("Horizontal") || ((Input.GetAxisRaw("Horizontal") > 0.2 && hasHorizontalAxisReset == true) || 
												   (Input.GetAxisRaw("Horizontal") < -0.2 && hasHorizontalAxisReset == true)))) {
			return true;
		} else {
			return false;
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
