using UnityEngine;


[System.Serializable]
public class Classes {

	public string className;
	public int classID;
	public string classDescription;
	public Sprite classIcon;
	[Range (1,4)]
	public int equipmentMaterialIndex;
	public int strengthLevelBonus;
	public int defenseLevelBonus;
	public int speedLevelBonus;
	public int intelligenceLevelBonus;
	public int healthLevelBonus;
	public int manaLevelBonus;

	public Classes (int id, string name, string description, int materialIndex, int strength, int defense, int speed, int intelligence, int health, int mana) {
		classID = id;
		className = name;
		classDescription = description;
		classIcon = Resources.Load<Sprite>("Class Icons/" + name);
		equipmentMaterialIndex = materialIndex;
		strengthLevelBonus = strength;
		defenseLevelBonus = defense;
		speedLevelBonus = speed;
		intelligenceLevelBonus = intelligence;
		healthLevelBonus = health;
		manaLevelBonus = mana;
	}
	
	public Classes () {
		
	}
	
}
