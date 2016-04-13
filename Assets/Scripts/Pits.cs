using UnityEngine;
using System.Collections;

public class Pits : MonoBehaviour {

	void OnTriggerEnter2D (Collider2D collider)
    {
        if (collider.gameObject.layer == 9 || collider.gameObject.layer == 14)
        {
            collider.BroadcastMessage("Death");
        }
    }
}
