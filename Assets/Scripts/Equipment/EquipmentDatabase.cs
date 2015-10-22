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
		//Helmet Section, IDs between 000 and 099.
		equipment.Add (new Equipment (0, "Hat", "Nothing fancy.", Equipment.EquipmentType.Head, 0, 1, 0, 0, 1, 0));
		equipment.Add (new Equipment (1, "Helmet", "Study lookin helmet.", Equipment.EquipmentType.Head, 1, 2, 1, 1, 2, 1));
		
		//Chest Section, IDs between 100 and 199.
		equipment.Add (new Equipment (2, "Shirt", "Nothing fancy, regular shirt.", Equipment.EquipmentType.Chest, 0, 1, 0, 0, 1, 0));
		equipment.Add (new Equipment (3, "Platemail", "Study lookin chest.", Equipment.EquipmentType.Chest, 1, 2, 1, 1, 2, 1));
		
		//Pants Section, IDs between 200 and 299.
		equipment.Add (new Equipment (4, "Skinny Jeans", "Nothing fancy, hipster pants.", Equipment.EquipmentType.Pants, 0, 1, 0, 0, 1, 0));
		equipment.Add (new Equipment (5, "Assless Chaps", "Study lookin pants.", Equipment.EquipmentType.Pants, 1, 2, 1, 1, 2, 1));
		
		//Feet Section, IDs between 300 and 399.
		equipment.Add (new Equipment (6, "Original Converse", "Chuck Taylors Yo.", Equipment.EquipmentType.Feet, 0, 1, 0, 0, 1, 0));
		equipment.Add (new Equipment (7, "Air Jordans", "Study lookin shoes.", Equipment.EquipmentType.Feet, 1, 2, 1, 1, 2, 1));
	}
}
