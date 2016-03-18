using UnityEngine;
using System.Collections;

public class EquipmentGained : MonoBehaviour {

    public static EquipmentGained equipmentGained;
    Animator animator;

    void Start()
    {
        equipmentGained = GetComponent<EquipmentGained>();
        animator = GetComponent<Animator>();
    }

    public void EquipmentGainedAnimation()
    {
        animator.Play("EquipmentGained");
    }
}
