using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class EquipmentInventory : MonoBehaviour {

	//access to gamecontrol, muy importante.
	private GameControl gameControl;
	
	//items that populate the inventory menu.
	public GameObject equipmentItem;
	
	//used to instantiate item use verification windows.
	public GameObject equipmentInventoryUse;
	public GameObject equipmentInventoryDestroy;
	
	//access the selected gameobject.
	private GameObject selectedItem;
	
	//gets access to the info items.
	private Text displayEquipmentName;
	private Image displayEquipmentIcon;
	private Text displayEquipmentDescription;
	private Text displayEquipmentType;
	
	//this can be private at some point.
	public List<Equipment> equipmentList;
	
	//used to add items from the item database.
	private EquipmentDatabase equipmentDatabase;
	
	private int equipmentSlots = 20;
	
	//used to turn off all interactability of item buttons in the content panel.
	private ContentPanel contentPanel;
	
	private Button equipButton;
	private Button destroyEquipmentButton;
	
	private GameObject equipVerificationCanvas;
	private GameObject destroyEquipmentVerificationCanvas;
	
	
	void Awake () {
		DontDestroyOnLoad (gameObject);
	}
	
	void Start () {
		gameControl = GameObject.FindObjectOfType<GameControl>();
		equipmentDatabase = GameObject.FindGameObjectWithTag("Equipment Database").GetComponent<EquipmentDatabase>();
		equipmentList = gameControl.equipmentInventoryList;
	}
	
	void Update () {
		GetEquipmentInfo ();
	}
	
	public void OpenEquipmentMenu () {
		
		AddTempData ();
		
		PopulateEquipment ();
		
		EquipmentInfoDisplay ();
	}
	
	void EquipmentInfoDisplay () {
		displayEquipmentName = GameObject.FindGameObjectWithTag ("Equipment Display Name").GetComponent<Text> ();
		displayEquipmentIcon = GameObject.FindGameObjectWithTag ("Equipment Display Icon").GetComponent<Image> ();
		displayEquipmentDescription = GameObject.FindGameObjectWithTag ("Equipment Display Description").GetComponent<Text> ();
		displayEquipmentType = GameObject.FindGameObjectWithTag ("Equipment Display Type").GetComponent<Text> ();
	}
	
	void AddTempData () {
		gameControl.equipmentInventoryList.Add (equipmentDatabase.equipment [0]);
		gameControl.equipmentInventoryList.Add (equipmentDatabase.equipment [1]);
		gameControl.equipmentInventoryList.Add (equipmentDatabase.equipment [2]);
		gameControl.equipmentInventoryList.Add (equipmentDatabase.equipment [3]);
		gameControl.equipmentInventoryList.Add (equipmentDatabase.equipment [4]);
		gameControl.equipmentInventoryList.Add (equipmentDatabase.equipment [5]);
		gameControl.equipmentInventoryList.Add (equipmentDatabase.equipment [6]);
		gameControl.equipmentInventoryList.Add (equipmentDatabase.equipment [7]);
	}
	
	void PopulateEquipment () {
		//FUGGIN MACK DADDY SETS THE CURRENT SELECTED OBJECT...WILL USE THIS TO CONTROL DIRECTION.
		EventSystem.current.SetSelectedGameObject(GameObject.FindGameObjectWithTag("Equipment Place Holder"),null);
		
		int inventoryCount = 0; //this works for now but i have my doubts...
		foreach (var equipment in gameControl.equipmentInventoryList) {
			GameObject newEquipment = Instantiate (equipmentItem) as GameObject;
			Text newequipmentText = newEquipment.GetComponentInChildren<Text>();
			newequipmentText.text = equipment.equipmentName;
			newEquipment.transform.SetParent (GameObject.FindGameObjectWithTag ("Equipment Content").transform, false);
			newEquipment.gameObject.name = equipment.equipmentName;
			
			//Puts the item ID as the text field for the second child of the new item button.
			GameObject newEquipmentIDObject = newEquipment.transform.GetChild(1).gameObject;
			Text newEquipmentID = newEquipmentIDObject.GetComponent<Text>();
			newEquipmentID.text = equipment.equipmentID.ToString();
			
			
			//Puts the inventory index as the text field for the third child of the new item button.
			GameObject newEquipmentIndexObject = newEquipment.transform.GetChild(2).gameObject;
			Text newEquipmentIndex = newEquipmentIndexObject.GetComponent<Text>();
			int equipmentInventoryCountIndex = inventoryCount;
			newEquipmentIndex.text = equipmentInventoryCountIndex.ToString();
			inventoryCount ++;
		}
	}
	
	
	public void GetEquipmentInfo () {
		selectedItem = EventSystem.current.currentSelectedGameObject;
		if (selectedItem.name == "Place Holder") {
			displayEquipmentName.text = "";
			displayEquipmentDescription.text = "";
			displayEquipmentType.text = "";
			displayEquipmentIcon.color = Color.black;
		} else if (selectedItem.name == "Equip") {
			
		} else if (selectedItem.name == "Destroy Equipment") {
			
		} else if (selectedItem.name == "Equip Equipment Yes") {
			
		} else if (selectedItem.name == "Equip Equipment No") {
			
		} else if (selectedItem.name == "Destroy Equipment Yes") {
			
		} else if (selectedItem.name == "Destroy Equipment No") {
			
		} else {
			displayEquipmentName.text = selectedItem.name;
			
			//Gets object number as string and converts to int.
			GameObject newEquipmentIDObject = selectedItem.transform.GetChild(1).gameObject;
			Text newEquipmentIDText = newEquipmentIDObject.GetComponent<Text>();
			int newEquipmentID = int.Parse(newEquipmentIDText.text);
			
			//Gets inventory index as string and converts to int, then pushes to playerprefsmanager.
			GameObject newItemIndexObject = selectedItem.transform.GetChild(2).gameObject;
			Text newItemIndexText = newItemIndexObject.GetComponent<Text>();
			int newItemIndex = int.Parse(newItemIndexText.text);
			PlayerPrefsManager.SetSelectItem(newItemIndex); //I think i use playerprefsmanger too much...consider just using local variables.
			
			displayEquipmentDescription.text = equipmentDatabase.equipment [newEquipmentID].equipmentDescription;
			displayEquipmentIcon.color = Color.white;
			displayEquipmentIcon.sprite = equipmentDatabase.equipment [newEquipmentID].equipmentIcon;
			displayEquipmentType.text = equipmentDatabase.equipment [newEquipmentID].equipmentType.ToString();
		}
	}
	
	
	public void AddEquipmentToInventory (int equipmentNumber) {
		if (equipmentList.Count == equipmentSlots) {
			return;
			//present message saying out of space? Might need to be more complicated...
		} else {
			gameControl.equipmentInventoryList.Add (equipmentDatabase.equipment [equipmentNumber]);
		}
	}
	
	public void SelectEquipment () {
		equipButton = GameObject.FindGameObjectWithTag("Equip Button").GetComponent<Button>();
		equipButton.interactable = true;
		
		destroyEquipmentButton = GameObject.FindGameObjectWithTag("Destroy Equipment Button").GetComponent<Button>();
		destroyEquipmentButton.interactable = true;
		
		contentPanel = GameObject.FindObjectOfType<ContentPanel>();
		contentPanel.DeactivateInventory();	
		EventSystem.current.SetSelectedGameObject(GameObject.FindGameObjectWithTag("Equip Button"),null);
	}
	
	public void EquipEquipmentInInventory () { //needs to be written for so equipping in same class doesn open a new menu.
//		equipButton = GameObject.FindGameObjectWithTag("Equip Button").GetComponent<Button>();
//		equipButton.interactable = false;
//		
//		destroyEquipmentButton = GameObject.FindGameObjectWithTag("Destroy Equipment Button").GetComponent<Button>();
//		destroyEquipmentButton.interactable = false;
//		
//		GameObject.Instantiate(equipmentInventoryUse);
//		
//		EventSystem.current.SetSelectedGameObject(GameObject.FindGameObjectWithTag("Equip Yes"),null);
	}
	
	public void EquipEquipmentYes () {
		gameControl = GameObject.FindObjectOfType<GameControl>();
		int itemIndex = PlayerPrefsManager.GetSelectItem ();
		gameControl.equipmentInventoryList.RemoveAt (itemIndex);
		
		equipVerificationCanvas = GameObject.FindGameObjectWithTag("Equip Equipment Verification Canvas");
		Destroy (equipVerificationCanvas.gameObject);
		
		contentPanel.RefreshInventory();
		
		PopulateEquipment();
		
		EquipmentInfoDisplay ();
	}
	
	public void EquipEquipmentNo () {
		equipVerificationCanvas = GameObject.FindGameObjectWithTag("Equip Equipment Verification Canvas");
		Destroy (equipVerificationCanvas.gameObject);
		contentPanel.ActivateInventory();
		
		int itemIndex = PlayerPrefsManager.GetSelectItem ();
		
		GameObject inventoryContentPanel = GameObject.FindGameObjectWithTag ("Equipment Content");
		GameObject lastSelected = inventoryContentPanel.transform.GetChild(itemIndex + 1).gameObject;
		EventSystem.current.SetSelectedGameObject(lastSelected,null);
	}
	
	public void DestroyEquipmentInInventory () {
		equipButton = GameObject.FindGameObjectWithTag("Equip Button").GetComponent<Button>();
		equipButton.interactable = false;
		
		destroyEquipmentButton = GameObject.FindGameObjectWithTag("Destroy Equipment Button").GetComponent<Button>();
		destroyEquipmentButton.interactable = false;
		
		GameObject.Instantiate(equipmentInventoryDestroy);
		
		EventSystem.current.SetSelectedGameObject(GameObject.FindGameObjectWithTag("Destroy Equipment Yes"),null);
	}
	
	public void DestoryEquipmentYes () {
		gameControl = GameObject.FindObjectOfType<GameControl>();
		int itemIndex = PlayerPrefsManager.GetSelectItem ();
		gameControl.equipmentInventoryList.RemoveAt (itemIndex);
		
		destroyEquipmentVerificationCanvas = GameObject.FindGameObjectWithTag("Destroy Equipment Verification Canvas");
		Destroy (destroyEquipmentVerificationCanvas.gameObject);
		
		contentPanel.RefreshInventory();
		
		PopulateEquipment();
		
		EquipmentInfoDisplay ();
	}
	
	public void DestroyEquipmentNo () {
		destroyEquipmentVerificationCanvas = GameObject.FindGameObjectWithTag("Destroy Equipment Verification Canvas");
		Destroy (destroyEquipmentVerificationCanvas.gameObject);
		contentPanel.ActivateInventory();
		
		int itemIndex = PlayerPrefsManager.GetSelectItem ();
		
		GameObject inventoryContentPanel = GameObject.FindGameObjectWithTag ("Equipment Content");
		GameObject lastSelected = inventoryContentPanel.transform.GetChild(itemIndex + 1).gameObject;
		EventSystem.current.SetSelectedGameObject(lastSelected,null);
	}
}
