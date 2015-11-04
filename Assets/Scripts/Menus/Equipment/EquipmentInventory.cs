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
	
	public GameObject equipmentSlotsMenu;
	
	public ClassesDatabase classesDatabase;
	
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
	private Text displayEquipmentMaterial;
	private Image displaySkillIcon;
	private Text displaySkillDescription;
	
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
	
	//stat change info panel
	private GameObject statChangeInfo;
	private Text strengthStat;
	private Text strengthLabel;
	private Text defenseStat;
	private Text defenseLabel;
	private Text intelligenceStat;
	private Text intelligenceLabel;
	private Text speedStat;
	private Text speedLabel;
	private Text healthStat;
	private Text healthLabel;
	private Text manaStat;
	private Text manaLabel;
	
	private int selectedEquipmentID;
	private int selectedEquipmentMaterialIndex;
	
	
	void Awake () {
		DontDestroyOnLoad (gameObject);
	}
	
	void Start () {
		gameControl = GameObject.FindObjectOfType<GameControl>();
		equipmentDatabase = GameObject.FindGameObjectWithTag("Equipment Database").GetComponent<EquipmentDatabase>();
//		classesDatabse = GameObject.FindObjectOfType<ClassesDatabase>();
		equipmentList = gameControl.equipmentInventoryList;
	}
	
	void Update () {
		if (GameObject.FindGameObjectWithTag("Equipment Menu")) {
			GetSlotEquipmentInfo ();
		} else if (GameObject.FindGameObjectWithTag("Base Equipment Menu")) {
			GetBaseEquipmentInfo ();
		}
	}
	
	//once equipment is un equipped by class shifts the basic equipment is equipped.
	public void AutoEquipClothes () {
		gameControl = GameObject.FindObjectOfType<GameControl>();
		
		if (gameControl.equippedHead == 0) { gameControl.equippedHead = 1; } 
		if (gameControl.equippedChest == 0) { gameControl.equippedChest = 2; } 
		if (gameControl.equippedPants == 0) { gameControl.equippedPants = 3; } 
		if (gameControl.equippedFeet == 0) { gameControl.equippedFeet = 4; }
	}
	
	public void OpenEquipmentBaseMenu () {
	
		AddTempData ();
	
		EquipmentInfoDisplay ();
		
		AutoEquipClothes ();
		
		EventSystem.current.SetSelectedGameObject(GameObject.FindGameObjectWithTag("Head Slot"),null);
	}
	
	public void OpenEquipmentSlotMenu () {
		equipmentDatabase = GameObject.FindGameObjectWithTag("Equipment Database").GetComponent<EquipmentDatabase>();
		Instantiate (equipmentSlotsMenu);
		Destroy (GameObject.FindGameObjectWithTag("Base Equipment Menu"));
		Equipment.EquipmentSlot selectSlot = equipmentDatabase.equipment [PlayerPrefsManager.GetEquipmentID()].equipmentSlot;
		
		PopulateEquipment (selectSlot);
		
		EquipmentInfoDisplay ();
		
		SetEquippedSlot ();
	}
	
	public void SetEquippedSlot () {
		equipmentDatabase = GameObject.FindGameObjectWithTag("Equipment Database").GetComponent<EquipmentDatabase>();
		
		GameObject equippedSlot = GameObject.FindGameObjectWithTag("Equipped Slot");
		GameObject equippedSlotIDObject = equippedSlot.transform.GetChild(1).gameObject;
		Text equippedSlotIDText = equippedSlotIDObject.GetComponent<Text>();
		equippedSlotIDText.text = PlayerPrefsManager.GetEquipmentID().ToString();
		GameObject equippedSlotName = equippedSlot.transform.GetChild(0).gameObject;
		Text equippedSlotNameText = equippedSlotName.GetComponent<Text>();
		equippedSlotNameText.text = equipmentDatabase.equipment[PlayerPrefsManager.GetEquipmentID()].equipmentName;
	}
	
	//gets the components in the info display panel for future use.
	void EquipmentInfoDisplay () {
		if (GameObject.FindGameObjectWithTag("Equipment Menu")) { // This is for the slots menus...
			displayEquipmentName = GameObject.FindGameObjectWithTag ("Equipment Display Name").GetComponent<Text> ();
			displayEquipmentIcon = GameObject.FindGameObjectWithTag ("Equipment Display Icon").GetComponent<Image> ();
			displayEquipmentDescription = GameObject.FindGameObjectWithTag ("Equipment Display Description").GetComponent<Text> ();
			displayEquipmentType = GameObject.FindGameObjectWithTag ("Equipment Display Type").GetComponent<Text> ();
			displayEquipmentMaterial = GameObject.FindGameObjectWithTag ("Equipment Display Material").GetComponent<Text> ();
			
			strengthStat = GameObject.FindGameObjectWithTag ("Strength").GetComponent<Text> ();
			strengthLabel = GameObject.FindGameObjectWithTag ("Strength Label").GetComponent<Text> ();
			defenseStat = GameObject.FindGameObjectWithTag ("Defense").GetComponent<Text> ();
			defenseLabel = GameObject.FindGameObjectWithTag ("Defense Label").GetComponent<Text> ();
			intelligenceStat = GameObject.FindGameObjectWithTag ("Intelligence").GetComponent<Text> ();
			intelligenceLabel = GameObject.FindGameObjectWithTag ("Intelligence Label").GetComponent<Text> ();
			speedStat = GameObject.FindGameObjectWithTag ("Speed").GetComponent<Text> ();
			speedLabel = GameObject.FindGameObjectWithTag ("Speed Label").GetComponent<Text> ();
			healthStat = GameObject.FindGameObjectWithTag ("Health").GetComponent<Text> ();
			healthLabel = GameObject.FindGameObjectWithTag ("Health Label").GetComponent<Text> ();
			manaStat = GameObject.FindGameObjectWithTag ("Mana").GetComponent<Text> ();
			manaLabel = GameObject.FindGameObjectWithTag ("Mana Label").GetComponent<Text> ();
		} else { // This is for the base menu.
			displayEquipmentName = GameObject.FindGameObjectWithTag ("Equipment Display Name").GetComponent<Text> ();
			displayEquipmentIcon = GameObject.FindGameObjectWithTag ("Equipment Display Icon").GetComponent<Image> ();
			displayEquipmentDescription = GameObject.FindGameObjectWithTag ("Equipment Display Description").GetComponent<Text> ();
			displayEquipmentType = GameObject.FindGameObjectWithTag ("Equipment Display Type").GetComponent<Text> ();
			displayEquipmentMaterial = GameObject.FindGameObjectWithTag ("Equipment Display Material").GetComponent<Text> ();
			displaySkillIcon = GameObject.FindGameObjectWithTag ("Skill Icon").GetComponent<Image>();
			displaySkillDescription = GameObject.FindGameObjectWithTag ("Skill Description").GetComponent<Text>();
			
			strengthStat = GameObject.FindGameObjectWithTag ("Strength").GetComponent<Text> ();
			strengthLabel = GameObject.FindGameObjectWithTag ("Strength Label").GetComponent<Text> ();
			defenseStat = GameObject.FindGameObjectWithTag ("Defense").GetComponent<Text> ();
			defenseLabel = GameObject.FindGameObjectWithTag ("Defense Label").GetComponent<Text> ();
			intelligenceStat = GameObject.FindGameObjectWithTag ("Intelligence").GetComponent<Text> ();
			intelligenceLabel = GameObject.FindGameObjectWithTag ("Intelligence Label").GetComponent<Text> ();
			speedStat = GameObject.FindGameObjectWithTag ("Speed").GetComponent<Text> ();
			speedLabel = GameObject.FindGameObjectWithTag ("Speed Label").GetComponent<Text> ();
			healthStat = GameObject.FindGameObjectWithTag ("Health").GetComponent<Text> ();
			healthLabel = GameObject.FindGameObjectWithTag ("Health Label").GetComponent<Text> ();
			manaStat = GameObject.FindGameObjectWithTag ("Mana").GetComponent<Text> ();
			manaLabel = GameObject.FindGameObjectWithTag ("Mana Label").GetComponent<Text> ();
		}
	}
	
	void AddTempData () {
		//take out of inventory at some point...
		gameControl.equipmentInventoryList.Add (equipmentDatabase.equipment [5]);
		gameControl.equipmentInventoryList.Add (equipmentDatabase.equipment [6]);
		gameControl.equipmentInventoryList.Add (equipmentDatabase.equipment [7]);
		gameControl.equipmentInventoryList.Add (equipmentDatabase.equipment [8]);
		gameControl.equipmentInventoryList.Add (equipmentDatabase.equipment [9]);
		gameControl.equipmentInventoryList.Add (equipmentDatabase.equipment [10]);
		gameControl.equipmentInventoryList.Add (equipmentDatabase.equipment [11]);
		gameControl.equipmentInventoryList.Add (equipmentDatabase.equipment [12]);
		gameControl.equipmentInventoryList.Add (equipmentDatabase.equipment [13]);		
		gameControl.equipmentInventoryList.Add (equipmentDatabase.equipment [14]);
		gameControl.equipmentInventoryList.Add (equipmentDatabase.equipment [15]);
		gameControl.equipmentInventoryList.Add (equipmentDatabase.equipment [16]);
	}
	
	void PopulateEquipment (Equipment.EquipmentSlot equipmentSlot) {
		gameControl = GameObject.FindObjectOfType<GameControl>();
		
		EventSystem.current.SetSelectedGameObject(GameObject.FindGameObjectWithTag("Equipped Slot"),null);
		
		foreach (var equipment in gameControl.equipmentInventoryList) {
			if (equipment.equipmentSlot == equipmentSlot) {
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
				int equipmentInventoryCountIndex = gameControl.equipmentInventoryList.IndexOf(equipment);
				newEquipmentIndex.text = equipmentInventoryCountIndex.ToString();
				
				//Puts the quantity of the object in the fourth child.
				GameObject newEquipmentQuantityObject = newEquipment.transform.GetChild(3).gameObject;
				Text newEquipmentQuantity = newEquipmentQuantityObject.GetComponent<Text>();
				newEquipmentQuantity.text = equipment.quantity.ToString();
			}
		}
	}
	
	
	public void GetBaseEquipmentInfo () {
		selectedItem = EventSystem.current.currentSelectedGameObject;
		int newEquipmentID = 0;
		if (selectedItem.name == "Head Slot") {
			newEquipmentID = gameControl.equippedHead;
			PlayerPrefsManager.SetEquipmentID(newEquipmentID);
		} else if (selectedItem.name == "Chest Slot") {
			newEquipmentID = gameControl.equippedChest;
			PlayerPrefsManager.SetEquipmentID(newEquipmentID);
		} else if (selectedItem.name == "Weapon Slot") {
			newEquipmentID = gameControl.equippedWeapon;
			PlayerPrefsManager.SetEquipmentID(newEquipmentID);
		} else if (selectedItem.name == "Pants Slot") {
			newEquipmentID = gameControl.equippedPants;
			PlayerPrefsManager.SetEquipmentID(newEquipmentID);
		} else {
			newEquipmentID = gameControl.equippedFeet;
			PlayerPrefsManager.SetEquipmentID(newEquipmentID);
		}
			
		displayEquipmentName.text = equipmentDatabase.equipment [newEquipmentID].equipmentName;
		displayEquipmentDescription.text = equipmentDatabase.equipment [newEquipmentID].equipmentDescription;
		displayEquipmentIcon.color = Color.white;
		displayEquipmentIcon.sprite = equipmentDatabase.equipment [newEquipmentID].equipmentIcon;
		displayEquipmentType.text = equipmentDatabase.equipment [newEquipmentID].equipmentType.ToString();
		displayEquipmentMaterial.text = equipmentDatabase.equipment [newEquipmentID].equipmentMaterial.ToString();
			
		//Stat Info Change Section 
		strengthStat.text = equipmentDatabase.equipment [newEquipmentID].equipmentStrength.ToString();
		defenseStat.text = equipmentDatabase.equipment [newEquipmentID].equipmentDefense.ToString();
		intelligenceStat.text = equipmentDatabase.equipment [newEquipmentID].equipmentIntelligence.ToString();
		speedStat.text = equipmentDatabase.equipment [newEquipmentID].equipmentSpeed.ToString();
		healthStat.text = equipmentDatabase.equipment [newEquipmentID].equipmentHealth.ToString();
		manaStat.text = equipmentDatabase.equipment [newEquipmentID].equipmentMana.ToString();
			
		strengthLabel.color = Color.white;
		defenseLabel.color = Color.white;
		intelligenceLabel.color = Color.white;
		speedLabel.color = Color.white;
		healthLabel.color = Color.white;
		manaLabel.color = Color.white;
	}
	
	public void GetSlotEquipmentInfo () {
		EquipmentInfoDisplay ();
		selectedItem = EventSystem.current.currentSelectedGameObject;
		if (selectedItem.name == "Place Holder") {
			displayEquipmentName.text = "";
			displayEquipmentDescription.text = "";
			displayEquipmentType.text = "";
			displayEquipmentIcon.color = Color.black;
			displayEquipmentMaterial.text = "";
			strengthStat.text = "";
			defenseStat.text = "";
			intelligenceStat.text = "";
			speedStat.text = "";
			healthStat.text = "";
			manaStat.text = "";
			strengthLabel.color = Color.black;
			defenseLabel.color = Color.black;
			intelligenceLabel.color = Color.black;
			speedLabel.color = Color.black;
			healthLabel.color = Color.black;
			manaLabel.color = Color.black;
		} else if (selectedItem.name == "Equip") {
			strengthStat.text = gameControl.currentStrength + " => " + (gameControl.currentStrength + equipmentDatabase.equipment [selectedEquipmentID].equipmentStrength).ToString();
			defenseStat.text = gameControl.currentDefense + " => " + (gameControl.currentDefense + equipmentDatabase.equipment [selectedEquipmentID].equipmentDefense).ToString();
			intelligenceStat.text = gameControl.currentIntelligence + " => " + (gameControl.currentIntelligence + equipmentDatabase.equipment [selectedEquipmentID].equipmentIntelligence).ToString();
			speedStat.text = gameControl.currentSpeed + " => " + (gameControl.currentSpeed + equipmentDatabase.equipment [selectedEquipmentID].equipmentSpeed).ToString();
			healthStat.text = gameControl.currentHealth + " => " + (gameControl.currentHealth + equipmentDatabase.equipment [selectedEquipmentID].equipmentHealth).ToString();
			manaStat.text = gameControl.currentMana + " => " + (gameControl.currentMana + equipmentDatabase.equipment [selectedEquipmentID].equipmentMana).ToString();
//			strengthLabel.color = Color.white;
//			defenseLabel.color = Color.white;
//			intelligenceLabel.color = Color.white;
//			speedLabel.color = Color.white;
//			healthLabel.color = Color.white;
//			manaLabel.color = Color.white;
		} else if (selectedItem.name == "Destroy Equipment") {
			strengthStat.text = gameControl.currentStrength + " => " + (gameControl.currentStrength + equipmentDatabase.equipment [selectedEquipmentID].equipmentStrength).ToString();
			defenseStat.text = gameControl.currentDefense + " => " + (gameControl.currentDefense + equipmentDatabase.equipment [selectedEquipmentID].equipmentDefense).ToString();
			intelligenceStat.text = gameControl.currentIntelligence + " => " + (gameControl.currentIntelligence + equipmentDatabase.equipment [selectedEquipmentID].equipmentIntelligence).ToString();
			speedStat.text = gameControl.currentSpeed + " => " + (gameControl.currentSpeed + equipmentDatabase.equipment [selectedEquipmentID].equipmentSpeed).ToString();
			healthStat.text = gameControl.currentHealth + " => " + (gameControl.currentHealth + equipmentDatabase.equipment [selectedEquipmentID].equipmentHealth).ToString();
			manaStat.text = gameControl.currentMana + " => " + (gameControl.currentMana + equipmentDatabase.equipment [selectedEquipmentID].equipmentMana).ToString();
//			strengthLabel.color = Color.white;
//			defenseLabel.color = Color.white;
//			intelligenceLabel.color = Color.white;
//			speedLabel.color = Color.white;
//			healthLabel.color = Color.white;
//			manaLabel.color = Color.white;
		} else if (selectedItem.name == "Equip Equipment Yes") {
			
		} else if (selectedItem.name == "Equip Equipment No") {
			
		} else if (selectedItem.name == "Destroy Equipment Yes") {
			
		} else if (selectedItem.name == "Destroy Equipment No") {
			
		} else {
			displayEquipmentName.text = selectedItem.transform.GetChild(0).GetComponent<Text>().text;
			
			//Gets object number as string and converts to int.
			GameObject newEquipmentIDObject = selectedItem.transform.GetChild(1).gameObject;
			Text newEquipmentIDText = newEquipmentIDObject.GetComponent<Text>();
			int newEquipmentID = int.Parse(newEquipmentIDText.text);
			selectedEquipmentID = newEquipmentID;
			PlayerPrefsManager.SetEquipmentID(selectedEquipmentID);
						
			//Gets inventory index as string and converts to int, then pushes to playerprefsmanager.
			GameObject newItemIndexObject = selectedItem.transform.GetChild(2).gameObject;
			Text newItemIndexText = newItemIndexObject.GetComponent<Text>();
			int newItemIndex = int.Parse(newItemIndexText.text);
			PlayerPrefsManager.SetSelectItem(newItemIndex);
			
			displayEquipmentDescription.text = equipmentDatabase.equipment [newEquipmentID].equipmentDescription;
			displayEquipmentIcon.color = Color.white;
			displayEquipmentIcon.sprite = equipmentDatabase.equipment [newEquipmentID].equipmentIcon;
			displayEquipmentType.text = equipmentDatabase.equipment [newEquipmentID].equipmentType.ToString();
			displayEquipmentMaterial.text = equipmentDatabase.equipment [newEquipmentID].equipmentMaterial.ToString();
			
			//Stat Info Change Section 
			strengthStat.text = equipmentDatabase.equipment [newEquipmentID].equipmentStrength.ToString();
			defenseStat.text = equipmentDatabase.equipment [newEquipmentID].equipmentDefense.ToString();
			intelligenceStat.text = equipmentDatabase.equipment [newEquipmentID].equipmentIntelligence.ToString();
			speedStat.text = equipmentDatabase.equipment [newEquipmentID].equipmentSpeed.ToString();
			healthStat.text = equipmentDatabase.equipment [newEquipmentID].equipmentHealth.ToString();
			manaStat.text = equipmentDatabase.equipment [newEquipmentID].equipmentMana.ToString();
			
			strengthLabel.color = Color.white;
			defenseLabel.color = Color.white;
			intelligenceLabel.color = Color.white;
			speedLabel.color = Color.white;
			healthLabel.color = Color.white;
			manaLabel.color = Color.white;
		}
	}
	
	//Have equipment drops call this as needed.
	public void AddEquipmentToInventory (int equipmentNumber) {
		if (equipmentList.Count == equipmentSlots) {
			return;
			//present message saying out of space? Might need to be more complicated...
		} else {
			if (gameControl.equipmentInventoryList.Contains(equipmentDatabase.equipment[equipmentNumber])) {
				gameControl.equipmentInventoryList[equipmentNumber].quantity ++;
			} else {
				gameControl.equipmentInventoryList.Add (equipmentDatabase.equipment[equipmentNumber]);
			}
		}
	}
	
	
	public void SelectEquipment () {
	
		selectedEquipmentMaterialIndex = GetEquipmentMaterialIndex ();
		
		equipButton = GameObject.FindGameObjectWithTag("Equip Button").GetComponent<Button>();
		equipButton.interactable = true;
		
		destroyEquipmentButton = GameObject.FindGameObjectWithTag("Destroy Equipment Button").GetComponent<Button>();
		destroyEquipmentButton.interactable = true;
		
		contentPanel = GameObject.FindObjectOfType<ContentPanel>();
		contentPanel.DeactivateInventory();	
		EventSystem.current.SetSelectedGameObject(GameObject.FindGameObjectWithTag("Equip Button"),null);
	}
	
	
	public int GetEquipmentMaterialIndex () {
		equipmentDatabase = GameObject.FindGameObjectWithTag("Equipment Database").GetComponent<EquipmentDatabase>();
		selectedEquipmentID = PlayerPrefsManager.GetEquipmentID();
		if (equipmentDatabase.equipment[selectedEquipmentID].equipmentMaterial == Equipment.EquipmentMaterial.Cloth) {
			return 1;
		} else if (equipmentDatabase.equipment [selectedEquipmentID].equipmentMaterial == Equipment.EquipmentMaterial.Leather) {
			return 2;
		} else if (equipmentDatabase.equipment [selectedEquipmentID].equipmentMaterial == Equipment.EquipmentMaterial.Chainmail) {
			return 3;
		} else if (equipmentDatabase.equipment [selectedEquipmentID].equipmentMaterial == Equipment.EquipmentMaterial.Platemail) {
			return 4;
		} else {
			return 5;
		}
	}
	
	
	public void EquipEquipmentInInventory () { 
		gameControl = GameObject.FindObjectOfType<GameControl>();		
		int itemIndex = PlayerPrefsManager.GetSelectItem ();
		equipButton = GameObject.FindGameObjectWithTag("Equip Button").GetComponent<Button>();
		destroyEquipmentButton = GameObject.FindGameObjectWithTag("Destroy Equipment Button").GetComponent<Button>();
		classesDatabase = GameObject.FindObjectOfType<ClassesDatabase>();
		
		if (classesDatabase.classes[gameControl.playerClass].equipmentMaterialIndex >= selectedEquipmentMaterialIndex) {
			if (equipmentDatabase.equipment [PlayerPrefsManager.GetEquipmentID()].equipmentType == Equipment.EquipmentType.Head) {
				if (gameControl.equippedHead != 1) {
					AddEquipmentToInventory (gameControl.equippedHead);
				}
				gameControl.equippedHead = PlayerPrefsManager.GetEquipmentID();
							
			} else if (equipmentDatabase.equipment [PlayerPrefsManager.GetEquipmentID()].equipmentType == Equipment.EquipmentType.Chest) {
				if (gameControl.equippedChest != 2) {
					AddEquipmentToInventory (gameControl.equippedChest);
				}
				gameControl.equippedChest = PlayerPrefsManager.GetEquipmentID();
				
			} else if (equipmentDatabase.equipment [PlayerPrefsManager.GetEquipmentID()].equipmentType == Equipment.EquipmentType.Pants) {
				if (gameControl.equippedPants != 3) {
					AddEquipmentToInventory (gameControl.equippedPants);
				}
				gameControl.equippedPants = PlayerPrefsManager.GetEquipmentID();
				
			} else {
				if (gameControl.equippedFeet != 4) {
					AddEquipmentToInventory (gameControl.equippedFeet);
				}
				gameControl.equippedFeet = PlayerPrefsManager.GetEquipmentID();
			}
		}
		
		if (gameControl.equipmentInventoryList [PlayerPrefsManager.GetEquipmentID()].quantity > 1) {
			gameControl.equipmentInventoryList [PlayerPrefsManager.GetEquipmentID()].quantity --;
		} else {
			gameControl.equipmentInventoryList.RemoveAt (itemIndex);
		}
		
		equipButton.interactable = false;
		destroyEquipmentButton.interactable = false;
	}
	
	
	public void EquipEquipmentYes () {		
		contentPanel.DeleteInventory();
		
//		PopulateEquipment();
		
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
		
		contentPanel.DeleteInventory();
		
		//before I invoked this method, I was getting nothing off populate because the objects hadn't been destroyed yet.
		Invoke ("RefreshInventory",0);
	}
	
	
	public void RefreshInventory () {
		Equipment.EquipmentSlot selectSlot = equipmentDatabase.equipment [PlayerPrefsManager.GetEquipmentID()].equipmentSlot;
		PopulateEquipment (selectSlot);
		EquipmentInfoDisplay ();
	}
	
	
	public void DestroyEquipmentNo () {
		destroyEquipmentVerificationCanvas = GameObject.FindGameObjectWithTag("Destroy Equipment Verification Canvas");
		Destroy (destroyEquipmentVerificationCanvas.gameObject);
		contentPanel.ActivateInventory();
		
		int itemIndex = PlayerPrefsManager.GetSelectItem ();
		
		GameObject inventoryContentPanel = GameObject.FindGameObjectWithTag ("Equipment Content");
		GameObject lastSelected = inventoryContentPanel.transform.GetChild(itemIndex - 2).gameObject;
		EventSystem.current.SetSelectedGameObject(lastSelected,null);
	}
}
