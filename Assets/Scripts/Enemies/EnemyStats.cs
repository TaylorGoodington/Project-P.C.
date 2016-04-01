using UnityEngine;
using System.Collections.Generic;

public class EnemyStats : MonoBehaviour {

    public int hP;
    public int maxHP;
    public int expGranted;
    public int maximumDamage;
    public int minimumDamage;
    public int defense;
    public int attackRange;
    public List<Skills> acquiredSkillsList;
    public List<Skills> activeSkillsList;
    public List<Items> itemsDropped;
    public List<Equipment> equipmentDropped;
    public float jumpHeight;
    public float patrolSpeed;
    public float chaseSpeed;
    public int knockbackForce;
    [Tooltip("The amount of time an enemy will remain engaged after losing the line of sight.")]
    public float chaseTime;
    [Tooltip("The length of time in seconds it takes for the enemy to change directions.")]
    public float pivotTime;

    //Add Plathrough modifier for the stats!!!
    void Start ()
    {

    }
}
