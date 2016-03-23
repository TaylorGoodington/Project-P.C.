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
        player.AttackLaunched();
    }
}
