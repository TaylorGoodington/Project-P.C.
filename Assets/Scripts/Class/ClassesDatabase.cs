using UnityEngine;
using System.Collections.Generic;

public class ClassesDatabase : MonoBehaviour {
	
	public List<Classes> classes; 
	
	void Awake () {
		//DontDestroyOnLoad (gameObject);
	}
	//Just before stats is the material equipment index, 1 = cloth, 2 = leather, 3 = chainmail, 4 = platemail.
	//the order for stats is: strength, defense, speed, intelligence, health, mana.
	void Start () {
		classes.Add (new Classes (0, "Soldier", "The way of the sword", 3, 5, 4, 3, 2, 6 , 1));
		classes.Add (new Classes (1, "Berserker", "The way of the axe", 3, 6, 4, 4, 1, 3, 3));
		classes.Add (new Classes (2, "Rogue", "The way of the dagger", 2, 4, 1, 6, 4, 4, 2));
		classes.Add (new Classes (3, "Ranger", "The way of the bow", 2, 4, 2, 5, 4, 2, 4));
		classes.Add (new Classes (4, "Wizard", "The way of the staff", 1, 1, 3, 2, 6, 4, 5));
		classes.Add (new Classes (5, "Sorcerer", "The way of the talisman", 1, 3, 3, 3, 5, 1, 6));
		classes.Add (new Classes (6, "Monk", "The way of the fist", 4, 2, 5, 4, 3, 3, 4));
		classes.Add (new Classes (7, "Paladin", "The way of the polearm", 4, 3, 6, 1, 3 , 5, 3));
	}
}
