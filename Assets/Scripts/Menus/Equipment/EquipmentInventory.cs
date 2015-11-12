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
	
	public GameObject equipmentBaseMenu;
	
	public GameObject equipmentSlotsMenu;
	
	public GameObject weaponEvolutionMenu;
	
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
		equipmentList = gameControl.equipmentInventoryList;
	}
	
	void Update () {
		if (GameObject.FindGameObjectWithTag("Equipment Menu")) {
			GetSlotEquipmentInfo ();
		} else if (GameObject.FindGameObjectWithTag("Base Equipment Menu")) {
			GetBaseEquipmentInfo ();
		}
	}
	
	
	public void OpenPreviousEquipmentMenu (int equipmentMenuLevel) {
		if (GameObject.FindGameObjectWithTag ("Equipment Content")) {
			contentPanel = GameObject.FindGameObjectWithTag ("Equipment Content").GetComponent<ContentPanel>();
		}
		
		if (equipmentMenuLevel == 4) {
			DestroyEquipmentNo();
		} else if (equipmentMenuLevel == 3) {
			equipButton = GameObject.FindGameObjectWithTag("Equip Button").GetComponent<Button>();
			equipButton.interactable = false;
			if (GameObject.FindGameObjectWithTag("Destroy Equipment Button")) {
				destroyEquipmentButton = GameObject.FindGameObjectWithTag("Destroy Equipment Button").GetComponent<Button>();
				destroyEquipmentButton.interactable = false;
			}
			contentPanel.ActivateInventory();
			EventSystem.current.SetSelectedGameObject(GameObject.FindGameObjectWithTag("Equipment Content").transform.GetChild(PlayerPrefsManager.GetLocalEquipmentIndex()).gameObject,null);
		} else if (equipmentMenuLevel == 2) {
			Destroy (GameObject.FindGameObjectWithTag("Equipment Menu"));
			Invoke ("OpenEquipmentBaseMenu",0);
		} else if (equipmentMenuLevel == 1) {
			Destroy (GameObject.FindGameObjectWithTag("Base Equipment Menu"));
			//Method for opening main menu level.
		}
	}
	
	
	public void OpenPreviousWeaponMenu (int equipmentMenuLevel) {
		if (equipmentMenuLevel == 2) {
			equipButton = GameObject.FindGameObjectWithTag("Equip Button").GetComponent<Button>();
			contentPanel = GameObject.FindObjectOfType<ContentPanel>();
			equipButton.interactable = false;
			contentPanel.ActivateInventory();	
			EventSystem.current.SetSelectedGameObject(contentPanel.transform.GetChild(PlayerPrefsManager.GetLocalEquipmentIndex()).gameObject,null);
		} else if (equipmentMenuLevel == 1) {
			Destroy (GameObject.FindGameObjectWithTag("Equipment Menu"));
			//Method for opening lady death menu.
		}
	}

	
	public void OpenEquipmentBaseMenu () {
		Instantiate (equipmentBaseMenu);
		EquipmentInfoDisplay ();
		AutoEquipClothes ();
		gameControl.equipmentMenuLevel = 1;
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
		gameControl.equipmentMenuLevel = 2;
	}
	
	public void OpenWeaponEvolutionMenu () {
		gameControl.weaponEvolutionMenuLevel = 1;
		Instantiate (weaponEvolutionMenu);
		contentPanel = GameObject.FindGameObjectWithTag ("Equipment Content").GetComponent<ContentPanel>();
		contentPanel.InitiateWeaponEvolutionMenuSlots();
		EventSystem.current.SetSelectedGameObject(GameObject.FindGameObjectWithTag("Equipment Place Holder"),null);
		EquipmentInfoDisplay ();
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
		
		if (equipmentDatabase.equipment[PlayerPrefsManager.GetEquipmentID()].equipmentSlot == Equipment.EquipmentSlot.Weapon) {
			GameObject equippedLevel = equippedSlot.transform.GetChild(3).gameObject;
			Text equippedLevelText = equippedLevel.GetComponent<Text>();
			equippedLevelText.text = equipmentDatabase.equipment[PlayerPrefsManager.GetEquipmentID()].quantity.ToString();
		}
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


	public void WeaponEvolution () {
		int equipID = PlayerPrefsManager.GetEquipmentID();
		gameControl = GameObject.FindObjectOfType<GameControl>();
		if (gameControl.availableEvolutions > 0) {
			if (equipmentDatabase.equipment[equipID].quantity < 8) {
				if (equipID == gameControl.equippedWeapon) {
					gameControl.equippedWeapon = equipID + 1;;
					UpdateEquippedStats();
				} else {
					gameControl.weaponsList.Remove (equipmentDatabase.equipment [equipID]);
					gameControl.weaponsList.Add (equipmentDatabase.equipment [equipID + 1]);
				}
			equipButton = GameObject.FindGameObjectWithTag("Equip Button").GetComponent<Button>();
			contentPanel = GameObject.FindObjectOfType<ContentPanel>();
			equipButton.interactable = false;
			contentPanel.UpdateWeaponEvolutionMenu(PlayerPrefsManager.GetEquipmentID());
			contentPanel.ActivateInventory();	
			EventSystem.current.SetSelectedGameObject(contentPanel.transform.GetChild(PlayerPrefsManager.GetLocalEquipmentIndex()).gameObject,null);
			}
			//present message about weapon being at max level. Or play noise or something.
		} else {
//			//present message about not having any souls.
		}
	}

	public void AddTempData () {
		//take out of inventory at some point...		
		AddEquipmentToInventory(5, 6, 5, 6, 7, 8);
	}
	
	public void PopulateEquipment (Equipment.EquipmentSlot equipmentSlot) {
		gameControl = GameObject.FindObjectOfType<GameControl>();
		EventSystem.current.SetSelectedGameObject(GameObject.FindGameObjectWithTag("Equipped Slot"),null);
		int localIndexCount = 1;
		
		if (equipmentSlot == Equipment.EquipmentSlot.Weapon) {
			//do some stuff about populating the weapon menu, which is not part of the inventory.
			foreach (var weapons in gameControl.weaponsList) {
				GameObject newEquipment = Instantiate (equipmentItem) as GameObject;
				Text newequipmentText = newEquipment.GetComponentInChildren<Text>();
				newequipmentText.text = weapons.equipmentName;
				newEquipment.transform.SetParent (GameObject.FindGameObjectWithTag ("Equipment Content").transform, false);
				newEquipment.gameObject.name = weapons.equipmentName;
				
				//Puts the item ID as the text field for the second child of the new item button.
				GameObject newEquipmentIDObject = newEquipment.transform.GetChild(1).gameObject;
				Text newEquipmentID = newEquipmentIDObject.GetComponent<Text>();
				newEquipmentID.text = weapons.equipmentID.ToString();
				
				//Puts the inventory index as the text field for the third child of the new item button.
				GameObject newEquipmentIndexObject = newEquipment.transform.GetChild(2).gameObject;
				Text newEquipmentIndex = newEquipmentIndexObject.GetComponent<Text>();
				int equipmentInventoryCountIndex = gameControl.equipmentInventoryList.IndexOf(weapons);
				newEquipmentIndex.text = equipmentInventoryCountIndex.ToString();
				
				//Puts the level of the weapon in the fourth child.
				GameObject newEquipmentQuantityObject = newEquipment.transform.GetChild(3).gameObject;
				Text newEquipmentQuantity = newEquipmentQuantityObject.GetComponent<Text>();
				newEquipmentQuantity.text = weapons.quantity.ToString();
				
				//puts the local index in the fifth child.
				localIndexCount ++;
				GameObject newEquipmentLocalIndex = newEquipment.transform.GetChild(4).gameObject;
				Text equipmentLocalIndex = newEquipmentLocalIndex.GetComponent<Text>();
				equipmentLocalIndex.text = localIndexCount.ToString();
			}
		} else {
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
					
					//puts the local index in the fifth child.
					localIndexCount ++;
					GameObject newEquipmentLocalIndex = newEquipment.transform.GetChild(4).gameObject;
					Text equipmentLocalIndex = newEquipmentLocalIndex.GetComponent<Text>();
					equipmentLocalIndex.text = localIndexCount.ToString();
				}
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
			string equipmentSlot = equipmentDatabase.equipment [PlayerPrefsManager.GetEquipmentID()].equipmentSlot.ToString();
			int equippedID = 0;
			if (equipmentSlot == "Head") {
				equippedID = gameControl.equippedHead;
			} else if (equipmentSlot == "Chest") {
				equippedID = gameControl.equippedChest;
			} else if (equipmentSlot == "Weapon") {
				equippedID = gameControl.equippedWeapon;
			} else if (equipmentSlot == "Pants") {
				equippedID = gameControl.equippedPants;
			} else {
				equippedID = gameControl.equippedFeet;
			}
			strengthStat.text = gameControl.currentStrength + " => " + (gameControl.currentStrength - equipmentDatabase.equipment [equippedID].equipmentStrength + 
							    																	  equipmentDatabase.equipment [selectedEquipmentID].equipmentStrength);
			defenseStat.text = gameControl.currentDefense + " => " + (gameControl.currentDefense - equipmentDatabase.equipment [equippedID].equipmentDefense + 
							   																	   equipmentDatabase.equipment [selectedEquipmentID].equipmentDefense);
			intelligenceStat.text = gameControl.currentIntelligence + " => " + (gameControl.currentIntelligence - equipmentDatabase.equipment [equippedID].equipmentIntelligence + 
																												  equipmentDatabase.equipment [selectedEquipmentID].equipmentIntelligence);
			speedStat.text = gameControl.currentSpeed + " => " + (gameControl.currentSpeed - equipmentDatabase.equipment [equippedID].equipmentSpeed + 
							 																 equipmentDatabase.equipment [selectedEquipmentID].equipmentSpeed);
			healthStat.text = gameControl.currentHealth + " => " + (gameControl.currentHealth - equipmentDatabase.equipment [equippedID].equipmentHealth + 
							  																	equipmentDatabase.equipment [selectedEquipmentID].equipmentHealth);
			manaStat.text = gameControl.currentMana + " => " + (gameControl.currentMana - equipmentDatabase.equipment [equippedID].equipmentMana + 
																						  equipmentDatabase.equipment [selectedEquipmentID].equipmentMana);
			
			//This section gets the stat color to change if the equipment is better or worse.
			//Strength
			if (equipmentDatabase.equipment [equippedID].equipmentStrength > equipmentDatabase.equipment [selectedEquipmentID].equipmentStrength) {
				strengthStat.color = Color.red;
			} else if (equipmentDatabase.equipment [equippedID].equipmentStrength < equipmentDatabase.equipment [selectedEquipmentID].equipmentStrength) {
				strengthStat.color = Color.green;
			} else {
				strengthStat.color = Color.white;
			}
			//Defense
			if (equipmentDatabase.equipment [equippedID].equipmentDefense > equipmentDatabase.equipment [selectedEquipmentID].equipmentDefense) {
				defenseStat.color = Color.red;
			} else if (equipmentDatabase.equipment [equippedID].equipmentDefense < equipmentDatabase.equipment [selectedEquipmentID].equipmentDefense) {
				defenseStat.color = Color.green;
			} else {
				defenseStat.color = Color.white;
			}
			//Speed
			if (equipmentDatabase.equipment [equippedID].equipmentSpeed > equipmentDatabase.equipment [selectedEquipmentID].equipmentSpeed) {
				speedStat.color = Color.red;
			} else if (equipmentDatabase.equipment [equippedID].equipmentSpeed < equipmentDatabase.equipment [selectedEquipmentID].equipmentSpeed) {
				speedStat.color = Color.green;
			} else {
				speedStat.color = Color.white;
			}
			//Intelligence
			if (equipmentDatabase.equipment [equippedID].equipmentIntelligence > equipmentDatabase.equipment [selectedEquipmentID].equipmentIntelligence) {
				intelligenceStat.color = Color.red;
			} else if (equipmentDatabase.equipment [equippedID].equipmentIntelligence < equipmentDatabase.equipment [selectedEquipmentID].equipmentIntelligence) {
				intelligenceStat.color = Color.green;
			} else {
				intelligenceStat.color = Color.white;
			}
			//Health
			if (equipmentDatabase.equipment [equippedID].equipmentHealth > equipmentDatabase.equipment [selectedEquipmentID].equipmentHealth) {
				healthStat.color = Color.red;
			} else if (equipmentDatabase.equipment [equippedID].equipmentHealth < equipmentDatabase.equipment [selectedEquipmentID].equipmentHealth) {
				healthStat.color = Color.green;
			} else {
				healthStat.color = Color.white;
			}
			//Mana
			if (equipmentDatabase.equipment [equippedID].equipmentMana > equipmentDatabase.equipment [selectedEquipmentID].equipmentMana) {
				manaStat.color = Color.red;
			} else if (equipmentDatabase.equipment [equippedID].equipmentMana < equipmentDatabase.equipment [selectedEquipmentID].equipmentMana) {
				manaStat.color = Color.green;
			} else {
				manaStat.color = Color.white;
			}

		} else if (selectedItem.name == "Evolve Weapon") {
			int equippedID = PlayerPrefsManager.GetEquipmentID();
			if (equipmentDatabase.equipment[equippedID].quantity < 8) {
				strengthStat.text = equipmentDatabase.equipment [equippedID].equipmentStrength + " => " + (equipmentDatabase.equipment [equippedID + 1].equipmentStrength);
				defenseStat.text = equipmentDatabase.equipment [equippedID].equipmentDefense + " => " + (equipmentDatabase.equipment [equippedID + 1].equipmentDefense);
				intelligenceStat.text = equipmentDatabase.equipment [equippedID].equipmentIntelligence + " => " + (equipmentDatabase.equipment [equippedID + 1].equipmentIntelligence);
				speedStat.text = equipmentDatabase.equipment [equippedID].equipmentSpeed + " => " + (equipmentDatabase.equipment [equippedID + 1].equipmentSpeed);
				healthStat.text = equipmentDatabase.equipment [equippedID].equipmentHealth + " => " + (equipmentDatabase.equipment [equippedID + 1].equipmentHealth);
				manaStat.text = equipmentDatabase.equipment [equippedID].equipmentMana + " => " + (equipmentDatabase.equipment [equippedID + 1].equipmentMana);
				
				strengthStat.color = Color.green;
				defenseStat.color = Color.green;
				speedStat.color = Color.green;
				intelligenceStat.color = Color.green;
				healthStat.color = Color.green;
				manaStat.color = Color.green;
			}
			
		} else if (selectedItem.name == "Destroy Equipment") {

		} else if (selectedItem.name == "Equip Equipment Yes") {
			
		} else if (selectedItem.name == "Equip Equipment No") {
			
		} else if (selectedItem.name == "Destroy Equipment Yes") {
			
		} else if (selectedItem.name == "Destroy Equipment No") {
			
		} else {
			displayEquipmentName.text = selectedItem.transform.GetChild(0).GetComponent<Text>().text;
			
			//Gets equipment ID as string and converts to int.
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
			
			//pushes the local index to a global variable.
			if (selectedItem.transform.GetChild(4).gameObject) {
				GameObject localEquipmentIndex = selectedItem.transform.GetChild(4).gameObject;
				Text localEquipmentIndexText = localEquipmentIndex.GetComponent<Text>();
				int localIndex = int.Parse(localEquipmentIndexText.text);
				PlayerPrefsManager.SetLocalEquipmentIndex(localIndex);
			}
			
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
			
			strengthStat.color = Color.white;
			defenseStat.color = Color.white;
			speedStat.color = Color.white;
			intelligenceStat.color = Color.white;
			healthStat.color = Color.white;
			manaStat.color = Color.white;
		}
	}
	
	
	public void SelectEquipment () {
		selectedEquipmentMaterialIndex = GetEquipmentMaterialIndex (PlayerPrefsManager.GetEquipmentID());
		gameControl = GameObject.FindObjectOfType<GameControl>();
		equipButton = GameObject.FindGameObjectWithTag("Equip Button").GetComponent<Button>();
		equipButton.interactable = true;
		if (selectedEquipmentMaterialIndex != 5) {
			destroyEquipmentButton = GameObject.FindGameObjectWithTag("Destroy Equipment Button").GetComponent<Button>();
			destroyEquipmentButton.interactable = true;
		}
		contentPanel = GameObject.FindObjectOfType<ContentPanel>();
		contentPanel.DeactivateInventory();	
		EventSystem.current.SetSelectedGameObject(GameObject.FindGameObjectWithTag("Equip Button"),null);
		
		if (gameControl.equipmentMenuLevel == 2) {
			gameControl.equipmentMenuLevel = 3;
		} else {
			gameControl.weaponEvolutionMenuLevel = 2;
		}
	}
	
	
	public int GetEquipmentMaterialIndex (int selectedEquipmentID) {
		equipmentDatabase = GameObject.FindGameObjectWithTag("Equipment Database").GetComponent<EquipmentDatabase>();
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
	
	
	//Have equipment drops call this as needed.
	public void AddEquipmentToInventory (params int[] equipmentNumber) {
		gameControl = GameObject.FindObjectOfType<GameControl>(); 
		if (equipmentList.Count == equipmentSlots) {
			return;
			//present message saying out of space? Might need to be more complicated...
		} else {
			foreach (int equipment in equipmentNumber) {
				if (gameControl.equipmentInventoryList.Contains(equipmentDatabase.equipment[equipment])) {
					int index = gameControl.equipmentInventoryList.IndexOf(equipmentDatabase.equipment[equipment]);
					gameControl.equipmentInventoryList[index].quantity ++;
				} else {
					gameControl.equipmentInventoryList.Add (equipmentDatabase.equipment[equipment]);
				}
			}
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
	
	
	public void EquipEquipmentInInventory () { 
		gameControl = GameObject.FindObjectOfType<GameControl>();		
		int itemIndex = PlayerPrefsManager.GetSelectItem ();
		equipButton = GameObject.FindGameObjectWithTag("Equip Button").GetComponent<Button>();
		destroyEquipmentButton = GameObject.FindGameObjectWithTag("Destroy Equipment Button").GetComponent<Button>();
		classesDatabase = GameObject.FindObjectOfType<ClassesDatabase>();
		
		//Weapons
		if (selectedEquipmentMaterialIndex == 5) {
			gameControl.weaponsList.Add (equipmentDatabase.equipment[gameControl.equippedWeapon]);
			gameControl.equippedWeapon = PlayerPrefsManager.GetEquipmentID();
			gameControl.weaponsList.Remove(equipmentDatabase.equipment [PlayerPrefsManager.GetEquipmentID()]);
			gameControl.CurrentClass();
			
			//checks equipped armor to see if it no longer meets class requirements.
			int classMaterialIndex = classesDatabase.classes[gameControl.playerClass].equipmentMaterialIndex;
			if (classMaterialIndex < GetEquipmentMaterialIndex(gameControl.equippedHead)) {
				AddEquipmentToInventory (gameControl.equippedHead);
				gameControl.equippedHead = 0;
			}
			if (classMaterialIndex < GetEquipmentMaterialIndex(gameControl.equippedChest)) {
				AddEquipmentToInventory (gameControl.equippedChest);
				gameControl.equippedChest = 0;
			}
			if (classMaterialIndex < GetEquipmentMaterialIndex(gameControl.equippedPants)) {
				AddEquipmentToInventory (gameControl.equippedPants);
				gameControl.equippedPants = 0;
			}
			if (classMaterialIndex < GetEquipmentMaterialIndex(gameControl.equippedFeet)) {
				AddEquipmentToInventory (gameControl.equippedFeet);
				gameControl.equippedFeet = 0;
			}
			SetEquippedSlot ();
			AutoEquipClothes ();
			
		//Armor
		} else if (classesDatabase.classes[gameControl.playerClass].equipmentMaterialIndex >= selectedEquipmentMaterialIndex) {
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
				
			} else if (equipmentDatabase.equipment [PlayerPrefsManager.GetEquipmentID()].equipmentType == Equipment.EquipmentType.Feet) {
				if (gameControl.equippedFeet != 4) {
					AddEquipmentToInventory (gameControl.equippedFeet);
				}
				gameControl.equippedFeet = PlayerPrefsManager.GetEquipmentID();
			}
			//updates quantity of equipped armor.
			if (gameControl.equipmentInventoryList [PlayerPrefsManager.GetSelectItem()].quantity > 1) {
				gameControl.equipmentInventoryList [PlayerPrefsManager.GetSelectItem()].quantity --;
			} else {
				gameControl.equipmentInventoryList.RemoveAt (itemIndex);
			}
			SetEquippedSlot ();
			
		//Unable to equip armor.
		} else {
			//write in call to unable to equip method? or just code it in.
		}
		
		contentPanel.DeleteInventory();
		PopulateEquipment(equipmentDatabase.equipment [PlayerPrefsManager.GetEquipmentID()].equipmentSlot);			
		EquipmentInfoDisplay ();
		UpdateEquippedStats ();

		equipButton.interactable = false;
		destroyEquipmentButton.interactable = false;
	}
	
	public void UpdateEquippedStats () {
		gameControl.currentStrength = gameControl.baseStrength + equipmentDatabase.equipment[gameControl.equippedHead].equipmentStrength +
																 equipmentDatabase.equipment[gameControl.equippedChest].equipmentStrength +
																 equipmentDatabase.equipment[gameControl.equippedWeapon].equipmentStrength +
																 equipmentDatabase.equipment[gameControl.equippedPants].equipmentStrength +
																 equipmentDatabase.equipment[gameControl.equippedFeet].equipmentStrength;
		gameControl.currentDefense = gameControl.baseDefense + equipmentDatabase.equipment[gameControl.equippedHead].equipmentDefense +
															   equipmentDatabase.equipment[gameControl.equippedChest].equipmentDefense +
															   equipmentDatabase.equipment[gameControl.equippedWeapon].equipmentDefense +
															   equipmentDatabase.equipment[gameControl.equippedPants].equipmentDefense +
															   equipmentDatabase.equipment[gameControl.equippedFeet].equipmentDefense;
		gameControl.currentSpeed = gameControl.baseSpeed + equipmentDatabase.equipment[gameControl.equippedHead].equipmentSpeed +
														   equipmentDatabase.equipment[gameControl.equippedChest].equipmentSpeed +
														   equipmentDatabase.equipment[gameControl.equippedWeapon].equipmentSpeed +
														   equipmentDatabase.equipment[gameControl.equippedPants].equipmentSpeed +
														   equipmentDatabase.equipment[gameControl.equippedFeet].equipmentSpeed;
		gameControl.currentIntelligence = gameControl.baseIntelligence + equipmentDatabase.equipment[gameControl.equippedHead].equipmentIntelligence +
																		 equipmentDatabase.equipment[gameControl.equippedChest].equipmentIntelligence +
																		 equipmentDatabase.equipment[gameControl.equippedWeapon].equipmentIntelligence +
																		 equipmentDatabase.equipment[gameControl.equippedPants].equipmentIntelligence +
																		 equipmentDatabase.equipment[gameControl.equippedFeet].equipmentIntelligence;
		gameControl.currentHealth = gameControl.baseHealth + equipmentDatabase.equipment[gameControl.equippedHead].equipmentHealth +
															 equipmentDatabase.equipment[gameControl.equippedChest].equipmentHealth +
															 equipmentDatabase.equipment[gameControl.equippedWeapon].equipmentHealth +
															 equipmentDatabase.equipment[gameControl.equippedPants].equipmentHealth +
															 equipmentDatabase.equipment[gameControl.equippedFeet].equipmentHealth;
		gameControl.currentMana = gameControl.baseMana + equipmentDatabase.equipment[gameControl.equippedHead].equipmentMana +
														 equipmentDatabase.equipment[gameControl.equippedChest].equipmentMana +
														 equipmentDatabase.equipment[gameControl.equippedWeapon].equipmentMana +
														 equipmentDatabase.equipment[gameControl.equippedPants].equipmentMana +
														 equipmentDatabase.equipment[gameControl.equippedFeet].equipmentMana;
	}
	
	
	public void DestroyEquipmentInInventory () {
		gameControl.equipmentMenuLevel = 4;
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
		contentPanel = GameObject.FindGameObjectWithTag ("Equipment Content").GetComponent<ContentPanel>();
		destroyEquipmentVerificationCanvas = GameObject.FindGameObjectWithTag("Destroy Equipment Verification Canvas");
		Destroy (destroyEquipmentVerificationCanvas.gameObject);
		contentPanel.ActivateInventory();
		EventSystem.current.SetSelectedGameObject(GameObject.FindGameObjectWithTag("Equipment Content").transform.GetChild(PlayerPrefsManager.GetLocalEquipmentIndex()).gameObject,null);
		Invoke ("SelectEquipment", 0);
	}
}
