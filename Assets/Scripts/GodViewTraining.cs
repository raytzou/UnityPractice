using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GodViewTraining : MonoBehaviour
{
    [SerializeField] Transform Target;　// 玩家

    private Vector3 _vectorTargetToCamera;
    private float _distanceToTarget;

    private void Start()
    {
        _vectorTargetToCamera = transform.position - Target.position;
        _distanceToTarget = _vectorTargetToCamera.magnitude;
        _vectorTargetToCamera.Normalize();
    }

    private void LateUpdate() // Player 晃動問題，許多物件同時 Update 導致，更換為 LateUpdate 或使用 singleton
    {
        if (Input.mouseScrollDelta.y > 0 && _distanceToTarget > 5)
            _distanceToTarget--;
        else if (Input.mouseScrollDelta.y < 0 && _distanceToTarget < 12)
            _distanceToTarget++;


        var cameraPosition = Target.position + _vectorTargetToCamera * _distanceToTarget; // Target 位置 + 向量 * 距離 = Camera 位置

        transform.position = cameraPosition;
    }
}
