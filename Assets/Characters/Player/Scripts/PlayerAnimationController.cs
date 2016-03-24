using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    public Animator hairAnimator;
    public Animator bodyAnimator;
    public Animator weaponAnimator;
    public Animator equipmentAnimator;

    //Naming Convention for player animations are as follows:
    //For Attacking: Hair1SwordAttacking1, (animator type)(animator type index)(weapon type)(animation name)(combo count)
    //Not Attacking: Hair1Jumping, (animator type)(animator type index)(animation name)
    //All weapons dont reference their weapon type.
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
        Attacking,
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
        if (animation == Animations.Attacking)
        {
            hairAnimator.Play("Hair" + 
                              GameControl.gameControl.hairIndex + 
                              EquipmentDatabase.equipmentDatabase.equipment[GameControl.gameControl.equippedWeapon].equipmentType + 
                              animationName + 
                              CombatEngine.combatEngine.comboCount);
        }
        else
        {
            hairAnimator.Play("Hair" + GameControl.gameControl.hairIndex + animationName);
        }
    }

    public void PlayBodyAnimation(Animations animation)
    {
        string animationName = animation.ToString();
        if (animation == Animations.Attacking)
        {
            bodyAnimator.Play("Body" + 
                              GameControl.gameControl.skinColorIndex +
                              EquipmentDatabase.equipmentDatabase.equipment[GameControl.gameControl.equippedWeapon].equipmentType + 
                              animationName +
                              CombatEngine.combatEngine.comboCount);
        }
        else
        {
            bodyAnimator.Play("Body" + GameControl.gameControl.skinColorIndex + animationName);
        }
    }

    public void PlayWeaponAnimation(Animations animation)
    {
        string animationName = animation.ToString();
        if (animation == Animations.Attacking)
        {
            weaponAnimator.Play("Weapon" +
                                GameControl.gameControl.equippedWeapon +
                                animationName +
                                CombatEngine.combatEngine.comboCount);
        }
        else
        {
            weaponAnimator.Play("Weapon" + GameControl.gameControl.equippedWeapon + animationName);
        }
    }

    public void PlayEquipmentAnimation(Animations animation)
    {
        string animationName = animation.ToString();
        if (animation == Animations.Attacking)
        {
            equipmentAnimator.Play("Equipment" + 
                                   GameControl.gameControl.equippedEquipmentIndex +
                                   EquipmentDatabase.equipmentDatabase.equipment[GameControl.gameControl.equippedWeapon].equipmentType + 
                                   animationName +
                                   CombatEngine.combatEngine.comboCount);
        }
        else
        {
            equipmentAnimator.Play("Equipment" + GameControl.gameControl.equippedEquipmentIndex + animationName);
        }
    }
}
