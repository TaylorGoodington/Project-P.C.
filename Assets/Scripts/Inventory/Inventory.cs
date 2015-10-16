using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Inventory : MonoBehaviour {

	public GameObject inventoryItem;
	
	
	private GameObject selectedItem;
	private Text displayItemName;
	private Image displayItemIcon;
	private Text displayItemDescription;
	private Text displayItemType;
	
	//this can be private at some point.
	public List<Items> inventory = new List<Items>();
	
	private ItemDatabase itemDatabase;
	private int inventorySlots = 20;
	
	
	void Awake () {
		DontDestroyOnLoad (gameObject);
	}
	
	void Start () {
		itemDatabase = GameObject.FindGameObjectWithTag("Items Database").GetComponent<ItemDatabase>();
		
		AddTempData ();
		
		//want to call this after the menu has been instantiated.
		PopulateInventory ();
		
		//call these variables after the menu has been instantiated.
		displayItemName = GameObject.FindGameObjectWithTag("Item Display Name").GetComponent<Text>();
		displayItemIcon = GameObject.FindGameObjectWithTag("Item Display Icon").GetComponent<Image>();
		displayItemDescription = GameObject.FindGameObjectWithTag("Item Display Description").GetComponent<Text>();
		displayItemType = GameObject.FindGameObjectWithTag("Item Display Type").GetComponent<Text>();
	}

	void AddTempData ()
	{
		inventory.Add (itemDatabase.items [1]);
		inventory.Add (itemDatabase.items [0]);
		inventory.Add (itemDatabase.items [0]);
		inventory.Add (itemDatabase.items [1]);
		inventory.Add (itemDatabase.items [0]);
		inventory.Add (itemDatabase.items [0]);
		inventory.Add (itemDatabase.items [1]);
		inventory.Add (itemDatabase.items [0]);
		inventory.Add (itemDatabase.items [0]);
		inventory.Add (itemDatabase.items [1]);
		inventory.Add (itemDatabase.items [0]);
		inventory.Add (itemDatabase.items [0]);
	}

	void PopulateInventory ()
	{
		foreach (var item in inventory) {
			GameObject newItem = Instantiate (inventoryItem) as GameObject;
			InventoryItemButton newButton = inventoryItem.GetComponent<InventoryItemButton> ();
			Text newItemText = newItem.GetComponentInChildren<Text>();
			newItemText.text = item.itemName;
			newItem.transform.SetParent (GameObject.FindGameObjectWithTag ("Inventory Content").transform, false);
			newItem.gameObject.name = item.itemName;
			
			//Puts the item ID as the text field for the second child of the new item button.
			GameObject newItemIDObject = newItem.transform.GetChild(1).gameObject;
			Text newItemID = newItemIDObject.GetComponent<Text>();
			newItemID.text = item.itemID.ToString();
		}
	}

	
	void Update () {
		GetItemInfo ();
	}
	
	public void GetItemInfo () {
		selectedItem = EventSystem.current.currentSelectedGameObject;
		if (selectedItem.name == "Place Holder") {
			displayItemName.text = "";
			displayItemDescription.text = "";
			displayItemType.text = "";
			displayItemIcon.color = Color.black;
		} else {
			displayItemName.text = selectedItem.name;
		
			//Gets object number as string and converts to int.
			GameObject newItemIDObject = selectedItem.transform.GetChild(1).gameObject;
			Text newItemIDText = newItemIDObject.GetComponent<Text>();
			int newItemID = int.Parse(newItemIDText.text);
		
			displayItemDescription.text = itemDatabase.items [newItemID].itemDescription;
			displayItemIcon.color = Color.white;
			displayItemIcon.sprite = itemDatabase.items [newItemID].itemIcon;
			displayItemType.text = itemDatabase.items [newItemID].itemType.ToString();
		}
	}
	
	
	public void AddItemToInventory (int itemNumber) {
		if (inventory.Count == inventorySlots) {
			return;
			//present message saying out of space? Might need to be more complicated...
		} else {
			inventory.Add (itemDatabase.items [itemNumber]);
		}
	
	//add piece that looks at inventory size.
	}
	
	public void RemoveItemFromInventory () {
	
	}
	
	public void EquipItemFromInventory () {
	
	}
	
	public void UseItemInInventory () {
	
	}
}
