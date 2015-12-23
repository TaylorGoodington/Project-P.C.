using UnityEngine;

public class PlayerAnimationController : MonoBehaviour {

	public Animator animator;

	void Start () {
		animator = GetComponent<Animator>();
	}

	public void PlayAnimation (string animationName, int direction) {
		if (direction == 1) {
			if (animationName == "Attack") {
				animator.Play("AttackingRight");
			} else if (animationName == "Running") {
				animator.Play("RunningRight");
			} else if (animationName == "Idle") {
				animator.Play("IdleRight");
			} else if (animationName == "Jumping") {
				animator.Play("JumpingRight");
			}
		} else {
			if (animationName == "Attack") {
				animator.Play("AttackingLeft");
			} else if (animationName == "Running") {
				animator.Play("RunningLeft");
			} else if (animationName == "Idle") {
				animator.Play("IdleLeft");
			} else if (animationName == "Jumping") {
				animator.Play("JumpingLeft");
			}
		}
	}
}
