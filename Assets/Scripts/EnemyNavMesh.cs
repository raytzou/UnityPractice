using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyNavMesh : MonoBehaviour
{
    public NavMeshAgent Agent;
    public GameObject Target;
    float _timeInterval = 0f;

    private void Update()
    {
        var distance = (transform.position - Target.transform.position).magnitude;
        
        if (distance <= Agent.stoppingDistance)
        {
            _timeInterval += Time.deltaTime;

            if(_timeInterval >= 1f)
            {
                var impactForce = (Target.transform.position - transform.position).normalized;

                impactForce.y += .5f;
                Target.GetComponent<CharacterController>().Move(Vector3.Lerp(impactForce, Target.transform.position, Time.deltaTime));
                Target.GetComponent<TPSControlTraining>().PlayerHP -= 0.1f;
                _timeInterval = 0f;
            }
            
        }
        else if (distance < 5f)
        {
            Agent.SetDestination(Target.transform.position);
        }
    }
}
