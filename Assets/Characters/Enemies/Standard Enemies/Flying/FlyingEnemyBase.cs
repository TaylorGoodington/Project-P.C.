using UnityEngine;
using System.Collections;

public class FlyingEnemyBase : EnemyBase
{
    #region Variables
    public Vector3[] localPatrolPoints;
    Vector3[] globalPatrolPoints;
    bool inPatrolZone;
    Vector3 primaryPatrolPoint;
    public float offScreenTimer;
    public Vector3 currentPatrolTarget;
    #endregion

    public override void Start()
    {
        base.Start();
        primaryPatrolPoint = transform.position;
        inPatrolZone = true;
        globalPatrolPoints = new Vector3[localPatrolPoints.Length];
        for (int i = 0; i < localPatrolPoints.Length; i++)
        {
            globalPatrolPoints[i] = localPatrolPoints[i] + transform.position;
        }

        currentPatrolTarget = globalPatrolPoints[0];
    }

    public override void Chasing()
    {
        inPatrolZone = false;
        //If the engagement counter has run out we stop chasing.
        if (engagementCounter <= 0)
        {
            state = EnemyBase.EnemyState.Patroling;
            ResetEngagementCountDown();
        }

        //If the engagement counter still has time we continue chasing.
        else if (engagementCounter > 0)
        {
            //Check if the line of sight is broken.
            if (!LineOfSight())
            {
                targetPosition = player.transform.position;
                state = EnemyBase.EnemyState.Investigating;
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
                        state = EnemyBase.EnemyState.Attacking;
                    }
                    else
                    {
                        float distance = Mathf.Sqrt((Mathf.Pow(player.transform.position.x - transform.position.x, 2f)) + (Mathf.Pow(player.transform.position.y - transform.position.y, 2f)));
                        float time = distance / chaseSpeed;
                        controller.Move((player.transform.position - transform.position) * Time.deltaTime / time, input);
                    }
                }
            }
        }
    }

    public override void Patrolling()
    {
        if (!inPatrolZone)
        {
            //enemy is on screen
            if (GetComponent<SpriteRenderer>().isVisible)
            {
                float distance = Mathf.Sqrt((Mathf.Pow(currentPatrolTarget.x - transform.position.x, 2f)) + (Mathf.Pow(currentPatrolTarget.y - transform.position.y, 2f)));
                float time = distance / patrolSpeed;
                controller.Move((primaryPatrolPoint - transform.position) * Time.deltaTime / time, input);
                offScreenTimer = 3;
                if (CloseToTheTarget(primaryPatrolPoint))
                {
                    inPatrolZone = true;
                }
            }

            //enemy is off screen.
            else if (!GetComponent<SpriteRenderer>().isVisible)
            {
                offScreenTimer -= Time.deltaTime;
                if (offScreenTimer < 0)
                {
                    transform.position = primaryPatrolPoint;
                    inPatrolZone = true;
                }
                else
                {
                    float distance = Mathf.Sqrt((Mathf.Pow(currentPatrolTarget.x - transform.position.x, 2f)) + (Mathf.Pow(currentPatrolTarget.y - transform.position.y, 2f)));
                    float time = distance / patrolSpeed;
                    controller.Move((primaryPatrolPoint - transform.position) * Time.deltaTime / time, input);
                }
            }
        }
        else
        {
            if (CloseToTheTarget(currentPatrolTarget))
            {
                int randomNumber = Random.Range(0, globalPatrolPoints.Length);
                currentPatrolTarget = globalPatrolPoints[randomNumber];
            }
            else
            {
                int targetDirection = (currentPatrolTarget.x > transform.position.x) ? 1 : -1;
                if ((controller.collisions.faceDir != targetDirection) && changingDirection == false)
                {
                    //velocity.x = Mathf.Lerp(velocity.x, 0f, 1f);
                    StartCoroutine(ChangeDirection(chaseSpeed));
                    changingDirection = true;
                }

                //if the enemy is facing the right direction.
                else if (controller.collisions.faceDir == targetDirection)
                {
                    changingDirection = false;
                    float distance = Mathf.Sqrt((Mathf.Pow(currentPatrolTarget.x - transform.position.x, 2f)) + (Mathf.Pow(currentPatrolTarget.y - transform.position.y, 2f)));
                    float time = distance / patrolSpeed;
                    controller.Move((currentPatrolTarget - transform.position) * Time.deltaTime / time, input);
                }
            }
        }
    }

    public override void Investigating()
    {
        inPatrolZone = false;
        //Set state to patrolling once the counter has run out.
        if (engagementCounter <= 0)
        {
            ResetEngagementCountDown();
            state = EnemyBase.EnemyState.Patroling;
        }

        //Engagement counter still has time.
        else if (engagementCounter > 0)
        {
            //Go back to chasing.
            if (LineOfSight())
            {
                ResetEngagementCountDown();
                state = EnemyBase.EnemyState.Chasing;
            }

            //Time to investigate.
            else if (!LineOfSight())
            {
                EngagementCountDown();
                float distance = Mathf.Sqrt((Mathf.Pow(targetPosition.x - transform.position.x, 2f)) + (Mathf.Pow(targetPosition.y - transform.position.y, 2f)));
                float time = distance / chaseSpeed;
                controller.Move((targetPosition - transform.position) * Time.deltaTime / time, input);
            }
        }

    }

    public bool CloseToTheTarget (Vector3 target)
    {
        int fudgeFactor = 1;
        if ((transform.position.x >= target.x - fudgeFactor && transform.position.x <= target.x + fudgeFactor) &&
            (transform.position.y >= target.y - fudgeFactor && transform.position.y <= target.y + fudgeFactor))
        {
            return true;
        }
        else
        {
            return false;
        }

    }

    //Draws the patrol points.
    void OnDrawGizmos()
    {
        if (localPatrolPoints != null)
        {
            Gizmos.color = Color.red;
            float size = 5f;

            for (int i = 0; i < localPatrolPoints.Length; i++)
            {
                Vector3 globalWaypointPos = (Application.isPlaying) ? globalPatrolPoints[i] : localPatrolPoints[i] + transform.position;
                Gizmos.DrawLine(globalWaypointPos - Vector3.up * size, globalWaypointPos + Vector3.up * size);
                Gizmos.DrawLine(globalWaypointPos - Vector3.left * size, globalWaypointPos + Vector3.left * size);
            }
        }
    }
}