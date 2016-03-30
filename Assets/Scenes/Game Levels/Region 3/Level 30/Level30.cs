using UnityEngine;
using System.Collections;

public class Level30 : MonoBehaviour {

    public GameObject bossTrap;
    public GameObject babaYaga;

	void Start () {
        bossTrap.SetActive(false);
        babaYaga.SetActive(false);
    }
	
	void Update () {
	    
	}

    void OnTriggerEnter2D (Collider2D collider)
    {
        if (collider.gameObject.layer == 9)
        {
            bossTrap.SetActive(true);
            babaYaga.SetActive(true);
        }
    }
}
