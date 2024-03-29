﻿using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Hazard))]
public class Scorpion1 : EnemyBase
{
    string enemyType = "Scorpion1";

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
            state = EnemyState.Patroling;
            enemyAnimationController.Play(enemyType + "Flinching");
            velocity.x = 0;
            float flinchTime = .33f;
            //transform.Translate((player.GetComponent<Player>().knockBackForce / flinchTime) * CombatEngine.combatEngine.enemyKnockBackDirection * Time.deltaTime, 0, 0, Space.Self);
            gravity = -1000;
            velocity.y += gravity * Time.deltaTime;
            velocity.x = (player.GetComponent<Player>().knockBackForce / flinchTime) * CombatEngine.combatEngine.enemyKnockBackDirection * Time.deltaTime;
            controller.Move(velocity, input);
            CreatePatrolPath();
        }

        else if (isAttacking)
        {
            velocity.x = 0;
        }

        else
        {
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
        
    }

    //Called by falling into a pit.
    public void Death()
    {
        stats.hP = 0;
    }

    //Called from the animator.
    public override void Attack()
    {
        //Scorpions do not attack...
    }
}