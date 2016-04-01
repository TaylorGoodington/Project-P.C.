using UnityEngine;

[RequireComponent(typeof(EnemyStats))]
[RequireComponent(typeof(Hazard))]
public class BabaSkulls : MonoBehaviour {

    EnemyStats stats;
    GameObject player;
    SpriteRenderer sprite;
    Animator animationController;
    float moveSpeed;
    float strikeSpeed;
    bool launched;
    Vector3 targetPosition;

	void Start ()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        sprite = GetComponent<SpriteRenderer>();
        stats = GetComponent<EnemyStats>();
        animationController = GetComponent<Animator>();
        moveSpeed = stats.patrolSpeed;
        strikeSpeed = stats.chaseSpeed;
        launched = false;
	}
	
	void Update ()
    {
        if (!launched)
        {
            if (transform.position.x > player.transform.position.x)
            {
                sprite.flipX = false;
            }
            else
            {
                sprite.flipX = true;
            }
        }
        else
        {
            //need to fix this.
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, strikeSpeed * Time.deltaTime);
        }

        if (!sprite.isVisible)
        {
            Destroy(this.gameObject);
        }
    }

    public void Strike ()
    {
        launched = true;
        targetPosition = GameObject.FindGameObjectWithTag("Player").transform.position;
    }
}
