using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Controller2D))]
public class Scorpion1 : MonoBehaviour
{
    [Tooltip("This field is used to specify which layers block the attacking and abilities raycasts.")]
    public LayerMask attackingLayer;

    public int hP;
    public int expGained;
    public float attackDamage;
    public float defense;
    //public List<Items> itemsDropped;
    //public List<Equipment> equipmentDropped;

    public float jumpHeight = 4;
    //private float minJumpHeight = 1;
    private float timeToJumpApex = .4f;
    //private float accelerationTimeAirborne = .2f;
    //private float accelerationTimeGrounded = .1f;
    public float patrolSpeed = 6;
    public float chaseSpeed = 6;
    [Tooltip("The amount of time an enemy will remain engaged after losing the line of sight.")]
    public float chaseTime;
    public float climbSpeed = 50;
    [Tooltip("The length of time in seconds it takes for the enemy to change directions.")]
    public float pivotTime;

    public float engagementCounter;

    public bool changingDirection;

    private float minPatrolX;
    private float maxPatrolX;

    public LayerMask patrolMask;

    private bool patrolPathCreated;

    //[HideInInspector]
    public EnemyState state;

    [HideInInspector]
    public bool isAttacking;

    private Animator enemyAnimationController;


    private float gravity;
    //private float maxJumpVelocity;
    //private float minJumpVelocity;
    private Vector3 velocity;

    private Controller2D controller;

    private BoxCollider2D enemyCollider;

    private PlayerDetection playerDetection;

    private GameObject player;
    private float playerPlatformMaxY;
    private float playerPlatformMinY;
    private float playerPlatformMaxX;
    private float playerPlatformMinX;

    void Start()
    {
        controller = GetComponent<Controller2D>();
        enemyAnimationController = GetComponent<Animator>();
        enemyCollider = GetComponent<BoxCollider2D>();
        playerDetection = transform.GetChild(0).GetComponent<PlayerDetection>();
        player = FindObjectOfType<Player>().gameObject;

        gravity = -1000;
        //maxJumpVelocity = Mathf.Abs(gravity) * (timeToJumpApex) * (maxJumpHeight * 0.02312f);
        //minJumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(gravity) * minJumpHeight);
        isAttacking = false;
        changingDirection = false;
        patrolPathCreated = false;

        engagementCounter = chaseTime + pivotTime;

        CreatePatrolPath();
        state = EnemyState.Patroling;
    }

    void Update()
    {
        //this will be changed based on the state of the enemy.
        Vector2 input = new Vector2(0, 0);
        //int wallDirX = (controller.collisions.left) ? 1 : 1;

        //flips sprite depending on direction facing.
        if (controller.collisions.faceDir == -1)
        {
            gameObject.GetComponent<SpriteRenderer>().flipX = false;
        }
        else
        {
            gameObject.GetComponent<SpriteRenderer>().flipX = true;
        }

        //cant move if attacking.
        if (isAttacking)
        {
            input = Vector2.zero;
        }

        //trigger for entering chase mode.
        if(playerDetection.playerInRadius)
        {
            if (LineOfSight())
            {
                state = EnemyState.Chasing;
                ResetEngagementCountDown();
            }
        }
        else
        {
            EngagementCountDown();
        }


        //Chasing
        if (state == EnemyState.Chasing)
        {
            if (!LineOfSight())
            {
                EngagementCountDown();
            }

            if (engagementCounter > 0)
            {
                int targetDirection = (transform.position.x >= player.transform.position.x) ? -1 : 1;

                //if the enemy is facing the right direction.
                if (controller.collisions.faceDir == targetDirection)
                {
                    changingDirection = false;

                    //if we are at the max of the original patrol path.
                    if (transform.position.x >= maxPatrolX || transform.position.x <= minPatrolX)
                    {
                        patrolPathCreated = false;

                        //see what platform the player is on.
                        float rayLength = 250f;
                        RaycastHit2D playerPlatform = Physics2D.Raycast(player.transform.position, Vector2.down, rayLength, patrolMask);
                        if (playerPlatform)
                        {
                            if (playerPlatform.collider.gameObject.layer == 10)
                            {
                                playerPlatformMaxX = playerPlatform.collider.bounds.max.x;
                                playerPlatformMinX = playerPlatform.collider.bounds.min.x;
                                playerPlatformMaxY = playerPlatform.collider.bounds.max.y;
                                playerPlatformMinY = playerPlatform.collider.bounds.min.y;
                            }
                        }

                        //compare where the player is to the enemy. See if the distance can be covered, by jumping.
                        //need to do math....yuck.
                        velocity.x = 0;
                    }

                    //still in the original patrol path.
                    else
                    {
                        velocity.x = Mathf.Lerp(velocity.x, controller.collisions.faceDir * chaseSpeed, 1f);
                    }
                }


                //if they enemy is facing the wrong direction.
                if ((controller.collisions.faceDir != targetDirection) && changingDirection == false)
                {
                    velocity.x = Mathf.Lerp(velocity.x, 0f, 1f);
                    StartCoroutine(ChangeDirection(chaseSpeed));
                    changingDirection = true;
                }

                gravity = -(2 * jumpHeight) / Mathf.Pow(timeToJumpApex, 2);
                velocity.y += gravity * Time.deltaTime;

                controller.Move(velocity * Time.deltaTime, input);
            }
            else
            {
                state = EnemyState.Patroling;
                ResetEngagementCountDown();
            }
        }


        //Patroling
        if (state == EnemyState.Patroling)
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
                velocity.x = Mathf.Lerp(velocity.x, 0f, 1f);
                StartCoroutine(ChangeDirection(patrolSpeed));
                changingDirection = true;
            }


            gravity = -(2 * jumpHeight) / Mathf.Pow(timeToJumpApex, 2);
            velocity.y += gravity * Time.deltaTime;

            controller.Move(velocity * Time.deltaTime, input);
        }


        if (controller.collisions.above || controller.collisions.below)
        {
            velocity.y = 0;
        }
    }

    public void EngagementCountDown ()
    {
        if (engagementCounter > 0)
        {
            engagementCounter -= Time.deltaTime;
        }
    }

    public void ResetEngagementCountDown ()
    {
        engagementCounter = chaseTime + pivotTime;
    }

    public bool LineOfSight ()
    {
        if (controller.collisions.faceDir == 1 && player.transform.position.x >= transform.position.x)
        {
            RaycastHit2D hit = Physics2D.Linecast(transform.position, player.transform.position, attackingLayer); 
            if (hit)
            {
                if (hit.collider.gameObject.tag == "Player")
                {
                    return true;
                }
                return false;
            }
            return false;
        } else if (controller.collisions.faceDir == -1 && player.transform.position.x <= transform.position.x)
        {
            RaycastHit2D hit = Physics2D.Linecast(transform.position, player.transform.position, attackingLayer);
            if (hit)
            {
                if (hit.collider.gameObject.tag == "Player")
                {
                    return true;
                }
                return false;
            }
            return false;
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
                minPatrolX = bottom.collider.bounds.min.x + (enemyCollider.size.x / 2);
                maxPatrolX = bottom.collider.bounds.max.x - (enemyCollider.size.x / 2);
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
        Attacking,
        Fleeing
    }

    //this will be used to gauge interactions...I might need to do these things in the climbable script.
    public void OnTriggerEnter2D(Collider2D collider)
    {
 
    }

    //this will be used to gauge interactions...I might need to do these things in the climbable script.
    public void OnTriggerExit2D(Collider2D collider)
    {

    }

    //called from attacking animation at the begining and end.
    public void IsAttacking()
    {
        isAttacking = !isAttacking;
    }

    //used to pause the animator
    public void PauseAnimator()
    {
        enemyAnimationController.enabled = false;
    }

    //called from the animations for attacking.

    //I think I will need to pass the collider being hit to the attacking function.
    public void Attack()
    {
        float directionX = controller.collisions.faceDir;
        float rayLength = 100f; //make each weapon have a length component?
        float rayOriginX = enemyCollider.bounds.max.x + 0.01f; //defined as the edge of the collider.
        float rayOriginY = enemyCollider.bounds.center.y; //defined as the center of the collider.
        Vector2 rayOrigin = new Vector2(rayOriginX, rayOriginY);

        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, attackingLayer);
        //Layer 14 is currently the enemies layer.
        if (hit)
        {
            if (hit.collider.gameObject.layer == 14)
            {
                CombatEngine.combatEngine.AttackingEnemies(hit.collider);
            }
        }
    }
}