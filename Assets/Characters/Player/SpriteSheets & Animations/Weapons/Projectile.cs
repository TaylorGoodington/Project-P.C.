using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {

    Controller2D player;
    SpriteRenderer sprite;
    Collider2D projectileCollider;
    Animator animationController;
    public float projectileSpeed = 200;
    bool hit;
    Transform objectHit;
    ProjectileType projectileType;
    int direction;
    bool decaying;
    float decayTimer = 0.5f;
    int projectileNumber;


    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Controller2D>();
        sprite = GetComponent<SpriteRenderer>();
        animationController = GetComponent<Animator>();
        projectileCollider = GetComponent<Collider2D>();
        hit = false;
        decaying = false;
        projectileType = (GameControl.gameControl.playerClass == 3) ? ProjectileType.Arrow : ProjectileType.MagicMissle;
        projectileNumber = GameControl.gameControl.equippedWeapon;
        UpdateDirection();
    }
    //Play correct animation.

    void Update()
    {
        animationController.Play("Projectile" + projectileNumber);

        if (!hit)
        {
            transform.Translate(new Vector3(direction, 0) * projectileSpeed * Time.deltaTime, Space.World);
        }
        else if (projectileType == ProjectileType.Arrow && hit)
        {
            //move with what was struck...child the projetile to the hit object?
            transform.SetParent(objectHit);
            decaying = true;
        }
        else if (projectileType == ProjectileType.MagicMissle && hit)
        {
            Destroy(this.gameObject);
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

    public enum ProjectileType
    {
        Arrow,
        MagicMissle
    }

    private void UpdateDirection()
    {
        if (player.collisions.faceDir == 1) 
        {
            sprite.flipX = false;
            direction = 1;
            projectileCollider.offset = new Vector2(15, -7.5f);
        }
        else
        {
            sprite.flipX = true;
            direction = -1;
            projectileCollider.offset = new Vector2(-15, -7.5f);
        }
    }

    void OnTriggerEnter2D (Collider2D collider)
    {
        if (collider.gameObject.layer == 14)
        {
            CombatEngine.combatEngine.AttackingEnemies(collider);
            hit = true;
            objectHit = collider.transform;
        }

        //Platforms
        else if (collider.gameObject.layer == 10)
        {
            hit = true;
            objectHit = collider.transform;
        }
    }
}
