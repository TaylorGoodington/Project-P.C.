using UnityEngine;

public class AbilityProjectiles : MonoBehaviour {

    Controller2D player;
    SpriteRenderer sprite;
    Collider2D projectileCollider;
    float initalXOffset;
    float initalYOffset;
    Animator animationController;
    public float projectileSpeed = 250;
    public bool sticksToObjectHit = false;
    public float decayTimer = 1;
    [HideInInspector]
    public bool hit;
    Transform objectHit;
    int direction;
    bool decaying;
    Skills currentSkill;


    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Controller2D>();
        sprite = GetComponent<SpriteRenderer>();
        animationController = GetComponent<Animator>();
        projectileCollider = GetComponent<Collider2D>();
        initalXOffset = projectileCollider.offset.x;
        initalYOffset = projectileCollider.offset.y;
        hit = false;
        decaying = false;
        currentSkill = SkillsController.skillsController.selectedSkill;
        UpdateDirection();
    }

    void Update()
    {
        //The projectile is traveling.
        if (!hit)
        {
            transform.Translate(new Vector3(direction, 0) * projectileSpeed * Time.deltaTime, Space.World);
            animationController.Play(currentSkill.skillName + " Movement");
        }
        //If the projectile sticks to the object struck.
        else if (sticksToObjectHit)
        {
            transform.SetParent(objectHit);
            transform.localScale = Vector3.one;
            decaying = true;
            animationController.Play(currentSkill.skillName + " Decaying");
        }
        //If the projectile doesn't stick to the object struck.
        else if (!sticksToObjectHit)
        {
            decaying = true;
            animationController.Play(currentSkill.skillName + " Decaying");
        }

        if (decaying)
        {
            decayTimer -= Time.deltaTime;
            if (decayTimer <= 0)
            {
                Destroy(this.gameObject);
            }
        }

        if (!sprite.isVisible)
        {
            Destroy(this.gameObject);
        }
    }

    private void UpdateDirection()
    {
        float xOffset;
        float yOffset;
        if (player.collisions.faceDir == 1)
        {
            sprite.flipX = false;
            direction = 1;
            xOffset = initalXOffset;
            yOffset = initalYOffset;
            projectileCollider.offset = new Vector2(xOffset, yOffset);
        }
        else
        {
            sprite.flipX = true;
            direction = -1;
            xOffset = initalXOffset * -1;
            yOffset = initalYOffset;
            projectileCollider.offset = new Vector2(xOffset, yOffset);
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.layer == 14)
        {
            hit = true;
            objectHit = collider.transform;
            this.transform.position = new Vector3(transform.position.x + (5 * direction), transform.position.y);
            CombatEngine.combatEngine.damagingAbility = currentSkill;
            player.gameObject.GetComponent<Player>().knockBackForce = currentSkill.knockbackForce;
            CombatEngine.combatEngine.enemyKnockBackDirection = direction;
            CombatEngine.combatEngine.AttackingEnemies(collider, true);
        }

        //Platforms
        else if (collider.gameObject.layer == 10 || collider.tag == "ArrowBlocks")
        {
            hit = true;
            objectHit = collider.transform;
        }
    }
}