using UnityEngine;
using System.Collections.Generic;

public class EquipmentDatabase : MonoBehaviour {

	public List<Equipment> equipment;

    public static EquipmentDatabase equipmentDatabase;

	//the order for stats is: strength, defense, speed, intelligence, health, mana.
	void Start () {
        equipmentDatabase = GetComponent<EquipmentDatabase>();

		equipment.Add (new Equipment (0, "Nothing", "Nothing", Equipment.EquipmentType.Head, Equipment.EquipmentMaterial.Cloth, Equipment.EquipmentSlot.Head, 0, 0, 0, 0, 1, 0, 1, 0, 0, 1, 0));

        //inital four that are equipped when nothing else is...They also dont show up in the inventory
        equipment.Add(new Equipment(1, "Hat", "Hat.", Equipment.EquipmentType.Head, Equipment.EquipmentMaterial.Cloth, Equipment.EquipmentSlot.Head, 0, 1, 0, 0, 1, 0, 1f, 0, 100, 1, 0));
        equipment.Add(new Equipment(2, "Shirt", "Shirt.", Equipment.EquipmentType.Chest, Equipment.EquipmentMaterial.Cloth, Equipment.EquipmentSlot.Chest, 0, 1, 0, 0, 1, 0, 1f, 0, 100, 1, 0));
        equipment.Add(new Equipment(3, "Skinny Jeans", "Pants.", Equipment.EquipmentType.Pants, Equipment.EquipmentMaterial.Cloth, Equipment.EquipmentSlot.Pants, 0, 1, 0, 0, 1, 0, 1f, 0, 100, 1, 0));
        equipment.Add(new Equipment(4, "Original Converse", "Shoes.", Equipment.EquipmentType.Feet, Equipment.EquipmentMaterial.Cloth, Equipment.EquipmentSlot.Feet, 0, 1, 0, 0, 1, 0, 1f, 0, 100, 1, 0));
        equipment.Add(new Equipment(5, "Helmet", "Sturdy Helmet", Equipment.EquipmentType.Head, Equipment.EquipmentMaterial.Chainmail, Equipment.EquipmentSlot.Head, 0, 3, 0, 0, 3, 0, 1f, 0, 100, 1, 0));
        equipment.Add(new Equipment(6, "Platemail", "Sturdy Chest", Equipment.EquipmentType.Chest, Equipment.EquipmentMaterial.Platemail, Equipment.EquipmentSlot.Chest, 0, 4, 0, 0, 4, 0, 1f, 0, 100, 1, 0));
        equipment.Add(new Equipment(7, "Assless Chaps", "Tiny Pants", Equipment.EquipmentType.Pants, Equipment.EquipmentMaterial.Leather, Equipment.EquipmentSlot.Pants, 0, 2, 5, 0, 2, 0, 1f, 0, 100, 1, 0));
        equipment.Add(new Equipment(8, "Air Jordans", "Ice Cream Paint Job", Equipment.EquipmentType.Feet, Equipment.EquipmentMaterial.Platemail, Equipment.EquipmentSlot.Feet, 0, 4, 0, 0, 4, 0, 1f, 0, 100, 1, 0));
        equipment.Add(new Equipment(9, "Air Jordans", "Ice Cream Paint Job", Equipment.EquipmentType.Feet, Equipment.EquipmentMaterial.Chainmail, Equipment.EquipmentSlot.Feet, 0, 4, 0, 0, 4, 0, 1f, 0, 100, 1, 0));
        equipment.Add(new Equipment(10, "Sword", "Basic copper sword", Equipment.EquipmentType.Sword, Equipment.EquipmentMaterial.Soldier, Equipment.EquipmentSlot.Weapon, 1, 2, 1, 1, 2, 1, 1f, 0, 100, 1, 0));
        equipment.Add(new Equipment(11, "Sword 2", "Sword 2", Equipment.EquipmentType.Sword, Equipment.EquipmentMaterial.Soldier, Equipment.EquipmentSlot.Weapon, 2, 4, 2, 2, 4, 2, 0.95f, 0, 100, 2, 0));
        equipment.Add(new Equipment(12, "Sword 3", "Sword 3", Equipment.EquipmentType.Sword, Equipment.EquipmentMaterial.Soldier, Equipment.EquipmentSlot.Weapon, 4, 8, 4, 4, 8, 4, 0.9f, 0, 100, 3, 0));
        equipment.Add(new Equipment(13, "Sword 4", "Sword 4", Equipment.EquipmentType.Sword, Equipment.EquipmentMaterial.Soldier, Equipment.EquipmentSlot.Weapon, 8, 16, 8, 8, 16, 8, 0.85f, 0, 100, 4, 0));
        equipment.Add(new Equipment(14, "Sword 5", "Sword 5", Equipment.EquipmentType.Sword, Equipment.EquipmentMaterial.Soldier, Equipment.EquipmentSlot.Weapon, 16, 32, 16, 16, 32, 16, 0.8f, 0, 100, 5, 0));
        equipment.Add(new Equipment(15, "Sword 6", "Sword 6", Equipment.EquipmentType.Sword, Equipment.EquipmentMaterial.Soldier, Equipment.EquipmentSlot.Weapon, 32, 64, 32, 32, 64, 32, 0.75f, 0, 100, 6, 0));
        equipment.Add(new Equipment(16, "Sword 7", "Sword 7", Equipment.EquipmentType.Sword, Equipment.EquipmentMaterial.Soldier, Equipment.EquipmentSlot.Weapon, 64, 128, 64, 64, 128, 64, 0.7f, 0, 100, 7, 0));
        equipment.Add(new Equipment(17, "Sword 8", "Sword 8", Equipment.EquipmentType.Sword, Equipment.EquipmentMaterial.Soldier, Equipment.EquipmentSlot.Weapon, 128, 256, 128, 128, 256, 128, 0.65f, 0, 100, 8, 0));
        equipment.Add(new Equipment(18, "Sword 9", "Sword 9", Equipment.EquipmentType.Sword, Equipment.EquipmentMaterial.Soldier, Equipment.EquipmentSlot.Weapon, 256, 512, 256, 256, 512, 256, 0.6f, 0, 100, 9, 0));
        equipment.Add(new Equipment(19, "Sword 10", "Sword 10", Equipment.EquipmentType.Sword, Equipment.EquipmentMaterial.Soldier, Equipment.EquipmentSlot.Weapon, 512, 1024, 512, 512, 1024, 512, 0.55f, 0, 100, 10, 0));
        equipment.Add(new Equipment(20, "Axe", "Axe", Equipment.EquipmentType.Axe, Equipment.EquipmentMaterial.Berserker, Equipment.EquipmentSlot.Weapon, 2, 2, 1, 1, 1, 1, 1.1f, 0, 100, 1, 0));
        equipment.Add(new Equipment(21, "Axe 2", "Axe 2", Equipment.EquipmentType.Axe, Equipment.EquipmentMaterial.Berserker, Equipment.EquipmentSlot.Weapon, 4, 4, 2, 2, 2, 2, 1.05f, 0, 100, 2, 0));
        equipment.Add(new Equipment(22, "Axe 3", "Axe 3", Equipment.EquipmentType.Axe, Equipment.EquipmentMaterial.Berserker, Equipment.EquipmentSlot.Weapon, 8, 8, 4, 4, 4, 4, 1f, 0, 100, 3, 0));
        equipment.Add(new Equipment(23, "Axe 4", "Axe 4", Equipment.EquipmentType.Axe, Equipment.EquipmentMaterial.Berserker, Equipment.EquipmentSlot.Weapon, 16, 16, 8, 8, 8, 8, 0.95f, 0, 100, 4, 0));
        equipment.Add(new Equipment(24, "Axe 5", "Axe 5", Equipment.EquipmentType.Axe, Equipment.EquipmentMaterial.Berserker, Equipment.EquipmentSlot.Weapon, 32, 32, 16, 16, 16, 16, 0.9f, 0, 100, 5, 0));
        equipment.Add(new Equipment(25, "Axe 6", "Axe 6", Equipment.EquipmentType.Axe, Equipment.EquipmentMaterial.Berserker, Equipment.EquipmentSlot.Weapon, 64, 64, 32, 32, 32, 32, 0.85f, 0, 100, 6, 0));
        equipment.Add(new Equipment(26, "Axe 7", "Axe 7", Equipment.EquipmentType.Axe, Equipment.EquipmentMaterial.Berserker, Equipment.EquipmentSlot.Weapon, 128, 128, 64, 64, 64, 64, 0.8f, 0, 100, 7, 0));
        equipment.Add(new Equipment(27, "Axe 8", "Axe 8", Equipment.EquipmentType.Axe, Equipment.EquipmentMaterial.Berserker, Equipment.EquipmentSlot.Weapon, 256, 256, 128, 128, 128, 128, 0.75f, 0, 100, 8, 0));
        equipment.Add(new Equipment(28, "Axe 9", "Axe 9", Equipment.EquipmentType.Axe, Equipment.EquipmentMaterial.Berserker, Equipment.EquipmentSlot.Weapon, 512, 512, 256, 256, 256, 256, 0.7f, 0, 100, 9, 0));
        equipment.Add(new Equipment(29, "Axe 10", "Axe 10", Equipment.EquipmentType.Axe, Equipment.EquipmentMaterial.Berserker, Equipment.EquipmentSlot.Weapon, 1024, 1024, 512, 512, 512, 512, 0.65f, 0, 100, 10, 0));
        equipment.Add(new Equipment(30, "Dagger", "Dagger", Equipment.EquipmentType.Dagger, Equipment.EquipmentMaterial.Rogue, Equipment.EquipmentSlot.Weapon, 1, 1, 2, 2, 1, 1, 0.75f, 0, 100, 1, 0));
        equipment.Add(new Equipment(31, "Dagger 2", "Dagger 2", Equipment.EquipmentType.Dagger, Equipment.EquipmentMaterial.Rogue, Equipment.EquipmentSlot.Weapon, 2, 2, 4, 4, 2, 2, 0.7f, 0, 100, 2, 0));
        equipment.Add(new Equipment(32, "Dagger 3", "Dagger 3", Equipment.EquipmentType.Dagger, Equipment.EquipmentMaterial.Rogue, Equipment.EquipmentSlot.Weapon, 4, 4, 8, 8, 4, 4, 0.65f, 0, 100, 3, 0));
        equipment.Add(new Equipment(33, "Dagger 4", "Dagger 4", Equipment.EquipmentType.Dagger, Equipment.EquipmentMaterial.Rogue, Equipment.EquipmentSlot.Weapon, 8, 8, 16, 16, 8, 8, 0.6f, 0, 100, 4, 0));
        equipment.Add(new Equipment(34, "Dagger 5", "Dagger 5", Equipment.EquipmentType.Dagger, Equipment.EquipmentMaterial.Rogue, Equipment.EquipmentSlot.Weapon, 16, 16, 32, 32, 16, 16, 0.55f, 0, 100, 5, 0));
        equipment.Add(new Equipment(35, "Dagger 6", "Dagger 6", Equipment.EquipmentType.Dagger, Equipment.EquipmentMaterial.Rogue, Equipment.EquipmentSlot.Weapon, 32, 32, 64, 64, 32, 32, 0.5f, 0, 100, 6, 0));
        equipment.Add(new Equipment(36, "Dagger 7", "Dagger 7", Equipment.EquipmentType.Dagger, Equipment.EquipmentMaterial.Rogue, Equipment.EquipmentSlot.Weapon, 64, 64, 128, 128, 64, 64, 0.45f, 0, 100, 7, 0));
        equipment.Add(new Equipment(37, "Dagger 8", "Dagger 8", Equipment.EquipmentType.Dagger, Equipment.EquipmentMaterial.Rogue, Equipment.EquipmentSlot.Weapon, 128, 128, 256, 256, 128, 128, 0.4f, 0, 100, 8, 0));
        equipment.Add(new Equipment(38, "Dagger 9", "Dagger 9", Equipment.EquipmentType.Dagger, Equipment.EquipmentMaterial.Rogue, Equipment.EquipmentSlot.Weapon, 256, 256, 512, 512, 256, 256, 0.35f, 0, 100, 9, 0));
        equipment.Add(new Equipment(39, "Dagger 10", "Dagger 10", Equipment.EquipmentType.Dagger, Equipment.EquipmentMaterial.Rogue, Equipment.EquipmentSlot.Weapon, 512, 512, 1024, 1024, 512, 512, 0.3f, 0, 100, 10, 0));
        equipment.Add(new Equipment(40, "Bow", "Bow", Equipment.EquipmentType.Bow, Equipment.EquipmentMaterial.Ranger, Equipment.EquipmentSlot.Weapon, 1, 1, 2, 2, 1, 1, 0.75f, 0, 100, 1, 0));
        equipment.Add(new Equipment(41, "Bow 2", "Bow 2", Equipment.EquipmentType.Bow, Equipment.EquipmentMaterial.Ranger, Equipment.EquipmentSlot.Weapon, 2, 2, 4, 4, 2, 2, 0.7f, 0, 100, 2, 0));
        equipment.Add(new Equipment(42, "Bow 3", "Bow 3", Equipment.EquipmentType.Bow, Equipment.EquipmentMaterial.Ranger, Equipment.EquipmentSlot.Weapon, 4, 4, 8, 8, 4, 4, 0.65f, 0, 100, 3, 0));
        equipment.Add(new Equipment(43, "Bow 4", "Bow 4", Equipment.EquipmentType.Bow, Equipment.EquipmentMaterial.Ranger, Equipment.EquipmentSlot.Weapon, 8, 8, 16, 16, 8, 8, 0.6f, 0, 100, 4, 0));
        equipment.Add(new Equipment(44, "Bow 5", "Bow 5", Equipment.EquipmentType.Bow, Equipment.EquipmentMaterial.Ranger, Equipment.EquipmentSlot.Weapon, 16, 16, 32, 32, 16, 16, 0.55f, 0, 100, 5, 0));
        equipment.Add(new Equipment(45, "Bow 6", "Bow 6", Equipment.EquipmentType.Bow, Equipment.EquipmentMaterial.Ranger, Equipment.EquipmentSlot.Weapon, 32, 32, 64, 64, 32, 32, 0.5f, 0, 100, 6, 0));
        equipment.Add(new Equipment(46, "Bow 7", "Bow 7", Equipment.EquipmentType.Bow, Equipment.EquipmentMaterial.Ranger, Equipment.EquipmentSlot.Weapon, 64, 64, 128, 128, 64, 64, 0.45f, 0, 100, 7, 0));
        equipment.Add(new Equipment(47, "Bow 8", "Bow 8", Equipment.EquipmentType.Bow, Equipment.EquipmentMaterial.Ranger, Equipment.EquipmentSlot.Weapon, 128, 128, 256, 256, 128, 128, 0.4f, 0, 100, 8, 0));
        equipment.Add(new Equipment(48, "Bow 9", "Bow 9", Equipment.EquipmentType.Bow, Equipment.EquipmentMaterial.Ranger, Equipment.EquipmentSlot.Weapon, 256, 256, 512, 512, 256, 256, 0.35f, 0, 100, 9, 0));
        equipment.Add(new Equipment(49, "Bow 10", "Bow 10", Equipment.EquipmentType.Bow, Equipment.EquipmentMaterial.Ranger, Equipment.EquipmentSlot.Weapon, 512, 512, 1024, 1024, 512, 512, 0.3f, 0, 100, 10, 0));
        equipment.Add(new Equipment(50, "Fist", "Fist", Equipment.EquipmentType.Fist, Equipment.EquipmentMaterial.Monk, Equipment.EquipmentSlot.Weapon, 1, 2, 2, 1, 1, 1, 0.5f, 0, 100, 1, 0));
        equipment.Add(new Equipment(51, "Fist 2", "Fist 2", Equipment.EquipmentType.Fist, Equipment.EquipmentMaterial.Monk, Equipment.EquipmentSlot.Weapon, 2, 4, 4, 2, 2, 2, 0.45f, 0, 100, 2, 0));
        equipment.Add(new Equipment(52, "Fist 3", "Fist 3", Equipment.EquipmentType.Fist, Equipment.EquipmentMaterial.Monk, Equipment.EquipmentSlot.Weapon, 4, 8, 8, 4, 4, 4, 0.4f, 0, 100, 3, 0));
        equipment.Add(new Equipment(53, "Fist 4", "Fist 4", Equipment.EquipmentType.Fist, Equipment.EquipmentMaterial.Monk, Equipment.EquipmentSlot.Weapon, 8, 16, 16, 8, 8, 8, 0.35f, 0, 100, 4, 0));
        equipment.Add(new Equipment(54, "Fist 5", "Fist 5", Equipment.EquipmentType.Fist, Equipment.EquipmentMaterial.Monk, Equipment.EquipmentSlot.Weapon, 16, 32, 32, 16, 16, 16, 0.3f, 0, 100, 5, 0));
        equipment.Add(new Equipment(55, "Fist 6", "Fist 6", Equipment.EquipmentType.Fist, Equipment.EquipmentMaterial.Monk, Equipment.EquipmentSlot.Weapon, 32, 64, 64, 32, 32, 32, 0.25f, 0, 100, 6, 0));
        equipment.Add(new Equipment(56, "Fist 7", "Fist 7", Equipment.EquipmentType.Fist, Equipment.EquipmentMaterial.Monk, Equipment.EquipmentSlot.Weapon, 64, 128, 128, 64, 64, 64, 0.2f, 0, 100, 7, 0));
        equipment.Add(new Equipment(57, "Fist 8", "Fist 8", Equipment.EquipmentType.Fist, Equipment.EquipmentMaterial.Monk, Equipment.EquipmentSlot.Weapon, 128, 256, 256, 128, 128, 128, 0.15f, 0, 100, 8, 0));
        equipment.Add(new Equipment(58, "Fist 9", "Fist 9", Equipment.EquipmentType.Fist, Equipment.EquipmentMaterial.Monk, Equipment.EquipmentSlot.Weapon, 256, 512, 512, 256, 256, 256, 0.1f, 0, 100, 9, 0));
        equipment.Add(new Equipment(59, "Fist 10", "Fist 10", Equipment.EquipmentType.Fist, Equipment.EquipmentMaterial.Monk, Equipment.EquipmentSlot.Weapon, 512, 1024, 1024, 512, 512, 512, 0.0500000000000001f, 0, 100, 10, 0));
        equipment.Add(new Equipment(60, "Talisman", "Talisman", Equipment.EquipmentType.Talisman, Equipment.EquipmentMaterial.Sorcerer, Equipment.EquipmentSlot.Weapon, 1, 1, 1, 2, 1, 2, 1f, 0, 100, 1, 0));
        equipment.Add(new Equipment(61, "Talisman 2", "Talisman 2", Equipment.EquipmentType.Talisman, Equipment.EquipmentMaterial.Sorcerer, Equipment.EquipmentSlot.Weapon, 2, 2, 2, 4, 2, 4, 0.95f, 0, 100, 2, 0));
        equipment.Add(new Equipment(62, "Talisman 3", "Talisman 3", Equipment.EquipmentType.Talisman, Equipment.EquipmentMaterial.Sorcerer, Equipment.EquipmentSlot.Weapon, 4, 4, 4, 8, 4, 8, 0.9f, 0, 100, 3, 0));
        equipment.Add(new Equipment(63, "Talisman 4", "Talisman 4", Equipment.EquipmentType.Talisman, Equipment.EquipmentMaterial.Sorcerer, Equipment.EquipmentSlot.Weapon, 8, 8, 8, 16, 8, 16, 0.85f, 0, 100, 4, 0));
        equipment.Add(new Equipment(64, "Talisman 5", "Talisman 5", Equipment.EquipmentType.Talisman, Equipment.EquipmentMaterial.Sorcerer, Equipment.EquipmentSlot.Weapon, 16, 16, 16, 32, 16, 32, 0.8f, 0, 100, 5, 0));
        equipment.Add(new Equipment(65, "Talisman 6", "Talisman 6", Equipment.EquipmentType.Talisman, Equipment.EquipmentMaterial.Sorcerer, Equipment.EquipmentSlot.Weapon, 32, 32, 32, 64, 32, 64, 0.75f, 0, 100, 6, 0));
        equipment.Add(new Equipment(66, "Talisman 7", "Talisman 7", Equipment.EquipmentType.Talisman, Equipment.EquipmentMaterial.Sorcerer, Equipment.EquipmentSlot.Weapon, 64, 64, 64, 128, 64, 128, 0.7f, 0, 100, 7, 0));
        equipment.Add(new Equipment(67, "Talisman 8", "Talisman 8", Equipment.EquipmentType.Talisman, Equipment.EquipmentMaterial.Sorcerer, Equipment.EquipmentSlot.Weapon, 128, 128, 128, 256, 128, 256, 0.65f, 0, 100, 8, 0));
        equipment.Add(new Equipment(68, "Talisman 9", "Talisman 9", Equipment.EquipmentType.Talisman, Equipment.EquipmentMaterial.Sorcerer, Equipment.EquipmentSlot.Weapon, 256, 256, 256, 512, 256, 512, 0.6f, 0, 100, 9, 0));
        equipment.Add(new Equipment(69, "Talisman 10", "Talisman 10", Equipment.EquipmentType.Talisman, Equipment.EquipmentMaterial.Sorcerer, Equipment.EquipmentSlot.Weapon, 512, 512, 512, 1024, 512, 1024, 0.55f, 0, 100, 10, 0));
        equipment.Add(new Equipment(70, "Staff", "Staff", Equipment.EquipmentType.Staff, Equipment.EquipmentMaterial.Wizard, Equipment.EquipmentSlot.Weapon, 1, 1, 1, 2, 1, 2, 1f, 0, 100, 1, 0));
        equipment.Add(new Equipment(71, "Staff 2", "Staff 2", Equipment.EquipmentType.Staff, Equipment.EquipmentMaterial.Wizard, Equipment.EquipmentSlot.Weapon, 2, 2, 2, 4, 2, 4, 0.95f, 0, 100, 2, 0));
        equipment.Add(new Equipment(72, "Staff 3", "Staff 3", Equipment.EquipmentType.Staff, Equipment.EquipmentMaterial.Wizard, Equipment.EquipmentSlot.Weapon, 4, 4, 4, 8, 4, 8, 0.9f, 0, 100, 3, 0));
        equipment.Add(new Equipment(73, "Staff 4", "Staff 4", Equipment.EquipmentType.Staff, Equipment.EquipmentMaterial.Wizard, Equipment.EquipmentSlot.Weapon, 8, 8, 8, 16, 8, 16, 0.85f, 0, 100, 4, 0));
        equipment.Add(new Equipment(74, "Staff 5", "Staff 5", Equipment.EquipmentType.Staff, Equipment.EquipmentMaterial.Wizard, Equipment.EquipmentSlot.Weapon, 16, 16, 16, 32, 16, 32, 0.8f, 0, 100, 5, 0));
        equipment.Add(new Equipment(75, "Staff 6", "Staff 6", Equipment.EquipmentType.Staff, Equipment.EquipmentMaterial.Wizard, Equipment.EquipmentSlot.Weapon, 32, 32, 32, 64, 32, 64, 0.75f, 0, 100, 6, 0));
        equipment.Add(new Equipment(76, "Staff 7", "Staff 7", Equipment.EquipmentType.Staff, Equipment.EquipmentMaterial.Wizard, Equipment.EquipmentSlot.Weapon, 64, 64, 64, 128, 64, 128, 0.7f, 0, 100, 7, 0));
        equipment.Add(new Equipment(77, "Staff 8", "Staff 8", Equipment.EquipmentType.Staff, Equipment.EquipmentMaterial.Wizard, Equipment.EquipmentSlot.Weapon, 128, 128, 128, 256, 128, 256, 0.65f, 0, 100, 8, 0));
        equipment.Add(new Equipment(78, "Staff 9", "Staff 9", Equipment.EquipmentType.Staff, Equipment.EquipmentMaterial.Wizard, Equipment.EquipmentSlot.Weapon, 256, 256, 256, 512, 256, 512, 0.6f, 0, 100, 9, 0));
        equipment.Add(new Equipment(79, "Staff 10", "Staff 10", Equipment.EquipmentType.Staff, Equipment.EquipmentMaterial.Wizard, Equipment.EquipmentSlot.Weapon, 512, 512, 512, 1024, 512, 1024, 0.55f, 0, 100, 10, 0));
        equipment.Add(new Equipment(80, "Polearm", "Polearm", Equipment.EquipmentType.Polearm, Equipment.EquipmentMaterial.Paladin, Equipment.EquipmentSlot.Weapon, 1, 2, 1, 1, 2, 1, 1.3f, 0, 100, 1, 0));
        equipment.Add(new Equipment(81, "Polearm 2", "Polearm 2", Equipment.EquipmentType.Polearm, Equipment.EquipmentMaterial.Paladin, Equipment.EquipmentSlot.Weapon, 2, 4, 2, 2, 4, 2, 1.25f, 0, 100, 2, 0));
        equipment.Add(new Equipment(82, "Polearm 3", "Polearm 3", Equipment.EquipmentType.Polearm, Equipment.EquipmentMaterial.Paladin, Equipment.EquipmentSlot.Weapon, 4, 8, 4, 4, 8, 4, 1.2f, 0, 100, 3, 0));
        equipment.Add(new Equipment(83, "Polearm 4", "Polearm 4", Equipment.EquipmentType.Polearm, Equipment.EquipmentMaterial.Paladin, Equipment.EquipmentSlot.Weapon, 8, 16, 8, 8, 16, 8, 1.15f, 0, 100, 4, 0));
        equipment.Add(new Equipment(84, "Polearm 5", "Polearm 5", Equipment.EquipmentType.Polearm, Equipment.EquipmentMaterial.Paladin, Equipment.EquipmentSlot.Weapon, 16, 32, 16, 16, 32, 16, 1.1f, 0, 100, 5, 0));
        equipment.Add(new Equipment(85, "Polearm 6", "Polearm 6", Equipment.EquipmentType.Polearm, Equipment.EquipmentMaterial.Paladin, Equipment.EquipmentSlot.Weapon, 32, 64, 32, 32, 64, 32, 1.05f, 0, 100, 6, 0));
        equipment.Add(new Equipment(86, "Polearm 7", "Polearm 7", Equipment.EquipmentType.Polearm, Equipment.EquipmentMaterial.Paladin, Equipment.EquipmentSlot.Weapon, 64, 128, 64, 64, 128, 64, 1f, 0, 100, 7, 0));
        equipment.Add(new Equipment(87, "Polearm 8", "Polearm 8", Equipment.EquipmentType.Polearm, Equipment.EquipmentMaterial.Paladin, Equipment.EquipmentSlot.Weapon, 128, 256, 128, 128, 256, 128, 0.95f, 0, 100, 8, 0));
        equipment.Add(new Equipment(88, "Polearm 9", "Polearm 9", Equipment.EquipmentType.Polearm, Equipment.EquipmentMaterial.Paladin, Equipment.EquipmentSlot.Weapon, 256, 512, 256, 256, 512, 256, 0.9f, 0, 100, 9, 0));
        equipment.Add(new Equipment(89, "Polearm 10", "Polearm 10", Equipment.EquipmentType.Polearm, Equipment.EquipmentMaterial.Paladin, Equipment.EquipmentSlot.Weapon, 512, 1024, 512, 512, 1024, 512, 0.85f, 0, 100, 10, 0));
    } 
}

