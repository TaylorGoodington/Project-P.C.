using UnityEngine;

[RequireComponent(typeof(EnemyStats))]
[RequireComponent(typeof(Hazard))]
public class FreePestel : MonoBehaviour {

    int maxPestelStrikes;
    int pestelStrikes;
    int strikeSpeed = 100;
    float targetHeight = 164;
    float floor = 64;
    float pestelStrikeIntervals;
    float intervalTimer;
    GameObject babaYaga;
    bool returning;
    int returnSpeed = 100;
    AttackPhase attackPhase;
    Animator animator;

	void Start () {
        babaYaga = GameObject.FindGameObjectWithTag("BabaYaga");
        maxPestelStrikes = 2 + babaYaga.GetComponent<BabaYaga>().aggressionPhase;
        pestelStrikeIntervals = 10 - (babaYaga.GetComponent<BabaYaga>().aggressionPhase * 2);
        returning = false;
        strikeSpeed = strikeSpeed + (babaYaga.GetComponent<BabaYaga>().aggressionPhase * 50);
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
                    float playerPositionX = GameObject.FindGameObjectWithTag("Player").transform.position.x;
                    int fudgeFactor = 1;

                    if (transform.position.x >= playerPositionX - fudgeFactor && transform.position.x <= playerPositionX + fudgeFactor)
                    {
                        //do nothing.
                    }
                    else if (playerPositionX < transform.position.x)
                    {
                        transform.Translate(new Vector3(-1, 0) * Time.deltaTime * strikeSpeed, Space.World);
                    }
                    else if (playerPositionX > transform.position.x)
                    {
                        transform.Translate(new Vector3(1, 0) * Time.deltaTime * strikeSpeed, Space.World);
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
        float babaPositionX = babaYaga.GetComponent<BabaYaga>().freePestelPosition.x;
        float babaPositionY = babaYaga.GetComponent<BabaYaga>().freePestelPosition.y;
        int fudgeFactor = 1;
        if ((transform.position.x >= babaPositionX - fudgeFactor && transform.position.x <= babaPositionX + fudgeFactor) &&
            (transform.position.y >= babaPositionY - fudgeFactor && transform.position.y <= babaPositionY + fudgeFactor))
        {
            babaYaga.GetComponent<BabaYaga>().pestelIsFree = false;
            babaYaga.GetComponent<BabaYaga>().pestel.Play("PestelFalling");
            Destroy(this.gameObject);
        }

        //On the move.
        else
        {
            //X movement.
            if (transform.position.x >= babaPositionX - 1 && transform.position.x <= babaPositionX + 1)
            {
                //Do Nothing.
            }
            else if (transform.position.x > babaPositionX)
            {
                transform.Translate(new Vector3(-1, 0) * Time.deltaTime * returnSpeed);
            }
            else
            {
                transform.Translate(new Vector3(1, 0) * Time.deltaTime * returnSpeed);
            }

            //Y movement.
            if (transform.position.y >= babaPositionY - 1 && transform.position.y <= babaPositionY + 1)
            {
                //Do Nothing.
            }
            else if (transform.position.y > babaPositionY)
            {
                transform.Translate(new Vector3(0, -1) * Time.deltaTime * returnSpeed);
            }
            else
            {
                transform.Translate(new Vector3(0, 1) * Time.deltaTime * returnSpeed);
            }
        }
    }

    public enum AttackPhase
    {
        Rising,
        Striking,
        Moving
    }
}
