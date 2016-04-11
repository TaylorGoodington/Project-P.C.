﻿using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Controller2D))]
[RequireComponent(typeof(EnemyStats))]
public class BoneMarauder1 : MonoBehaviour
{
    string enemyType = "BoneMarauder1";
    #region Variables
    [HideInInspector]
    public EnemyStats stats;

    [Tooltip("This field is used to specify which layers block the attacking and abilities raycasts.")]
    public LayerMask attackingLayer;

    private float jumpHeight;
    private float timeToJumpApex = .3f;
    private float patrolSpeed;
    private float chaseSpeed;
    [Tooltip("The amount of time an enemy will remain engaged after losing the line of sight.")]
    private float chaseTime;
    //private float climbSpeed = 50;
    [Tooltip("The length of time in seconds it takes for the enemy to change directions.")]
    private float pivotTime;

    public float engagementCounter;

    public bool changingDirection;

    public float minPatrolX;
    public float maxPatrolX;
    public GameObject patrolPlatform;

    public LayerMask patrolMask;

    private bool patrolPathCreated;

    //[HideInInspector]
    public EnemyState state;

    //[HideInInspector]
    public bool isAttacking;

    bool beingAttacked;

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
    public Vector2 jumpTarget;
    public GameObject targetPlatform;

    private Vector3 velocity;

    private Controller2D controller;

    private BoxCollider2D enemyCollider;

    private PlayerDetection playerDetection;

    private GameObject player;

    private GameObject freeFallPoints;
    private BoxCollider2D fallPoint1;
    private BoxCollider2D fallPoint2;
    private BoxCollider2D fallPoint3;

    private GameObject jumpPoints;
    private BoxCollider2D jumpPoint1;
    private BoxCollider2D jumpPoint2;
    private BoxCollider2D jumpPoint3;
    private BoxCollider2D jumpPoint4;
    #endregion

    void Start()
    {
        stats = GetComponent<EnemyStats>();
        patrolSpeed = stats.patrolSpeed;
        chaseSpeed = stats.chaseSpeed;
        chaseTime = stats.chaseTime;
        jumpHeight = stats.jumpHeight;
        pivotTime = stats.pivotTime;
        AddSkills();
        AddItemsAndEquipmentDrops();

        controller = GetComponent<Controller2D>();
        enemyAnimationController = GetComponent<Animator>();
        enemyCollider = GetComponent<BoxCollider2D>();
        playerDetection = transform.GetChild(0).GetComponent<PlayerDetection>();
        jumpPoints = transform.GetChild(1).gameObject;
        freeFallPoints = transform.GetChild(2).gameObject;
        player = FindObjectOfType<Player>().gameObject;

        CalculatePhysics();

        airborne = false;
        isAttacking = false;
        changingDirection = false;
        patrolPathCreated = false;
        investigating = false;
        beingAttacked = false;

        engagementCounter = chaseTime + pivotTime;

        eyePositionModifierX = (enemyCollider.size.x / 2) * .5f;
        eyePositionModifierY = (enemyCollider.size.y / 2) * .0f;

        jumpPoints = transform.GetChild(1).gameObject;

        CreatePatrolPath();
        state = EnemyState.Patroling;

        jumpPoint1 = jumpPoints.transform.GetChild(0).GetComponent<BoxCollider2D>();
        jumpPoint2 = jumpPoints.transform.GetChild(1).GetComponent<BoxCollider2D>();
        jumpPoint3 = jumpPoints.transform.GetChild(2).GetComponent<BoxCollider2D>();
        jumpPoint4 = jumpPoints.transform.GetChild(3).GetComponent<BoxCollider2D>();

        fallPoint1 = freeFallPoints.transform.GetChild(0).GetComponent<BoxCollider2D>();
        fallPoint2 = freeFallPoints.transform.GetChild(1).GetComponent<BoxCollider2D>();
        fallPoint3 = freeFallPoints.transform.GetChild(2).GetComponent<BoxCollider2D>();

        CalculateJumpCollders();
    }

    //Add Skills Here
    private void AddSkills ()
    {
        stats.acquiredSkillsList.Add(SkillsDatabase.skillsDatabase.skills[0]);
    }

    //Add Items and Equipment Drops here...
    public void AddItemsAndEquipmentDrops ()
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
            isAttacking = false;
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
                eyePositionLeft = new Vector2(transform.position.x - eyePositionModifierX, transform.position.y + eyePositionModifierY);
                eyePosition = eyePositionLeft;
                jumpPoints.transform.localScale = new Vector3(1, 1, 1);
                freeFallPoints.transform.localScale = new Vector3(1, 1, 1);
            }
            else
            {
                gameObject.GetComponent<SpriteRenderer>().flipX = true;
                eyePositionRight = new Vector2(transform.position.x + eyePositionModifierX, transform.position.y + eyePositionModifierY);
                eyePosition = eyePositionRight;
                jumpPoints.transform.localScale = new Vector3(-1, 1, 1);
                freeFallPoints.transform.localScale = new Vector3(-1, 1, 1);
            }

            if (controller.collisions.below == false)
            {
                enemyAnimationController.Play(enemyType + "Jumping");
            }

            //cant move if attacking.
            if (isAttacking)
            {
                input = Vector2.zero;
            }

            //Creates a patrol path when we are on the ground.
            //if (velocity.y == 0) TESTING OUT THE ONE BELOW FOR LOWER PROFORMANCE NEEDS...
            if ((velocity.y == 0 && transform.position.x > maxPatrolX) || (velocity.y == 0 && transform.position.x < minPatrolX))
            {
                RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, (enemyCollider.size.y / 2) + 5, patrolMask);
                if (hit)
                {
                    CreatePatrolPath();
                }
            }

            //trigger for entering chase mode.
            if (playerDetection.playerInRadius && state == EnemyState.Patroling)
            {
                if (OriginalLineOfSight())
                {
                    state = EnemyState.Chasing;
                    ResetEngagementCountDown();
                }
            }

            //Attacking
            if (state == EnemyState.Attacking)
            {
                //This is where we would call the animator to attack.
                if (!InAttackRange())
                {
                    isAttacking = false;
                    state = EnemyState.Chasing;
                }
                else
                {
                    if (!isAttacking && controller.collisions.below == true)
                    {
                        isAttacking = true;
                        enemyAnimationController.Play(enemyType + "Attacking");
                    }
                }
            }

            //Chasing
            if (state == EnemyState.Chasing)
            {
                Chasing();

                gravity = -1000;
                velocity.y += gravity * Time.deltaTime;
                controller.Move(velocity * Time.deltaTime, input);
                if (velocity.x != 0)
                {
                    enemyAnimationController.Play(enemyType + "Running");
                }
                else
                {
                    enemyAnimationController.Play(enemyType + "Idle");
                }
            }


            //Investigating
            if (state == EnemyState.Investigating)
            {
                Investigating();

                gravity = -1000;
                velocity.y += gravity * Time.deltaTime;
                controller.Move(velocity * Time.deltaTime, input);
                if (velocity.x != 0)
                {
                    enemyAnimationController.Play(enemyType + "Running");
                }
                else
                {
                    enemyAnimationController.Play(enemyType + "Idle");
                }
            }


            //Patroling
            if (state == EnemyState.Patroling)
            {
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
            }


            if (controller.collisions.above || controller.collisions.below)
            {
                velocity.y = 0;
            }
        }
    }

    private void CalculatePhysics()
    {
        gravity = -1000;
        maxJumpVelocity = (Mathf.Abs(gravity) * (timeToJumpApex)) * ((Mathf.Pow(jumpHeight, -0.5221f)) * 0.1694f) * jumpHeight;
        timeAirborn = (maxJumpVelocity / Mathf.Abs(gravity)) * 2;
        maxJumpDistance = timeAirborn * chaseSpeed;
    }

    private void CalculateJumpCollders ()
    {
        //Currently only works if the enemy collider is a box.
        jumpPoint1.size = enemyCollider.size;
        jumpPoint2.size = enemyCollider.size;
        jumpPoint3.size = enemyCollider.size;
        jumpPoint4.size = enemyCollider.size;

        //Adjusts jump points based on current speed and jump height.
        float jumpFactor = 0.7f;
        float airborneAdjustment = timeAirborn / 2 * jumpFactor;

        jumpPoint1.transform.localPosition = new Vector2((timeAirborn / 2) * jumpFactor * chaseSpeed * -1, (((maxJumpVelocity * airborneAdjustment) - 
                                                    (Mathf.Abs(gravity) * 0.5f * (Mathf.Pow(airborneAdjustment,2)))) - jumpHeight * 0.1f) + enemyCollider.offset.y);
        jumpPoint2.transform.localPosition = new Vector2(maxJumpDistance / 2 * -1, jumpHeight + enemyCollider.offset.y);

        jumpPoint3.transform.localPosition = new Vector2((timeAirborn / 2) * (1 - jumpFactor + 1) * chaseSpeed * -1, (((maxJumpVelocity * airborneAdjustment) -
                                                    (Mathf.Abs(gravity) * 0.5f * (Mathf.Pow(airborneAdjustment, 2)))) - jumpHeight * 0.1f) + enemyCollider.offset.y);
        jumpPoint4.transform.localPosition = new Vector2(-chaseSpeed * 1.5f, ((maxJumpVelocity * 2) - (500 * (1.5f * 1.5f))) + enemyCollider.offset.y);

        //Adjusts fall points based on current speed and jump height.
        fallPoint1.size = enemyCollider.size;
        fallPoint2.size = enemyCollider.size;
        fallPoint3.size = enemyCollider.size;

        fallPoint1.transform.localPosition = new Vector2((.1f * -chaseSpeed) - enemyCollider.size.x, ((gravity / 2) * (.1f * .1f)) + enemyCollider.offset.y);
        fallPoint2.transform.localPosition = new Vector2((.3f * -chaseSpeed) - enemyCollider.size.x, ((gravity / 2) * (.3f * .3f)) + enemyCollider.offset.y);
        fallPoint3.transform.localPosition = new Vector2((1f * -chaseSpeed) - enemyCollider.size.x, ((gravity / 2) * (1f * 1f)) + enemyCollider.offset.y);
    }

    private void Chasing() 
    {
        //If the enemy is in the air we dont want them loosing their directive and trying to make a path. So we make finishing the jump the priority.
        if (airborne)
        {
            if (velocity.y != 0)
            {
                velocity.x = controller.collisions.faceDir * chaseSpeed;
            }
            else
            {
                airborne = false;
            }
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

                        //If we are close enough to attack.
                        if (InAttackRange())
                        {
                            state = EnemyState.Attacking;
                        }

                        //If were at the max/min patrol point. CHANGE TO ACCOUNT FOR WHERE THE PLAYER IS AS TO WHICH PLATFORM WE ARE WORKING WITH.
                        if (AtNearestPatrolPointToTarget())
                        {
                            //See if jumping makes sense.
                            if (player.transform.position.y >= transform.position.y && IsJumpPossible(JumpDirection.Up))
                            {
                                Debug.Log("Jumping!");
                                velocity.x = controller.collisions.faceDir * chaseSpeed;
                                velocity.y = maxJumpVelocity;
                                airborne = true;
                            }

                            //see if falling makes sense.
                            else if (player.transform.position.y + 5 < transform.position.y && IsJumpPossible(JumpDirection.Down))
                            {
                                Debug.Log("Free Falling!");
                                velocity.x = controller.collisions.faceDir * chaseSpeed;
                                airborne = true;
                            }

                            //If the first two don't work. TAKEN OUT FOR NOW...
                            //else if (IsJumpPossible(JumpDirection.LastChance))
                            //{
                            //    Debug.Log("Last Chance!");
                            //    velocity.x = controller.collisions.faceDir * chaseSpeed;
                            //    velocity.y = maxJumpVelocity;
                            //    airborne = true;
                            //}

                            //We can go no further.
                            else
                            {
                                Debug.Log("Not Moving!");
                                velocity.x = 0;
                            }
                        }

                        //If were not at the max/min patrol point.
                        else
                        {
                            velocity.x = controller.collisions.faceDir * chaseSpeed;
                        }
                    }
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
            if (velocity.y != 0)
            {
                velocity.x = controller.collisions.faceDir * chaseSpeed;
            }
            else
            {
                airborne = false;
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

                //Time to investigate.
                else if (!LineOfSight())
                {
                    EngagementCountDown();

                    //If we have been through the investigation scenarios and have a target.
                    if (investigating)
                    {
                        //If we have reached our investigation target and still have no LOS on the player.
                        if ((transform.position.x >= jumpTargetX && controller.collisions.faceDir == 1) || (transform.position.x <= jumpTargetX && controller.collisions.faceDir == -1))
                        {
                            velocity.x = 0;
                        }

                        //If we have not reached our jump target and still have no LOS on the player.
                        else
                        {
                            //If were at the max/min patrol point.
                            if (AtNearestPatrolPointToTarget())
                            {
                                //See if jumping makes sense.
                                if (jumpTargetY >= transform.position.y && IsJumpPossible(JumpDirection.Up))
                                {
                                    Debug.Log("Jumping!");
                                    velocity.x = controller.collisions.faceDir * chaseSpeed;
                                    velocity.y = maxJumpVelocity;
                                    airborne = true;
                                }

                                //see if falling makes sense.
                                else if (jumpTargetY + 5 < transform.position.y && IsJumpPossible(JumpDirection.Down))
                                {
                                    Debug.Log("Free Falling!");
                                    velocity.x = controller.collisions.faceDir * chaseSpeed;
                                    airborne = true;
                                }

                                //If the first two don't work. TAKEN OUT FOR NOW...
                                //else if (IsJumpPossible(JumpDirection.LastChance))
                                //{
                                //    Debug.Log("Last Chance!");
                                //    velocity.x = controller.collisions.faceDir * chaseSpeed;
                                //    velocity.y = maxJumpVelocity;
                                //    airborne = true;
                                //}

                                //We can go no further.
                                else
                                {
                                    Debug.Log("Not Moving!");
                                    velocity.x = 0;
                                }
                            }

                            //If were not at the max/min patrol point.
                            else
                            {
                                velocity.x = controller.collisions.faceDir * chaseSpeed;
                            }
                        }
                    }

                    //We need to set the investigation targets.
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
                                
                                investigating = true;
                            }  
                        }

                        //If we didn't find a platform under the player when the LOS broke.
                        else
                        {
                            if ((transform.position.x >= minPatrolX && transform.position.x <= maxPatrolX))
                            {
                                changingDirection = false;
                                velocity.x = Mathf.Lerp(velocity.x, controller.collisions.faceDir * chaseSpeed, 1f);
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

    public void BeingAttacked ()
    {
        beingAttacked = true;
    }

    public void FlinchRecovered ()
    {
        beingAttacked = false;
    }

    public bool AtNearestPatrolPointToTarget ()
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
        else if (transform.position.x >=maxPatrolX)
        {
            if (player.transform.position.x > transform.position.x)
            {
                return true;
            }
        }
        return false;
    }

    private bool IsJumpPossible(JumpDirection direction) 
    {
        if (direction == JumpDirection.Up || direction == JumpDirection.LastChance)
        {
            //Assign the Raycast Points.
            Vector2 jumpPoint1a;
            Vector2 jumpPoint1b;
            Vector2 jumpPoint1c;
            Vector2 jumpPoint1d;
            Vector2 jumpPoint1e;

            Vector2 jumpPoint2a;
            Vector2 jumpPoint2b;
            Vector2 jumpPoint2c;
            Vector2 jumpPoint2d;
            Vector2 jumpPoint2e;
            Vector2 jumpPoint2f;

            Vector2 jumpPoint3a;
            Vector2 jumpPoint3b;

            if (controller.collisions.faceDir == 1)
            {
                jumpPoint1a = new Vector2(enemyCollider.bounds.max.x, enemyCollider.bounds.min.y);
                jumpPoint1b = new Vector2(jumpPoint1.bounds.max.x, jumpPoint1.bounds.min.y);
                jumpPoint1c = new Vector2(jumpPoint2.transform.position.x, jumpPoint2.bounds.min.y);
                jumpPoint1d = new Vector2(jumpPoint3.bounds.min.x, jumpPoint3.bounds.min.y);
                jumpPoint1e = new Vector2(jumpPoint4.bounds.min.x, jumpPoint4.bounds.min.y);

                jumpPoint2a = new Vector2(enemyCollider.bounds.min.x, enemyCollider.bounds.max.y);
                jumpPoint2b = new Vector2(jumpPoint1.bounds.min.x, jumpPoint1.bounds.max.y);
                jumpPoint2c = new Vector2(jumpPoint2.bounds.min.x, jumpPoint2.bounds.max.y);
                jumpPoint2d = new Vector2(jumpPoint2.bounds.max.x, jumpPoint2.bounds.max.y);
                jumpPoint2e = new Vector2(jumpPoint3.bounds.max.x, jumpPoint3.bounds.max.y);
                jumpPoint2f = new Vector2(jumpPoint4.bounds.max.x, jumpPoint4.bounds.max.y);

                jumpPoint3a = new Vector2(jumpPoint3.bounds.max.x, jumpPoint3.bounds.min.y);
                jumpPoint3b = new Vector2(jumpPoint4.bounds.max.x, jumpPoint4.bounds.min.y);
            }
            else
            {
                jumpPoint1a = new Vector2(enemyCollider.bounds.min.x, enemyCollider.bounds.min.y);
                jumpPoint1b = new Vector2(jumpPoint1.bounds.min.x, jumpPoint1.bounds.min.y);
                jumpPoint1c = new Vector2(jumpPoint2.transform.position.x, jumpPoint2.bounds.min.y);
                jumpPoint1d = new Vector2(jumpPoint3.bounds.max.x, jumpPoint3.bounds.min.y);
                jumpPoint1e = new Vector2(jumpPoint4.bounds.max.x, jumpPoint4.bounds.min.y);

                jumpPoint2a = new Vector2(enemyCollider.bounds.max.x, enemyCollider.bounds.max.y);
                jumpPoint2b = new Vector2(jumpPoint1.bounds.max.x, jumpPoint1.bounds.max.y);
                jumpPoint2c = new Vector2(jumpPoint2.bounds.max.x, jumpPoint2.bounds.max.y);
                jumpPoint2d = new Vector2(jumpPoint2.bounds.min.x, jumpPoint2.bounds.max.y);
                jumpPoint2e = new Vector2(jumpPoint3.bounds.min.x, jumpPoint3.bounds.max.y);
                jumpPoint2f = new Vector2(jumpPoint4.bounds.min.x, jumpPoint4.bounds.max.y);

                jumpPoint3a = new Vector2(jumpPoint3.bounds.min.x, jumpPoint3.bounds.min.y);
                jumpPoint3b = new Vector2(jumpPoint4.bounds.min.x, jumpPoint4.bounds.min.y);
            }

            RaycastHit2D segment1a = Physics2D.Linecast(jumpPoint1a, jumpPoint1b, patrolMask);
            RaycastHit2D segment1b = Physics2D.Linecast(jumpPoint1b, jumpPoint1c, patrolMask);
            RaycastHit2D segment1c = Physics2D.Linecast(jumpPoint1c, jumpPoint1d, patrolMask);
            RaycastHit2D segment1d = Physics2D.Linecast(jumpPoint1d, jumpPoint1e, patrolMask);

            RaycastHit2D segment2a = Physics2D.Linecast(jumpPoint2a, jumpPoint2b, patrolMask);
            RaycastHit2D segment2b = Physics2D.Linecast(jumpPoint2b, jumpPoint2c, patrolMask);
            RaycastHit2D segment2c = Physics2D.Linecast(jumpPoint2c, jumpPoint2d, patrolMask);
            RaycastHit2D segment2d = Physics2D.Linecast(jumpPoint2d, jumpPoint2e, patrolMask);
            RaycastHit2D segment2e = Physics2D.Linecast(jumpPoint2e, jumpPoint2f, patrolMask);

            RaycastHit2D segment3a = Physics2D.Linecast(jumpPoint1c, jumpPoint3a, patrolMask);
            RaycastHit2D segment3b = Physics2D.Linecast(jumpPoint3a, jumpPoint3b, patrolMask);

            //check the jump raycasts.

            //Always reject hits on 2a and 2d.
            if (segment2a || segment2d)
            {
                return false;
            }

            if (segment1a)
            {
                if (segment1a.collider.gameObject != targetPlatform.gameObject)
                {
                    return false;
                }
            }

            //1b.
            if (segment1b)
            {
                //If hit we can check if below the hit is a platform
                Vector2 hitOrigin = new Vector2(segment1b.point.x - (1 * controller.collisions.faceDir), segment1b.collider.bounds.min.y - .5f);
                RaycastHit2D hit = Physics2D.Raycast(hitOrigin, Vector2.down, 200, patrolMask);
                if (hit)
                {
                    //If the platform below is not the patrol platform and is higher than the patrol platform.
                    if (hit.collider.gameObject != patrolPlatform.gameObject && hit.collider.bounds.max.y > patrolPlatform.GetComponent<Collider2D>().bounds.max.y)
                    {
                        return true;
                    }
                }
                return false;
            }

            //2b.
            if (segment2b)
            {
                //If hit we can check if below the hit is a platform
                Vector2 hitOrigin = new Vector2(segment2b.point.x - (1 * controller.collisions.faceDir), segment2b.collider.bounds.min.y - .5f);
                RaycastHit2D hit = Physics2D.Raycast(hitOrigin, Vector2.down, 200, patrolMask);
                if (hit)
                {
                    //If the platform below is not the patrol platform and is higher than the patrol platform.
                    if (hit.collider.gameObject != patrolPlatform.gameObject && hit.collider.bounds.max.y > patrolPlatform.GetComponent<Collider2D>().bounds.max.y)
                    {
                        return true;
                    }
                }
                return false;
            }

            //2c.
            if (segment2c)
            {
                //If hit we can check if below the hit is a platform
                Vector2 hitOrigin = new Vector2(segment2c.point.x - (1 * controller.collisions.faceDir), segment2c.collider.bounds.min.y - .5f);
                RaycastHit2D hit = Physics2D.Raycast(hitOrigin, Vector2.down, 200, patrolMask);
                if (hit)
                {
                    //If the platform below is not the patrol platform and is higher than the patrol platform.
                    if (hit.collider.gameObject != patrolPlatform.gameObject && hit.collider.bounds.max.y > patrolPlatform.GetComponent<Collider2D>().bounds.max.y)
                    {
                        return true;
                    }
                }
                return false;
            }

            //1c.
            if (segment1c)
            {
                if (direction == JumpDirection.Up)
                {
                    //If the platform hit is higher than the patrol platform.
                    if (segment1c.collider.bounds.max.y > patrolPlatform.GetComponent<Collider2D>().bounds.max.y)
                    {
                        //If we hit the top of a platform.
                        if (segment1c.point.y >= segment1c.collider.bounds.max.y - .25f && segment1c.point.y <= segment1c.collider.bounds.max.y + .25f)
                        {
                            return true;
                        }

                        else {
                            //If they hit we can check if below the hit is a platform
                            Vector2 hitOrigin = new Vector2(segment1c.point.x - (1 * controller.collisions.faceDir), segment1c.point.y);
                            RaycastHit2D hit = Physics2D.Raycast(hitOrigin, Vector2.down, 200, patrolMask);
                            if (hit)
                            {
                                //If the platform below is not the patrol platform.
                                if (hit.collider.gameObject != patrolPlatform.gameObject)
                                {
                                    return true;
                                }
                            }
                        }
                    }
                }

                //Last chance ignores if the platform is higher than the patrol platform.
                else
                {
                    //If we hit the top of a platform.
                    if (segment1c.point.y >= segment1c.collider.bounds.max.y - .25f && segment1c.point.y <= segment1c.collider.bounds.max.y + .25f)
                    {
                        return true;
                    }

                    else {
                        //If they hit we can check if below the hit is a platform
                        Vector2 hitOrigin = new Vector2(segment1c.point.x - (1 * controller.collisions.faceDir), segment1c.point.y);
                        RaycastHit2D hit = Physics2D.Raycast(hitOrigin, Vector2.down, 200, patrolMask);
                        if (hit)
                        {
                            //If the platform below is not the patrol platform.
                            if (hit.collider.gameObject != patrolPlatform.gameObject)
                            {
                                return true;
                            }
                        }
                    }
                }
            }

            //1d.
            if (segment1d)
            {
                if (direction == JumpDirection.Up)
                {
                    //If the platform hit is higher than the patrol platform.
                    if (segment1d.collider.bounds.max.y > patrolPlatform.GetComponent<Collider2D>().bounds.max.y)
                    {
                        //If we hit the top of a platform.
                        if (segment1d.point.y >= segment1d.collider.bounds.max.y - .25f && segment1d.point.y <= segment1d.collider.bounds.max.y + .25f)
                        {
                            return true;
                        }

                        else {
                            //If they hit we can check if below the hit is a platform
                            Vector2 hitOrigin = new Vector2(segment1d.point.x - (1 * controller.collisions.faceDir), segment1d.point.y);
                            RaycastHit2D hit = Physics2D.Raycast(hitOrigin, Vector2.down, 200, patrolMask);
                            if (hit)
                            {
                                //If the platform below is not the patrol platform.
                                if (hit.collider.gameObject != patrolPlatform.gameObject)
                                {
                                    return true;
                                }
                            }
                        }
                    }
                }

                //Last chance ignores if the platform is higher than the patrol platform.
                else
                {
                    //If we hit the top of a platform.
                    if (segment1d.point.y >= segment1d.collider.bounds.max.y - .25f && segment1d.point.y <= segment1d.collider.bounds.max.y + .25f)
                    {
                        return true;
                    }

                    else {
                        //If they hit we can check if below the hit is a platform
                        Vector2 hitOrigin = new Vector2(segment1d.point.x - (1 * controller.collisions.faceDir), segment1d.point.y);
                        RaycastHit2D hit = Physics2D.Raycast(hitOrigin, Vector2.down, 200, patrolMask);
                        if (hit)
                        {
                            //If the platform below is not the patrol platform.
                            if (hit.collider.gameObject != patrolPlatform.gameObject)
                            {
                                return true;
                            }
                        }
                    }
                }
            }

            //2e.
            if (segment2e)
            {
                if (segment3b)
                {
                    if (segment2e.collider.gameObject != segment3b.collider.gameObject)
                    {
                        return false;
                    }
                }
                if (segment1d)
                {
                    if (segment2e.collider.gameObject != segment1d.collider.gameObject)
                    {
                        return false;
                    }
                }
            }

            //3a.
            if (segment3a)
            {
                if (direction == JumpDirection.Up)
                {
                    //If the platform hit is higher than the patrol platform.
                    if (segment3a.collider.bounds.max.y > patrolPlatform.GetComponent<Collider2D>().bounds.max.y)
                    {
                        //If we hit the top of a platform.
                        if (segment3a.point.y >= segment3a.collider.bounds.max.y - .25f && segment3a.point.y <= segment3a.collider.bounds.max.y + .25f)
                        {
                            return true;
                        }

                        else {
                            //If they hit we can check if below the hit is a platform
                            Vector2 hitOrigin = new Vector2(segment3a.point.x - (1 * controller.collisions.faceDir), segment3a.point.y);
                            RaycastHit2D hit = Physics2D.Raycast(hitOrigin, Vector2.down, 200, patrolMask);
                            if (hit)
                            {
                                //If the platform below is not the patrol platform.
                                if (hit.collider.gameObject != patrolPlatform.gameObject)
                                {
                                    return true;
                                }
                            }
                        }
                    }
                }

                //Last chance ignores if the platform is higher than the patrol platform.
                else
                {
                    //If we hit the top of a platform.
                    if (segment3a.point.y >= segment3a.collider.bounds.max.y - .25f && segment3a.point.y <= segment3a.collider.bounds.max.y + .25f)
                    {
                        return true;
                    }

                    else {
                        //If they hit we can check if below the hit is a platform
                        Vector2 hitOrigin = new Vector2(segment3a.point.x - (1 * controller.collisions.faceDir), segment3a.point.y);
                        RaycastHit2D hit = Physics2D.Raycast(hitOrigin, Vector2.down, 200, patrolMask);
                        if (hit)
                        {
                            //If the platform below is not the patrol platform.
                            if (hit.collider.gameObject != patrolPlatform.gameObject)
                            {
                                return true;
                            }
                        }
                    }
                }
            }

            //3b.
            if (segment3b)
            {
                if (direction == JumpDirection.Up)
                {
                    //If the platform hit is higher than the patrol platform.
                    if (segment3b.collider.bounds.max.y > patrolPlatform.GetComponent<Collider2D>().bounds.max.y)
                    {
                        //If we hit the top of a platform.
                        if (segment3b.point.y >= segment3b.collider.bounds.max.y - .25f && segment3b.point.y <= segment3b.collider.bounds.max.y + .25f)
                        {
                            return true;
                        }

                        else {
                            //If they hit we can check if below the hit is a platform
                            Vector2 hitOrigin = new Vector2(segment3b.point.x - (1 * controller.collisions.faceDir), segment3b.point.y);
                            RaycastHit2D hit = Physics2D.Raycast(hitOrigin, Vector2.down, 200, patrolMask);
                            if (hit)
                            {
                                //If the platform below is not the patrol platform.
                                if (hit.collider.gameObject != patrolPlatform.gameObject)
                                {
                                    return true;
                                }
                            }
                        }
                    }
                }

                //Last chance ignores if the platform is higher than the patrol platform.
                else
                {
                    //If we hit the top of a platform.
                    if (segment3b.point.y >= segment3b.collider.bounds.max.y - .25f && segment3b.point.y <= segment3b.collider.bounds.max.y + .25f)
                    {
                        return true;
                    }

                    else {
                        //If they hit we can check if below the hit is a platform
                        Vector2 hitOrigin = new Vector2(segment3b.point.x - (1 * controller.collisions.faceDir), segment3b.point.y);
                        RaycastHit2D hit = Physics2D.Raycast(hitOrigin, Vector2.down, 200, patrolMask);
                        if (hit)
                        {
                            //If the platform below is not the patrol platform.
                            if (hit.collider.gameObject != patrolPlatform.gameObject)
                            {
                                return true;
                            }
                        }
                    }
                }
            }

            return false;
        }

        else if (direction == JumpDirection.Down)
        {
            //check the free fall raycasts.
            //1 is the back corner.
            Vector2 fallPoint1a;
            Vector2 fallPoint1b;
            Vector2 fallPoint1c;

            //2 is the top corner.
            Vector2 fallPoint2a;
            Vector2 fallPoint2b;
            Vector2 fallPoint2c;

            //3 is the front corner.
            Vector2 fallPoint3a;
            Vector2 fallPoint3b;
            Vector2 fallPoint3c;

            if (controller.collisions.faceDir == 1)
            {
                fallPoint1a = new Vector2(fallPoint1.bounds.min.x, fallPoint1.bounds.min.y);
                fallPoint1b = new Vector2(fallPoint2.bounds.min.x, fallPoint2.bounds.min.y);
                fallPoint1c = new Vector2(fallPoint3.bounds.min.x, fallPoint3.bounds.min.y);

                fallPoint2a = new Vector2(fallPoint1.bounds.max.x, fallPoint1.bounds.max.y);
                fallPoint2b = new Vector2(fallPoint2.bounds.max.x, fallPoint2.bounds.max.y);
                fallPoint2c = new Vector2(fallPoint3.bounds.max.x, fallPoint3.bounds.max.y);
            }
            else
            {
                fallPoint1a = new Vector2(fallPoint1.bounds.max.x, fallPoint1.bounds.min.y);
                fallPoint1b = new Vector2(fallPoint2.bounds.max.x, fallPoint2.bounds.min.y);
                fallPoint1c = new Vector2(fallPoint3.bounds.max.x, fallPoint3.bounds.min.y);

                fallPoint2a = new Vector2(fallPoint1.bounds.min.x, fallPoint1.bounds.max.y);
                fallPoint2b = new Vector2(fallPoint2.bounds.min.x, fallPoint2.bounds.max.y);
                fallPoint2c = new Vector2(fallPoint3.bounds.min.x, fallPoint3.bounds.max.y);
            }

            fallPoint3a = new Vector2(fallPoint1.transform.position.x, fallPoint1.bounds.min.y);
            fallPoint3b = new Vector2(fallPoint2.transform.position.x, fallPoint2.bounds.min.y);
            fallPoint3c = new Vector2(fallPoint3.transform.position.x, fallPoint3.bounds.min.y);

            RaycastHit2D segment1a = Physics2D.Linecast(fallPoint1a, fallPoint1b, patrolMask);
            RaycastHit2D segment1b = Physics2D.Linecast(fallPoint1b, fallPoint1c, patrolMask);
            
            RaycastHit2D segment2a = Physics2D.Linecast(fallPoint2a, fallPoint2b, patrolMask);
            RaycastHit2D segment2b = Physics2D.Linecast(fallPoint2b, fallPoint2c, patrolMask);

            RaycastHit2D segment3a = Physics2D.Linecast(fallPoint3a, fallPoint3b, patrolMask);
            RaycastHit2D segment3b = Physics2D.Linecast(fallPoint3b, fallPoint3c, patrolMask);

            if (segment2a)
            {
                if (segment1b)
                {
                    if (segment2a.collider.gameObject != segment1b.collider.gameObject)
                    {
                        return false;
                    }
                }
                if (segment3b)
                {
                    if (segment2a.collider.gameObject != segment3b.collider.gameObject)
                    {
                        return false;
                    }
                }
            }

            if (segment2b)
            {
                if (segment1b)
                {
                    if (segment2b.collider.gameObject != segment1b.collider.gameObject)
                    {
                        return false;
                    } 
                }
                if (segment3b)
                {
                    if (segment2b.collider.gameObject != segment3b.collider.gameObject)
                    {
                        return false;
                    }
                }
            }

            if (segment1a || segment1b || segment3a || segment3b)
            {
                if (segment1a)
                {
                    if (segment1a.point.y >= segment1a.collider.bounds.max.y - .25f && segment1a.point.y <= segment1a.collider.bounds.max.y + .25f)
                    {
                        return true;
                    }
                }
                if (segment1b)
                {
                    if (segment1b.point.y >= segment1b.collider.bounds.max.y - .25f && segment1b.point.y <= segment1b.collider.bounds.max.y + .25f)
                    {
                        return true;
                    }
                }
                if (segment3a)
                {
                    if (segment3a.point.y >= segment3a.collider.bounds.max.y - .25f && segment3a.point.y <= segment3a.collider.bounds.max.y + .25f)
                    {
                        return true;
                    }
                }
                if (segment3b)
                {
                    if (segment3b.point.y >= segment3b.collider.bounds.max.y - .25f && segment3b.point.y <= segment3b.collider.bounds.max.y + .25f)
                    {
                        return true;
                    }
                }
            }
            
            return false;
        }

        return false;
    }

    public enum JumpDirection
    {
        Up,
        Down,
        LastChance
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

    //this will be used to gauge interactions...I might need to do these things in the climbable script.
    public void OnTriggerEnter2D(Collider2D collider)
    {
        //Falling off the world
        if (collider.gameObject.layer == 19)
        {
            stats.hP = 0;
        }
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
    
    //Check to see if we are close enough to enter attack mode.
    public bool InAttackRange () 
    {
        float directionX = controller.collisions.faceDir;
        float rayLength = stats.attackRange;
        float rayOriginX = enemyCollider.bounds.center.x;
        float rayOriginY = enemyCollider.bounds.center.y;
        Vector2 rayOrigin = new Vector2(rayOriginX, rayOriginY);

        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, attackingLayer);
        
        if (hit)
        {
            if (hit.collider.gameObject.layer == 9)
            {
                return true;
            }
        }
        return false;
    }

    //Called from the animator.
    public void Attack()
    {
        float directionX = controller.collisions.faceDir;
        float rayLength = stats.attackRange;
        float rayOriginX = (directionX == 1) ? enemyCollider.bounds.max.x + 0.01f : enemyCollider.bounds.min.x - 0.01f;
        float rayOriginY = enemyCollider.bounds.center.y;
        Vector2 rayOrigin = new Vector2(rayOriginX, rayOriginY);

        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, attackingLayer);
        //Layer 9 is currently the players layer.
        if (hit)
        {
            if (hit.collider.gameObject.layer == 9)
            {
                CombatEngine.combatEngine.AttackingPlayer(this.GetComponent<Collider2D>(), stats.maximumDamage);
            }
        }
    }

    public void CallEnemyDefeated ()
    {
        StartCoroutine("EnemyDefeated");
    }

    //When the enemy dies.
    public IEnumerator EnemyDefeated () 
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
    public void EquipmentDrops ()
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
    public void ItemDrops ()
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