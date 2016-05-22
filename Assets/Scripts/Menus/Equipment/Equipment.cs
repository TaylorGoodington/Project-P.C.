[System.Serializable]
public class Equipment {

	public int equipmentID;
    public string equipmentName;
    public string equipmentDescription;
	public EquipmentType equipmentType;
    public int equipmentTier;
    public int equipmentPowerLevel;
    public int equipmentLevelRequirement;
	public int equipmentStrength;
	public int equipmentDefense;
	public int equipmentSpeed;
	public int equipmentIntelligence;
	public int equipmentHealth;
	public int equipmentMana;
	public int equipmentSkill;
	public int knockbackForce;
    public int maxCombos;

	
	public enum EquipmentType {
        Cloth,
        Leather,
        Chainmail,
        Platemail,
        Sword,
		Axe,
		Dagger,
		Bow,
		Fist,
		Talisman,
		Staff,
		Polearm
	}
	
	
	public Equipment (int id, string name, string description, EquipmentType type, int tier, int powerLevel, int levelRequirement,
					  int strength, int defense, int speed, int intelligence, int health, int mana,
					  int skill, int knockback, int combos) {
		equipmentID = id;
		equipmentName = name;
		equipmentDescription = description;
		equipmentType = type;
        equipmentTier = tier;
        equipmentPowerLevel = powerLevel;
        equipmentLevelRequirement = levelRequirement;
		equipmentStrength = strength;
		equipmentDefense = defense;
		equipmentSpeed = speed;
		equipmentIntelligence = intelligence;
		equipmentHealth = health;
		equipmentMana = mana;
		equipmentSkill = skill;
		knockbackForce = knockback;
        maxCombos = combos;
	}
}