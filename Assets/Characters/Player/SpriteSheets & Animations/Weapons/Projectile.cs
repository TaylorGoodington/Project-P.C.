using UnityEngine;

public class Projectile : MonoBehaviour {

    Controller2D player;
    SpriteRenderer sprite;
    Collider2D projectileCollider;
    float initalXOffset;
    float initalYOffset;
    Animator animationController;
    public float projectileSpeed = 250;
    public bool hit;
    Transform objectHit;
    ProjectileType projectileType;
    int direction;
    bool decaying;
    float decayTimer = 2f;
    int projectileNumber;
    string weaponType;


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
        projectileType = (GameControl.gameControl.playerClass == 3) ? ProjectileType.Arrow : ProjectileType.MagicMissle;
        weaponType = (GameControl.gameControl.playerClass == 3) ? "Bow" : "Staff";
        projectileNumber = EquipmentDatabase.equipmentDatabase.equipment[GameControl.gameControl.equippedWeapon].equipmentTier;
        UpdateDirection();
    }
    //Play correct animation.

    void Update()
    {
        animationController.Play(weaponType + "Projectile" + projectileNumber);

        if (!hit)
        {
            transform.Translate(new Vector3(direction, 0) * projectileSpeed * Time.deltaTime, Space.World);
        }
        else if (projectileType == ProjectileType.Arrow && hit)
        {
            //move with what was struck...child the projetile to the hit object?
            transform.SetParent(objectHit);
            transform.localScale = Vector3.one;
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

    void OnTriggerEnter2D (Collider2D collider)
    {
        if (collider.gameObject.layer == 14)
        {
            hit = true;
            objectHit = collider.transform;
            this.transform.position = new Vector3(transform.position.x + (5 * direction), transform.position.y);
            StartCoroutine(CombatEngine.combatEngine.AttackingEnemy(collider));
        }

        //Platforms
        else if (collider.gameObject.layer == 10 || collider.tag == "ArrowBlocks")
        {
            hit = true;
            objectHit = collider.transform;
        }
    }
}