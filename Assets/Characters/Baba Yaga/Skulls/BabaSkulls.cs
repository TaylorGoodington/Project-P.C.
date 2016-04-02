using UnityEngine;

[RequireComponent(typeof(EnemyStats))]
[RequireComponent(typeof(Hazard))]
public class BabaSkulls : MonoBehaviour {

    EnemyStats stats;
    GameObject player;
    SpriteRenderer sprite;
    Animator animationController;
    float strikeSpeed;
    bool launched;
    Vector2 playerTargetPosition;
    Vector2 targetStrikePosiion;
    Vector2 currentPosition;
    BabaYaga babaYaga;
    public LayerMask skullTargets;


    void Start ()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        sprite = GetComponent<SpriteRenderer>();
        stats = GetComponent<EnemyStats>();
        animationController = GetComponent<Animator>();
        babaYaga = GameObject.FindGameObjectWithTag("BabaYaga").GetComponent<BabaYaga>();
        strikeSpeed = stats.chaseSpeed + (babaYaga.GetComponent<BabaYaga>().aggressionPhase * 50);
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
            transform.position = Vector3.MoveTowards(transform.position, targetStrikePosiion, strikeSpeed * Time.deltaTime);
        }

        if (!sprite.isVisible)
        {
            Destroy(this.gameObject);
        }
    }

    public void Strike ()
    {
        launched = true;
        animationController.Play("Idle");
        playerTargetPosition = GameObject.FindGameObjectWithTag("Player").transform.position;
        currentPosition = transform.position;
        playerTargetPosition = playerTargetPosition - currentPosition;
        RaycastHit2D hit = Physics2D.Raycast(currentPosition, playerTargetPosition, 500, skullTargets);
        if (hit)
        {
            if (hit.collider.gameObject.layer == 23)
            {
                targetStrikePosiion = hit.point;
            }
        }
    }
}