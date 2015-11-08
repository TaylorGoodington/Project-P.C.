using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EquipmentDatabase : MonoBehaviour {

	public List<Equipment> equipment; 
	
	void Awake () {
		DontDestroyOnLoad (gameObject);
	}
	//the order for stats is: strength, defense, speed, intelligence, health, mana.
	void Start () {
		equipment.Add (new Equipment (0, "Nothing", "Nothing", Equipment.EquipmentType.Head, Equipment.EquipmentMaterial.Cloth, Equipment.EquipmentSlot.Head, 0, 0, 0, 0, 1, 0, 1, 0, 0, 1));
		
		//inital four that are equipped when nothing else is...They also dont show up in the inventory
		equipment.Add (new Equipment (1, "Hat", "Nothing fancy.", Equipment.EquipmentType.Head, Equipment.EquipmentMaterial.Cloth, Equipment.EquipmentSlot.Head, 0, 1, 0, 0, 1, 0, 1, 0, 0, 1));
		equipment.Add (new Equipment (2, "Shirt", "Nothing fancy, regular shirt.", Equipment.EquipmentType.Chest, Equipment.EquipmentMaterial.Cloth, Equipment.EquipmentSlot.Chest, 0, 1, 0, 0, 1, 0, 1, 0, 0, 1));
		equipment.Add (new Equipment (3, "Skinny Jeans", "Nothing fancy, hipster pants.", Equipment.EquipmentType.Pants, Equipment.EquipmentMaterial.Cloth, Equipment.EquipmentSlot.Pants, 0, 1, 0, 0, 1, 0, 1, 0, 0, 1));
		equipment.Add (new Equipment (4, "Original Converse", "Chuck Taylors Yo.", Equipment.EquipmentType.Feet, Equipment.EquipmentMaterial.Cloth, Equipment.EquipmentSlot.Feet, 0, 1, 0, 0, 1, 0, 1, 0, 0, 1));
		
		equipment.Add (new Equipment (5, "Helmet", "Study lookin helmet.", Equipment.EquipmentType.Head, Equipment.EquipmentMaterial.Chainmail, Equipment.EquipmentSlot.Head, 1, 2, 1, 1, 2, 1, 1, 0, 100, 1));
		equipment.Add (new Equipment (6, "Platemail", "Study lookin chest.", Equipment.EquipmentType.Chest, Equipment.EquipmentMaterial.Platemail, Equipment.EquipmentSlot.Chest, 1, 2, 1, 1, 2, 1, 1, 0, 100, 1));
		equipment.Add (new Equipment (7, "Assless Chaps", "Study lookin pants.", Equipment.EquipmentType.Pants, Equipment.EquipmentMaterial.Leather, Equipment.EquipmentSlot.Pants, 1, 2, 1, 1, 2, 1, 1, 0, 100, 1));
		equipment.Add (new Equipment (8, "Air Jordans", "Study lookin shoes.", Equipment.EquipmentType.Feet, Equipment.EquipmentMaterial.Leather, Equipment.EquipmentSlot.Feet, 1, 2, 1, 1, 2, 1, 1, 0, 100, 1));
		
		equipment.Add (new Equipment (9, "Sword", "Basic copper sword", Equipment.EquipmentType.Sword, Equipment.EquipmentMaterial.Soldier, Equipment.EquipmentSlot.Weapon, 1, 1, 1, 1, 2, 1, 1, 0, 100, 1));
		equipment.Add (new Equipment (10, "Axe", "Basic copper axe", Equipment.EquipmentType.Axe, Equipment.EquipmentMaterial.Berserker, Equipment.EquipmentSlot.Weapon, 2, 1, 1, 0, 1, 0, 1.2f, 0, 100, 1));
		equipment.Add (new Equipment (11, "Dagger", "Basic copper dagger", Equipment.EquipmentType.Dagger, Equipment.EquipmentMaterial.Rogue, Equipment.EquipmentSlot.Weapon, 0, 0, 2, 0, 1, 0, 0.5f, 0, 100, 1));
		equipment.Add (new Equipment (12, "Bow", "Basic wooden bow", Equipment.EquipmentType.Bow, Equipment.EquipmentMaterial.Ranger, Equipment.EquipmentSlot.Weapon, 1, 0, 2, 1, 1, 1, 0.75f, 0, 100, 1));
		equipment.Add (new Equipment (13, "Knuckles", "Basic brass knuckles", Equipment.EquipmentType.Fist, Equipment.EquipmentMaterial.Monk, Equipment.EquipmentSlot.Weapon, 0, 0, 0, 2, 1, 2, 1, 0, 100, 1));
		equipment.Add (new Equipment (14, "Talisman", "Basic dusty talisman", Equipment.EquipmentType.Talisman, Equipment.EquipmentMaterial.Sorcerer, Equipment.EquipmentSlot.Weapon, 0, 0, 0, 2, 1, 2, 1, 0, 100, 1));
		equipment.Add (new Equipment (15, "Staff", "Basic wooden staff", Equipment.EquipmentType.Staff, Equipment.EquipmentMaterial.Wizard, Equipment.EquipmentSlot.Weapon, 2, 2, 1, 0, 1, 0, 1.2f, 0, 100, 1));
		equipment.Add (new Equipment (16, "Polearm", "Basic copper polearm", Equipment.EquipmentType.Polearm, Equipment.EquipmentMaterial.Paladin, Equipment.EquipmentSlot.Weapon, 2, 3, 0, 0, 1, 0, 1.5f, 0, 100, 1));
	}
}