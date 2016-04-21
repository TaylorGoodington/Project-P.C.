using UnityEngine;
using System.Collections;

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

    private Vector2 input;
	public Vector2 wallJumpClimb;
	public Vector2 wallJumpOff;
	public Vector2 wallLeap;
	
	public float wallSlideSpeedMax = 3;
	public float wallStickTime = .25f;
	
	[HideInInspector]
	public bool isAttacking;
    public bool attackLaunched;
	public bool isJumping;
	public bool isClimbable;
	public bool climbing;
    bool climbingUpMovement;
    [HideInInspector]
    public int knockBackForce;

    public float climbingUpPosition;
    public bool climbingUp;
    
    PlayerAnimationController animator;

    float timeToWallUnstick;
	
	float gravity;
	float maxJumpVelocity;
	float minJumpVelocity;
	Vector3 velocity;
	float velocityXSmoothing;
	
	Controller2D controller;

    private BoxCollider2D playerCollider;
    [HideInInspector]
    public bool flinching;
    public bool deathStanding;
    bool knockBack;
	
	void Start() {
		controller = GetComponent<Controller2D> ();
        animator = GetComponent<PlayerAnimationController>();
        playerCollider = GetComponent<BoxCollider2D>();

        gravity = -1000;
        maxJumpVelocity = (Mathf.Abs(gravity) * (timeToJumpApex)) * ((Mathf.Pow(maxJumpHeight, -0.5221f)) * 0.1694f) * maxJumpHeight;
        minJumpVelocity = Mathf.Sqrt (2 * Mathf.Abs (gravity) * minJumpHeight);
		isAttacking = false;
        attackLaunched = false;
		isJumping = false;
        flinching = false;
        deathStanding = false;
        knockBack = false;
        climbingUpMovement = false;
	}

    void Update()
    {
        if (deathStanding)
        {
            animator.PlayAnimation(PlayerAnimationController.Animations.DeathStanding);
        }
        else if (GameControl.gameControl.hp <= 0)
        {
            GameControl.gameControl.dying = true;
            if (controller.collisions.below)
            {
                input = Vector2.zero;
                GameControl.gameControl.playerHasControl = false;
                UnPauseAnimators();
                animator.PlayAnimation(PlayerAnimationController.Animations.DeathFalling);
                MusicManager.musicManager.PlayMusic(7, false);
            }
            //Player needs to hit the ground before the animation plays.
            else
            {
                gravity = -1000;
                velocity.y += gravity * Time.deltaTime;
                controller.Move(velocity * Time.deltaTime, new Vector2(1, 0));
            }
        }
        else if (GameControl.gameControl.endOfLevel)
        {
            gravity = -1000;
            velocity.y += gravity * Time.deltaTime;
            velocity.x = moveSpeed;
            controller.Move(velocity * Time.deltaTime, new Vector2(1,0));
            if (controller.collisions.below == false)
            {
                animator.PlayAnimation(PlayerAnimationController.Animations.Jumping);
            }
            else
            {
                animator.PlayAnimation(PlayerAnimationController.Animations.Running);
            }
        }
        else if (flinching) {
            isAttacking = false;
            attackLaunched = false;
            CombatEngine.combatEngine.comboCount = 1;
            animator.PlayAnimation(PlayerAnimationController.Animations.Flinching);
            PlayerSoundEffects.playerSoundEffects.PlaySoundEffect(PlayerSoundEffects.playerSoundEffects.SoundEffectToArrayInt(PlayerSoundEffects.SoundEffect.MenuUnable));
            if (knockBack)
            {
                float flinchTime = .1f;
                transform.Translate((CombatEngine.combatEngine.enemyKnockBackForce / flinchTime) * CombatEngine.combatEngine.enemyFaceDirection * Time.deltaTime, 0, 0, Space.Self);
            }
            gravity = -1000;
            velocity.y += gravity * Time.deltaTime;
            velocity.x = 0;
            controller.Move(velocity * Time.deltaTime, input);
        }
        else
        {
            //If a menu is open or the player doesn't have control.
            if (GameControl.gameControl.AnyOpenMenus() == true || GameControl.gameControl.playerHasControl == false)
            {
                input = Vector2.zero;
            }

            else if (GameControl.gameControl.AnyOpenMenus() == false || GameControl.gameControl.playerHasControl == true)
            {
                input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
                int wallDirX = (controller.collisions.left) ? -1 : 1;
                bool wallSliding = false;
                if ((controller.collisions.left || controller.collisions.right) && !controller.collisions.below && velocity.y < 0 && controller.isWallJumpable == true)
                {
                    wallSliding = true;

                    if (velocity.y < -wallSlideSpeedMax)
                    {
                        velocity.y = -wallSlideSpeedMax;
                    }

                    if (timeToWallUnstick > 0)
                    {
                        velocityXSmoothing = 0;
                        velocity.x = 0;

                        if (input.x != wallDirX && input.x != 0)
                        {
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

                //Launching an attack.
                if (Input.GetButtonDown("Attack") && !isAttacking && !attackLaunched)
                {
                    attackLaunched = true;
                }

                //cant jump if attacking.
                if (!isAttacking)
                {
                    if (Input.GetButtonDown("Jump"))
                    {
                        if (climbing)
                        {
                            velocity.x = controller.collisions.faceDir * wallJumpClimb.x;
                            velocity.y = wallJumpOff.y;
                            climbing = false;
                        }

                        if (wallSliding)
                        {
                            if (wallDirX == input.x)
                            {
                                velocity.x = -wallDirX * wallJumpClimb.x;
                                velocity.y = wallJumpClimb.y;
                            }
                            else if (input.x == 0)
                            {
                                velocity.x = -wallDirX * wallJumpOff.x;
                                velocity.y = wallJumpOff.y;
                            }
                            else {
                                velocity.x = -wallDirX * wallLeap.x;
                                velocity.y = wallLeap.y;
                            }
                        }
                        if (controller.collisions.below)
                        {
                            velocity.y = maxJumpVelocity;
                        }
                    }
                    if (Input.GetButtonUp("Jump"))
                    {
                        if (velocity.y > minJumpVelocity)
                        {
                            velocity.y = minJumpVelocity;
                        }
                    }
                }

                //climbing stuff
                if (isClimbable)
                {
                    if (Input.GetButtonDown("Interact"))
                    {
                        climbing = true;
                        velocity.y = 0;
                    }
                }
            }

            ////climbingupmovement
            //if (controller.collisions.below)
            //{
            //    climbingUpMovement = false;
            //}


            //flips sprite depending on direction facing.
            if (controller.collisions.faceDir == 1)
            {
                animator.hairAnimator.gameObject.GetComponent<SpriteRenderer>().flipX = false;
                animator.bodyAnimator.gameObject.GetComponent<SpriteRenderer>().flipX = false;
                animator.equipmentAnimator.gameObject.GetComponent<SpriteRenderer>().flipX = false;
                animator.weaponAnimator.gameObject.GetComponent<SpriteRenderer>().flipX = false;
            }
            else
            {
                animator.hairAnimator.gameObject.GetComponent<SpriteRenderer>().flipX = true;
                animator.bodyAnimator.gameObject.GetComponent<SpriteRenderer>().flipX = true;
                animator.equipmentAnimator.gameObject.GetComponent<SpriteRenderer>().flipX = true;
                animator.weaponAnimator.gameObject.GetComponent<SpriteRenderer>().flipX = true;
            }

            //cant move if attacking.
            if (isAttacking)
            {
                input = Vector2.zero;
            }

            //Animation Call Section
            
            if (climbingUp)
            {
                UnPauseAnimators();
                climbingUpMovement = true;
                animator.PlayAnimation(PlayerAnimationController.Animations.ClimbingUp);
            }

            if (SkillsController.skillsController.activatingAbility &!climbingUp)
            {
                //play the ability animation.
            }

            if (attackLaunched && !climbingUp && !SkillsController.skillsController.activatingAbility)
            {
                UnPauseAnimators();
                animator.PlayAnimation(PlayerAnimationController.Animations.Attacking);

            }

            if (climbing && !isAttacking && !climbingUp && !SkillsController.skillsController.activatingAbility)
            {
                UnPauseAnimators();
                animator.PlayAnimation(PlayerAnimationController.Animations.Climbing);
            }
            if (climbing && (velocity.y == 0 && velocity.x == 0) && !isAttacking && !climbingUp && !SkillsController.skillsController.activatingAbility)
            {
                Invoke("PauseAnimators", 0.1f);
            }

            if (velocity.y != 0 && controller.collisions.below == false && isAttacking == false && !climbing && !climbingUp && !climbingUpMovement && !SkillsController.skillsController.activatingAbility)
            {
                UnPauseAnimators();
                animator.PlayAnimation(PlayerAnimationController.Animations.Jumping);
            }

            if ((input.x != 0 && controller.collisions.below == true && !climbing && !climbingUp) || GameControl.gameControl.endOfLevel == true && !SkillsController.skillsController.activatingAbility)
            {
                UnPauseAnimators();
                animator.PlayAnimation(PlayerAnimationController.Animations.Running);
            }

            if (input.x == 0 && isAttacking == false && controller.collisions.below == true && !climbing && !climbingUp && GameControl.gameControl.endOfLevel == false && !SkillsController.skillsController.activatingAbility)
            {
                UnPauseAnimators();
                animator.PlayAnimation(PlayerAnimationController.Animations.Idle);
            }


            float targetVelocityX = input.x * moveSpeed;
            velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below) ? accelerationTimeGrounded : accelerationTimeAirborne);
            if (controller.collisions.left && controller.collisions.faceDir == -1)
            {
                velocity.x = 0;
            } 
            else if (controller.collisions.right && controller.collisions.faceDir == 1)
            {
                velocity.x = 0;
            }


            if (climbing)
            {
                gravity = 0;
                velocity.y = input.y * climbSpeed;
                velocity.x = input.x * climbSpeed;

                if (transform.position.y >= climbingUpPosition)
                {
                    velocity = Vector3.zero;
                    climbingUp = true;
                    Invoke("MovePlayerWhenClimbingUp", 0.125f);
                }
                
            }
            else {
                gravity = -1000;
                velocity.y += gravity * Time.deltaTime;
            }

            controller.Move(velocity * Time.deltaTime, input);

            if (controller.collisions.above || controller.collisions.below)
            {
                velocity.y = 0;
            }

            //TODO testing buffs
            if (Input.GetKeyDown(KeyCode.U))
            {
                animator.PlayAnimation(PlayerAnimationController.Animations.Buff);
            }
        }
    }

    //Called from combat engine.
    public void Knockback ()
    {
        knockBack = true;
    }

    public void Death ()
    {
        GameControl.gameControl.hp = 0;
    }

    //Triggers dictate climbing, interactables, level triggers, and other things.
    public void OnTriggerEnter2D (Collider2D collider) {
		if (collider.gameObject.GetComponent<IsClimbable>())
        {
			isClimbable = true;
            climbingUpPosition = collider.bounds.max.y + 20;
        }
        //climbing up action
        if (collider.gameObject.name == "ClimbingUp" && climbing) {
            climbingUpPosition = collider.bounds.max.y;
        }

        //Reaching the Goal
        if (collider.gameObject.layer == 18)
        {
           GameControl.gameControl.endOfLevel = true;
            velocity.x = 1 * moveSpeed;
            UserInterface uI = GameObject.FindGameObjectWithTag("UserInterface").GetComponent<UserInterface>();
            uI.EndOfLevel();
        }

        //Falling off the world
        if (collider.gameObject.layer == 19)
        {
            GameControl.gameControl.hp = 0;
        }

        //Interactable Objects
        if (collider.gameObject.layer == 21)
        {
            GameObject.FindGameObjectWithTag("UserInterface").GetComponent<UserInterface>().showInteractableDisplay = true;
        }
    }
	
	//this will be used to gauge interactions...I might need to do these things in the climbable script.
	public void OnTriggerExit2D (Collider2D collider) {
		if (collider.gameObject.GetComponent<IsClimbable>()) {
			isClimbable = false;
			climbing = false;
            climbingUp = false;
		}

        //Interactable Objects
        if (collider.gameObject.layer == 21)
        {
            GameObject.FindGameObjectWithTag("UserInterface").GetComponent<UserInterface>().showInteractableDisplay = false;
        }
    }
	//called from attacking animation at the begining and end.
	public void IsAttacking () {
		isAttacking = !isAttacking;
	}

    //Resets the ability to attack & calls the combo countdown, called by the animation.
    public void EndOfAttack ()
    {
        attackLaunched = false;
        CombatEngine.combatEngine.runComboClock = true;
        CombatEngine.combatEngine.comboCountDown = CombatEngine.combatEngine.comboWindow;

        if (CombatEngine.combatEngine.comboCount < CombatEngine.combatEngine.maxCombos)
        {
            CombatEngine.combatEngine.comboCount++;
        }
        else
        {
            CombatEngine.combatEngine.comboCount = 1;
        }
    }
 
    //called by climbing up animation to stop animating.
    public void IsClimbingUp() {
        climbingUp = false;
    }

    //called by animator.
    public void FlinchRecovered ()
    {
        flinching = false;
        knockBack = false;
    }

    //used as an invoke to move the player
    public void MovePlayerWhenClimbingUp() {
        float playerAdjustment = transform.position.y - playerCollider.bounds.min.y;
        this.gameObject.transform.position = new Vector3(transform.position.x, climbingUpPosition + playerAdjustment);
        climbingUpMovement = false;
    }

    public void PauseAnimators() {
        //playerAnimationController.enabled = false;
        animator.hairAnimator.gameObject.GetComponent<Animator>().enabled = false;
        animator.bodyAnimator.gameObject.GetComponent<Animator>().enabled = false;
        animator.equipmentAnimator.gameObject.GetComponent<Animator>().enabled = false;
        animator.weaponAnimator.gameObject.GetComponent<Animator>().enabled = false;
    }

    public void UnPauseAnimators ()
    {
        animator.hairAnimator.gameObject.GetComponent<Animator>().enabled = true;
        animator.bodyAnimator.gameObject.GetComponent<Animator>().enabled = true;
        animator.equipmentAnimator.gameObject.GetComponent<Animator>().enabled = true;
        animator.weaponAnimator.gameObject.GetComponent<Animator>().enabled = true;
    }

    //called by the animator.
    public void FullyRevived ()
    {
        deathStanding = false;
    }
	
	//called from the animations for attacking.
	public void Attack () {
		float directionX = controller.collisions.faceDir;
        float rayLength = EquipmentDatabase.equipmentDatabase.equipment[GameControl.gameControl.equippedWeapon].attackRange;
		float rayOriginX = (directionX == 1) ? playerCollider.bounds.max.x + 0.01f : playerCollider.bounds.min.x - 0.01f;
		float rayOriginY = playerCollider.bounds.center.y;
		Vector2 rayOrigin = new Vector2 (rayOriginX, rayOriginY);

        knockBackForce = EquipmentDatabase.equipmentDatabase.equipment[GameControl.gameControl.equippedWeapon].knockbackForce;
        CombatEngine.combatEngine.enemyKnockBackDirection = controller.collisions.faceDir;

        //Wizards(4) and Rangers(3) fire a projectile that calls attack on contact.
        if (GameControl.gameControl.playerClass == 3)
        {
            int projectileNumber = GameControl.gameControl.equippedWeapon % 10;
            //fire projectile.
            Instantiate(ClassesDatabase.classDatabase.arrows[projectileNumber], transform.position, Quaternion.identity);

        }
        else if (GameControl.gameControl.playerClass == 4)
        {
            int projectileNumber = GameControl.gameControl.equippedWeapon % 10;
            //fire projectile.
            Instantiate(ClassesDatabase.classDatabase.magicMissles[projectileNumber], transform.position, Quaternion.identity);

        }
        // everyone else swings.
        else
        {
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
}