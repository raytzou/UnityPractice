using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TPSCameraTarget : MonoBehaviour
{
    [SerializeField] Transform Target;

    [SerializeField] float CameraHeight;


    public void UpdateCameraTargetTransform()
    {
        transform.position = Target.position + Vector3.up * CameraHeight;
    }
}
