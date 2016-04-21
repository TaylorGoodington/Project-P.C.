using UnityEngine;
using System.Collections;

public class Test : MonoBehaviour {
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

    void Start()
    {
        controller = GetComponent<Controller2D>();
        animator = GetComponent<PlayerAnimationController>();
        playerCollider = GetComponent<BoxCollider2D>();

        gravity = -1000;
        maxJumpVelocity = (Mathf.Abs(gravity) * (timeToJumpApex)) * ((Mathf.Pow(maxJumpHeight, -0.5221f)) * 0.1694f) * maxJumpHeight;
        minJumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(gravity) * minJumpHeight);
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
            controller.Move(velocity * Time.deltaTime, new Vector2(1, 0));
            if (controller.collisions.below == false)
            {
                animator.PlayAnimation(PlayerAnimationController.Animations.Jumping);
            }
            else
            {
                animator.PlayAnimation(PlayerAnimationController.Animations.Running);
            }
        }
        else if (flinching)
        {
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
                        else
                        {
                            timeToWallUnstick = wallStickTime;
                        }
                    }
                    else
                    {
                        timeToWallUnstick = wallStickTime;
                    }
                }

                //Launching an attack.
                if (Input.GetButtonDown("Attack") && !isAttacking)
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
                            else
                            {
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
            Animations();

            float targetVelocityX = input.x * moveSpeed;
            velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below) ? accelerationTimeGrounded : accelerationTimeAirborne);



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
            else
            {
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

    private void Animations()
    {
        if (climbingUp)
        {
            UnPauseAnimators();
            climbingUpMovement = true;
            animator.PlayAnimation(PlayerAnimationController.Animations.ClimbingUp);
        }
        else if (SkillsController.skillsController.activatingAbility)
        {
            //play the ability animation.
        }
        else if (attackLaunched)
        {
            UnPauseAnimators();
            animator.PlayAnimation(PlayerAnimationController.Animations.Attacking);
        }
        else if (climbing)
        {
            UnPauseAnimators();
            animator.PlayAnimation(PlayerAnimationController.Animations.Climbing);
        }
        else if (climbing && (velocity.y == 0 && velocity.x == 0))
        {
            Invoke("PauseAnimators", 0.1f);
        }
        else if (velocity.y != 0 && !controller.collisions.below)
        {
            UnPauseAnimators();
            animator.PlayAnimation(PlayerAnimationController.Animations.Jumping);
        }
        else if (input.x != 0 && controller.collisions.below)
        {
            UnPauseAnimators();
            animator.PlayAnimation(PlayerAnimationController.Animations.Running);
        }
        else
        {
            UnPauseAnimators();
            animator.PlayAnimation(PlayerAnimationController.Animations.Idle);
        }
    }

    public void PauseAnimators()
    {
        //playerAnimationController.enabled = false;
        animator.hairAnimator.gameObject.GetComponent<Animator>().enabled = false;
        animator.bodyAnimator.gameObject.GetComponent<Animator>().enabled = false;
        animator.equipmentAnimator.gameObject.GetComponent<Animator>().enabled = false;
        animator.weaponAnimator.gameObject.GetComponent<Animator>().enabled = false;
    }

    public void UnPauseAnimators()
    {
        animator.hairAnimator.gameObject.GetComponent<Animator>().enabled = true;
        animator.bodyAnimator.gameObject.GetComponent<Animator>().enabled = true;
        animator.equipmentAnimator.gameObject.GetComponent<Animator>().enabled = true;
        animator.weaponAnimator.gameObject.GetComponent<Animator>().enabled = true;
    }
}
