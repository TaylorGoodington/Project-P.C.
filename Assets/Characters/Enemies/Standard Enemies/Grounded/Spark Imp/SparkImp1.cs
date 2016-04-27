using UnityEngine;

[RequireComponent(typeof(Controller2D))]
[RequireComponent(typeof(EnemyStats))]
[RequireComponent(typeof(Hazard))]

public class SparkImp1 : EnemyBase {

    string enemyType = "SparkImp1";
    public override void Start()
    {
        base.Start();
    }

    public override void Update()
    {
        Patrolling();

        gravity = -1000;
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime, input);
        
        enemyAnimationController.Play(enemyType + "Moving");

        if (controller.collisions.above || controller.collisions.below)
        {
            velocity.y = 0;
        }
    }

    //Called from the animator.
    public override void Attack()
    {
        //Spark Imps do not attack...
    }
}