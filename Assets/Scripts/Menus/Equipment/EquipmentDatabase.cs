using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EquipmentDatabase : MonoBehaviour {

	public List<Equipment> equipment; 
	
	void Awake () {
		DontDestroyOnLoad (gameObject);
	}
	//the order for stats is: strength, defense, speed, intelligence, health, mana.
	//there cant be null references in between item IDs so for now they look like this....
	void Start () {
		//placeholder
		equipment.Add (new Equipment (0, "Nothing", "Nothing",Equipment.EquipmentType.Head, Equipment.EquipmentMaterial.Cloth, 0, 0, 0, 0, 0, 0));
	
		//Helmet Section, IDs between 1 and 100.
		equipment.Add (new Equipment (1, "Hat", "Nothing fancy.", Equipment.EquipmentType.Head, Equipment.EquipmentMaterial.Cloth, 0, 1, 0, 0, 1, 0));
		equipment.Add (new Equipment (2, "Helmet", "Study lookin helmet.", Equipment.EquipmentType.Head, Equipment.EquipmentMaterial.Chainmail, 1, 2, 1, 1, 2, 1));
		
		//Chest Section, IDs between 101 and 200.
		equipment.Add (new Equipment (3, "Shirt", "Nothing fancy, regular shirt.", Equipment.EquipmentType.Chest, Equipment.EquipmentMaterial.Cloth, 0, 1, 0, 0, 1, 0));
		equipment.Add (new Equipment (4, "Platemail", "Study lookin chest.", Equipment.EquipmentType.Chest, Equipment.EquipmentMaterial.Platemail, 1, 2, 1, 1, 2, 1));
		
		//Pants Section, IDs between 201 and 300.
		equipment.Add (new Equipment (5, "Skinny Jeans", "Nothing fancy, hipster pants.", Equipment.EquipmentType.Pants, Equipment.EquipmentMaterial.Cloth, 0, 1, 0, 0, 1, 0));
		equipment.Add (new Equipment (6, "Assless Chaps", "Study lookin pants.", Equipment.EquipmentType.Pants, Equipment.EquipmentMaterial.Leather, 1, 2, 1, 1, 2, 1));
		
		//Feet Section, IDs between 301 and 400.
		equipment.Add (new Equipment (7, "Original Converse", "Chuck Taylors Yo.", Equipment.EquipmentType.Feet, Equipment.EquipmentMaterial.Cloth, 0, 1, 0, 0, 1, 0));
		equipment.Add (new Equipment (8, "Air Jordans", "Study lookin shoes.", Equipment.EquipmentType.Feet, Equipment.EquipmentMaterial.Leather, 1, 2, 1, 1, 2, 1));
	}
}
