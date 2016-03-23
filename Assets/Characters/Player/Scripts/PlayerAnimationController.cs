using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    public Animator hairAnimator;
    public Animator bodyAnimator;
    public Animator weaponAnimator;
    public Animator equipmentAnimator;

    //Naming Convention for player animations are as follows:
    //For Attacking: Hair1SwordAttacking1, (animator type)(animator type index)(weapon type)(animation name)
    //Not Attacking: Hair1Jumping, (animator type)(animator type index)(animation name)
    //All weapons follow the second example
    //ToDo Skills...

    //Master list of animations.
    public enum Animations
    {
        Idle,
        Running,
        Jumping,
        Flinching,
        Climbing,
        ClimbingUp,
        Attacking1,
        Attacking2,
        Attacking3,
        Attacking4,
        Attacking5,
        Attacking6,
        DeathFalling,
        DeathStanding
    }

    public void PlayAnimation (Animations animation)
    {
        PlayHairAnimation(animation);
        PlayBodyAnimation(animation);
        PlayEquipmentAnimation(animation);
        PlayWeaponAnimation(animation);
    }

    public void PlayHairAnimation (Animations animation)
    {
        string animationName = animation.ToString();
        if (animation == Animations.Attacking1 || animation == Animations.Attacking2 || animation == Animations.Attacking3 ||
            animation == Animations.Attacking4 || animation == Animations.Attacking5 || animation == Animations.Attacking6)
        {
            hairAnimator.Play("Hair" + GameControl.gameControl.hairIndex + 
                               EquipmentDatabase.equipmentDatabase.equipment[GameControl.gameControl.equippedWeapon].equipmentType + animationName);
        }
        else
        {
            hairAnimator.Play("Hair" + GameControl.gameControl.hairIndex + animationName);
        }
    }

    public void PlayBodyAnimation(Animations animation)
    {
        string animationName = animation.ToString();
        if (animation == Animations.Attacking1 || animation == Animations.Attacking2 || animation == Animations.Attacking3 ||
            animation == Animations.Attacking4 || animation == Animations.Attacking5 || animation == Animations.Attacking6)
        {
            bodyAnimator.Play("Body" + GameControl.gameControl.skinColorIndex +
                               EquipmentDatabase.equipmentDatabase.equipment[GameControl.gameControl.equippedWeapon].equipmentType + animationName);
        }
        else
        {
            bodyAnimator.Play("Body" + GameControl.gameControl.skinColorIndex + animationName);
        }
    }

    public void PlayWeaponAnimation(Animations animation)
    {
        string animationName = animation.ToString();
        weaponAnimator.Play("Weapon" + GameControl.gameControl.equippedWeapon + animationName);
    }

    public void PlayEquipmentAnimation(Animations animation)
    {
        string animationName = animation.ToString();
        if (animation == Animations.Attacking1 || animation == Animations.Attacking2 || animation == Animations.Attacking3 ||
            animation == Animations.Attacking4 || animation == Animations.Attacking5 || animation == Animations.Attacking6)
        {
            equipmentAnimator.Play("Equipment" + GameControl.gameControl.equippedEquipmentIndex +
                               EquipmentDatabase.equipmentDatabase.equipment[GameControl.gameControl.equippedWeapon].equipmentType + animationName);
        }
        else
        {
            equipmentAnimator.Play("Equipment" + GameControl.gameControl.equippedEquipmentIndex + animationName);
        }
    }
}
