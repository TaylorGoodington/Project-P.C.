using UnityEngine;
using System.Collections;

[RequireComponent(typeof(EnemyStats))]
public class BabaYaga : MonoBehaviour {

    #region Variables
    [HideInInspector]
    public EnemyStats stats;
    string enemyType = "BabaYaga";
    public EnemyState state;
    public bool isAttacking;
    bool beingAttacked;
    public bool changingLocations;
    Animator animationController;
    //GameObject player;

    public Vector3 leftPosition;
    public Vector3 rightPosition;

    Collider2D hitBox;
    public Animator pestel;
    public GameObject freePestel;
    [HideInInspector]
    public Vector3 freePestelPosition;
    public float swapTimer;
    public float initialSwapTime;
    int attackNumber;
    public int aggressionPhase;
    #endregion

    void Start()
    {
        stats = GetComponent<EnemyStats>();
        hitBox = GetComponent<BoxCollider2D>();
        AddSkills();
        AddItemsAndEquipmentDrops();
        animationController = GetComponent<Animator>();
        //player = FindObjectOfType<Player>().gameObject;
        isAttacking = false;
        changingLocations = false;
        beingAttacked = false;
        state = EnemyState.Idle;
        swapTimer = initialSwapTime - (aggressionPhase * 5);
        aggressionPhase = 1;
        freePestelPosition = new Vector3(1535, 153);
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

    void Update()
    {
        AggressionPhase();

        //Checks for death.
        if (stats.hP <= 0)
        {
            animationController.Play(enemyType + "Death");
        }

        //player is dead.
        else if (GameControl.gameControl.hp == 0)
        {
            //laughing maybe??
        }

        else if (beingAttacked)
        {
            //flinching?
            //once enough damage or time has passed....
            //StartChangingLocations();
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
                Attack();
            }

        }
    }

    void AggressionPhase ()
    {
        if (stats.hP > (stats.maxHP * .75))
        {
            aggressionPhase = 1;
        }
        else if (stats.hP < (stats.maxHP * .75) && stats.hP < (stats.maxHP * .5))
        {
            aggressionPhase = 2;
        }
        else if (stats.hP < (stats.maxHP * .5) && stats.hP < (stats.maxHP * .25))
        {
            aggressionPhase = 3;
        }
        else
        {
            aggressionPhase = 4;
        }
    }

    void PositionSwapCounter ()
    {
        swapTimer -= Time.deltaTime;

        if (swapTimer <= 0)
        {
            StartChangingLocations();
        }
    }

    void StartChangingLocations ()
    {
        changingLocations = true;
        swapTimer = initialSwapTime - (aggressionPhase * 5);
        animationController.Play("FadeOut");
        pestel.Play("PestelFadeOut");
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
            freePestelPosition = new Vector3(1272, 153);
        }
        animationController.Play("FadeIn");
        pestel.Play("PestelFadeIn");
    }

    //called from the animator
    public void LocationsChanged ()
    {
        changingLocations = false;
    }

    public void BeingAttacked()
    {
        beingAttacked = true;
    }

    public void FlinchRecovered()
    {
        beingAttacked = false;
    }

    public enum EnemyState
    {
        Idle,
        PestelAttack,
        SkullAttack1,
        SkullAttack2,
        SkullAttack3
    }

    //called from attacking animation at the begining and end.
    public void IsAttacking()
    {
        isAttacking = !isAttacking;
    }

    //Called from the animator.
    public void Attack()
    {
        isAttacking = true;
        attackNumber = Random.Range(0, 5);
        if (attackNumber == 1)
        {
            PestelAttack();
        }
        else if (attackNumber == 2)
        {
            //Skull Attack 1
            PestelAttack();
        }
        else if (attackNumber == 3)
        {
            //Skull Attack 2
            PestelAttack();
        }
        else
        {
            //Skull Attack 3
            PestelAttack();
        }
    }

    void PestelAttack ()
    {
        animationController.Play("Casting");
        pestel.Play("PestelRising");
    }

    public void FireProjectiles ()
    {
        if (attackNumber == 1)
        {
            Instantiate(freePestel, freePestelPosition, Quaternion.identity);
        }
        else
        {
            Instantiate(freePestel, freePestelPosition, Quaternion.identity);
        }
    }

    public void CallEnemyDefeated()
    {
        StartCoroutine("EnemyDefeated");
    }

    //When the enemy dies.
    public IEnumerator EnemyDefeated()
    {
        while (GameObject.FindGameObjectWithTag("UserInterface").GetComponent<UserInterface>().tallyingSpoils == true)
        {
            yield return null;
        }

        GameObject.FindGameObjectWithTag("UserInterface").GetComponent<UserInterface>().tallyingSpoils = true;
        LevelManager.levelManager.enemiesDefeated += 1;

        GameObject.FindGameObjectWithTag("UserInterface").GetComponent<UserInterface>().ReceiveXP(stats.expGranted);
        GameControl.gameControl.xp += stats.expGranted;

        EquipmentDrops();
        ItemDrops();
        GameObject.FindGameObjectWithTag("UserInterface").GetComponent<UserInterface>().CallReceiveEquipment();

        Destroy(this.gameObject);
    }

    //Add Equipment drops to player list
    public void EquipmentDrops()
    {
        foreach (Equipment equipment in stats.equipmentDropped)
        {
            int randomNumber = Random.Range(0, 101);
            if (randomNumber <= equipment.dropRate)
            {
                GameObject.FindGameObjectWithTag("UserInterface").GetComponent<UserInterface>().receivedEquipment.Add(EquipmentDatabase.equipmentDatabase.equipment[equipment.equipmentID]);
            }
        }
    }

    //Add Item drops to player list
    public void ItemDrops()
    {
        foreach (Items item in stats.itemsDropped)
        {
            int randomNumber = Random.Range(0, 101);
            if (randomNumber <= item.dropRate)
            {
                GameObject.FindGameObjectWithTag("UserInterface").GetComponent<UserInterface>().receivedItems.Add(ItemDatabase.itemDatabase.items[item.itemID]);
            }
        }
    }
}
