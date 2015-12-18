using UnityEngine;
using System.Collections;

public class PlayerAnimationController : MonoBehaviour {

	public Animator animator;

	void Start () {
		animator = GetComponent<Animator>();
	}
	
	void Update () {
	
	}
	
	public void PlayAnimation (string animationName, int direction) {
		if (direction == 1) {
			if (animationName == "Attack") {
				animator.Play("AttackingRight");
			}
		}
	}
	
	public void AdjustPosition (float directionX, float directionY) {
//		transform.position.x = gameObject.transform.position.x + directionX;
//		transform.position.y = gameObject.transform.position.y + directionY;
	}
}
