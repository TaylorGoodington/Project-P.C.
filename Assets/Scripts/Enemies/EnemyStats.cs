using UnityEngine;
using System.Collections;

public class EnemyStats : MonoBehaviour {

    public int hP;
    public int expGained;
    public float attackDamage;
    public float defense;
    //public List<Items> itemsDropped;
    //public List<Equipment> equipmentDropped;
    public float jumpHeight;
    public float patrolSpeed;
    public float chaseSpeed;
    [Tooltip("The amount of time an enemy will remain engaged after losing the line of sight.")]
    public float chaseTime;
    [Tooltip("The length of time in seconds it takes for the enemy to change directions.")]
    public float pivotTime;

}
