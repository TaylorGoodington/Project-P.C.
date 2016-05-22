using UnityEngine;
using UnityEngine.UI;

public class DeleteCounter : MonoBehaviour {

	private bool hasVerticalAxisReset;
	private Text numberOfItemsText;
	private int numberOfItems;
	private int maxItemQuantity;
	
	private ItemDatabase itemDatabase;
	//private EquipmentDatabase equipmentDatabase;
	private PlayerSoundEffects playerSoundEffects;
	
	// Use this for initialization
	void Start () {
		itemDatabase = GameObject.FindGameObjectWithTag("Items Database").GetComponent<ItemDatabase>();
		//equipmentDatabase = GameObject.FindGameObjectWithTag("Equipment Database").GetComponent<EquipmentDatabase>();
		playerSoundEffects = GameObject.FindObjectOfType<PlayerSoundEffects>();
	
		numberOfItems = 1;
		hasVerticalAxisReset = true;
		numberOfItemsText = this.GetComponent<Text>();
		numberOfItemsText.text = numberOfItems.ToString();
		
		//sets the max item quantity
		if (GameObject.FindGameObjectWithTag("Item Destroy Verification Canvas")) {
			maxItemQuantity = itemDatabase.items[PlayerPrefsManager.GetEquipmentID()].quantity;
		} else {
			//maxItemQuantity = equipmentDatabase.equipment[PlayerPrefsManager.GetEquipmentID()].quantity;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetAxisRaw("Vertical") < 0.2 && Input.GetAxisRaw("Vertical") > -0.2 && hasVerticalAxisReset == false) {
			hasVerticalAxisReset = true;
		}
	
		if (Input.GetKeyDown(KeyCode.UpArrow) || (Input.GetAxisRaw("Vertical") > 0.2 && hasVerticalAxisReset == true)) {
			if (numberOfItems < maxItemQuantity) {
				playerSoundEffects.PlaySoundEffect(playerSoundEffects.SoundEffectToArrayInt(PlayerSoundEffects.SoundEffect.MenuNavigation));
				numberOfItems ++;
				numberOfItemsText.text = numberOfItems.ToString();
				PlayerPrefsManager.SetQuantity(numberOfItems);
			} else {
				playerSoundEffects.PlaySoundEffect(playerSoundEffects.SoundEffectToArrayInt(PlayerSoundEffects.SoundEffect.MenuUnable));
				PlayerPrefsManager.SetQuantity(numberOfItems);
			}
			hasVerticalAxisReset = false;
		} else if (Input.GetKeyDown(KeyCode.DownArrow) || (Input.GetAxisRaw("Vertical") < -0.2 && hasVerticalAxisReset == true)) {
			if (numberOfItems > 1) {
				playerSoundEffects.PlaySoundEffect(playerSoundEffects.SoundEffectToArrayInt(PlayerSoundEffects.SoundEffect.MenuNavigation));
				numberOfItems --;
				numberOfItemsText.text = numberOfItems.ToString();
				PlayerPrefsManager.SetQuantity(numberOfItems);
			} else {
				playerSoundEffects.PlaySoundEffect(playerSoundEffects.SoundEffectToArrayInt(PlayerSoundEffects.SoundEffect.MenuUnable));
				PlayerPrefsManager.SetQuantity(numberOfItems);
			}
			hasVerticalAxisReset = false;
		}
	}
}
