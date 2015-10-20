﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ItemDatabase : MonoBehaviour {

	public List<Items> items = new List<Items>(); 
	
	void Awake () {
		DontDestroyOnLoad (gameObject);
	}
	
	void Start () {
		items.Add (new Items (0, "Egg", "Just a normal egg, looks pretty good.", Items.ItemType.Consumable, Items.ItemTarget.Self, Items.ItemUseOcassion.Anytime, 0, 1));
		items.Add (new Items (1, "Potion", "Get yourself a feel.", Items.ItemType.Consumable, Items.ItemTarget.Self, Items.ItemUseOcassion.Anytime, 0, 1));
	}
	
}