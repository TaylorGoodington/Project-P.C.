using UnityEngine;
using System.Collections;


[System.Serializable]
public class Equipment {
	
	public string equipmentName;
	public int equipmentID;
	public string equipmentDescription;
	public Sprite equipmentIcon;
	public EquipmentType equipmentType;
	public int equipmentStrength;
	public int equipmentDefense;
	public int equipmentSpeed;
	public int equipmentIntelligence;
	public int equipmentHealth;
	public int equipmentMana;

	
	public enum EquipmentType { 
		Head,
		Chest,
		Pants,
		Feet
	}
	
	
	public Equipment (int id, string name, string description, EquipmentType type, int strength, int defense, int speed, int intelligence, int health, int mana) {
		equipmentID = id;
		equipmentName = name;
		equipmentDescription = description;
		equipmentIcon = Resources.Load<Sprite>("Item Icons/" + name);
		equipmentType = type;
		equipmentStrength = strength;
		equipmentDefense = defense;
		equipmentSpeed = speed;
		equipmentIntelligence = intelligence;
		equipmentHealth = health;
		equipmentMana = mana;
	}
	
	public Equipment () {
		
	}
	
}
