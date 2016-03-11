using UnityEngine;


[System.Serializable]
public class Equipment {
	
	public string equipmentName;
	public int equipmentID;
	public string equipmentDescription;
	public EquipmentType equipmentType;
	public EquipmentMaterial equipmentMaterial;
	public EquipmentSlot equipmentSlot;
	public int equipmentStrength;
	public int equipmentDefense;
	public int equipmentSpeed;
	public int equipmentIntelligence;
	public int equipmentHealth;
	public int equipmentMana;
	public float attackSpeedModifier;
	public int equipmentSkill;
	public int buyPrice;
	public int quantity;
    public int dropRate;

	
	public enum EquipmentType { 
		Head,
		Chest,
		Pants,
		Feet,
		Sword,
		Axe,
		Dagger,
		Bow,
		Fist,
		Talisman,
		Staff,
		Polearm
	}
	
	public enum EquipmentMaterial {
		Cloth,
		Leather,
		Chainmail,
		Platemail,
		Soldier,
		Berserker,
		Rogue,
		Ranger,
		Wizard,
		Sorcerer,
		Monk,
		Paladin
	}
	
	public enum EquipmentSlot {
		Head,
		Chest,
		Weapon,
		Pants,
		Feet
	}
	
	
	public Equipment (int id, string name, string description, EquipmentType type, EquipmentMaterial material, EquipmentSlot slot, 
					  int strength, int defense, int speed, int intelligence, int health, int mana,
					  float atkMod, int skill, int price, int qty, int drop) {
		equipmentID = id;
		equipmentName = name;
		equipmentDescription = description;
		equipmentType = type;
		equipmentMaterial = material;
		equipmentSlot = slot;
		equipmentStrength = strength;
		equipmentDefense = defense;
		equipmentSpeed = speed;
		equipmentIntelligence = intelligence;
		equipmentHealth = health;
		equipmentMana = mana;
		attackSpeedModifier = atkMod;
		equipmentSkill = skill;
		buyPrice = price;
		quantity = qty;
        dropRate = drop;
	}
	
	public Equipment () {
		
	}
	
}
