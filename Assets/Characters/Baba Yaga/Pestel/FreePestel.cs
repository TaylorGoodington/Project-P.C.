using UnityEngine;

public class FreePestel : MonoBehaviour {

    int maxPestelStrikes;
    public int pestelStrikes;
    public int strikeDamage;
    public int strikeSpeed;
    public float targetHeight;
    public float floor;
    float pestelStrikeIntervals;
    public float intervalTimer;
    GameObject babaYaga;
    bool returning;
    public int returnSpeed = 3;
    public AttackPhase attackPhase;
    Animator animator;

	void Start () {
        babaYaga = GameObject.FindGameObjectWithTag("BabaYaga");
        maxPestelStrikes = 2 + babaYaga.GetComponent<BabaYaga>().aggressionPhase;
        pestelStrikeIntervals = 10 - (babaYaga.GetComponent<BabaYaga>().aggressionPhase * 2);
        returning = false;
        strikeDamage = strikeDamage * GameControl.gameControl.playThroughNumber;
        strikeSpeed = strikeSpeed + babaYaga.GetComponent<BabaYaga>().aggressionPhase;
        animator = GetComponent<Animator>();
        attackPhase = AttackPhase.Rising;
        pestelStrikes = 1;
    }
	
    void Update ()
    {
        if (returning)
        {
            ReturnToBaba();
        }
        else
        {
            StrikeTheBlood();
        }
    }

    void StrikeTheBlood()
    {
        if (pestelStrikes <= maxPestelStrikes)
        {
            if (attackPhase == AttackPhase.Rising)
            {
                if (transform.position.y < targetHeight)
                {
                    transform.Translate(new Vector3(0, 1) * Time.deltaTime * strikeSpeed, Space.World);
                }
                else
                {
                    intervalTimer = pestelStrikeIntervals;
                    attackPhase = AttackPhase.Moving;
                }
            }

            if (attackPhase == AttackPhase.Striking)
            {
                if (transform.position.y > floor)
                {
                    transform.Translate(new Vector3(0, -1) * Time.deltaTime * strikeSpeed, Space.World);
                }
                else
                {
                    pestelStrikes++;
                    attackPhase = AttackPhase.Rising;
                }
            }

            if (attackPhase == AttackPhase.Moving)
            {
                intervalTimer -= Time.deltaTime;

                if (intervalTimer <= 0)
                {
                    animator.Play("StrikeSignal");
                }
                else
                {
                    if (GameObject.FindGameObjectWithTag("Player").transform.position.x < transform.position.x)
                    {
                        transform.Translate(new Vector3(-1, 0) * Time.deltaTime * strikeSpeed, Space.World);
                    }
                    else if (GameObject.FindGameObjectWithTag("Player").transform.position.x > transform.position.x)
                    {
                        transform.Translate(new Vector3(1, 0) * Time.deltaTime * strikeSpeed, Space.World);
                    }
                    else
                    {
                        transform.Translate(new Vector3(0, 0) * Time.deltaTime * strikeSpeed, Space.World);
                    }
                }
            }
        }
        else
        {
            returning = true;
        }
    }

    //called by animator
    public void Strike ()
    {
        attackPhase = AttackPhase.Striking;
    }

    void ReturnToBaba ()
    {
        if (transform.position != babaYaga.GetComponent<BabaYaga>().freePestelPosition)
        {
            float targetX = (transform.position.x > babaYaga.GetComponent<BabaYaga>().freePestelPosition.x) ? -1 : 1;
            float targetY = (transform.position.y > babaYaga.GetComponent<BabaYaga>().freePestelPosition.y) ? -1 : 1;
            transform.Translate(new Vector3(targetX, targetY) * Time.deltaTime * returnSpeed);
        }
        else
        {
            babaYaga.GetComponent<BabaYaga>().pestel.Play("PestelFalling");
            Destroy(this);
        }
    }

    void OnTriggerEnter2D (Collider2D collider)
    {
        if (collider.gameObject.layer == 9)
        {
            CombatEngine.combatEngine.AttackingPlayer(babaYaga.GetComponent<Collider2D>(), strikeDamage);
        }
    }

    public enum AttackPhase
    {
        Rising,
        Striking,
        Moving
    }
}
