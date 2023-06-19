using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GodViewTraining : MonoBehaviour
{
    [SerializeField] Transform Target;�@// ���a

    private Vector3 _vectorTargetToCamera;
    private float _distanceToTarget;

    private void Start()
    {
        _vectorTargetToCamera = transform.position - Target.position;
        _distanceToTarget = _vectorTargetToCamera.magnitude;
        _vectorTargetToCamera.Normalize();
    }

    private void LateUpdate() // Player �̰ʰ��D�A�\�h����P�� Update �ɭP�A�󴫬� LateUpdate �Ψϥ� singleton
    {
        if (Input.mouseScrollDelta.y > 0 && _distanceToTarget > 5)
            _distanceToTarget--;
        else if (Input.mouseScrollDelta.y < 0 && _distanceToTarget < 12)
            _distanceToTarget++;


        var cameraPosition = Target.position + _vectorTargetToCamera * _distanceToTarget; // Target ��m + �V�q * �Z�� = Camera ��m

        transform.position = cameraPosition;
    }
}
