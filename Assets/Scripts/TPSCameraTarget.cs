using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TPSCameraTarget : MonoBehaviour
{
    [SerializeField] Transform Target;

    [SerializeField] float CameraHeight;
    Vector3 currentVel = Vector3.zero;

    public bool LerpCameraFollow = true;

    public void UpdateCameraTargetTransform()
    {
        //transform.position = Target.position + Vector3.up * CameraHeight;
        var vectorToTarget = Target.position + Vector3.up * CameraHeight;
        #region ���Y���H��k1
        if (LerpCameraFollow)
            transform.position = Vector3.Lerp(transform.position, vectorToTarget, 0.1f);
        #endregion

        #region ���Y���H��k2
        else
            transform.position = Vector3.SmoothDamp(transform.position, vectorToTarget, ref currentVel, Time.deltaTime);
        #endregion
    }
}
