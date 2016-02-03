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
    private float timeToJumpApex = .3f;
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
    GameObject patrolPlatform;

    public LayerMask patrolMask;

    private bool patrolPathCreated;

    //[HideInInspector]
    public EnemyState state;

    [HideInInspector]
    public bool isAttacking;

    private Animator enemyAnimationController;

    Vector2 targetPosition;

    Vector2 eyePositionLeft;
    Vector2 eyePositionRight;
    Vector2 eyePosition;
    float eyePositionModifierX;
    float eyePositionModifierY;

    private float gravity;
    private float maxJumpVelocity;
    private float maxJumpDistance;
    private float timeAirborn;
    public bool airborne;
    public bool investigating;
    float jumpTargetX;
    float jumpTargetY;
    private Vector2 jumpTarget;
    GameObject targetPlatform;

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
        maxJumpVelocity = (Mathf.Abs(gravity) * (timeToJumpApex)) * ((Mathf.Pow(jumpHeight, -0.5221f)) * 0.1694f) * jumpHeight;
        timeAirborn = (maxJumpVelocity / Mathf.Abs(gravity)) * 2;
        maxJumpDistance = timeAirborn * chaseSpeed;
        airborne = false;
        isAttacking = false;
        changingDirection = false;
        patrolPathCreated = false;
        investigating = false;

        engagementCounter = chaseTime + pivotTime;

        eyePositionModifierX = (enemyCollider.size.x / 2) * .5f;
        eyePositionModifierY = (enemyCollider.size.y / 2) * .0f;

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
            eyePositionLeft = new Vector2(transform.position.x - eyePositionModifierX, transform.position.y + eyePositionModifierY);
            eyePosition = eyePositionLeft;
        }
        else
        {
            gameObject.GetComponent<SpriteRenderer>().flipX = true;
            eyePositionRight = new Vector2(transform.position.x + eyePositionModifierX, transform.position.y + eyePositionModifierY);
            eyePosition = eyePositionRight;
        }

        //cant move if attacking.
        if (isAttacking)
        {
            input = Vector2.zero;
        }

        //trigger for entering chase mode.
        if(playerDetection.playerInRadius && state == EnemyState.Patroling)
        {
            if (OriginalLineOfSight())
            {
                state = EnemyState.Chasing;
                ResetEngagementCountDown();
            }
        }


        //Chasing
        if (state == EnemyState.Chasing)
        {
            Chasing (input);

            gravity = -1000;
            velocity.y += gravity * Time.deltaTime;
            controller.Move(velocity * Time.deltaTime, input);
        }


        //Investigating
        if (state == EnemyState.Investigating)
        {
            Investigating();

            gravity = -1000;
            velocity.y += gravity * Time.deltaTime;
            controller.Move(velocity * Time.deltaTime, input);
        }


        //Patroling
        if (state == EnemyState.Patroling)
        {
            Patrolling();

            gravity = -1000;
            velocity.y += gravity * Time.deltaTime;
            controller.Move(velocity * Time.deltaTime, input);
        }


        if (controller.collisions.above || controller.collisions.below)
        {
            velocity.y = 0;
        }
    }

    //chasing needs work....
    private void Chasing(Vector2 input)
    {
        //If the enemy is in the air we dont want them loosing their directive and trying to make a path. So we make finishing the jump the priority.
        if (airborne)
        {
            engagementCounter += 0.8f;
        }

        //If the enemy is on the ground they continue operations as normal.
        else if (!airborne)
        {
            //If the engagement counter has run out we stop chasing.
            if (engagementCounter <= 0)
            {
                state = EnemyState.Patroling;
                ResetEngagementCountDown();
            }

            //If the engagement counter still has time we continue chasing.
            else if (engagementCounter > 0)
            {
                //Check if the line of sight is broken.
                if (!LineOfSight())
                {
                    targetPosition = player.transform.position;
                    state = EnemyState.Investigating;
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

                        //run through the scenarios....
                        ChaseScernarios();

                        //if we are at the max of the original patrol path.
                        if (transform.position.x >= maxPatrolX || transform.position.x <= minPatrolX)
                        {
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

                            //Scenario 1
                            //the player is above the enemy and within the enemies vertical jump range.
                            if (playerPlatformMaxY - (transform.position.y - enemyCollider.size.y / 2) < (jumpHeight - 5) && playerPlatformMaxY > enemyCollider.bounds.min.y)
                            {
                                //if the platform is to the left of the enemy.
                                if (transform.position.x > playerPlatformMaxX)
                                {
                                    jumpTargetX = playerPlatformMaxX - (enemyCollider.size.x / 2);
                                }
                                //if the platform is to the right of the enemy.
                                else
                                {

                                }

                            }

                            //Scenario 2
                            //the player is below the enemy and within the enemies horizontal jump range.
                            if (playerPlatformMaxY - (transform.position.y - enemyCollider.size.y / 2) < maxJumpDistance && playerPlatformMaxY > enemyCollider.bounds.min.y)
                            {

                            }

                            velocity.x = 0;
                        }

                        //still in the original patrol path.
                        else
                        {
                            velocity.x = Mathf.Lerp(velocity.x, controller.collisions.faceDir * chaseSpeed, 1f);
                        }
                    }

                    gravity = -1000;
                    velocity.y += gravity * Time.deltaTime;

                    controller.Move(velocity * Time.deltaTime, input);
                }
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

    private void Investigating()
    {
        if (airborne)
        {
            if ((jumpTargetX <= transform.position.x && controller.collisions.faceDir == -1) || (jumpTargetX >= transform.position.x && controller.collisions.faceDir == 1))
            {
                velocity.x = controller.collisions.faceDir * chaseSpeed;
            }
            else
            {
                velocity.x = 0;
                airborne = false;
                CreatePatrolPath();
            }
        }
        else
        {
            //Set state to patrolling once the counter has run out.
            if (engagementCounter <= 0)
            {
                ResetEngagementCountDown();
                investigating = false;
                state = EnemyState.Patroling;
            }

            //Engagement counter still has time.
            else if (engagementCounter > 0)
            {
                //Go back to chasing.
                if (LineOfSight())
                {
                    ResetEngagementCountDown();
                    investigating = false;
                    state = EnemyState.Chasing;
                }
                else if (!LineOfSight())
                {
                    EngagementCountDown();
                    //If we have been through the investigation scenarios and have a target.
                    if (investigating)
                    {
                        //Check if jump is necessary.
                        //Jumping is not necessary.
                        if (!IsJumpNecessary())
                        {
                            Debug.Log("no jumping needed.");
                            if ((jumpTargetX < transform.position.x && controller.collisions.faceDir == -1) || (jumpTargetX > transform.position.x && controller.collisions.faceDir == 1))
                            {
                                velocity.x = controller.collisions.faceDir * chaseSpeed;
                            }
                            else
                            {
                                ChangeDirection(chaseSpeed);
                            }
                        }

                        //Jumping is necessary.
                        else
                        {
                            Debug.Log("jumping needed.");
                            Debug.Log(jumpTargetX - maxJumpDistance);

                            //if (((jumpTargetX - maxJumpDistance <= maxPatrolX) && controller.collisions.faceDir == 1) || ((jumpTargetX + maxJumpDistance >= minPatrolX) && controller.collisions.faceDir == -1))
                            
                            //check if the jump is possible.
                            if (IsJumpPossible())
                            {
                                Debug.Log("jump is possible");
                                //Movement.
                                if ((jumpTargetX <= transform.position.x && controller.collisions.faceDir == -1) || (jumpTargetX >= transform.position.x && controller.collisions.faceDir == 1))
                                {
                                    velocity.x = controller.collisions.faceDir * chaseSpeed;
                                }
                                else
                                {
                                    velocity.x = 0;
                                }

                                //Jumping.
                                if ((transform.position.x <= minPatrolX || transform.position.x >= maxPatrolX))
                                {
                                    velocity.y = maxJumpVelocity;
                                    airborne = true;
                                }
                            }
                            else
                            {
                                if ((transform.position.x >= minPatrolX && transform.position.x <= maxPatrolX))
                                {
                                    changingDirection = false;
                                    velocity.x = Mathf.Lerp(velocity.x, controller.collisions.faceDir * patrolSpeed, 1f);
                                }

                                if ((transform.position.x <= minPatrolX || transform.position.x >= maxPatrolX))
                                {
                                    velocity.x = 0;
                                }
                            }
                        }

                        //If we have reached the last known position and still have no LOS, we wait until the timer runs out.
                        if ((jumpTargetX <= transform.position.x && controller.collisions.faceDir == 1) || (jumpTargetX >= transform.position.x && controller.collisions.faceDir == -1))
                        {
                            velocity.x = 0;
                        }
                    }
                    else if (!investigating)
                    {
                        //Check if a platform is below where the player is.
                        RaycastHit2D hit = Physics2D.Raycast(targetPosition, Vector2.down, 500, patrolMask);
                        if (hit)
                        {
                            if (hit.collider.gameObject.layer == 10)
                            {
                                targetPlatform = hit.collider.gameObject;
                                jumpTargetX = targetPosition.x;
                                jumpTargetY = hit.collider.bounds.max.y + (enemyCollider.size.y / 2);
                                jumpTarget = new Vector2(jumpTargetX, jumpTargetY);

                                //Move through the investigation scenarios.
                                InvestigationScenarios();
                            }

                            //If we didn't find a platform under the player when the LOS broke.
                            else
                            {
                                if ((transform.position.x >= minPatrolX && transform.position.x <= maxPatrolX))
                                {
                                    changingDirection = false;
                                    velocity.x = Mathf.Lerp(velocity.x, controller.collisions.faceDir * patrolSpeed, 1f);
                                }

                                if ((transform.position.x <= minPatrolX || transform.position.x >= maxPatrolX))
                                {
                                    velocity.x = 0;
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    private bool IsJumpPossible() //NEEDS WORK.
    {
        Vector2 position1 = new Vector2(1, 1);
        Vector2 position2 = new Vector2(1, 1);
        Vector2 position3 = new Vector2(1, 1);

        RaycastHit2D part1 = Physics2D.Linecast(transform.position, position1, attackingLayer);
        RaycastHit2D part2 = Physics2D.Linecast(position1, position2, attackingLayer);
        RaycastHit2D part3 = Physics2D.Linecast(position2, position3, attackingLayer);
        RaycastHit2D part4 = Physics2D.Linecast(position3, jumpTarget, attackingLayer);
        return true;
    }

    public bool IsJumpNecessary () //NEEDS WORK.
    {
        return true;
    }

    public void InvestigationScenarios ()
    {
        RaycastHit2D hit = Physics2D.Linecast(eyePosition, jumpTarget, attackingLayer);
        
        //Scenario 1 - No Jumping is required for investigation, Same Platform.
        if (!hit && patrolPlatform == targetPlatform)
        {
            if(jumpTargetX < transform.position.x)
            {
                jumpTargetX = jumpTargetX + (enemyCollider.size.x / 2);
            }
            else
            {
                jumpTargetX = jumpTargetX - (enemyCollider.size.x / 2);
            }
            jumpTargetY = transform.position.y;
            investigating = true;
            Debug.Log("Investigation #1");
        }

        //Scenario 2 - Jumping is required for investigation, Different platforms, No Overlap.
        else if (patrolPlatform != targetPlatform && !PlatformOverlap())
        {
            if (jumpTargetX < transform.position.x)
            {
                jumpTargetX = targetPlatform.GetComponent<BoxCollider2D>().bounds.max.x - (enemyCollider.size.x / 2);
            }
            else
            {
                jumpTargetX = targetPlatform.GetComponent<BoxCollider2D>().bounds.min.x + (enemyCollider.size.x / 2);
            }

            investigating = true;
            Debug.Log("Investigation #2");
        }
        
        //Scenario 3 - Jumping is required for investigation, Different platforms, Overlap.
        else if (patrolPlatform != targetPlatform && PlatformOverlap())
        {
            //Keep the jump targets as they were.
            investigating = true;
            Debug.Log("Investigation #3");
        }

        //Scenario 4 - Jumping is required for investigation, Same platform.
        if (hit && patrolPlatform == targetPlatform)
        {
            investigating = true;
            Debug.Log("Investigation #4");
        }
        jumpTarget = new Vector2(jumpTargetX, jumpTargetY);
    }

    public void ChaseScernarios ()
    {

    }

    public bool PlatformOverlap ()
    {
        Bounds targetPlatformBounds = targetPlatform.GetComponent<Collider2D>().bounds;
        Bounds patrolPlatformBounds = patrolPlatform.GetComponent<Collider2D>().bounds;
        if (targetPlatformBounds.max.x > patrolPlatformBounds.min.x || targetPlatformBounds.min.x < patrolPlatformBounds.max.x)
        {
            return true;
        }
        return false;
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

    public bool OriginalLineOfSight ()
    {
        if (controller.collisions.faceDir == 1 && player.transform.position.x >= transform.position.x)
        {
            RaycastHit2D hit = Physics2D.Linecast(eyePosition, player.transform.position, attackingLayer); 
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
            RaycastHit2D hit = Physics2D.Linecast(eyePosition, player.transform.position, attackingLayer);
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

    public bool LineOfSight ()
    {
        RaycastHit2D hit = Physics2D.Linecast(eyePosition, player.transform.position, attackingLayer);
        if (hit)
        {
            if (hit.collider.gameObject.tag == "Player" && playerDetection.playerInRadius)
            {
                return true;
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