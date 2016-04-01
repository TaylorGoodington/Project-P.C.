using UnityEngine;
using System.Collections;

public class SkullFormation : MonoBehaviour {

    public GameObject skull;
    int skullStrikeInterval;
    public GameObject[] skullPoints;
    BabaYaga babaYaga;

    void Start ()
    {
        StartCoroutine("LoadTheGun");
        babaYaga = GameObject.FindGameObjectWithTag("BabaYaga").GetComponent<BabaYaga>();
        skullStrikeInterval = 5 - (babaYaga.aggressionPhase);

        Invoke("PullBackTheHammer", skullStrikeInterval);
    }

    IEnumerator LoadTheGun ()
    {
        foreach (GameObject point in skullPoints)
        {
            GameObject newSkull = Instantiate(skull, point.transform.position, Quaternion.identity) as GameObject;
            newSkull.transform.SetParent(point.transform);
            yield return new WaitForSeconds(0.3f);
        }
    }

    void SpinTheBarrel()
    {
        for (int i = 0; i < skullPoints.Length; i++)
        {
            GameObject temp = skullPoints[i];
            int randomIndex = Random.Range(i, skullPoints.Length);
            skullPoints[i] = skullPoints[randomIndex];
            skullPoints[randomIndex] = temp;
        }
    }

    void PullBackTheHammer ()
    {
        SpinTheBarrel();
        StartCoroutine("Fire");
    }

    IEnumerator Fire ()
    {
        foreach (GameObject point in skullPoints)
        {
            point.transform.GetChild(0).gameObject.SendMessage("Strike");
            EmptyClip();
            yield return new WaitForSeconds(skullStrikeInterval);
        }
    }

    void EmptyClip ()
    {
        if (skullPoints[5].transform.childCount == 0)
        {
            Destroy(this.gameObject);
            babaYaga.isAttacking = false;
        }
    }
}
