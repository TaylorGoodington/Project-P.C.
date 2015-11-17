using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class PauseMenu : MonoBehaviour {

	public GameObject itemMenu;
	public GameObject equipmentMenu;
	public GameObject skillsMenu;
	public GameObject playerOptionsMenu;
	
	//item inventory script access.
	public GameObject itemInventory;
	
	// equipment inventory script access.
	public GameObject equipmentInventory;
	
	// Use this for initialization
	void Start () {
		EventSystem.current.SetSelectedGameObject(GameObject.FindGameObjectWithTag("Equipment Menu"),null);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void OpenEquipmentMenu () {
		PlayerSoundEffects sound = GameObject.FindGameObjectWithTag("Player Sound Effects").GetComponent<PlayerSoundEffects>();
		sound.PlaySoundEffect(sound.SoundEffectToArrayInt(PlayerSoundEffects.SoundEffect.MenuNavigation));
		
		Destroy (gameObject);
		equipmentInventory.GetComponent<EquipmentInventory>().OpenEquipmentBaseMenu();
	}
	
	public void OpenItemMenu () {
		PlayerSoundEffects sound = GameObject.FindGameObjectWithTag("Player Sound Effects").GetComponent<PlayerSoundEffects>();
		sound.PlaySoundEffect(sound.SoundEffectToArrayInt(PlayerSoundEffects.SoundEffect.MenuNavigation));
		itemInventory.GetComponent<Inventory>().OpenItemMenu();
		Destroy (gameObject);
	}
	
	

}
