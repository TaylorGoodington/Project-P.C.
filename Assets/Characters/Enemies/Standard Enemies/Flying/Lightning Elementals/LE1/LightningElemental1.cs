using UnityEngine;
using System.Collections;

public class LightningElemental1 : FlyingEnemyBase {

    string enemyType = "LightningElemental1";
    bool recharging;
    public float attackCooldown;
    Vector2 playerTargetPosition;
    Vector2 currentPosition;
    public LayerMask playerLayer;

    public override void Start()
    {
        base.Start();
        AddSkills();
        AddItemsAndEquipmentDrops();
        recharging = false;
        attackCooldown = 0;
    }

    public override void Update()
    {
        base.Update();

        //Checks for death.
        if (stats.hP <= 0)
        {
            velocity = Vector3.zero;
            enemyAnimationController.Play(enemyType + "Death");
        }

        //player is dead.
        else if (GameControl.gameControl.hp == 0)
        {
            Patrolling();

            enemyAnimationController.Play(enemyType + "Flying");
        }

        else if (beingAttacked && !enraged)
        {
            isAttacking = false;
            StopCoroutine("Attack");
            state = EnemyState.Patroling;
            enemyAnimationController.Play(enemyType + "Flinching");
            velocity = Vector3.zero;
            float flinchTime = .33f;
            //transform.Translate((player.GetComponent<Player>().knockBackForce / flinchTime) * CombatEngine.combatEngine.enemyKnockBackDirection * Time.deltaTime, 0, 0, Space.Self);
            gravity = -1000;
            velocity.y += gravity * Time.deltaTime;
            velocity.x = (player.GetComponent<Player>().knockBackForce / flinchTime) * CombatEngine.combatEngine.enemyKnockBackDirection * Time.deltaTime;
            controller.Move(velocity * Time.deltaTime, input);
        }

        else if (isAttacking)
        {
            velocity = Vector3.zero;
        }

        else
        {
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
                if (!isAttacking)
                {
                    if (!InAttackRange())
                    {
                        state = EnemyState.Chasing;
                    }
                    else if (InAttackRange() && attackCooldown <= 0)
                    {
                        isAttacking = true;
                        enemyAnimationController.Play(enemyType + "Attacking");
                    }
                }
            }

            if (recharging)
            {
                if (attackCooldown > 0)
                {
                    attackCooldown -= Time.deltaTime;
                }
                else
                {
                    recharging = false;
                }
            }

            //Chasing
            if (state == EnemyState.Chasing)
            {
                Chasing();
                enemyAnimationController.Play(enemyType + "Flying");
            }


            //Investigating
            if (state == EnemyState.Investigating)
            {
                Investigating();

                enemyAnimationController.Play(enemyType + "Flying");
            }


            //Patroling
            if (state == EnemyState.Patroling)
            {
                Patrolling();

                enemyAnimationController.Play(enemyType + "Flying");
            }
        }
    }

    //Add Skills Here
    private void AddSkills()
    {
        //stats.acquiredSkillsList.Add(SkillsDatabase.skillsDatabase.skills[0]);
    }

    //Add Items and Equipment Drops here...
    public void AddItemsAndEquipmentDrops()
    {
        //stats.itemsDropped.Add(ItemDatabase.itemDatabase.items[2]);
        //stats.itemsDropped[0].dropRate = 50;

        //stats.equipmentDropped.Add(EquipmentDatabase.equipmentDatabase.equipment[0]);
        //stats.equipmentDropped[0].dropRate = 50;
    }

    //Called at the end of the attack animation.
    public void Recharging ()
    {
        attackCooldown = 3f;
        isAttacking = false;
        StopCoroutine("Attack");
        recharging = true;
    }

    //Called during the attack animation
    public void Thunder ()
    {
        StartCoroutine("Attack");
    }

    //Called by falling into a pit.
    public void Death()
    {
        stats.hP = 0;
    }

    //Called from the animator.
    new IEnumerator Attack()
    {
        while (isAttacking)
        {
            currentPosition = GetComponent<Collider2D>().bounds.center;
            playerTargetPosition = player.transform.position;
            playerTargetPosition = playerTargetPosition - currentPosition;
            RaycastHit2D hit = Physics2D.Raycast(currentPosition, playerTargetPosition, 45, playerLayer);
            if (hit)
            {
                if (hit.collider.gameObject.layer == 9)
                {
                    CombatEngine.combatEngine.AttackingPlayer(this.GetComponent<Collider2D>(), stats.maximumDamage);
                }
            }

            yield return new WaitForSeconds(0.2f);
        }
    }
}