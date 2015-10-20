﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Inventory : MonoBehaviour {

	//access to gamecontrol, muy importante.
	private GameControl gameControl;

	//items that populate the inventory menu.
	public GameObject inventoryItem;
	
	//used to instantiate item use verification window.
	public GameObject itemInventoryUse;
	
	private GameObject selectedItem;
	
	private Text displayItemName;
	private Image displayItemIcon;
	private Text displayItemDescription;
	private Text displayItemType;
	
	//this can be private at some point.
	public List<Items> inventoryList;
	
	//used to add items from the item database.
	private ItemDatabase itemDatabase;
	
	private int inventorySlots = 20;
	
	//used to turn off all interactability of item buttons in the content panel.
	private ContentPanel contentPanel;
	
	private Button useItemButton;
	private Button destroyItemButton;
	
	private GameObject useItemVerificationCanvas;
	
	
	void Awake () {
		DontDestroyOnLoad (gameObject);
	}
	
	void Start () {
		gameControl = GameObject.FindObjectOfType<GameControl>();
		itemDatabase = GameObject.FindGameObjectWithTag("Items Database").GetComponent<ItemDatabase>();
		inventoryList = gameControl.itemInventoryList;
	}
	
	void Update () {
		GetItemInfo ();
	}
	
	public void OpenItemMenu () {
		
		AddTempData ();
		
		PopulateInventory ();
		
		ItemInfoDisplay ();
	}

	void ItemInfoDisplay () {
		displayItemName = GameObject.FindGameObjectWithTag ("Item Display Name").GetComponent<Text> ();
		displayItemIcon = GameObject.FindGameObjectWithTag ("Item Display Icon").GetComponent<Image> ();
		displayItemDescription = GameObject.FindGameObjectWithTag ("Item Display Description").GetComponent<Text> ();
		displayItemType = GameObject.FindGameObjectWithTag ("Item Display Type").GetComponent<Text> ();
	}

	void AddTempData () {
		gameControl.itemInventoryList.Add (itemDatabase.items [1]);
		gameControl.itemInventoryList.Add (itemDatabase.items [0]);
		gameControl.itemInventoryList.Add (itemDatabase.items [1]);
		gameControl.itemInventoryList.Add (itemDatabase.items [1]);
		gameControl.itemInventoryList.Add (itemDatabase.items [1]);
		gameControl.itemInventoryList.Add (itemDatabase.items [0]);
	}

	void PopulateInventory () {
		//FUGGIN MACK DADDY SETS THE CURRENT SELECTED OBJECT...WILL USE THIS TO CONTROL DIRECTION.
		EventSystem.current.SetSelectedGameObject(GameObject.FindGameObjectWithTag("Item Inventory Place Holder"),null);
		
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
		}
	}

	
	public void GetItemInfo () {
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
			
		} else {
			displayItemName.text = selectedItem.name;
		
			//Gets object number as string and converts to int.
			GameObject newItemIDObject = selectedItem.transform.GetChild(1).gameObject;
			Text newItemIDText = newItemIDObject.GetComponent<Text>();
			int newItemID = int.Parse(newItemIDText.text);
			
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
	
	
	public void AddItemToInventory (int itemNumber) {
		if (inventoryList.Count == inventorySlots) {
			return;
			//present message saying out of space? Might need to be more complicated...
		} else {
			gameControl.itemInventoryList.Add (itemDatabase.items [itemNumber]);
		}
	}
	
	public void SelectItem () {
		useItemButton = GameObject.FindGameObjectWithTag("Use Item Button").GetComponent<Button>();
		useItemButton.interactable = true;
		
		destroyItemButton = GameObject.FindGameObjectWithTag("Destroy Item Button").GetComponent<Button>();
		destroyItemButton.interactable = true;
		
		contentPanel = GameObject.FindObjectOfType<ContentPanel>();
		contentPanel.DeactivateInventory();	
		EventSystem.current.SetSelectedGameObject(GameObject.FindGameObjectWithTag("Use Item Button"),null);
	}
	
	public void UseItemInInventory () {
		useItemButton = GameObject.FindGameObjectWithTag("Use Item Button").GetComponent<Button>();
		useItemButton.interactable = false;
		
		destroyItemButton = GameObject.FindGameObjectWithTag("Destroy Item Button").GetComponent<Button>();
		destroyItemButton.interactable = false;
		
		GameObject useMenu = GameObject.Instantiate(itemInventoryUse);
		
		EventSystem.current.SetSelectedGameObject(GameObject.FindGameObjectWithTag("Use Item Yes"),null);
	}

	public void UseItemYes () {
		gameControl = GameObject.FindObjectOfType<GameControl>();
		int itemIndex = PlayerPrefsManager.GetSelectItem ();
		gameControl.itemInventoryList.RemoveAt (itemIndex);
		
		useItemVerificationCanvas = GameObject.FindGameObjectWithTag("Use Item Verification Canvas");
		Destroy (useItemVerificationCanvas.gameObject);
		contentPanel.RefreshInventory();
		
		PopulateInventory();
		
		ItemInfoDisplay ();
	}
	
	public void UseItemNo () {
		
	}
	
	public void RemoveItemInInventory () {
		useItemButton = GameObject.FindGameObjectWithTag("Use Item Button").GetComponent<Button>();
		useItemButton.interactable = false;
		
		useItemButton = GameObject.FindGameObjectWithTag("Destroy Item Button").GetComponent<Button>();
		useItemButton.interactable = false;
		
//		GameObject useMenu = GameObject.Instantiate(itemInventoryUse);
	}
	
	public void RemoveItemYes () {
		int itemIndex = PlayerPrefsManager.GetSelectItem();
		inventoryList.RemoveAt(itemIndex);
	}
}