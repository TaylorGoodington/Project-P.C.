using UnityEngine;

[RequireComponent (typeof (Controller2D))]
public class Player : MonoBehaviour {
    [Tooltip("This field is used to specify which layers block the attacking and abilities raycasts.")]
    public LayerMask attackingLayer;

    public float maxJumpHeight = 4;
	public float minJumpHeight = 1;
	private float timeToJumpApex = .3f;
	float accelerationTimeAirborne = .1f;
	float accelerationTimeGrounded = .1f;
	public float moveSpeed = 120;
	public float climbSpeed = 50;
	
	public Vector2 wallJumpClimb;
	public Vector2 wallJumpOff;
	public Vector2 wallLeap;
	
	public float wallSlideSpeedMax = 3;
	public float wallStickTime = .25f;
	
	[HideInInspector]
	public bool isAttacking;
	public bool isJumping;
	public bool isClimbable;
	public bool climbing;

    private float climbingUpPosition;
    private bool climbingUp;

    private Animator playerAnimationController;

    float timeToWallUnstick;
	
	float gravity;
	float maxJumpVelocity;
	float minJumpVelocity;
	Vector3 velocity;
	float velocityXSmoothing;
	
	Controller2D controller;

    private BoxCollider2D playerCollider;
	
	void Start() {
		controller = GetComponent<Controller2D> ();
        playerAnimationController = GetComponent<Animator>();
        playerCollider = GetComponent<BoxCollider2D>();

        gravity = -1000;
        maxJumpVelocity = (Mathf.Abs(gravity) * (timeToJumpApex)) * ((Mathf.Pow(maxJumpHeight, -0.5221f)) * 0.1694f) * maxJumpHeight;
        minJumpVelocity = Mathf.Sqrt (2 * Mathf.Abs (gravity) * minJumpHeight);
		isAttacking = false;
		isJumping = false;
	}
	
	void Update() {
		if (GameControl.gameControl.AnyOpenMenus() == false) {
			Vector2 input = new Vector2 (Input.GetAxisRaw ("Horizontal"), Input.GetAxisRaw ("Vertical"));
			int wallDirX = (controller.collisions.left) ? -1 : 1;

            //flips sprite depending on direction facing.
            if (controller.collisions.faceDir == 1)
            {
                gameObject.GetComponent<SpriteRenderer>().flipX = false;
            }
            else
            {
                gameObject.GetComponent<SpriteRenderer>().flipX = true;
            }
			
			//cant move if attacking.
			if (isAttacking) {
				input = Vector2.zero;
			}

            //Animation Call Section
            if (climbingUp)
            {
                playerAnimationController.enabled = true;
                playerAnimationController.Play("ClimbingUp");
            }
            if (Input.GetButtonDown("Attack") && !climbingUp)
            {
                playerAnimationController.enabled = true;
                playerAnimationController.Play("SwordAttack1");
            }

            if (climbing && !isAttacking && !climbingUp)
            {
                playerAnimationController.enabled = true;
                playerAnimationController.Play("Climbing");
            }
            if (climbing && (velocity.y == 0 && velocity.x == 0) && !isAttacking && !climbingUp)
            {
                Invoke("PauseAnimator", 0.1f);
            }

            if (velocity.y != 0 && controller.collisions.below == false && isAttacking == false && !climbing)
            {
                playerAnimationController.enabled = true;
                playerAnimationController.Play("Jumping");
            }

            if (input.x != 0 && controller.collisions.below == true && !climbing && !climbingUp)
            {
                playerAnimationController.Play("Running");
            }

            if (input.x == 0 && isAttacking == false && controller.collisions.below == true && !climbing && !climbingUp)
            {
                playerAnimationController.Play("Idle");
            }


            float targetVelocityX = input.x * moveSpeed;
			velocity.x = Mathf.SmoothDamp (velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below)?accelerationTimeGrounded:accelerationTimeAirborne);
			
			bool wallSliding = false;
			if ((controller.collisions.left || controller.collisions.right) && !controller.collisions.below && velocity.y < 0 && controller.isWallJumpable == true) {
				wallSliding = true;
				
				if (velocity.y < -wallSlideSpeedMax) {
					velocity.y = -wallSlideSpeedMax;
				}
				
				if (timeToWallUnstick > 0) {
					velocityXSmoothing = 0;
					velocity.x = 0;
					
					if (input.x != wallDirX && input.x != 0) {
						timeToWallUnstick -= Time.deltaTime;
					}
					else {
						timeToWallUnstick = wallStickTime;
					}
				}
				else {
					timeToWallUnstick = wallStickTime;
				}
			}
			
			//cant jump if attacking.
			if (!isAttacking) {
				if (Input.GetButtonDown ("Jump")) {
					if (climbing) {
						velocity.x = controller.collisions.faceDir * wallJumpClimb.x;
						velocity.y = wallJumpOff.y;
						climbing = false;
					}
					
					if (wallSliding) {
						if (wallDirX == input.x) {
							velocity.x = -wallDirX * wallJumpClimb.x;
							velocity.y = wallJumpClimb.y;
						}
						else if (input.x == 0) {
							velocity.x = -wallDirX * wallJumpOff.x;
							velocity.y = wallJumpOff.y;
						}
						else {
							velocity.x = -wallDirX * wallLeap.x;
							velocity.y = wallLeap.y;
						}
					}
					if (controller.collisions.below) {
						velocity.y = maxJumpVelocity;
					}
				}
				if (Input.GetButtonUp ("Jump")) {
					if (velocity.y > minJumpVelocity) {
						velocity.y = minJumpVelocity;
					}
				}
			}
			
			//climbing stuff
			if (isClimbable) {
				if (Input.GetButtonDown("Interact")) {
					climbing = true;
					velocity.y = 0;
				}
			}
			
			if (climbing) {
				gravity = 0;
				velocity.y = input.y * climbSpeed;
				velocity.x = input.x * climbSpeed;

                if (climbingUp) {
                    velocity = Vector3.zero;
                    Invoke("MovePlayerWhenClimbingUp", 0.125f);
                }
			} else {
				gravity = -1000;
				velocity.y += gravity * Time.deltaTime;
			}
			
			
			controller.Move (velocity * Time.deltaTime, input);
			
			if (controller.collisions.above || controller.collisions.below) {
				velocity.y = 0;
			}
		}
	}
	
	//this will be used to gauge interactions...I might need to do these things in the climbable script.
	public void OnTriggerEnter2D (Collider2D collider) {
		if (collider.gameObject.GetComponent<IsClimbable>()) {
			isClimbable = true;
		}
        //climbing up action
        if (collider.gameObject.layer == 15 && climbing) {
            ClimbingTransition(collider);
        }
	}
	
	//this will be used to gauge interactions...I might need to do these things in the climbable script.
	public void OnTriggerExit2D (Collider2D collider) {
		if (collider.gameObject.GetComponent<IsClimbable>()) {
			isClimbable = false;
			climbing = false;
		}
    }

    public void ClimbingTransition(Collider2D collider) {
        climbingUpPosition = collider.bounds.max.y;
        climbingUp = true;
    }
	
	
	//called from attacking animation at the begining and end.
	public void IsAttacking () {
		isAttacking = !isAttacking;
	}

    //called by climbing up animation to stop animating.
    public void IsClimbingUp() {
        climbingUp = false;
    }

    //used as an invoke to move the player
    public void MovePlayerWhenClimbingUp() {
        this.gameObject.transform.position = new Vector3(transform.position.x, climbingUpPosition);
    }

    //used to pause the animator
    public void PauseAnimator() {
        playerAnimationController.enabled = false;
    }
	
	//called from the animations for attacking.
    
    //I think I will need to pass the collider being hit to the attacking function.
	public void Attack () {
		float directionX = controller.collisions.faceDir;
		float rayLength = 100f; //make each weapon have a length component?
		float rayOriginX = playerCollider.bounds.max.x + 0.01f; //defined as the edge of the collider.
		float rayOriginY = playerCollider.bounds.center.y; //defined as the center of the collider.
		Vector2 rayOrigin = new Vector2 (rayOriginX, rayOriginY);

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