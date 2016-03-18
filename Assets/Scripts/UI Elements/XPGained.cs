using UnityEngine;

public class XPGained : MonoBehaviour {

    public static XPGained xpGained;
    Animator animator;

	void Start () {
        xpGained = GetComponent<XPGained>();
        animator = GetComponent<Animator>();
	}

    public void XPGainedAnimation ()
    {
        animator.Play("XPGained");
    }
}
