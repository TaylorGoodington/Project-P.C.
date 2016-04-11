using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Controller2D))]
[RequireComponent(typeof(EnemyStats))]
[RequireComponent(typeof(Hazard))]

public class Boar1 : MonoBehaviour
{

    string enemyType = "Boar1";
    #region Variables
    [HideInInspector]
    public EnemyStats stats;
    private float patrolSpeed;
    private float pivotTime;

    public bool changingDirection;

    public float minPatrolX;
    public float maxPatrolX;
    public GameObject patrolPlatform;

    public LayerMask patrolMask;

    private bool patrolPathCreated;

    public EnemyState state;

    bool beingAttacked;

    private Animator enemyAnimationController;

    private float gravity;
    public GameObject targetPlatform;

    private Vector3 velocity;

    private Controller2D controller;

    private BoxCollider2D enemyCollider;

    private GameObject player;
    #endregion

    void Start()
    {
        stats = GetComponent<EnemyStats>();
        patrolSpeed = stats.patrolSpeed;
        pivotTime = stats.pivotTime;
        AddSkills();
        AddItemsAndEquipmentDrops();

        controller = GetComponent<Controller2D>();
        enemyAnimationController = GetComponent<Animator>();
        enemyCollider = GetComponent<BoxCollider2D>();
        player = FindObjectOfType<Player>().gameObject;
        changingDirection = false;
        patrolPathCreated = false;
        beingAttacked = false;

        CreatePatrolPath();
        state = EnemyState.Patroling;
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
    }

    void Update()
    {
        //Checks for death.
        if (stats.hP <= 0)
        {
            velocity.x = 0;
            enemyAnimationController.Play(enemyType + "Death");
        }

        //player is dead.
        else if (GameControl.gameControl.hp == 0)
        {
            Patrolling();

            gravity = -1000;
            velocity.y += gravity * Time.deltaTime;
            controller.Move(velocity * Time.deltaTime, Vector2.zero);
            if (velocity.x != 0)
            {
                enemyAnimationController.Play(enemyType + "Walking");
            }
            else
            {
                enemyAnimationController.Play(enemyType + "Idle");
            }
        }

        else if (beingAttacked)
        {
            state = EnemyState.Attacking;
            enemyAnimationController.Play(enemyType + "Flinching");
            velocity.x = 0;
            float flinchTime = .33f;
            transform.Translate((player.GetComponent<Player>().knockBackForce / flinchTime) * CombatEngine.combatEngine.enemyKnockBackDirection *
                                 Time.deltaTime, 0, 0, Space.Self);
        }
        //not dead!
        else
        {
            Vector2 input = new Vector2(0, 0);

            //flips sprite depending on direction facing.
            if (controller.collisions.faceDir == -1)
            {
                gameObject.GetComponent<SpriteRenderer>().flipX = false;
            }
            else
            {
                gameObject.GetComponent<SpriteRenderer>().flipX = true;
            }

            //Creates a patrol path when we are on the ground.
            if ((velocity.y == 0 && transform.position.x > maxPatrolX) || (velocity.y == 0 && transform.position.x < minPatrolX))
            {
                RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, (enemyCollider.size.y / 2) + 5, patrolMask);
                if (hit)
                {
                    CreatePatrolPath();
                }
            }

            Patrolling();

            gravity = -1000;
            velocity.y += gravity * Time.deltaTime;
            controller.Move(velocity * Time.deltaTime, input);
            if (velocity.x != 0)
            {
                enemyAnimationController.Play(enemyType + "Walking");
            }
            else
            {
                enemyAnimationController.Play(enemyType + "Idle");
            }


            if (controller.collisions.above || controller.collisions.below)
            {
                velocity.y = 0;
            }
        }
    }

    private void Patrolling()
    {
        if (!patrolPathCreated)
        {
            CreatePatrolPath();
        }

        if ((transform.position.x >= minPatrolX && transform.position.x <= maxPatrolX))
        {
            changingDirection = false;
            velocity.x = Mathf.Lerp(velocity.x, controller.collisions.faceDir * patrolSpeed, 1f);
        }

        if ((transform.position.x <= minPatrolX || transform.position.x >= maxPatrolX) && changingDirection == false)
        {
            //check if the current direction puts the enemy off the platform.
            if (((transform.position.x + ((1 * controller.collisions.faceDir) * 5) > maxPatrolX) || (transform.position.x + ((1 * controller.collisions.faceDir) * 5) < minPatrolX)) && changingDirection == false)
            {
                velocity.x = 0;
                StartCoroutine(ChangeDirection(patrolSpeed));
                changingDirection = true;
            }
            else
            {
                velocity.x = Mathf.Lerp(velocity.x, controller.collisions.faceDir * patrolSpeed, 1f);
            }
        }
    }

    public void BeingAttacked()
    {
        beingAttacked = true;
    }

    public void FlinchRecovered()
    {
        beingAttacked = false;
    }

    public bool AtNearestPatrolPointToTarget()
    {
        //See if we are at the minPatrolX.
        if (transform.position.x <= minPatrolX)
        {
            if (player.transform.position.x < transform.position.x)
            {
                return true;
            }
        }

        //See if we are at the maxPatrolX.
        else if (transform.position.x >= maxPatrolX)
        {
            if (player.transform.position.x > transform.position.x)
            {
                return true;
            }
        }
        return false;
    }

    IEnumerator ChangeDirection(float moveSpeed)
    {
        yield return new WaitForSeconds(pivotTime);
        controller.collisions.faceDir = controller.collisions.faceDir * -1;
        velocity.x = Mathf.Lerp(velocity.x, controller.collisions.faceDir * moveSpeed, 1f);
    }

    public void CreatePatrolPath()
    {
        float rayLength = 5000f;
        float rayOriginX = enemyCollider.bounds.center.x;
        float rayOriginY = enemyCollider.bounds.center.y;
        Vector2 rayOrigin = new Vector2(rayOriginX, rayOriginY);

        RaycastHit2D bottom = Physics2D.Raycast(rayOrigin, Vector2.down, rayLength, patrolMask);

        if (bottom)
        {
            if (bottom.collider.gameObject.layer == 10)
            {
                minPatrolX = bottom.collider.bounds.min.x + (enemyCollider.size.x / 2) + 1;
                maxPatrolX = bottom.collider.bounds.max.x - (enemyCollider.size.x / 2) - 1;
                patrolPlatform = bottom.collider.gameObject;
            }
        }

        RaycastHit2D left = Physics2D.Raycast(rayOrigin, Vector2.left, rayLength, patrolMask);

        if (left)
        {
            if (left.collider.gameObject.layer == 10)
            {
                if ((left.collider.bounds.max.x + 1) >= minPatrolX)
                {
                    minPatrolX = left.collider.bounds.max.x + ((enemyCollider.size.x / 2) + 1);
                }
            }
        }

        RaycastHit2D right = Physics2D.Raycast(rayOrigin, Vector2.right, rayLength, patrolMask);

        if (right)
        {
            if (right.collider.gameObject.layer == 10)
            {
                if ((right.collider.bounds.min.x - ((enemyCollider.size.x / 2) + 1)) <= maxPatrolX)
                {
                    maxPatrolX = right.collider.bounds.min.x - ((enemyCollider.size.x / 2) + 1);
                }
            }
        }

        patrolPathCreated = true;
    }

    public enum EnemyState
    {
        Patroling,
        Chasing,
        Investigating,
        Attacking,
        Fleeing
    }

    public void CallEnemyDefeated()
    {
        StartCoroutine("EnemyDefeated");
    }

    public void OnTriggerEnter2D(Collider2D collider)
    {
        //Falling off the world
        if (collider.gameObject.layer == 19)
        {
            stats.hP = 0;
        }
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
