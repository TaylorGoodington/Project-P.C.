using UnityEngine;


[System.Serializable]
public class Items {

	public string itemName;
	public int itemID;
	public string itemDescription;
	public ItemType itemType;
	public ItemTarget itemTarget;
	public ItemUseOcassion itemUseOcassion;
//	public float castTimeInSeconds;
	[Range(0,1)]
	public float successRatePercent;
	public int skillIndex;
	public int buyPrice;
	public int quantity;
	
	
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
	
	public Items (int id, string name, string description, ItemType type, ItemTarget target, ItemUseOcassion useOcassion, float successRate, int skill, int price, int qty) {
		itemID = id;
		itemName = name;
		itemDescription = description;
		itemType = type;
		itemTarget = target;
		itemUseOcassion = useOcassion;
		successRatePercent = successRate;
		skillIndex = skill;
		buyPrice = price;
		quantity = qty;
	}
	
	public Items () {
	
	}
	
}
