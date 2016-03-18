using UnityEngine;
using System.Collections.Generic;

public class ItemDatabase : MonoBehaviour {

    public static ItemDatabase itemDatabase;
	public List<Items> items; 
	
	void Start () {
        itemDatabase = GetComponent<ItemDatabase>();

        items.Add (new Items (0, "None", "Nothing", Items.ItemType.Consumable, Items.ItemTarget.Self, Items.ItemUseOcassion.Anytime, 1, 1, 10, 1, 0));
        items.Add (new Items (1, "Egg", "Just a normal egg, looks pretty good.", Items.ItemType.Consumable, Items.ItemTarget.Self, Items.ItemUseOcassion.Anytime, 1, 1, 10, 1, 0));
		items.Add (new Items (2, "Potion", "Get yourself a feel.", Items.ItemType.Consumable, Items.ItemTarget.Self, Items.ItemUseOcassion.Anytime, 1, 1, 10, 1, 0));
	}
	
}
