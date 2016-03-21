using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class LadyDeath : MonoBehaviour {

    public bool interactable;
    public bool interacting;
    public GameObject ladyDeathMenu;
    Animator headAnimator;
    List<string> insultsList;
    public Text insultText;

    void Start () {
        interactable = false;
        interacting = false;
        headAnimator = transform.GetChild(0).GetComponent<Animator>();
        AddInsulstsToList();
        ShuffleInsultList();
        StartCoroutine("HurlInsults");
    }
	
	void Update () {
	    if (interactable)
        {
            if (Input.GetButtonDown("Interact") && interacting == false)
            {
                OpenLadyDeathMenu();
                interacting = true;
            }
        }
	}

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.layer == 9)
        {
            GameObject.FindGameObjectWithTag("UserInterface").GetComponent<UserInterface>().showInteractableDisplay = true;
            interactable = true;
        }
    }

    void OpenLadyDeathMenu ()
    {
        //GameControl.gameControl.ladyDeathMenu = 1;
        //Instantiate(ladyDeathMenu);
    }

    void CloseLadyDeathMenu ()
    {
        GameControl.gameControl.ladyDeathMenu = 0;
        StartCoroutine("HurlInsults");
    }

    void AddInsulstsToList()
    {
        insultsList = new List<string> {
            "fuck",
            "shit",
            "asshole",
            "gaylord" };
    }

    void ShuffleInsultList ()
    {
        for (int i = 0; i < insultsList.Count; i++)
        {
            string temp = insultsList[i];
            int randomIndex = Random.Range(i, insultsList.Count);
            insultsList[i] = insultsList[randomIndex];
            insultsList[randomIndex] = temp;
        }
    }

    IEnumerator HurlInsults()
    {
        while (!interacting)
        {
            for (int i = 0; i < insultsList.Count; i++)
            {
                if (i == insultsList.Count - 1)
                {
                    insultText.text = insultsList[i];
                    headAnimator.Play("Insults");
                    yield return new WaitForSeconds(3);
                    ShuffleInsultList();
                }
                else
                {
                    insultText.text = insultsList[i];
                    headAnimator.Play("Insults");
                    yield return new WaitForSeconds(3);
                }
            }
        }
    }
}
