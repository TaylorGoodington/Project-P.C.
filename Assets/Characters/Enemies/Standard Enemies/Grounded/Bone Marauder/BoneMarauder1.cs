using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Controller2D))]
[RequireComponent(typeof(EnemyStats))]
public class BoneMarauder1 : EnemyBase
{
    string enemyType = "BoneMarauder1";

    public override void Start()
    {
        base.Start();
        AddSkills();
        AddItemsAndEquipmentDrops();
    }

    public override void Update()
    {
        base.Update();

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

        else if (beingAttacked && !enraged)
        {
            isAttacking = false;
            state = EnemyState.Attacking;
            enemyAnimationController.Play(enemyType + "Flinching");
            velocity.x = 0;
            float flinchTime = .33f;
            transform.Translate((player.GetComponent<Player>().knockBackForce / flinchTime) * CombatEngine.combatEngine.enemyKnockBackDirection * Time.deltaTime, 0, 0, Space.Self);
            CreatePatrolPath();
        }

        else if (isAttacking)
        {
            velocity.x = 0;
        }

        else
        {
            if (controller.collisions.below == false)
            {
                enemyAnimationController.Play(enemyType + "Jumping");
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
        }
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

    //Called by falling into a pit.
    public void Death()
    {
        stats.hP = 0;
    }

    //Called from the animator.
    public override void Attack()
    {
        //Overwrite if neccesary.
        base.Attack();
    }
}