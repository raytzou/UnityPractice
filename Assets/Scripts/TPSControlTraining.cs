using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Class Note:
 * 四元數多半使用於向量需要做Rotation等transform
 * **/

public class TPSControlTraining : MonoBehaviour
{
    [SerializeField]
    [Range(1f, 20f)]
    float MouseSensitive = 3f;

    public TPSCameraTarget CameraTarget;
    public CharacterController Controller;
    public LayerMask LayerMask; // for SphereCast
    public float Velocity = 5f;

    public Transform CameraTransform;
    private Vector3 HorizontalDirection;
    private float VerticalDegree = 0f; // degree = degree + sensitive * (mouseY axis)
    public float DistanceToTarget = 5f; // for zoom-in, out

    private void Start()
    {
        HorizontalDirection = transform.forward;
    }

    private void Update()
    {
        CameraTarget.UpdateCameraTargetTransform(); // 防止多筆物件同時Update造成物件(Target或Camera)快速抖動，因此先Update Target再Update Camera
        MouseMove();
        CharacterMove();
    }

    private void MouseMove()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y"); // -1 to 1

        #region 處理水平視角
        HorizontalDirection = Quaternion.Euler(0, mouseX * MouseSensitive, 0) * HorizontalDirection; // 水平旋轉
        //var newCameraPosition = CameraTarget.transform.position - HorizontalDirection * Distance; // 水平旋轉結果
        #endregion

        #region 處理垂直視角
        VerticalDegree += mouseY * MouseSensitive;
        //Debug.LogError(VerticalDegree); //mouse up = positive degree
        if (VerticalDegree > 80f) VerticalDegree = 80f;
        else if (VerticalDegree < -80f) VerticalDegree = -80f;
        Vector3 horizontalAxis = Vector3.Cross(Vector3.up, HorizontalDirection); // 水平方向與全域座標上方互相外積，求得水平軸，用來與垂直度數計算向量軸的四元數
        var finalVector = Quaternion.AngleAxis(VerticalDegree, horizontalAxis) * HorizontalDirection; // 最後結果之向量
        #endregion


        //var newCameraPosition = CameraTarget.transform.position - finalVector * DistanceToTarget; // 處理過水平、垂直後的攝影機定位
        var newCameraPosition = Vector3.zero; // 重新初始化


        #region 處理鏡頭超出牆壁或地上
        /// SphereCast(Target出發點，半徑，攝影鏡頭方向，raycast hit，鏡頭和Target的距離再加上微調，LayerMask)
        if (Physics.SphereCast(CameraTarget.transform.position, 0.2f, -finalVector, out RaycastHit rayHit, DistanceToTarget + 0.2f, LayerMask))
        {
            newCameraPosition = CameraTarget.transform.position - finalVector * (rayHit.distance - 0.1f); // 計算Camera到Target距離
        }
        else
        {
            newCameraPosition = CameraTarget.transform.position - finalVector * DistanceToTarget;
        }
        #endregion

        CameraTransform.position = newCameraPosition;
        CameraTransform.LookAt(CameraTarget.transform.position);
    }

    private void CharacterMove()
    {
        float MoveVertical = Input.GetAxis("Vertical");
        float MoveHorizontal = Input.GetAxis("Horizontal");

        transform.Rotate(0, MoveHorizontal, 0); // 角色旋轉，單純Rotate Horizontal Axis即可
        Controller.SimpleMove(MoveVertical * Velocity * transform.forward);
    }
}
