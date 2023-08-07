using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieStatus : MonoBehaviour
{
    Animator ZombieAnimator;
    CharacterController ZombieController;
    public float Speed = 0f;
    public Transform Target;

    private float _attackInterval = 0f;

    private void Start()
    {
        ZombieAnimator = GetComponent<Animator>();
        ZombieController = GetComponent<CharacterController>();
        ZombieAnimator.SetBool("Idle", false);
        ZombieAnimator.SetBool("Tracing", false);
        ZombieAnimator.SetBool("Attack", false);
    }

    private void Update()
    {
        var distanceToPlayer = (GameObject.Find("Player").transform.position - transform.position).magnitude;
        //Debug.LogError(distanceToPlayer);
        distanceToPlayer = Mathf.Round(distanceToPlayer * 100) / 100;

        //Debug.LogError(distanceToPlayer);

        Tracing(distanceToPlayer);

        if (distanceToPlayer > 5.00f) // Idle
        {
            //Debug.LogError("Idle");
            ZombieAnimator.SetBool("Idle", true);
            ZombieAnimator.SetBool("Tracing", false);
            ZombieAnimator.SetBool("Attack", false);
        }
        else if (distanceToPlayer <= 5.00f && distanceToPlayer > 1.20f) // Tracing
        {
            //Debug.LogError("Tracing");
            ZombieAnimator.SetBool("Idle", false);
            ZombieAnimator.SetBool("Tracing", true);
            ZombieAnimator.SetBool("Attack", false);

            //Debug.LogError(-Mathf.Sin(distanceToPlayer)); 
            ZombieAnimator.SetFloat("TraceStatus", distanceToPlayer);
        }
        else if (distanceToPlayer <= 1.19f) // Attack
        {
            //Debug.LogError("Attack");
            ZombieAnimator.SetBool("Idle", false);
            ZombieAnimator.SetBool("Tracing", false);
            ZombieAnimator.SetBool("Attack", true);

            _attackInterval += Time.deltaTime;

            if (_attackInterval >= 1f)
                Attack();
        }
        //Debug.LogError(animator.GetCurrentAnimatorStateInfo(0));
    }

    private void Tracing(float distance)
    {
        if (distance <= 1.19f || distance > 5.00f) return;
        var vec = Target.position - transform.position;

        ZombieController.Move(Time.deltaTime * Speed * vec);
        Vector3 targetPos = Target.position;
        targetPos.y = 0;
        transform.LookAt(targetPos);
    }

    private void Attack()
    {
        var impactForce = (Target.position - transform.position).normalized;

        impactForce.y += .5f;
        //impactForce = Vector3.Lerp(Target.position, impactForce, Time.deltaTime);

        Target.GetComponent<CharacterController>().Move(Vector3.Lerp(impactForce, Target.position, Time.deltaTime)); // transform work with Character Controller, maybe use animator to make it smoother?
        Target.GetComponent<TPSControlTraining>().PlayerHP -= 0.1f;
        _attackInterval = 0f;
    }
}
