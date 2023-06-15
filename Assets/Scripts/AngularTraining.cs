using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngularTraining : MonoBehaviour
{
    [SerializeField]
    Transform Head, Target; // Unity-chan的頭跟直視目標物

    /// <summary>
    /// Unity-chan looks at a sphere
    /// </summary>
    private void LateUpdate()
    {
        Vector3 vectorToTarget = Target.position - Head.position; // Vector from Unity-chan to Target
        Vector3 vectorForward = transform.forward; // Unity-chan's forward vector
        Vector3 horizontalVector = vectorToTarget;

        horizontalVector.y = 0f; // init, 不看y軸
        horizontalVector.Normalize(); // 正規化，取單位向量

        //以下角度為純量，無方向性，計算後交由拉角、四元數，稍後用來計算旋轉軸角度
        float verticleAngle = Vector3.Angle(vectorToTarget, horizontalVector);
        float horizontalAngle = Vector3.Angle(horizontalVector, vectorForward);
        //Debug.LogError("pitch: " + verticleAngle + " yaw: " + horizontalAngle);

        //計算機圖形學常用來判斷方向，如兩向量內積大於0，則它們的方向朝向相近；如果小於0，則方向相反。(0代表水平置中)，thx wikipedia summary help math idiot lol
        float dotHorizontalAngle = Vector3.Dot(transform.right, horizontalVector);
        //Debug.LogError(dotHorizontalAngle);

        if (horizontalAngle > 30f) horizontalAngle = 30f; // 防止頭旋轉過頭造成怪異現象
        if (dotHorizontalAngle < 0f) horizontalAngle = -horizontalAngle; // 水平角度，純量角度不具方向性，透過向量內積取得方向性
        if (verticleAngle > 10f) verticleAngle = 10f; // 防止頭旋轉過頭造成怪異現象
        if (vectorToTarget.y > 0f) verticleAngle = -verticleAngle; // 垂直角度可直接由Unity-chan往Target的向量的Y值判斷方向，Unity-chan頭的Y軸向下，因此Y軸大於零角度為負
        

        // 計算四元數尤拉角
        Quaternion quaRotationAngle = Quaternion.Euler(0, horizontalAngle, 0);
        // 利用尤拉角計算水平向量(垂直軸，Unity-chan左右轉頭)
        Vector3 vectorWithRotationAngle = quaRotationAngle * vectorForward;
        // 外積計算垂直向量(水平軸，Unity-chan垂直點頭)
        Vector3 horizontalAxis = Vector3.Cross(Vector3.up, vectorWithRotationAngle);
        // 利用四元數計算軸線
        Quaternion quaRotationAxis = Quaternion.AngleAxis(verticleAngle, horizontalAxis);

        // 重新計算最後向量
        vectorToTarget = quaRotationAxis * vectorWithRotationAngle;
        // 在這裡真正旋轉Unity-chan的頭 lol
        Head.rotation = quaRotationAxis * quaRotationAngle * Head.rotation; // 四元數沒有交換率，不可以寫成 Head.rotation *= xxx;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(Head.position, Target.position);
    }
}
