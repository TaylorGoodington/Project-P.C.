using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class StartMenuControl : MonoBehaviour {

	public GameControl gameControl;
    Animator animator;

	// Use this for initialization
	void Start () {
        animator = GetComponent<Animator>();
		gameControl.mainMenuLevel = 0;
        animator.Play("Transition In");
	}
	
	public void StartGame () {
		PlayerSoundEffects sound = GameObject.FindGameObjectWithTag("Player Sound Effects").GetComponent<PlayerSoundEffects>();
		sound.PlaySoundEffect(sound.SoundEffectToArrayInt(PlayerSoundEffects.SoundEffect.MenuConfirm));
		SceneManager.LoadScene("Main Menu");
	}
	
	public void QuitGame () {
		PlayerSoundEffects sound = GameObject.FindGameObjectWithTag("Player Sound Effects").GetComponent<PlayerSoundEffects>();
		sound.PlaySoundEffect(sound.SoundEffectToArrayInt(PlayerSoundEffects.SoundEffect.MenuConfirm));
		Application.Quit();
	}

    public void SelectGameobject ()
    {
        EventSystem.current.SetSelectedGameObject(GameObject.Find("Start"), null);
        GameControl.gameControl.playerHasControl = true;
    }

    //Called by the animator
    public void FadeMusicIn()
    {
        MusicManager.musicManager.fadeMusicIn = true;
    }
}
