using UnityEngine;
using System.Collections;

public class DamageReceived : MonoBehaviour
{
    
    public Sprite[] Numbers;
    public GameObject oneDigitNumber;
    public GameObject twoDigitNumber;
    public GameObject threeDigitNumber;
    public GameObject fourDigitNumber;
    GameObject number;
    BoxCollider2D enemy;

    void Start()
    {
        enemy = transform.parent.GetComponent<BoxCollider2D>();
        transform.position = new Vector2(enemy.bounds.center.x, enemy.bounds.max.y + 5);
    }

    public void CalculateTheNumber(int damage)
    {
        //Single Digit
        if (damage < 10)
        {
            StartCoroutine("DisplayTheDamage", 1);
        }
        //Double Digit 
        else if (damage < 100)
        {
            StartCoroutine("DisplayTheDamage", 2);
        }
        //Triple Digit
        else if (damage < 1000)
        {
            StartCoroutine("DisplayTheDamage", 3);
        }
        //Quadruple Digit
        else
        {
            StartCoroutine("DisplayTheDamage", 4);
        }
    }

    IEnumerator DisplayTheDamage(int numberOfDigits)
    {
        if (numberOfDigits == 1)
        {
            number = oneDigitNumber;
        }
        else if (numberOfDigits == 2)
        {
            number = twoDigitNumber;
        }
        else if (numberOfDigits == 3)
        {
            number = threeDigitNumber;
        }
        else if (numberOfDigits == 4)
        {
            number = fourDigitNumber;
        }
        GameObject damage = Instantiate(number, transform.position, Quaternion.identity) as GameObject;
        //TODO  Still need to change the numbers that make up the display.
        yield return null;
    }
}
