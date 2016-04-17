using UnityEngine;
using System.Collections;

public class EnemyTest : EnemyBase {

    #region Variables
    public Vector3[] localPatrolPoints;
    Vector3[] globalPatrolPoints;
    bool inPatrolZone;
    Vector3 primaryPatrolPoint;
    float offScreenTimer;
    Vector3 currentPatrolTarget;
    #endregion

    public override void Start()
    {
        base.Start();
        primaryPatrolPoint = transform.position;
        inPatrolZone = true;
        globalPatrolPoints = new Vector3[localPatrolPoints.Length];
        for (int i = 0; i < localPatrolPoints.Length; i++)
        {
            globalPatrolPoints[i] = localPatrolPoints[i] + transform.position;
        }

        currentPatrolTarget = globalPatrolPoints[0];
    }

    public override void Chasing()
    {
        inPatrolZone = false;
        //If the engagement counter has run out we stop chasing.
        if (engagementCounter <= 0)
        {
            state = EnemyBase.EnemyState.Patroling;
            ResetEngagementCountDown();
        }

        //If the engagement counter still has time we continue chasing.
        else if (engagementCounter > 0)
        {
            //Check if the line of sight is broken.
            if (!LineOfSight())
            {
                targetPosition = player.transform.position;
                state = EnemyBase.EnemyState.Investigating;
            }

            //If we have a LOS we continue to chase.
            else if (LineOfSight())
            {
                ResetEngagementCountDown();

                int targetDirection = (transform.position.x >= player.transform.position.x) ? -1 : 1;

                //if they enemy is facing the wrong direction.
                if ((controller.collisions.faceDir != targetDirection) && changingDirection == false)
                {
                    velocity.x = Mathf.Lerp(velocity.x, 0f, 1f);
                    StartCoroutine(ChangeDirection(chaseSpeed));
                    changingDirection = true;
                }

                //if the enemy is facing the right direction.
                else if (controller.collisions.faceDir == targetDirection)
                {
                    changingDirection = false;

                    //If we are close enough to attack.
                    if (InAttackRange())
                    {
                        state = EnemyBase.EnemyState.Attacking;
                    }
                    else
                    {
                        transform.position = Vector3.MoveTowards(transform.position, player.transform.position, Time.deltaTime * chaseSpeed);
                    }
                }
            }
        }
    }

    public override void Patrolling()
    {
        if (!inPatrolZone)
        {
            //enemy is on screen
            if (GetComponent<SpriteRenderer>().isVisible)
            {
                transform.position = Vector3.MoveTowards(transform.position, primaryPatrolPoint, Time.deltaTime * patrolSpeed);
                offScreenTimer = 3;
                if (transform.position == primaryPatrolPoint)
                {
                    inPatrolZone = true;
                }
            }

            //enemy is off screen.
            else if (!GetComponent<SpriteRenderer>().isVisible)
            {
                offScreenTimer -= Time.deltaTime;
                if (offScreenTimer < 0)
                {
                    transform.position = primaryPatrolPoint;
                    inPatrolZone = true;
                }
                else
                {
                    transform.position = Vector3.MoveTowards(transform.position, primaryPatrolPoint, Time.deltaTime * patrolSpeed);
                }
            }
        }
        else
        {
            if (transform.position == currentPatrolTarget)
            {
                int randomNumber = Random.Range(0, globalPatrolPoints.Length);
                currentPatrolTarget = globalPatrolPoints[randomNumber];
            }
            else
            {
                int targetDirection = (currentPatrolTarget.x > transform.position.x) ? 1 : -1;
                if ((controller.collisions.faceDir != targetDirection) && changingDirection == false)
                {
                    velocity.x = Mathf.Lerp(velocity.x, 0f, 1f);
                    StartCoroutine(ChangeDirection(chaseSpeed));
                    changingDirection = true;
                }

                //if the enemy is facing the right direction.
                else if (controller.collisions.faceDir == targetDirection)
                {
                    changingDirection = false;
                    transform.position = Vector3.MoveTowards(transform.position, currentPatrolTarget, Time.deltaTime * patrolSpeed);
                }
            }
        }
    }

    public override void Investigating()
    {
        inPatrolZone = false;
        //Set state to patrolling once the counter has run out.
        if (engagementCounter <= 0)
        {
            ResetEngagementCountDown();
            //investigating = false;
            state = EnemyBase.EnemyState.Patroling;
        }

        //Engagement counter still has time.
        else if (engagementCounter > 0)
        {
            //Go back to chasing.
            if (LineOfSight())
            {
                ResetEngagementCountDown();
                //investigating = false;
                state = EnemyBase.EnemyState.Chasing;
            }

            //Time to investigate.
            else if (!LineOfSight())
            {
                EngagementCountDown();

                transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * chaseSpeed);
            }
        }

    }

    //Draws the patrol points.
    void OnDrawGizmos()
    {
        if (localPatrolPoints != null)
        {
            Gizmos.color = Color.red;
            float size = 5f;

            for (int i = 0; i < localPatrolPoints.Length; i++)
            {
                Vector3 globalWaypointPos = (Application.isPlaying) ? globalPatrolPoints[i] : localPatrolPoints[i] + transform.position;
                Gizmos.DrawLine(globalWaypointPos - Vector3.up * size, globalWaypointPos + Vector3.up * size);
                Gizmos.DrawLine(globalWaypointPos - Vector3.left * size, globalWaypointPos + Vector3.left * size);
            }
        }
    }




    //#region Variables
    //[HideInInspector]
    //public EnemyStats stats;

    //[Tooltip("This field is used to specify which layers block the attacking and abilities raycasts.")]
    //public LayerMask attackingLayer;

    //[Tooltip("This field is used to specify which layers the enemy can move on.")]
    //public LayerMask patrolMask;

    //public Vector3[] localPatrolPoints;
    //Vector3[] globalPatrolPoints;
    //bool inPatrolZone;
    //Vector3 primaryPatrolPoint;
    //float offScreenTimer;
    //public Vector3 currentPatrolTarget;

    //public DamageReceived damageReceived;

    //private float patrolSpeed;
    //private float chaseSpeed;
    //private float chaseTime;
    //private float pivotTime;

    //[HideInInspector]
    //public float engagementCounter;
    //[HideInInspector]
    //public bool changingDirection;

    //public EnemyState state;

    //[HideInInspector]
    //public bool isAttacking;
    //[HideInInspector]
    //public bool beingAttacked;
    //[HideInInspector]
    //public Animator enemyAnimationController;

    //Vector2 targetPosition;
    //[HideInInspector]
    //public Vector2 eyePositionLeft;
    //[HideInInspector]
    //public Vector2 eyePositionRight;
    //[HideInInspector]
    //public Vector2 eyePosition;
    //[HideInInspector]
    //public float eyePositionModifierX;
    //[HideInInspector]
    //public float eyePositionModifierY;

    //[HideInInspector]
    //public float gravity;

    //[HideInInspector]
    //public Vector3 velocity;
    //[HideInInspector]
    //public Vector2 input;
    //[HideInInspector]
    //public Controller2D controller;
    //[HideInInspector]
    //public BoxCollider2D enemyCollider;
    //[HideInInspector]
    //public PlayerDetection playerDetection;
    //[HideInInspector]
    //public GameObject player;
    //[HideInInspector]

    ////[HideInInspector]
    //public bool enraged;
    //[HideInInspector]
    //public float enragedTimer;
    //float maxEnragedTimer;
    //bool buildingRage;
    //bool losingRage;
    //[HideInInspector]
    //public float rageTimer;
    //float maxRageTimer;
    //#endregion

    //public virtual void Start()
    //{
    //    stats = GetComponent<EnemyStats>();
    //    patrolSpeed = stats.patrolSpeed;
    //    chaseSpeed = stats.chaseSpeed;
    //    chaseTime = stats.chaseTime;
    //    pivotTime = stats.pivotTime;
    //    maxEnragedTimer = stats.maxEnragedTimer;
    //    maxRageTimer = stats.maxRageTimer;

    //    primaryPatrolPoint = transform.position;
    //    controller = GetComponent<Controller2D>();
    //    enemyAnimationController = GetComponent<Animator>();
    //    enemyCollider = GetComponent<BoxCollider2D>();
    //    playerDetection = transform.GetChild(0).GetComponent<PlayerDetection>();
    //    player = FindObjectOfType<Player>().gameObject;

    //    input = new Vector2(0, 0);

    //    isAttacking = false;
    //    changingDirection = false;
    //    beingAttacked = false;
    //    inPatrolZone = true;

    //    enraged = false;
    //    buildingRage = false;
    //    losingRage = false;

    //    engagementCounter = chaseTime + pivotTime;

    //    eyePositionModifierX = (enemyCollider.size.x / 2) * .5f;
    //    eyePositionModifierY = (enemyCollider.size.y / 2) * .0f;

    //    globalPatrolPoints = new Vector3[localPatrolPoints.Length];
    //    for (int i = 0; i < localPatrolPoints.Length; i++)
    //    {
    //        globalPatrolPoints[i] = localPatrolPoints[i] + transform.position;
    //    }

    //    currentPatrolTarget = globalPatrolPoints[0];

    //    state = EnemyState.Patroling;
    //}

    //public virtual void Update()
    //{
    //    RageManagement();

    //    //flips sprite depending on direction facing.
    //    if (controller.collisions.faceDir == -1)
    //    {
    //        gameObject.GetComponent<SpriteRenderer>().flipX = false;
    //        eyePositionLeft = new Vector2(transform.position.x - eyePositionModifierX, transform.position.y + eyePositionModifierY);
    //        eyePosition = eyePositionLeft;
    //    }
    //    else
    //    {
    //        gameObject.GetComponent<SpriteRenderer>().flipX = true;
    //        eyePositionRight = new Vector2(transform.position.x + eyePositionModifierX, transform.position.y + eyePositionModifierY);
    //        eyePosition = eyePositionRight;
    //    }
    //    if (controller.collisions.above || controller.collisions.below)
    //    {
    //        velocity.y = 0;
    //    }
    //}

    //public void RageManagement()
    //{
    //    if (maxEnragedTimer != 0)
    //    {
    //        //Building Rage
    //        if (buildingRage)
    //        {
    //            if (rageTimer <= 0)
    //            {
    //                enragedTimer = maxEnragedTimer;
    //                enraged = true;
    //                buildingRage = false;
    //                losingRage = false;
    //                rageTimer = maxRageTimer;
    //            }
    //            else
    //            {
    //                rageTimer -= Time.deltaTime;
    //            }
    //        }

    //        //Losing Rage
    //        if (losingRage)
    //        {
    //            if (rageTimer >= maxRageTimer)
    //            {
    //                losingRage = false;
    //                rageTimer = maxRageTimer;
    //            }
    //            else
    //            {
    //                rageTimer += Time.deltaTime;
    //            }
    //        }

    //        //Conditions for Building Rage
    //        if (beingAttacked && !enraged)
    //        {
    //            buildingRage = true;
    //            losingRage = false;
    //        }

    //        //Conditions for Losing Rage
    //        if (state == EnemyState.Chasing || state == EnemyState.Patroling || state == EnemyState.Investigating || state == EnemyState.Fleeing)
    //        {
    //            losingRage = true;
    //            buildingRage = false;
    //        }

    //        //Being Enraged
    //        if (enraged)
    //        {
    //            if (enragedTimer <= 0)
    //            {
    //                enraged = false;
    //                rageTimer = maxRageTimer;
    //            }
    //            else
    //            {
    //                enragedTimer -= Time.deltaTime;
    //            }
    //        }
    //    }
    //}

    //public void Chasing()
    //{
    //    inPatrolZone = false;
    //    //If the engagement counter has run out we stop chasing.
    //    if (engagementCounter <= 0)
    //    {
    //        state = EnemyState.Patroling;
    //        ResetEngagementCountDown();
    //    }

    //    //If the engagement counter still has time we continue chasing.
    //    else if (engagementCounter > 0)
    //    {
    //        //Check if the line of sight is broken.
    //        if (!LineOfSight())
    //        {
    //            targetPosition = player.transform.position;
    //            state = EnemyState.Investigating;
    //        }

    //        //If we have a LOS we continue to chase.
    //        else if (LineOfSight())
    //        {
    //            ResetEngagementCountDown();

    //            int targetDirection = (transform.position.x >= player.transform.position.x) ? -1 : 1;

    //            //if they enemy is facing the wrong direction.
    //            if ((controller.collisions.faceDir != targetDirection) && changingDirection == false)
    //            {
    //                velocity.x = Mathf.Lerp(velocity.x, 0f, 1f);
    //                StartCoroutine(ChangeDirection(chaseSpeed));
    //                changingDirection = true;
    //            }

    //            //if the enemy is facing the right direction.
    //            else if (controller.collisions.faceDir == targetDirection)
    //            {
    //                changingDirection = false;

    //                //If we are close enough to attack.
    //                if (InAttackRange())
    //                {
    //                    state = EnemyState.Attacking;
    //                }
    //                else
    //                {
    //                    transform.position = Vector3.MoveTowards(transform.position, player.transform.position, Time.deltaTime * chaseSpeed);
    //                }
    //            }
    //        }
    //    }
    //}

    //public void Patrolling()
    //{
    //    if (!inPatrolZone)
    //    {
    //        //enemy is on screen
    //        if (GetComponent<SpriteRenderer>().isVisible)
    //        {
    //            transform.position = Vector3.MoveTowards(transform.position, primaryPatrolPoint, Time.deltaTime * patrolSpeed);
    //            offScreenTimer = 3;
    //            if (transform.position == primaryPatrolPoint)
    //            {
    //                inPatrolZone = true;
    //            }
    //        }

    //        //enemy is off screen.
    //        else if (!GetComponent<SpriteRenderer>().isVisible)
    //        {
    //            offScreenTimer -= Time.deltaTime;
    //            if (offScreenTimer < 0)
    //            {
    //                transform.position = primaryPatrolPoint;
    //                inPatrolZone = true;
    //            }
    //            else
    //            {
    //                transform.position = Vector3.MoveTowards(transform.position, primaryPatrolPoint, Time.deltaTime * patrolSpeed);
    //            }
    //        }
    //    }
    //    else
    //    {
    //        if (transform.position == currentPatrolTarget)
    //        {
    //            int randomNumber = Random.Range(0, globalPatrolPoints.Length);
    //            currentPatrolTarget = globalPatrolPoints[randomNumber];
    //        }
    //        else
    //        {
    //            int targetDirection = (currentPatrolTarget.x > transform.position.x) ? 1 : -1;
    //            if ((controller.collisions.faceDir != targetDirection) && changingDirection == false)
    //            {
    //                velocity.x = Mathf.Lerp(velocity.x, 0f, 1f);
    //                StartCoroutine(ChangeDirection(chaseSpeed));
    //                changingDirection = true;
    //            }

    //            //if the enemy is facing the right direction.
    //            else if (controller.collisions.faceDir == targetDirection)
    //            {
    //                changingDirection = false;
    //                transform.position = Vector3.MoveTowards(transform.position, currentPatrolTarget, Time.deltaTime * patrolSpeed);
    //            }
    //        }
    //    }
    //}

    //public void Investigating()
    //{
    //    inPatrolZone = false;
    //    //Set state to patrolling once the counter has run out.
    //    if (engagementCounter <= 0)
    //    {
    //        ResetEngagementCountDown();
    //        //investigating = false;
    //        state = EnemyState.Patroling;
    //    }

    //    //Engagement counter still has time.
    //    else if (engagementCounter > 0)
    //    {
    //        //Go back to chasing.
    //        if (LineOfSight())
    //        {
    //            ResetEngagementCountDown();
    //            //investigating = false;
    //            state = EnemyState.Chasing;
    //        }

    //        //Time to investigate.
    //        else if (!LineOfSight())
    //        {
    //            EngagementCountDown();

    //            transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * chaseSpeed);
    //        }
    //    }

    //}

    //public void EngagementCountDown()
    //{
    //    if (engagementCounter > 0)
    //    {
    //        engagementCounter -= Time.deltaTime;
    //    }
    //}

    //public void ResetEngagementCountDown()
    //{
    //    engagementCounter = chaseTime + pivotTime;
    //}

    //public bool OriginalLineOfSight()
    //{
    //    if (controller.collisions.faceDir == 1 && player.transform.position.x >= transform.position.x)
    //    {
    //        RaycastHit2D hit = Physics2D.Linecast(eyePosition, player.transform.position, attackingLayer);
    //        if (hit)
    //        {
    //            if (hit.collider.gameObject.tag == "Player")
    //            {
    //                return true;
    //            }
    //            return false;
    //        }
    //        return false;
    //    }
    //    else if (controller.collisions.faceDir == -1 && player.transform.position.x <= transform.position.x)
    //    {
    //        RaycastHit2D hit = Physics2D.Linecast(eyePosition, player.transform.position, attackingLayer);
    //        if (hit)
    //        {
    //            if (hit.collider.gameObject.tag == "Player")
    //            {
    //                return true;
    //            }
    //            return false;
    //        }
    //        return false;
    //    }
    //    return false;
    //}

    //public bool LineOfSight()
    //{
    //    RaycastHit2D hit = Physics2D.Linecast(eyePosition, player.transform.position, attackingLayer);
    //    if (hit)
    //    {
    //        if (hit.collider.gameObject.tag == "Player" && playerDetection.playerInRadius)
    //        {
    //            return true;
    //        }
    //        return false;
    //    }
    //    return false;
    //}

    //IEnumerator ChangeDirection(float moveSpeed)
    //{
    //    yield return new WaitForSeconds(pivotTime);
    //    controller.collisions.faceDir = controller.collisions.faceDir * -1;
    //    velocity.x = Mathf.Lerp(velocity.x, controller.collisions.faceDir * moveSpeed, 1f);
    //}

    //public enum EnemyState
    //{
    //    Patroling,
    //    Chasing,
    //    Investigating,
    //    Attacking,
    //    Fleeing
    //}

    ////Check to see if we are close enough to enter attack mode.
    //public bool InAttackRange()
    //{
    //    float directionX = controller.collisions.faceDir;
    //    float rayLength = stats.attackRange;
    //    float rayOriginX = enemyCollider.bounds.center.x;
    //    float rayOriginY = enemyCollider.bounds.center.y;
    //    Vector2 rayOrigin = new Vector2(rayOriginX, rayOriginY);

    //    RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, attackingLayer);

    //    if (hit)
    //    {
    //        if (hit.collider.gameObject.layer == 9)
    //        {
    //            return true;
    //        }
    //    }
    //    return false;
    //}

    //public void CallEnemyDefeated()
    //{
    //    StartCoroutine("EnemyDefeated");
    //}

    ////When the enemy dies.
    //public IEnumerator EnemyDefeated()
    //{
    //    while (GameObject.FindGameObjectWithTag("UserInterface").GetComponent<UserInterface>().tallyingSpoils == true)
    //    {
    //        yield return null;
    //    }

    //    GameObject.FindGameObjectWithTag("UserInterface").GetComponent<UserInterface>().tallyingSpoils = true;
    //    LevelManager.levelManager.enemiesDefeated += 1;

    //    GameObject.FindGameObjectWithTag("UserInterface").GetComponent<UserInterface>().ReceiveXP(stats.expGranted);
    //    GameControl.gameControl.xp += stats.expGranted;

    //    EquipmentDrops();
    //    ItemDrops();
    //    GameObject.FindGameObjectWithTag("UserInterface").GetComponent<UserInterface>().CallReceiveEquipment();

    //    Destroy(this.gameObject);
    //}

    ////Add Equipment drops to player list
    //public void EquipmentDrops()
    //{
    //    foreach (Equipment equipment in stats.equipmentDropped)
    //    {
    //        int randomNumber = Random.Range(0, 101);
    //        if (randomNumber <= equipment.dropRate)
    //        {
    //            GameObject.FindGameObjectWithTag("UserInterface").GetComponent<UserInterface>().receivedEquipment.Add(EquipmentDatabase.equipmentDatabase.equipment[equipment.equipmentID]);
    //        }
    //    }
    //}

    ////Add Item drops to player list
    //public void ItemDrops()
    //{
    //    foreach (Items item in stats.itemsDropped)
    //    {
    //        int randomNumber = Random.Range(0, 101);
    //        if (randomNumber <= item.dropRate)
    //        {
    //            GameObject.FindGameObjectWithTag("UserInterface").GetComponent<UserInterface>().receivedItems.Add(ItemDatabase.itemDatabase.items[item.itemID]);
    //        }
    //    }
    //}

    ////Called from the animator.
    //public void FlinchRecovered()
    //{
    //    beingAttacked = false;
    //}

    ////this will be used to gauge interactions...I might need to do these things in the climbable script.
    //public void OnTriggerEnter2D(Collider2D collider)
    //{

    //}

    ////this will be used to gauge interactions...I might need to do these things in the climbable script.
    //public void OnTriggerExit2D(Collider2D collider)
    //{

    //}

    ////called from attacking animation at the begining and end.
    //public void IsAttacking()
    //{
    //    isAttacking = !isAttacking;
    //}

    ////used to pause the animator
    //public void PauseAnimator()
    //{
    //    enemyAnimationController.enabled = false;
    //}

    ////Called from the animator.
    //public virtual void Attack()
    //{
    //    float directionX = controller.collisions.faceDir;
    //    float rayLength = stats.attackRange;
    //    float rayOriginX = (directionX == 1) ? enemyCollider.bounds.max.x + 0.01f : enemyCollider.bounds.min.x - 0.01f;
    //    float rayOriginY = enemyCollider.bounds.center.y;
    //    Vector2 rayOrigin = new Vector2(rayOriginX, rayOriginY);

    //    RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, attackingLayer);
    //    //Layer 9 is currently the players layer.
    //    if (hit)
    //    {
    //        if (hit.collider.gameObject.layer == 9)
    //        {
    //            CombatEngine.combatEngine.AttackingPlayer(this.GetComponent<Collider2D>(), stats.maximumDamage);
    //        }
    //    }
    //}

    ////Triggered by the Combat Engine.
    //public void DisplayDamageReceived(int damage)
    //{
    //    damageReceived.CalculateTheNumber(damage);
    //}

    ////Called by Combat Engine.
    //public void BeingAttacked()
    //{
    //    beingAttacked = true;
    //}

    ////Draws the patrol points.
    //void OnDrawGizmos()
    //{
    //    if (localPatrolPoints != null)
    //    {
    //        Gizmos.color = Color.red;
    //        float size = 5f;

    //        for (int i = 0; i < localPatrolPoints.Length; i++)
    //        {
    //            Vector3 globalWaypointPos = (Application.isPlaying) ? globalPatrolPoints[i] : localPatrolPoints[i] + transform.position;
    //            Gizmos.DrawLine(globalWaypointPos - Vector3.up * size, globalWaypointPos + Vector3.up * size);
    //            Gizmos.DrawLine(globalWaypointPos - Vector3.left * size, globalWaypointPos + Vector3.left * size);
    //        }
    //    }
    //}
}