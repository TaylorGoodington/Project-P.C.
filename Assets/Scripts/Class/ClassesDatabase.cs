using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ClassesDatabase : MonoBehaviour {
	
	public List<Classes> classes; 
	
	void Awake () {
		DontDestroyOnLoad (gameObject);
	}
	//Just before stats is the material equipment index, 1 = cloth, 2 = leather, 3 = chainmail, 4 = platemail.
	//the order for stats is: strength, defense, speed, intelligence, health, mana.
	//there cant be null references in between item IDs so for now they look like this....
	void Start () {
		//Stats need to be re done to reflect new info on the drive.
		classes.Add (new Classes (0, "Soldier", "The way of the sword", 3, 4, 3, 2, 1, 5, 1));
		classes.Add (new Classes (1, "Berserker", "The way of the great axe", 3, 5, 3, 3, 1, 2, 2));
		classes.Add (new Classes (2, "Rogue", "The way of the dagger", 2, 3, 1, 5, 3, 3, 1));
		classes.Add (new Classes (3, "Ranger", "The way of the bow", 2, 3, 1, 4, 3, 2, 3));
		classes.Add (new Classes (4, "Wizard", "The way of the tome", 1, 1, 2, 1, 5, 3, 4));
		classes.Add (new Classes (5, "Sorcerer", "The way of the talisman", 1, 2, 2, 2, 4, 1, 5));
		classes.Add (new Classes (6, "Monk", "The way of the staff", 4, 1, 4, 3, 2, 2, 4));
		classes.Add (new Classes (7, "Paladin", "The way of the polearm", 4, 2, 5, 1, 2, 4, 2));
	}
}
