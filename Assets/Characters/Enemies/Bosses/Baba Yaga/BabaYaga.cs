using UnityEngine;
using System.Collections;

public class BabaYaga : EnemyBase
{

    #region Variables
    public bool changingLocations;

    public Vector3 leftPosition;
    public Vector3 rightPosition;

    Collider2D hitBox;
    public Animator pestel;
    public GameObject freePestel;
    [HideInInspector]
    public Vector3 freePestelPosition;
    public bool pestelIsFree;
    public float swapTimer;
    public float initialSwapTime;
    public int attackNumber;
    public int aggressionPhase;
    bool damagedTimerIsOn;
    public float beingAttackedTimer;

    public GameObject skullFormation1;
    public GameObject skullFormation2;
    public GameObject skullFormation3;

    bool death;
    #endregion

    public override void Start()
    {
        base.Start();
        hitBox = GetComponent<BoxCollider2D>();
        AddSkills();
        AddItemsAndEquipmentDrops();
        changingLocations = false;
        beingAttacked = false;
        swapTimer = initialSwapTime - aggressionPhase;
        aggressionPhase = 1;
        freePestelPosition = new Vector3(1535, 153);
        pestelIsFree = false;
        damagedTimerIsOn = false;
    }

    //Add Skills Here
    private void AddSkills()
    {
        stats.acquiredSkillsList.Add(SkillsDatabase.skillsDatabase.skills[0]);
    }

    //Add Items and Equipment Drops here...
    public void AddItemsAndEquipmentDrops()
    {
        stats.itemsDropped.Add(ItemDatabase.itemDatabase.items[2]);
        stats.itemsDropped[0].dropRate = 50;

        stats.equipmentDropped.Add(EquipmentDatabase.equipmentDatabase.equipment[0]);
        stats.equipmentDropped[0].dropRate = 50;

        stats.equipmentDropped.Add(EquipmentDatabase.equipmentDatabase.equipment[1]);
        stats.equipmentDropped[1].dropRate = 50;

        stats.equipmentDropped.Add(EquipmentDatabase.equipmentDatabase.equipment[1]);
        stats.equipmentDropped[2].dropRate = 50;
    }

    public override void Update()
    {
        GameObject damageDisplay = transform.GetChild(5).gameObject;
        damageDisplay.transform.position = new Vector3(transform.position.x, transform.position.y + 55);

        AggressionPhase();

        if (enemyAnimationController.GetCurrentAnimatorStateInfo(0).IsName("FadeOut"))
        {

        }
        else
        { 
            //Checks for death.
            if (stats.hP <= 0)
            {
                enemyAnimationController.Play("BabaYagaDeath");
                pestel.Play("PestelFadeOut");
                if (!death)
                {
                    death = true;
                    if (attackNumber == 1)
                    {
                        Destroy(GameObject.FindObjectOfType<FreePestel>().gameObject);
                    }
                    else
                    {
                        GameObject formation = GameObject.FindObjectOfType<SkullFormation>().gameObject;
                        Destroy(formation);
                        isAttacking = false;
                    }
                }
            }

            //player is dead.
            else if (GameControl.gameControl.hp == 0)
            {
                //laughing maybe??
            }

            else if (beingAttacked)
            {
                if (enemyAnimationController.GetCurrentAnimatorStateInfo(0).IsName("Casting"))
                {
                    //continue the casting animation.
                } 
                else
                {
                    //Play Flinch Animation.
                    enemyAnimationController.Play("BabaYagaFlinching");
                }

                if (!damagedTimerIsOn)
                {
                    beingAttackedTimer = 1.5f;
                    damagedTimerIsOn = true;
                }

                else if (damagedTimerIsOn)
                {
                    if (beingAttackedTimer > 0)
                    {
                        beingAttackedTimer -= Time.deltaTime;
                    }
                    else
                    {
                        damagedTimerIsOn = false;
                        if (enemyAnimationController.GetCurrentAnimatorStateInfo(0).IsName("Casting"))
                        {

                        }
                        else
                        {
                            StartChangingLocations();
                        }
                    }
                }
            }

            //not dead!
            else
            {
                //flips sprite depending on location on screen.
                if (transform.position == rightPosition)
                {
                    gameObject.GetComponent<SpriteRenderer>().flipX = false;
                    pestel.gameObject.GetComponent<SpriteRenderer>().flipX = false;
                    hitBox.offset = new Vector2(-21.5f, 41);
                }
                else
                {
                    gameObject.GetComponent<SpriteRenderer>().flipX = true;
                    pestel.gameObject.GetComponent<SpriteRenderer>().flipX = true;
                    hitBox.offset = new Vector2(21.5f, 41);
                }

                //Movement
                if (!changingLocations)
                {
                    PositionSwapCounter();
                }

                //Attack!
                if (!isAttacking)
                {
                    StartCoroutine("Attack");
                }
            }
        }
    }

    void AggressionPhase()
    {
        int oldAggressionPhase = aggressionPhase;
        if (stats.hP > (stats.maxHP * .75))
        {
            aggressionPhase = 1;
        }
        else if (stats.hP < (stats.maxHP * .75) && stats.hP > (stats.maxHP * .5))
        {
            aggressionPhase = 2;
        }
        else if (stats.hP < (stats.maxHP * .5) && stats.hP > (stats.maxHP * .25))
        {
            aggressionPhase = 3;
        }
        else
        {
            aggressionPhase = 4;
        }

        if (oldAggressionPhase != aggressionPhase)
        {
            ClearTheBoard();
        }
    }

    void ClearTheBoard()
    {
        changingLocations = false;
        swapTimer = 0;
        if (attackNumber == 1)
        {
            if (GameObject.FindObjectOfType<FreePestel>())
            {
                GameObject.FindObjectOfType<FreePestel>().returning = true;
            }
        }
        else
        {
            if (GameObject.FindObjectOfType<SkullFormation>())
            {
                GameObject formation = GameObject.FindObjectOfType<SkullFormation>().gameObject;
                Destroy(formation);
                isAttacking = false;
            }
        }
    }

    void PositionSwapCounter ()
    {
        swapTimer -= Time.deltaTime;

        if (swapTimer <= 0)
        {
            if (enemyAnimationController.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
            {
                StartChangingLocations();
            }
        }
    }

    void StartChangingLocations ()
    {
        changingLocations = true;
        swapTimer = initialSwapTime - aggressionPhase;
        enemyAnimationController.Play("FadeOut");
        if (!pestelIsFree)
        {
            pestel.Play("PestelFadeOut");
        }
    }

    //called from the animator.
    public void ChangeLocations ()
    {
        if (transform.position == leftPosition)
        {
            transform.position = rightPosition;
            freePestelPosition = new Vector3(1535, 153);
        }
        else
        {
            transform.position = leftPosition;
            freePestelPosition = new Vector3(1265, 153);
        }

        FlinchRecovered();
        enemyAnimationController.Play("FadeIn");

        if (!pestelIsFree)
        {
            pestel.Play("PestelFadeIn");
        }
    }

    //called from the animator
    public void LocationsChanged ()
    {
        changingLocations = false;
    }

    //Called from the animator.
    new IEnumerator Attack()
    {
        isAttacking = true;
        attackNumber = Random.Range(0, 5);
        yield return new WaitForSeconds(5 - aggressionPhase);

        if (attackNumber == 1)
        {
            PestelAttack();
        }
        else 
        {
            enemyAnimationController.Play("Casting");
        }
    }

    void PestelAttack ()
    {
        enemyAnimationController.Play("Casting");
        pestel.Play("PestelRising");
    }

    public void FireProjectiles ()
    {
        if (attackNumber == 1)
        {
            Instantiate(freePestel, freePestelPosition, Quaternion.identity);
            pestelIsFree = true;
        }
        else if (attackNumber == 2)
        {
            Vector3 targetPosition = (transform.position == leftPosition) ? leftPosition : rightPosition;
            targetPosition.y += 80;
            Instantiate(skullFormation1, targetPosition, Quaternion.identity);
        }
        else if (attackNumber == 3)
        {
            Instantiate(skullFormation2);
        }
        else
        {
            Instantiate(skullFormation3);
        }
    }
}