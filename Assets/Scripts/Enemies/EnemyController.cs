using UnityEngine;

[RequireComponent(typeof(Controller2D))]
public class EnemyController : MonoBehaviour
{
    //[Tooltip("This field is used to specify which layers block the attacking and abilities raycasts.")]
    //public LayerMask attackingLayer;

    //public float maxJumpHeight = 4;
    //public float minJumpHeight = 1;
    //public float timeToJumpApex = .4f;
    //private float accelerationTimeAirborne = .2f;
    //private float accelerationTimeGrounded = .1f;
    //public float patrolSpeed = 6;
    //[Tooltip("The distance from the center of the enemy to the max/min x value they can patrol.")]
    //public float patrolDistance;
    //public float chaseSpeed = 6;
    //[Tooltip("The amount of time an enemy will remain engaged after losing the line of sight.")]
    //public float chaseTime;
    //public float climbSpeed = 50;
    //[Tooltip("The length of time in seconds it takes for the enemy to change directions.")]
    //public float pivotTime;

    //private bool changingDirection;

    //public float minPatrolX;
    //public float maxPatrolX;

    //public LayerMask patrolMask;

    //private bool patrolPathCreated;

    ////[HideInInspector]
    //public EnemyState state;

    //public Vector2 wallJumpClimb;
    //public Vector2 wallJumpOff;
    //public Vector2 wallLeap;

    //private float wallSlideSpeedMax = 3;
    //private float wallStickTime = .25f;

    //[HideInInspector]
    //public bool isAttacking;
    //[HideInInspector]
    //public bool isJumping;
    //[HideInInspector]
    //public bool isClimbable;
    //[HideInInspector]
    //public bool climbing;

    //private float climbingUpPosition;
    //private bool climbingUp;

    //private Animator enemyAnimationController;

    //private float timeToWallUnstick;

    //private float gravity;
    //private float maxJumpVelocity;
    //private float minJumpVelocity;
    //private Vector3 velocity;
    //private float velocityXSmoothing;

    //private Controller2D controller;

    //private BoxCollider2D enemyCollider;

    //void Start()
    //{
    //    controller = GetComponent<Controller2D>();
    //    enemyAnimationController = GetComponent<Animator>();
    //    enemyCollider = GetComponent<BoxCollider2D>();

    //    gravity = -(2 * maxJumpHeight) / Mathf.Pow(timeToJumpApex, 2);
    //    maxJumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
    //    minJumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(gravity) * minJumpHeight);
    //    isAttacking = false;
    //    isJumping = false;
    //    changingDirection = false;
    //    patrolPathCreated = false;

    //    CreatePatrolPath();
    //    state = EnemyState.Patroling;
    //}

    //void Update()
    //{
    //    //this will be changed based on the state of the enemy.
    //    Vector2 input = new Vector2(0, 0);
    //    int wallDirX = (controller.collisions.left) ? 1 : 1;

    //    //flips sprite depending on direction facing.
    //    if (controller.collisions.faceDir == -1)
    //    {
    //        gameObject.GetComponent<SpriteRenderer>().flipX = false;
    //    }
    //    else
    //    {
    //        gameObject.GetComponent<SpriteRenderer>().flipX = true;
    //    }

    //    //cant move if attacking.
    //    if (isAttacking)
    //    {
    //        input = Vector2.zero;
    //    }




    //    if (state == EnemyState.Patroling)
    //    {
    //        if (!patrolPathCreated)
    //        {
    //            CreatePatrolPath();
    //        }

    //        if ((transform.position.x >= minPatrolX && transform.position.x <= maxPatrolX))
    //        {
    //            changingDirection = false;
    //            velocity.x = Mathf.Lerp(velocity.x, controller.collisions.faceDir * patrolSpeed, 1f);
    //        }

    //        if ((transform.position.x <= minPatrolX || transform.position.x >= maxPatrolX) && changingDirection == false)
    //        {
    //            velocity.x = Mathf.Lerp(velocity.x, 0f, 1f);
    //            Invoke("ChangeDirection", pivotTime);
    //            changingDirection = true;
    //        }


    //        gravity = -(2 * maxJumpHeight) / Mathf.Pow(timeToJumpApex, 2);
    //        velocity.y += gravity * Time.deltaTime;

    //        controller.Move(velocity * Time.deltaTime, input);
    //    }


    //    if (controller.collisions.above || controller.collisions.below)
    //    {
    //        velocity.y = 0;
    //    }
    //}

    //public void ChangeDirection()
    //{
    //    controller.collisions.faceDir = controller.collisions.faceDir * -1;
    //    velocity.x = Mathf.Lerp(velocity.x, controller.collisions.faceDir * patrolSpeed, 1f);
    //}

    //public void CreatePatrolPath()
    //{
    //    float rayLength = 5000f;
    //    float rayOriginX = enemyCollider.bounds.center.x;
    //    float rayOriginY = enemyCollider.bounds.center.y;
    //    Vector2 rayOrigin = new Vector2(rayOriginX, rayOriginY);

    //    RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.down, rayLength, patrolMask);

    //    if (hit)
    //    {
    //        if (hit.collider.gameObject.layer == 10)
    //        {
    //            minPatrolX = hit.collider.bounds.min.x + (enemyCollider.size.x / 2);
    //            maxPatrolX = hit.collider.bounds.max.x - (enemyCollider.size.x / 2);
    //        }
    //    }
    //    patrolPathCreated = true;
    //}

    //public enum EnemyState
    //{
    //    Patroling,
    //    Chasing,
    //    Attacking,
    //    Fleeing
    //}

    ////this will be used to gauge interactions...I might need to do these things in the climbable script.
    //public void OnTriggerEnter2D(Collider2D collider)
    //{
    //    if (collider.gameObject.GetComponent<IsClimbable>())
    //    {
    //        isClimbable = true;
    //    }
    //    //climbing up action
    //    if (collider.gameObject.layer == 15 && climbing)
    //    {
    //        ClimbingTransition(collider);
    //    }
    //}

    ////this will be used to gauge interactions...I might need to do these things in the climbable script.
    //public void OnTriggerExit2D(Collider2D collider)
    //{
    //    if (collider.gameObject.GetComponent<IsClimbable>())
    //    {
    //        isClimbable = false;
    //        climbing = false;
    //    }
    //}

    //public void ClimbingTransition(Collider2D collider)
    //{
    //    climbingUpPosition = collider.bounds.max.y;
    //    climbingUp = true;
    //}


    ////called from attacking animation at the begining and end.
    //public void IsAttacking()
    //{
    //    isAttacking = !isAttacking;
    //}

    ////called by climbing up animation to stop animating.
    //public void IsClimbingUp()
    //{
    //    climbingUp = false;
    //}

    ////used as an invoke to move the player
    //public void MovePlayerWhenClimbingUp()
    //{
    //    this.gameObject.transform.position = new Vector3(transform.position.x, climbingUpPosition);
    //}

    ////used to pause the animator
    //public void PauseAnimator()
    //{
    //    enemyAnimationController.enabled = false;
    //}

    ////called from the animations for attacking.

    ////I think I will need to pass the collider being hit to the attacking function.
    //public void Attack()
    //{
    //    float directionX = controller.collisions.faceDir;
    //    float rayLength = 100f; //make each weapon have a length component?
    //    float rayOriginX = enemyCollider.bounds.max.x + 0.01f; //defined as the edge of the collider.
    //    float rayOriginY = enemyCollider.bounds.center.y; //defined as the center of the collider.
    //    Vector2 rayOrigin = new Vector2(rayOriginX, rayOriginY);

    //    RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, attackingLayer);
    //    //Layer 14 is currently the enemies layer.
    //    if (hit)
    //    {
    //        if (hit.collider.gameObject.layer == 14)
    //        {
    //            CombatEngine.combatEngine.AttackingEnemies(hit.collider);
    //        }
    //    }
    //}
}