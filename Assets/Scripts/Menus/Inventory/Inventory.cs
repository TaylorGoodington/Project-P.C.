using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Inventory : MonoBehaviour {

    public static Inventory inventory;

	//access to gamecontrol, muy importante.
	private GameControl gameControl;

	//items that populate the inventory menu.
	public GameObject inventoryItem;
	
	//used to instantiate item use verification windows.
	public GameObject itemInventoryUse;
	public GameObject itemInventoryDestroy;
	
	//Item Inventory Menu
	public GameObject itemMenu;
	
	//access the selected gameobject.
	private GameObject selectedItem;
	
	//gets access to the info items.
	private Text displayItemName;
	private Image displayItemIcon;
	private Text displayItemDescription;
	private Text displayItemType;
	
	//this can be private at some point.
	public List<Items> inventoryList;

    public List<Items> consumableItems;
	
	//used to add items from the item database.
	private ItemDatabase itemDatabase;
	
	private int inventorySlots = 20;
	
	//used to turn off all interactability of item buttons in the content panel.
	private ContentPanel contentPanel;
	
	private Button useItemButton;
	private Button destroyItemButton;
	
	private GameObject useItemVerificationCanvas;
	private GameObject destroyItemVerificationCanvas;
	
	
	void Awake () {
		//DontDestroyOnLoad (gameObject);
	}
	
	void Start () {
        inventory = GetComponent<Inventory>();
		gameControl = GameObject.FindObjectOfType<GameControl>();
		itemDatabase = GameObject.FindGameObjectWithTag("Items Database").GetComponent<ItemDatabase>();
		inventoryList = gameControl.itemInventoryList;
	}
	
	void Update () {
		if (GameObject.FindGameObjectWithTag("Item Menu")) {
			GetItemInfo ();
		}
	}

    ////I need to have this run if active item = 0, should be in this script.
    //public void CycleActiveItems(float direction) {
    //    //I think i will have to make a new list of just the consumable items, I will add them to the new list in the foreach loop.
    //    foreach (Items item in GameControl.gameControl.itemInventoryList) {
    //        if (item.itemType == Items.ItemType.Consumable) {
    //            consumableItems.Add(item);
    //        }
    //    }
        
    //    int consumableItemsLength = consumableItems.Count;
    //    //This will run if there isn't an acitve item.
    //    if (GameControl.gameControl.activeItem == 0 && consumableItemsLength > 0)
    //    {
    //        GameControl.gameControl.activeItem = consumableItems[0].itemID;
    //    }
    //    //this runs if there is an active item.
    //    else if (GameControl.gameControl.activeItem != 0)
    //    {
    //        if (consumableItemsLength == 0)
    //        {
    //            Debug.Log("0");
    //        }
    //        if (direction == 1)
    //        {
    //            if (consumableItems.IndexOf(GameControl.gameControl.itemInventoryList[GameControl.gameControl.activeItem]) == 1)
    //            {

    //            }
    //        }
    //        Debug.Log(direction);
    //    }
    //}
	
	public void OpenItemMenu () {
		gameControl = GameObject.FindObjectOfType<GameControl>();
		Instantiate (itemMenu);
		PopulateInventory ();
		ItemInfoDisplay ();
		gameControl.itemsMenuLevel = 1;
	}
	
	public void OpenPreviousWeaponMenu (int itemMenuLevel) {
		PlayerSoundEffects sound = GameObject.FindGameObjectWithTag("Player Sound Effects").GetComponent<PlayerSoundEffects>();
		sound.PlaySoundEffect(sound.SoundEffectToArrayInt(PlayerSoundEffects.SoundEffect.MenuNavigation));
		gameControl = GameObject.FindObjectOfType<GameControl>();
		if (itemMenuLevel == 3) {
			if (GameObject.FindGameObjectWithTag("Item Destroy Verification Canvas")) {
				sound.PlaySoundEffect(sound.SoundEffectToArrayInt(PlayerSoundEffects.SoundEffect.MenuNavigation));
				gameControl = GameObject.FindObjectOfType<GameControl>();
				destroyItemVerificationCanvas = GameObject.FindGameObjectWithTag("Item Destroy Verification Canvas");
				Destroy (destroyItemVerificationCanvas.gameObject);
				useItemButton = GameObject.FindGameObjectWithTag("Use Item Button").GetComponent<Button>();
				useItemButton.interactable = true;
				
				destroyItemButton = GameObject.FindGameObjectWithTag("Destroy Item Button").GetComponent<Button>();
				destroyItemButton.interactable = true;
				EventSystem.current.SetSelectedGameObject(GameObject.FindGameObjectWithTag("Use Item Button"),null);
			} else {
				sound.PlaySoundEffect(sound.SoundEffectToArrayInt(PlayerSoundEffects.SoundEffect.MenuNavigation));
				gameControl = GameObject.FindObjectOfType<GameControl>();
				useItemVerificationCanvas = GameObject.FindGameObjectWithTag("Use Item Verification Canvas");
				Destroy (useItemVerificationCanvas.gameObject);
				useItemButton = GameObject.FindGameObjectWithTag("Use Item Button").GetComponent<Button>();
				useItemButton.interactable = true;
				
				destroyItemButton = GameObject.FindGameObjectWithTag("Destroy Item Button").GetComponent<Button>();
				destroyItemButton.interactable = true;
				EventSystem.current.SetSelectedGameObject(GameObject.FindGameObjectWithTag("Use Item Button"),null);	
			}
		} else if (itemMenuLevel == 2) {
			useItemButton = GameObject.FindGameObjectWithTag("Use Item Button").GetComponent<Button>();
			useItemButton.interactable = false;
			
			destroyItemButton = GameObject.FindGameObjectWithTag("Destroy Item Button").GetComponent<Button>();
			destroyItemButton.interactable = false;
			contentPanel = GameObject.FindGameObjectWithTag ("Inventory Content").GetComponent<ContentPanel>();
			contentPanel.ActivateInventory();
			int itemIndex = PlayerPrefsManager.GetSelectItem ();
			GameObject inventoryContentPanel = GameObject.FindGameObjectWithTag ("Inventory Content");
			GameObject lastSelected = inventoryContentPanel.transform.GetChild(itemIndex + 1).gameObject;
			EventSystem.current.SetSelectedGameObject(lastSelected,null);
		} else if (itemMenuLevel == 1) {
			Destroy (GameObject.FindGameObjectWithTag("Item Menu"));
			gameControl.OpenPauseMenu();
			GameControl.gameControl.pauseMenuLevel = 1;
		}
	}

	void ItemInfoDisplay () {
		displayItemName = GameObject.FindGameObjectWithTag ("Item Display Name").GetComponent<Text> ();
		displayItemIcon = GameObject.FindGameObjectWithTag ("Item Display Icon").GetComponent<Image> ();
		displayItemDescription = GameObject.FindGameObjectWithTag ("Item Display Description").GetComponent<Text> ();
		displayItemType = GameObject.FindGameObjectWithTag ("Item Display Type").GetComponent<Text> ();
	}

	public void AddTempData () {
		AddItemsToInventory(2, 1, 2, 2, 2, 1);
	}

	void PopulateInventory () {
		gameControl = GameObject.FindObjectOfType<GameControl>();
		//FUGGIN MACK DADDY SETS THE CURRENT SELECTED OBJECT...WILL USE THIS TO CONTROL DIRECTION.
		EventSystem.current.SetSelectedGameObject(GameObject.FindGameObjectWithTag("Item Inventory Place Holder"),null);
		int localIndexCount = 1;
		int inventoryCount = 0; //this works for now but i have my doubts...
		foreach (var item in gameControl.itemInventoryList) {
			GameObject newItem = Instantiate (inventoryItem) as GameObject;
			Text newItemText = newItem.GetComponentInChildren<Text>();
			newItemText.text = item.itemName;
			newItem.transform.SetParent (GameObject.FindGameObjectWithTag ("Inventory Content").transform, false);
			newItem.gameObject.name = item.itemName;
			
			//Puts the item ID as the text field for the second child of the new item button.
			GameObject newItemIDObject = newItem.transform.GetChild(1).gameObject;
			Text newItemID = newItemIDObject.GetComponent<Text>();
			newItemID.text = item.itemID.ToString();
	
			
			//Puts the inventory index as the text field for the third child of the new item button.
			GameObject newItemIndexObject = newItem.transform.GetChild(2).gameObject;
			Text newItemIndex = newItemIndexObject.GetComponent<Text>();
			int inventoryCountIndex = inventoryCount;
			newItemIndex.text = inventoryCountIndex.ToString();
			inventoryCount ++;
			
			//Puts the level of the weapon in the fourth child.
			GameObject newItemQuantityObject = newItem.transform.GetChild(3).gameObject;
			Text newItemQuantity = newItemQuantityObject.GetComponent<Text>();
			newItemQuantity.text = item.quantity.ToString();
			
			//puts the local index in the fifth child.
			localIndexCount ++;
			GameObject newItemLocalIndex = newItem.transform.GetChild(4).gameObject;
			Text itemLocalIndex = newItemLocalIndex.GetComponent<Text>();
			itemLocalIndex.text = localIndexCount.ToString();
		}
	}

	
	public void GetItemInfo () {
		ItemInfoDisplay();
		selectedItem = EventSystem.current.currentSelectedGameObject;
		if (selectedItem.name == "Place Holder") {
			displayItemName.text = "";
			displayItemDescription.text = "";
			displayItemType.text = "";
			displayItemIcon.color = Color.black;
		} else if (selectedItem.name == "Use Item") {
		
		} else if (selectedItem.name == "Destroy Item") {
			
		} else if (selectedItem.name == "Use Item Yes") {
			
		} else if (selectedItem.name == "Use Item No") {
		
		} else if (selectedItem.name == "Destroy Item Yes") {
			
		} else if (selectedItem.name == "Destroy Item No") {
			
		} else {
			displayItemName.text = selectedItem.name;
		
			//Gets object number as string and converts to int.
			GameObject newItemIDObject = selectedItem.transform.GetChild(1).gameObject;
			Text newItemIDText = newItemIDObject.GetComponent<Text>();
			int newItemID = int.Parse(newItemIDText.text);
			PlayerPrefsManager.SetEquipmentID(newItemID);
			
			//Gets inventory index as string and converts to int, then pushes to playerprefsmanager.
			GameObject newItemIndexObject = selectedItem.transform.GetChild(2).gameObject;
			Text newItemIndexText = newItemIndexObject.GetComponent<Text>();
			int newItemIndex = int.Parse(newItemIndexText.text);
			PlayerPrefsManager.SetSelectItem(newItemIndex); //I think i use playerprefsmanger too much...consider just using local variables.
				
			displayItemDescription.text = itemDatabase.items [newItemID].itemDescription;
			displayItemIcon.color = Color.white;
			displayItemIcon.sprite = itemDatabase.items [newItemID].itemIcon;
			displayItemType.text = itemDatabase.items [newItemID].itemType.ToString();
		}
	}
	
	
	//Have item drops call this as needed.
	public void AddItemsToInventory (params int[] itemNumber) {
		gameControl = GameObject.FindObjectOfType<GameControl>();
		itemDatabase = GameObject.FindGameObjectWithTag("Items Database").GetComponent<ItemDatabase>();
		if (gameControl.itemInventoryList.Count == inventorySlots) {
			return;
			//present message saying out of space? Might need to be more complicated...
		} else {
			foreach (int item in itemNumber) {
				if (gameControl.itemInventoryList.Contains(itemDatabase.items[item])) {
					int index = gameControl.itemInventoryList.IndexOf(itemDatabase.items[item]);
					gameControl.itemInventoryList[index].quantity ++;
				} else {
					gameControl.itemInventoryList.Add (itemDatabase.items[item]);
				}
			}
		}
	}

	
	public void SelectItem () {
		PlayerSoundEffects sound = GameObject.FindGameObjectWithTag("Player Sound Effects").GetComponent<PlayerSoundEffects>();
        //This section was commented out because at 1/3/2016 we no longer have usable items.
		//sound.PlaySoundEffect(sound.SoundEffectToArrayInt(PlayerSoundEffects.SoundEffect.MenuNavigation));
		//gameControl = GameObject.FindObjectOfType<GameControl>();
		//gameControl.itemsMenuLevel = 2;
		//useItemButton = GameObject.FindGameObjectWithTag("Use Item Button").GetComponent<Button>();
		//useItemButton.interactable = true;
		
		//destroyItemButton = GameObject.FindGameObjectWithTag("Destroy Item Button").GetComponent<Button>();
		//destroyItemButton.interactable = true;
		
		//contentPanel = GameObject.FindObjectOfType<ContentPanel>();
		//contentPanel.DeactivateInventory();	
		//EventSystem.current.SetSelectedGameObject(GameObject.FindGameObjectWithTag("Use Item Button"),null);
	}
	
	public void UseItemInInventory () {
		PlayerSoundEffects sound = GameObject.FindGameObjectWithTag("Player Sound Effects").GetComponent<PlayerSoundEffects>();
		sound.PlaySoundEffect(sound.SoundEffectToArrayInt(PlayerSoundEffects.SoundEffect.MenuNavigation));
		gameControl = GameObject.FindObjectOfType<GameControl>();
		gameControl.itemsMenuLevel = 3;
		useItemButton = GameObject.FindGameObjectWithTag("Use Item Button").GetComponent<Button>();
		useItemButton.interactable = false;
		
		destroyItemButton = GameObject.FindGameObjectWithTag("Destroy Item Button").GetComponent<Button>();
		destroyItemButton.interactable = false;
		
		GameObject.Instantiate(itemInventoryUse);
		
		EventSystem.current.SetSelectedGameObject(GameObject.FindGameObjectWithTag("Use Item Yes"),null);
	}

	public void UseItemYes () {
		PlayerSoundEffects sound = GameObject.FindGameObjectWithTag("Player Sound Effects").GetComponent<PlayerSoundEffects>();
		sound.PlaySoundEffect(sound.SoundEffectToArrayInt(PlayerSoundEffects.SoundEffect.MenuConfirm));
		gameControl = GameObject.FindObjectOfType<GameControl>();
		gameControl.itemsMenuLevel = 1;
		itemDatabase = GameObject.FindGameObjectWithTag("Items Database").GetComponent<ItemDatabase>();
		
		//some function about actually using the item goes here.
		
		int index = gameControl.itemInventoryList.IndexOf(itemDatabase.items[PlayerPrefsManager.GetEquipmentID ()]);
		if (gameControl.itemInventoryList[index].quantity > 1) {
			gameControl.itemInventoryList[index].quantity --;
		} else {
			gameControl.itemInventoryList.Remove (itemDatabase.items[PlayerPrefsManager.GetEquipmentID ()]);
		}
		
		useItemVerificationCanvas = GameObject.FindGameObjectWithTag("Use Item Verification Canvas");
		Destroy (useItemVerificationCanvas.gameObject);
		
		contentPanel.DeleteInventory();
		
		PopulateInventory();
		
		ItemInfoDisplay ();
	}
	
	public void UseItemNo () {
		PlayerSoundEffects sound = GameObject.FindGameObjectWithTag("Player Sound Effects").GetComponent<PlayerSoundEffects>();
		sound.PlaySoundEffect(sound.SoundEffectToArrayInt(PlayerSoundEffects.SoundEffect.MenuNavigation));
		gameControl = GameObject.FindObjectOfType<GameControl>();
		gameControl.itemsMenuLevel = 2;
		useItemVerificationCanvas = GameObject.FindGameObjectWithTag("Use Item Verification Canvas");
		Destroy (useItemVerificationCanvas.gameObject);
		useItemButton = GameObject.FindGameObjectWithTag("Use Item Button").GetComponent<Button>();
		useItemButton.interactable = true;
		
		destroyItemButton = GameObject.FindGameObjectWithTag("Destroy Item Button").GetComponent<Button>();
		destroyItemButton.interactable = true;
		EventSystem.current.SetSelectedGameObject(GameObject.FindGameObjectWithTag("Use Item Button"),null);
	}
	
	public void RemoveItemInInventory () {
		PlayerSoundEffects sound = GameObject.FindGameObjectWithTag("Player Sound Effects").GetComponent<PlayerSoundEffects>();
		sound.PlaySoundEffect(sound.SoundEffectToArrayInt(PlayerSoundEffects.SoundEffect.MenuNavigation));
		gameControl = GameObject.FindObjectOfType<GameControl>();
		gameControl.itemsMenuLevel = 3;
		useItemButton = GameObject.FindGameObjectWithTag("Use Item Button").GetComponent<Button>();
		useItemButton.interactable = false;
		
		destroyItemButton = GameObject.FindGameObjectWithTag("Destroy Item Button").GetComponent<Button>();
		destroyItemButton.interactable = false;
		
		GameObject.Instantiate(itemInventoryDestroy);
		
		EventSystem.current.SetSelectedGameObject(GameObject.FindGameObjectWithTag("Destroy Item Yes"),null);
	}
	
	public void RemoveItemYes () {
		PlayerSoundEffects sound = GameObject.FindGameObjectWithTag("Player Sound Effects").GetComponent<PlayerSoundEffects>();
		sound.PlaySoundEffect(sound.SoundEffectToArrayInt(PlayerSoundEffects.SoundEffect.MenuConfirm));
		gameControl = GameObject.FindObjectOfType<GameControl>();
		gameControl.itemsMenuLevel = 1;
		itemDatabase = GameObject.FindGameObjectWithTag("Items Database").GetComponent<ItemDatabase>();
		
		int index = gameControl.itemInventoryList.IndexOf(itemDatabase.items[PlayerPrefsManager.GetEquipmentID ()]);
		if (PlayerPrefsManager.GetQuantity() < gameControl.itemInventoryList[index].quantity) {
			gameControl.itemInventoryList[index].quantity = gameControl.itemInventoryList[index].quantity - PlayerPrefsManager.GetQuantity();
		} else {
			gameControl.itemInventoryList.Remove (itemDatabase.items[PlayerPrefsManager.GetEquipmentID ()]);
		}
		
		destroyItemVerificationCanvas = GameObject.FindGameObjectWithTag("Item Destroy Verification Canvas");
		Destroy (destroyItemVerificationCanvas.gameObject);
		
		contentPanel.DeleteInventory();
		
		PopulateInventory();
		
		ItemInfoDisplay ();
	}
	
	public void RemoveItemNo () {
		PlayerSoundEffects sound = GameObject.FindGameObjectWithTag("Player Sound Effects").GetComponent<PlayerSoundEffects>();
		sound.PlaySoundEffect(sound.SoundEffectToArrayInt(PlayerSoundEffects.SoundEffect.MenuNavigation));
		gameControl = GameObject.FindObjectOfType<GameControl>();
		gameControl.itemsMenuLevel = 2;
		destroyItemVerificationCanvas = GameObject.FindGameObjectWithTag("Item Destroy Verification Canvas");
		Destroy (destroyItemVerificationCanvas.gameObject);
		useItemButton = GameObject.FindGameObjectWithTag("Use Item Button").GetComponent<Button>();
		useItemButton.interactable = true;
		
		destroyItemButton = GameObject.FindGameObjectWithTag("Destroy Item Button").GetComponent<Button>();
		destroyItemButton.interactable = true;
		EventSystem.current.SetSelectedGameObject(GameObject.FindGameObjectWithTag("Use Item Button"),null);
	}
}
