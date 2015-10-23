using UnityEngine;
using System.Collections;


[System.Serializable]
public class Equipment {
	
	public string equipmentName;
	public int equipmentID;
	public string equipmentDescription;
	public Sprite equipmentIcon;
	public EquipmentType equipmentType;
	public EquipmentMaterial equipmentMaterial;
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
	
	public enum EquipmentMaterial {
		Cloth,
		Leather,
		Chainmail,
		Platemail
	}
	
	
	public Equipment (int id, string name, string description, EquipmentType type, EquipmentMaterial material, int strength, int defense, int speed, int intelligence, int health, int mana) {
		equipmentID = id;
		equipmentName = name;
		equipmentDescription = description;
		equipmentIcon = Resources.Load<Sprite>("Equipment Icons/" + name);
		equipmentType = type;
		equipmentMaterial = material;
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
