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
}
