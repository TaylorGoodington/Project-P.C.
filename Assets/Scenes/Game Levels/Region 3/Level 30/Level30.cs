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
}
