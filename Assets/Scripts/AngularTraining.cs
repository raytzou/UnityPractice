using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngularTraining : MonoBehaviour
{
    [SerializeField]
    Transform Head, Target; // Unity-chan���Y��

    /// <summary>
    /// Unity-chan looks at a sphere
    /// </summary>
    private void LateUpdate()
    {
        Vector3 vectorToTarget = Target.position - Head.position; // Vector from Unity-chan to Target
        Vector3 vectorForward = transform.forward; // Unity-chan's forward vector
        Vector3 horizontalVector = vectorToTarget;

        horizontalVector.y = 0f; // init, ����y�b
        horizontalVector.Normalize(); // ���W�ơA�����V�q

        //�H�U���׬��¶q�A�L��V�ʡA�p����ѩԨ��B�|���ơA�y��Ψӭp�����b����
        float verticleAngle = Vector3.Angle(vectorToTarget, horizontalVector);
        float horizontalAngle = Vector3.Angle(horizontalVector, vectorForward);
        //Debug.LogError("pitch: " + verticleAngle + " yaw: " + horizontalAngle);

        //�p����ϧξǱ`�ΨӧP�_��V�A�p��V�q���n�j��0�A�h���̪���V�¦V�۪�F�p�G�p��0�A�h��V�ۤϡC(0�N������m��)�Athx wikipedia summary help math idiot lol
        float dotHorizontalAngle = Vector3.Dot(transform.right, horizontalVector);
        //Debug.LogError(dotHorizontalAngle);

        if (horizontalAngle > 30f) horizontalAngle = 30f; // �����Y����L�Y�y���ǲ��{�H
        if (dotHorizontalAngle < 0f) horizontalAngle = -horizontalAngle; // �������סA�¶q���פ����V�ʡA�z�L�V�q���n���o��V��
        if (verticleAngle > 10f) verticleAngle = 10f; // �����Y����L�Y�y���ǲ��{�H
        if (vectorToTarget.y > 0f) verticleAngle = -verticleAngle; // �������ץi������Unity-chan��Target���V�q��Y�ȧP�_��V
        

        // �p��|���ƤשԨ�
        Quaternion quaRotationAngle = Quaternion.Euler(0, horizontalAngle, 0);
        // �Q�ΤשԨ��p��έp��s�V�q(Unity-chan���k���Y)
        Vector3 vectorWithRotationAngle = quaRotationAngle * vectorForward;
        // �~�n�p�⫫���V�q(���o�����b�AUnity-chan�����I�Y)
        Vector3 horizontalAxis = Vector3.Cross(Vector3.up, vectorWithRotationAngle);
        // �Q�Υ|���ƭp��b�u
        Quaternion quaRotationAxis = Quaternion.AngleAxis(verticleAngle, horizontalAxis);

        // ���s�p��̫�V�q
        vectorToTarget = quaRotationAxis * vectorWithRotationAngle;
        // �b�o�̯u������Unity-chan���Y lol
        Head.rotation = quaRotationAxis * quaRotationAngle * Head.rotation; // �|���ƨS���洫�v�A���i�H�g�� Head.rotation *= xxx;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(Head.position, Target.position);
    }
}