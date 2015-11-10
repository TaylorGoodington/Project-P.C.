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
		equipment.Add (new Equipment (1, "Hat", "Hat.", Equipment.EquipmentType.Head, Equipment.EquipmentMaterial.Cloth, Equipment.EquipmentSlot.Head, 0, 1, 0, 0, 1, 0, 1f, 0, 100, 1));
		equipment.Add (new Equipment (2, "Shirt", "Shirt.", Equipment.EquipmentType.Chest, Equipment.EquipmentMaterial.Cloth, Equipment.EquipmentSlot.Chest, 0, 1, 0, 0, 1, 0, 1f, 0, 100, 1));
		equipment.Add (new Equipment (3, "Skinny Jeans", "Pants.", Equipment.EquipmentType.Pants, Equipment.EquipmentMaterial.Cloth, Equipment.EquipmentSlot.Pants, 0, 1, 0, 0, 1, 0, 1f, 0, 100, 1));
		equipment.Add (new Equipment (4, "Original Converse", "Shoes.", Equipment.EquipmentType.Feet, Equipment.EquipmentMaterial.Cloth, Equipment.EquipmentSlot.Feet, 0, 1, 0, 0, 1, 0, 1f, 0, 100, 1));
		
		equipment.Add (new Equipment (5, "Helmet", "Sturdy Helmet", Equipment.EquipmentType.Head, Equipment.EquipmentMaterial.Chainmail, Equipment.EquipmentSlot.Head, 0, 3, 0, 0, 3, 0, 1f, 0, 100, 1));
		equipment.Add (new Equipment (6, "Platemail", "Sturdy Chest", Equipment.EquipmentType.Chest, Equipment.EquipmentMaterial.Platemail, Equipment.EquipmentSlot.Chest, 0, 4, 0, 0, 4, 0, 1f, 0, 100, 1));
		equipment.Add (new Equipment (7, "Assless Chaps", "Tiny Pants", Equipment.EquipmentType.Pants, Equipment.EquipmentMaterial.Leather, Equipment.EquipmentSlot.Pants, 0, 2, 5, 0, 2, 0, 1f, 0, 100, 1));
		equipment.Add (new Equipment (8, "Air Jordans", "Ice Cream Paint Job", Equipment.EquipmentType.Feet, Equipment.EquipmentMaterial.Platemail, Equipment.EquipmentSlot.Feet, 0, 4, 0, 0, 4, 0, 1f, 0, 100, 1));
		
		equipment.Add (new Equipment (9, "Sword", "Basic copper sword", Equipment.EquipmentType.Sword, Equipment.EquipmentMaterial.Soldier, Equipment.EquipmentSlot.Weapon, 1, 2, 1, 1, 2, 1, 1f, 0, 100, 1));
		equipment.Add (new Equipment (10, "Sword 2", "Sword 2", Equipment.EquipmentType.Sword, Equipment.EquipmentMaterial.Soldier, Equipment.EquipmentSlot.Weapon, 2, 4, 2, 2, 4, 2, 0.95f, 0, 100, 2));
		equipment.Add (new Equipment (11, "Sword 3", "Sword 3", Equipment.EquipmentType.Sword, Equipment.EquipmentMaterial.Soldier, Equipment.EquipmentSlot.Weapon, 4, 8, 4, 4, 8, 4, 0.9f, 0, 100, 3));
		equipment.Add (new Equipment (12, "Sword 4", "Sword 4", Equipment.EquipmentType.Sword, Equipment.EquipmentMaterial.Soldier, Equipment.EquipmentSlot.Weapon, 8, 16, 8, 8, 16, 8, 0.85f, 0, 100, 4));
		equipment.Add (new Equipment (13, "Sword 5", "Sword 5", Equipment.EquipmentType.Sword, Equipment.EquipmentMaterial.Soldier, Equipment.EquipmentSlot.Weapon, 16, 32, 16, 16, 32, 16, 0.8f, 0, 100, 5));
		equipment.Add (new Equipment (14, "Sword 6", "Sword 6", Equipment.EquipmentType.Sword, Equipment.EquipmentMaterial.Soldier, Equipment.EquipmentSlot.Weapon, 32, 64, 32, 32, 64, 32, 0.75f, 0, 100, 6));
		equipment.Add (new Equipment (15, "Sword 7", "Sword 7", Equipment.EquipmentType.Sword, Equipment.EquipmentMaterial.Soldier, Equipment.EquipmentSlot.Weapon, 64, 128, 64, 64, 128, 64, 0.7f, 0, 100, 7));
		equipment.Add (new Equipment (16, "Sword 8", "Sword 8", Equipment.EquipmentType.Sword, Equipment.EquipmentMaterial.Soldier, Equipment.EquipmentSlot.Weapon, 128, 256, 128, 128, 256, 128, 0.65f, 100, 100, 8));
		equipment.Add (new Equipment (17, "Axe", "Axe", Equipment.EquipmentType.Axe, Equipment.EquipmentMaterial.Berserker, Equipment.EquipmentSlot.Weapon, 2, 2, 1, 1, 1, 1, 1.1f, 0, 100, 1));
		equipment.Add (new Equipment (18, "Axe 2", "Axe 2", Equipment.EquipmentType.Axe, Equipment.EquipmentMaterial.Berserker, Equipment.EquipmentSlot.Weapon, 4, 4, 2, 2, 2, 2, 1.05f, 0, 100, 2));
		equipment.Add (new Equipment (19, "Axe 3", "Axe 3", Equipment.EquipmentType.Axe, Equipment.EquipmentMaterial.Berserker, Equipment.EquipmentSlot.Weapon, 8, 8, 4, 4, 4, 4, 1f, 0, 100, 3));
		equipment.Add (new Equipment (20, "Axe 4", "Axe 4", Equipment.EquipmentType.Axe, Equipment.EquipmentMaterial.Berserker, Equipment.EquipmentSlot.Weapon, 16, 16, 8, 8, 8, 8, 0.95f, 0, 100, 4));
		equipment.Add (new Equipment (21, "Axe 5", "Axe 5", Equipment.EquipmentType.Axe, Equipment.EquipmentMaterial.Berserker, Equipment.EquipmentSlot.Weapon, 32, 32, 16, 16, 16, 16, 0.9f, 0, 100, 5));
		equipment.Add (new Equipment (22, "Axe 6", "Axe 6", Equipment.EquipmentType.Axe, Equipment.EquipmentMaterial.Berserker, Equipment.EquipmentSlot.Weapon, 64, 64, 32, 32, 32, 32, 0.85f, 0, 100, 6));
		equipment.Add (new Equipment (23, "Axe 7", "Axe 7", Equipment.EquipmentType.Axe, Equipment.EquipmentMaterial.Berserker, Equipment.EquipmentSlot.Weapon, 128, 128, 64, 64, 64, 64, 0.8f, 0, 100, 7));
		equipment.Add (new Equipment (24, "Axe 8", "Axe 8", Equipment.EquipmentType.Axe, Equipment.EquipmentMaterial.Berserker, Equipment.EquipmentSlot.Weapon, 256, 256, 128, 128, 128, 128, 0.75f, 100, 100, 8));
		equipment.Add (new Equipment (25, "Dagger", "Dagger", Equipment.EquipmentType.Dagger, Equipment.EquipmentMaterial.Rogue, Equipment.EquipmentSlot.Weapon, 1, 1, 2, 2, 1, 1, 0.75f, 0, 100, 1));
		equipment.Add (new Equipment (26, "Dagger 2", "Dagger 2", Equipment.EquipmentType.Dagger, Equipment.EquipmentMaterial.Rogue, Equipment.EquipmentSlot.Weapon, 2, 2, 4, 4, 2, 2, 0.7f, 0, 100, 2));
		equipment.Add (new Equipment (27, "Dagger 3", "Dagger 3", Equipment.EquipmentType.Dagger, Equipment.EquipmentMaterial.Rogue, Equipment.EquipmentSlot.Weapon, 4, 4, 8, 8, 4, 4, 0.65f, 0, 100, 3));
		equipment.Add (new Equipment (28, "Dagger 4", "Dagger 4", Equipment.EquipmentType.Dagger, Equipment.EquipmentMaterial.Rogue, Equipment.EquipmentSlot.Weapon, 8, 8, 16, 16, 8, 8, 0.6f, 0, 100, 4));
		equipment.Add (new Equipment (29, "Dagger 5", "Dagger 5", Equipment.EquipmentType.Dagger, Equipment.EquipmentMaterial.Rogue, Equipment.EquipmentSlot.Weapon, 16, 16, 32, 32, 16, 16, 0.55f, 0, 100, 5));
		equipment.Add (new Equipment (30, "Dagger 6", "Dagger 6", Equipment.EquipmentType.Dagger, Equipment.EquipmentMaterial.Rogue, Equipment.EquipmentSlot.Weapon, 32, 32, 64, 64, 32, 32, 0.5f, 0, 100, 6));
		equipment.Add (new Equipment (31, "Dagger 7", "Dagger 7", Equipment.EquipmentType.Dagger, Equipment.EquipmentMaterial.Rogue, Equipment.EquipmentSlot.Weapon, 64, 64, 128, 128, 64, 64, 0.45f, 0, 100, 7));
		equipment.Add (new Equipment (32, "Dagger 8", "Dagger 8", Equipment.EquipmentType.Dagger, Equipment.EquipmentMaterial.Rogue, Equipment.EquipmentSlot.Weapon, 128, 128, 256, 256, 128, 128, 0.4f, 100, 100, 8));
		equipment.Add (new Equipment (33, "Bow", "Bow", Equipment.EquipmentType.Bow, Equipment.EquipmentMaterial.Ranger, Equipment.EquipmentSlot.Weapon, 1, 1, 2, 2, 1, 1, 0.75f, 0, 100, 1));
		equipment.Add (new Equipment (34, "Bow 2", "Bow 2", Equipment.EquipmentType.Bow, Equipment.EquipmentMaterial.Ranger, Equipment.EquipmentSlot.Weapon, 2, 2, 4, 4, 2, 2, 0.7f, 0, 100, 2));
		equipment.Add (new Equipment (35, "Bow 3", "Bow 3", Equipment.EquipmentType.Bow, Equipment.EquipmentMaterial.Ranger, Equipment.EquipmentSlot.Weapon, 4, 4, 8, 8, 4, 4, 0.65f, 0, 100, 3));
		equipment.Add (new Equipment (36, "Bow 4", "Bow 4", Equipment.EquipmentType.Bow, Equipment.EquipmentMaterial.Ranger, Equipment.EquipmentSlot.Weapon, 8, 8, 16, 16, 8, 8, 0.6f, 0, 100, 4));
		equipment.Add (new Equipment (37, "Bow 5", "Bow 5", Equipment.EquipmentType.Bow, Equipment.EquipmentMaterial.Ranger, Equipment.EquipmentSlot.Weapon, 16, 16, 32, 32, 16, 16, 0.55f, 0, 100, 5));
		equipment.Add (new Equipment (38, "Bow 6", "Bow 6", Equipment.EquipmentType.Bow, Equipment.EquipmentMaterial.Ranger, Equipment.EquipmentSlot.Weapon, 32, 32, 64, 64, 32, 32, 0.5f, 0, 100, 6));
		equipment.Add (new Equipment (39, "Bow 7", "Bow 7", Equipment.EquipmentType.Bow, Equipment.EquipmentMaterial.Ranger, Equipment.EquipmentSlot.Weapon, 64, 64, 128, 128, 64, 64, 0.45f, 0, 100, 7));
		equipment.Add (new Equipment (40, "Bow 8", "Bow 8", Equipment.EquipmentType.Bow, Equipment.EquipmentMaterial.Ranger, Equipment.EquipmentSlot.Weapon, 128, 128, 256, 256, 128, 128, 0.4f, 100, 100, 8));
		equipment.Add (new Equipment (41, "Fist", "Fist", Equipment.EquipmentType.Fist, Equipment.EquipmentMaterial.Monk, Equipment.EquipmentSlot.Weapon, 1, 2, 2, 1, 1, 1, 0.5f, 0, 100, 1));
		equipment.Add (new Equipment (42, "Fist 2", "Fist 2", Equipment.EquipmentType.Fist, Equipment.EquipmentMaterial.Monk, Equipment.EquipmentSlot.Weapon, 2, 4, 4, 2, 2, 2, 0.45f, 0, 100, 2));
		equipment.Add (new Equipment (43, "Fist 3", "Fist 3", Equipment.EquipmentType.Fist, Equipment.EquipmentMaterial.Monk, Equipment.EquipmentSlot.Weapon, 4, 8, 8, 4, 4, 4, 0.4f, 0, 100, 3));
		equipment.Add (new Equipment (44, "Fist 4", "Fist 4", Equipment.EquipmentType.Fist, Equipment.EquipmentMaterial.Monk, Equipment.EquipmentSlot.Weapon, 8, 16, 16, 8, 8, 8, 0.35f, 0, 100, 4));
		equipment.Add (new Equipment (45, "Fist 5", "Fist 5", Equipment.EquipmentType.Fist, Equipment.EquipmentMaterial.Monk, Equipment.EquipmentSlot.Weapon, 16, 32, 32, 16, 16, 16, 0.3f, 0, 100, 5));
		equipment.Add (new Equipment (46, "Fist 6", "Fist 6", Equipment.EquipmentType.Fist, Equipment.EquipmentMaterial.Monk, Equipment.EquipmentSlot.Weapon, 32, 64, 64, 32, 32, 32, 0.25f, 0, 100, 6));
		equipment.Add (new Equipment (47, "Fist 7", "Fist 7", Equipment.EquipmentType.Fist, Equipment.EquipmentMaterial.Monk, Equipment.EquipmentSlot.Weapon, 64, 128, 128, 64, 64, 64, 0.2f, 0, 100, 7));
		equipment.Add (new Equipment (48, "Fist 8", "Fist 8", Equipment.EquipmentType.Fist, Equipment.EquipmentMaterial.Monk, Equipment.EquipmentSlot.Weapon, 128, 256, 256, 128, 128, 128, 0.15f, 100, 100, 8));
		equipment.Add (new Equipment (49, "Talisman", "Talisman", Equipment.EquipmentType.Talisman, Equipment.EquipmentMaterial.Sorcerer, Equipment.EquipmentSlot.Weapon, 1, 1, 1, 2, 1, 2, 1f, 0, 100, 1));
		equipment.Add (new Equipment (50, "Talisman 2", "Talisman 2", Equipment.EquipmentType.Talisman, Equipment.EquipmentMaterial.Sorcerer, Equipment.EquipmentSlot.Weapon, 2, 2, 2, 4, 2, 4, 0.95f, 0, 100, 2));
		equipment.Add (new Equipment (51, "Talisman 3", "Talisman 3", Equipment.EquipmentType.Talisman, Equipment.EquipmentMaterial.Sorcerer, Equipment.EquipmentSlot.Weapon, 4, 4, 4, 8, 4, 8, 0.9f, 0, 100, 3));
		equipment.Add (new Equipment (52, "Talisman 4", "Talisman 4", Equipment.EquipmentType.Talisman, Equipment.EquipmentMaterial.Sorcerer, Equipment.EquipmentSlot.Weapon, 8, 8, 8, 16, 8, 16, 0.85f, 0, 100, 4));
		equipment.Add (new Equipment (53, "Talisman 5", "Talisman 5", Equipment.EquipmentType.Talisman, Equipment.EquipmentMaterial.Sorcerer, Equipment.EquipmentSlot.Weapon, 16, 16, 16, 32, 16, 32, 0.8f, 0, 100, 5));
		equipment.Add (new Equipment (54, "Talisman 6", "Talisman 6", Equipment.EquipmentType.Talisman, Equipment.EquipmentMaterial.Sorcerer, Equipment.EquipmentSlot.Weapon, 32, 32, 32, 64, 32, 64, 0.75f, 0, 100, 6));
		equipment.Add (new Equipment (55, "Talisman 7", "Talisman 7", Equipment.EquipmentType.Talisman, Equipment.EquipmentMaterial.Sorcerer, Equipment.EquipmentSlot.Weapon, 64, 64, 64, 128, 64, 128, 0.7f, 0, 100, 7));
		equipment.Add (new Equipment (56, "Talisman 8", "Talisman 8", Equipment.EquipmentType.Talisman, Equipment.EquipmentMaterial.Sorcerer, Equipment.EquipmentSlot.Weapon, 128, 128, 128, 256, 128, 256, 0.65f, 100, 100, 8));
		equipment.Add (new Equipment (57, "Staff", "Staff", Equipment.EquipmentType.Staff, Equipment.EquipmentMaterial.Wizard, Equipment.EquipmentSlot.Weapon, 1, 1, 1, 2, 1, 2, 1f, 0, 100, 1));
		equipment.Add (new Equipment (58, "Staff 2", "Staff 2", Equipment.EquipmentType.Staff, Equipment.EquipmentMaterial.Wizard, Equipment.EquipmentSlot.Weapon, 2, 2, 2, 4, 2, 4, 0.95f, 0, 100, 2));
		equipment.Add (new Equipment (59, "Staff 3", "Staff 3", Equipment.EquipmentType.Staff, Equipment.EquipmentMaterial.Wizard, Equipment.EquipmentSlot.Weapon, 4, 4, 4, 8, 4, 8, 0.9f, 0, 100, 3));
		equipment.Add (new Equipment (60, "Staff 4", "Staff 4", Equipment.EquipmentType.Staff, Equipment.EquipmentMaterial.Wizard, Equipment.EquipmentSlot.Weapon, 8, 8, 8, 16, 8, 16, 0.85f, 0, 100, 4));
		equipment.Add (new Equipment (61, "Staff 5", "Staff 5", Equipment.EquipmentType.Staff, Equipment.EquipmentMaterial.Wizard, Equipment.EquipmentSlot.Weapon, 16, 16, 16, 32, 16, 32, 0.8f, 0, 100, 5));
		equipment.Add (new Equipment (62, "Staff 6", "Staff 6", Equipment.EquipmentType.Staff, Equipment.EquipmentMaterial.Wizard, Equipment.EquipmentSlot.Weapon, 32, 32, 32, 64, 32, 64, 0.75f, 0, 100, 6));
		equipment.Add (new Equipment (63, "Staff 7", "Staff 7", Equipment.EquipmentType.Staff, Equipment.EquipmentMaterial.Wizard, Equipment.EquipmentSlot.Weapon, 64, 64, 64, 128, 64, 128, 0.7f, 0, 100, 7));
		equipment.Add (new Equipment (64, "Staff 8", "Staff 8", Equipment.EquipmentType.Staff, Equipment.EquipmentMaterial.Wizard, Equipment.EquipmentSlot.Weapon, 128, 128, 128, 256, 128, 256, 0.65f, 100, 100, 8));
		equipment.Add (new Equipment (65, "Polearm", "Polearm", Equipment.EquipmentType.Polearm, Equipment.EquipmentMaterial.Paladin, Equipment.EquipmentSlot.Weapon, 1, 2, 1, 1, 2, 1, 1.3f, 0, 100, 1));
		equipment.Add (new Equipment (66, "Polearm 2", "Polearm 2", Equipment.EquipmentType.Polearm, Equipment.EquipmentMaterial.Paladin, Equipment.EquipmentSlot.Weapon, 2, 4, 2, 2, 4, 2, 1.25f, 0, 100, 2));
		equipment.Add (new Equipment (67, "Polearm 3", "Polearm 3", Equipment.EquipmentType.Polearm, Equipment.EquipmentMaterial.Paladin, Equipment.EquipmentSlot.Weapon, 4, 8, 4, 4, 8, 4, 1.2f, 0, 100, 3));
		equipment.Add (new Equipment (68, "Polearm 4", "Polearm 4", Equipment.EquipmentType.Polearm, Equipment.EquipmentMaterial.Paladin, Equipment.EquipmentSlot.Weapon, 8, 16, 8, 8, 16, 8, 1.15f, 0, 100, 4));
		equipment.Add (new Equipment (69, "Polearm 5", "Polearm 5", Equipment.EquipmentType.Polearm, Equipment.EquipmentMaterial.Paladin, Equipment.EquipmentSlot.Weapon, 16, 32, 16, 16, 32, 16, 1.1f, 0, 100, 5));
		equipment.Add (new Equipment (70, "Polearm 6", "Polearm 6", Equipment.EquipmentType.Polearm, Equipment.EquipmentMaterial.Paladin, Equipment.EquipmentSlot.Weapon, 32, 64, 32, 32, 64, 32, 1.05f, 0, 100, 6));
		equipment.Add (new Equipment (71, "Polearm 7", "Polearm 7", Equipment.EquipmentType.Polearm, Equipment.EquipmentMaterial.Paladin, Equipment.EquipmentSlot.Weapon, 64, 128, 64, 64, 128, 64, 1f, 0, 100, 7));
		equipment.Add (new Equipment (72, "Polearm 8", "Polearm 8", Equipment.EquipmentType.Polearm, Equipment.EquipmentMaterial.Paladin, Equipment.EquipmentSlot.Weapon, 128, 256, 128, 128, 256, 128, 0.95f, 100, 100, 8));	
	} 
}

