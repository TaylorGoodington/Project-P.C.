using UnityEngine;
using System.Collections;

public class WeaponAnimator : MonoBehaviour {

    public Player player;

    public void IsAttacking ()
    {
        player.IsAttacking();
    }

    public void Attack ()
    {
        player.Attack();
    }

    public void AttackLaunched ()
    {
        player.EndOfAttack();
    }

    public void IsClimbingUp ()
    {
        player.IsClimbingUp();
    }

    public void FlinchRecovered ()
    {
        player.FlinchRecovered();
    }

    public void FullyRevived ()
    {
        player.FullyRevived();
    }

    public void TransitionToPit ()
    {
        GameObject.FindGameObjectWithTag("UserInterface").GetComponent<Animator>().Play("DeathToPit");
    }

    public void DoneActivatingAbility ()
    {
        SkillsController.skillsController.activatingAbility = false;
    }
}
