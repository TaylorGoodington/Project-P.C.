using UnityEngine;
using System.Collections;


[System.Serializable]
public class Items {

	public string itemName;
	public int itemID;
	public string itemDescription;
	public Texture2D itemIcon;
	public ItemType itemType;
	public ItemTarget itemTarget;
	
	public enum ItemType { 
		Consumable,
		Quest
	}
	
	public enum ItemTarget {
		Self,
		CurrentTarget,
		AllEnemies
	}
	
}
