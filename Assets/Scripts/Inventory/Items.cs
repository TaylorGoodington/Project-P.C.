using UnityEngine;
using System.Collections;


[System.Serializable]
public class Items {

	public string itemName;
	public int itemID;
	public string itemDescription;
	public Sprite itemIcon;
	public ItemType itemType;
	public ItemTarget itemTarget;
	//public int maxCharges; 
	//public int currentCharges; 
	public ItemUseOcassion itemUseOcassion;
	public float castTimeInSeconds;
	[Range(0,1)]
	public float successRatePercent;
	//public int quantity; 
	
	public enum ItemType { 
		Consumable,
		Quest,
		KeyItem
	}
	
	public enum ItemTarget {
		Self,
		CurrentTarget,
		AllEnemies
	}
	
	public enum ItemUseOcassion {
		Anytime,
		Combat,
		OutOfCombat
	}
	
	public Items (int id, string name, string description, ItemType type, ItemTarget target, ItemUseOcassion useOcassion, float castTime, float successRate) {
		itemID = id;
		itemName = name;
		itemDescription = description;
		itemIcon = Resources.Load<Sprite>("Item Icons/" + name);
		itemType = type;
		itemTarget = target;
		itemUseOcassion = useOcassion;
		castTimeInSeconds = castTime;
		successRatePercent = successRate;
	}
	
	public Items () {
	
	}
	
}
