using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour {

	public GameObject itemMenu;
	public GameObject equipmentMenu;
	public GameObject skillsMenu;
	public GameObject playerOptionsMenu;
	
	//item inventory script access.
	public GameObject itemInventory;
	
	// equipment inventory script access.
	public GameObject equipmentInventory;

    private GameObject exitObject;
    private Text exitText;
	
	void Start () {
		EventSystem.current.SetSelectedGameObject(GameObject.FindGameObjectWithTag("Equipment Menu"),null);
        //ASSIGN EXIT OBJECT
	}

    void Update ()
    {
        //adjust exitText based on where the pause menu is being opened from.
    }
	
	public void OpenEquipmentMenu () {
		PlayerSoundEffects sound = GameObject.FindGameObjectWithTag("Player Sound Effects").GetComponent<PlayerSoundEffects>();
		sound.PlaySoundEffect(sound.SoundEffectToArrayInt(PlayerSoundEffects.SoundEffect.MenuNavigation));
		
		Destroy (gameObject);
		//equipmentInventory.GetComponent<EquipmentInventory>().OpenEquipmentBaseMenu();
	}
	
	public void OpenItemMenu () {
		PlayerSoundEffects sound = GameObject.FindGameObjectWithTag("Player Sound Effects").GetComponent<PlayerSoundEffects>();
		sound.PlaySoundEffect(sound.SoundEffectToArrayInt(PlayerSoundEffects.SoundEffect.MenuNavigation));
		itemInventory.GetComponent<Inventory>().OpenItemMenu();
		Destroy (gameObject);
	}

    public void AdjustExitFunctionality ()
    {
        //Change listeners to have the correct exit button.
    }

    public void OpenExitDialogue ()
    {
        //Turn on exit object
        //set new selected object
        //need to do something with pause menu levels to make sure back works.
        AdjustExitFunctionality();
    }

    public void CloseExitDialogue ()
    {
        //turn off exit object
        //set exit to selected object
        //need to do something with pause menu levels to make sure back works.
    }

    //Calls navigation sound effect from PlayerSoundEffects.
    public void PlayNavigationSound ()
    {
        PlayerSoundEffects.playerSoundEffects.PlayNavigationSound();
    }
}