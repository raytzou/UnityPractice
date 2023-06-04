using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnityChanAnimator : MonoBehaviour
{
    [SerializeField]
    Animator animator;

    //private int _hitCounter = 0;
    
    void Update()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("StandA_idleA"))
        {
            if (!animator.GetBool("BoolAttack1"))
            {
                for (int i = 1; i <= 5; i++)
                {
                    animator.SetBool("BoolAttack" + i, false);
                }
            }

            if (Input.GetKey(KeyCode.Mouse0))
            {
                animator.SetBool("BoolAttack1", true);
            }
            else
                animator.SetBool("BoolAttack1", false);
            //Debug.LogError("standby");
        }

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("WGS_attackA1"))
        {
            if (Input.GetKey(KeyCode.Mouse0))
            {
                animator.SetBool("BoolAttack2", true);
                animator.SetBool("BoolAttack1", false);
            }
            else
                animator.SetBool("BoolAttack2", false);
            //Debug.LogError("attack1");
        }

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("WGS_attackA2"))
        {
            if (Input.GetKey(KeyCode.Mouse0))
            {
                animator.SetBool("BoolAttack3", true);
                animator.SetBool("BoolAttack2", false);
            }
            else
                animator.SetBool("BoolAttack3", false);
            //Debug.LogError("attack2");
        }

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("WGS_attackA3"))
        {
            if (Input.GetKey(KeyCode.Mouse0))
            {
                animator.SetBool("BoolAttack4", true);
            }
            else
                animator.SetBool("BoolAttack4", false);
            //Debug.LogError("attack3");
        }

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("WGS_attackA4"))
        {
            if (Input.GetKey(KeyCode.Mouse0))
            {
                animator.SetBool("BoolAttack5", true);
            }
            else
                animator.SetBool("BoolAttack5", false);
            //Debug.LogError("attack4");
        }

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("WGS_attackA5"))
        {
            if (Input.GetKey(KeyCode.Mouse0))
            {
                animator.SetBool("BoolAttack5", true);
            }
            else
                animator.SetBool("BoolAttack5", false);
            //Debug.LogError("attack4");
        }
    }
}
