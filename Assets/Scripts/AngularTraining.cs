using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngularTraining : MonoBehaviour
{
    [SerializeField]
    Transform Head, Target;

    /// <summary>
    /// Unity-chan looks at a sphere
    /// </summary>
    private void LateUpdate()
    {
        Vector3 vector = Target.position - Head.position; // Vector from Unity-chan to Target
        Vector3 vectorForward = transform.forward; // Unity-chan's forward vector
        Vector3 horizontalVector = vector;

        horizontalVector.y = 0f; // init
        horizontalVector.Normalize(); // 正規化，取單位向量

        float pitch = Vector3.Angle(vector, horizontalVector); // vertical angle
        float yaw = Vector3.Angle(horizontalVector, vectorForward);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(Head.position, Target.position);
    }
}