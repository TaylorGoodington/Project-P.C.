using UnityEngine;
using System.Collections;

public class Hazard : MonoBehaviour {

    public int damagePerSecond;
    public bool causingDamage;

	void Start ()
    {
        causingDamage = false;
	}

    public IEnumerator TakeDamageOverTime ()
    {
        while(causingDamage)
        {
            GameControl.gameControl.hp -= damagePerSecond;
            GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().flinching = true;
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
}