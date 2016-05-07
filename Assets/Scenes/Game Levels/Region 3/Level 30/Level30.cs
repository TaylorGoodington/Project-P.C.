using UnityEngine;

public class Level30 : MonoBehaviour {

    public GameObject bossTrap;
    public GameObject babaYaga;

    public GameObject leftSideGateWall;
    public GameObject leftSideGateHazard;
    public GameObject rightSideGateWall;
    public GameObject rightSideGateHazard;

    Animator animator;

    bool gateClosed;
    bool madeIt;

    void Start () {
        bossTrap.SetActive(false);
        babaYaga.SetActive(false);
        leftSideGateHazard.SetActive(false);
        leftSideGateWall.SetActive(false);
        rightSideGateHazard.SetActive(false);
        rightSideGateWall.SetActive(false);
        animator = GetComponent<Animator>();
        animator.Play("IdleNoGate");
        gateClosed = false;
        madeIt = false;
    }

    void OnTriggerEnter2D (Collider2D collider)
    {
        if (collider.gameObject.layer == 9)
        {
            if (!gateClosed)
            {
                bossTrap.SetActive(true);
                babaYaga.SetActive(true);
                animator.Play("ClosingTheGate");
                gateClosed = true;
            }
        }
    }

    public void ActivateTheTrap ()
    {
        leftSideGateHazard.SetActive(true);
        leftSideGateWall.SetActive(true);
        rightSideGateHazard.SetActive(true);
        rightSideGateWall.SetActive(true);
    }

    void Update()
    {
        if (gateClosed && !madeIt)
        {
            if (GameObject.FindGameObjectWithTag("Player").transform.position.x >= 1405)
            {
                GameControl.gameControl.endOfLevel = false;
                GameControl.gameControl.playerHasControl = true;
                madeIt = true;
            }
            else
            {
                GameControl.gameControl.playerHasControl = false;
                GameControl.gameControl.endOfLevel = true;
            }
        }
        if (!babaYaga)
        {
            //do nothing.
        }
        else if (babaYaga.GetComponent<EnemyStats>().hP <= 0)
        {
            animator.Play("OpeningTheGate");
        }
    }
}
