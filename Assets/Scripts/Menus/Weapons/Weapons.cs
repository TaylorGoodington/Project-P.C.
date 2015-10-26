using UnityEngine;
using System.Collections;


[System.Serializable]
public class Weapons {
	
	public string weaponName;
	public int weaponID;
	public string weaponDescription;
	public Sprite weaponIcon;
	public WeaponType equipmentType;
	public int equipmentStrength;
	public int equipmentDefense;
	public int equipmentSpeed;
	public int equipmentIntelligence;
	public int equipmentHealth;
	public int equipmentMana;
	
	
	public enum WeaponType { 
		Sword
	}
	
	
	public Weapons (int id, string name, string description, WeaponType type, int strength, int defense, int speed, int intelligence, int health, int mana) {
		weaponID = id;
		weaponName = name;
		weaponDescription = description;
		weaponIcon = Resources.Load<Sprite>("Equipment Icons/" + name);
		equipmentType = type;
		equipmentStrength = strength;
		equipmentDefense = defense;
		equipmentSpeed = speed;
		equipmentIntelligence = intelligence;
		equipmentHealth = health;
		equipmentMana = mana;
	}
	
	public Weapons () {
		
	}
	
}
