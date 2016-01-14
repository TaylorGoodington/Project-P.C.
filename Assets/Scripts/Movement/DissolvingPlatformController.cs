using UnityEngine;
using System.Collections;

public class DissolvingPlatformController : MonoBehaviour {

    private Collider2D boxCollider;
    private SpriteRenderer spriteRenderer;
    private Animator animator;

	void Start () {
        boxCollider = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
	}

    public void StartDissolving()
    {
        animator.Play("DissolvingPlatform");
    }

    public void ToggleBoxCollider()
    {
        boxCollider.enabled = !boxCollider.enabled;
    }

    public void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            //I need to somehow also check to see if I have been standing on the platform.
            //I think i can do it by comparing the positions of both colliders.
            StartDissolving();
        }
    }
}
