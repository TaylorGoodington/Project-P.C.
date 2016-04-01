using UnityEngine;
using System.Collections;

[RequireComponent(typeof(EnemyStats))]
public class Hazard : MonoBehaviour {

    int damagePerSecond;
    bool causingDamage;
    EnemyStats stats;
    Collider2D hitCollider;

	void Start ()
    {
        stats = GetComponent<EnemyStats>();
        hitCollider = GetComponent<Collider2D>();
        stats.acquiredSkillsList.Add(SkillsDatabase.skillsDatabase.skills[0]);
        causingDamage = false;
        damagePerSecond = stats.maximumDamage;
	}

    public IEnumerator TakeDamageOverTime ()
    {
        while(causingDamage)
        {
            CombatEngine.combatEngine.AttackingPlayer(hitCollider, damagePerSecond);
            yield return new WaitForSeconds(1);
        }
    }

    public void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.layer == 9)
        {
            causingDamage = true;
            StartCoroutine(TakeDamageOverTime());
        }
    }

    public void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.layer == 9)
        {
            causingDamage = false;
        }
    }

    public void KnockBack ()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        CombatEngine.combatEngine.enemyKnockBackForce = stats.knockbackForce;
        int direction = (transform.position.x > player.transform.position.x) ? -1 : 1;
        CombatEngine.combatEngine.enemyFaceDirection = direction;
        player.GetComponent<Player>().Knockback();
    }
}