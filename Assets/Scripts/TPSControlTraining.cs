using System.Collections;
using System.Collections.Generic;
using Cinemachine.Utility;
using UnityEngine;

/*
 * Class Note:
 * �|���Ʀh�b�ϥΩ�V�q�ݭn��Rotation��transform
 * SimpleMove�ɭP����}�B�A�i��O�]��Character Controller�ɭP(Skin Width in Inspecter)
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

        Main.Singleton.Init(); // singleton test
        Main.Singleton.LoadResourcesTest(); // singleton test
    }

    private void Update()
    {
        CameraTarget.UpdateCameraTargetTransform(); // ����h������P��Update�y������(Target��Camera)�ֳt�ݰʡA�]����Update Target�AUpdate Camera
        MouseMove();
        CharacterMove();
    }

    private void MouseMove()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y"); // -1 to 1

        #region �B�z��������
        HorizontalDirection = Quaternion.Euler(0, mouseX * MouseSensitive, 0) * HorizontalDirection; // ��������
        //var newCameraPosition = CameraTarget.transform.position - HorizontalDirection * Distance; // �������൲�G
        #endregion

        #region �B�z��������
        VerticalDegree += mouseY * MouseSensitive;
        //Debug.LogError(VerticalDegree); //mouse up = positive degree
        if (VerticalDegree > 80f) VerticalDegree = 80f;
        else if (VerticalDegree < -80f) VerticalDegree = -80f;
        Vector3 horizontalAxis = Vector3.Cross(Vector3.up, HorizontalDirection); // ������V�P����y�ФW�褬�ۥ~�n�A�D�o�����b�A�ΨӻP�����׼ƭp��V�q�b���|����
        var finalVector = Quaternion.AngleAxis(VerticalDegree, horizontalAxis) * HorizontalDirection; // �̫ᵲ�G���V�q
        #endregion


        //var newCameraPosition = CameraTarget.transform.position - finalVector * DistanceToTarget; // �B�z�L�����B�����᪺��v���w��
        Vector3 newCameraPosition; // ���s��l��


        #region �B�z���Y�W�X����Φa�W
        /// SphereCast(Target�X�o�I�A�b�|�A��v���Y��V�Araycast hit�A���Y�MTarget���Z���A�[�W�L�աALayerMask)
        if (Physics.SphereCast(CameraTarget.transform.position, 0.2f, -finalVector, out RaycastHit rayHit, DistanceToTarget + 0.2f, LayerMask))
        {
            newCameraPosition = CameraTarget.transform.position - finalVector * (rayHit.distance - 0.1f); // �p��Camera��Target�Z��
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

        //transform.Rotate(0f, MoveHorizontal, 0f); // �������A���Rotate Horizontal Axis�Y�i
        
        var vectorToCameraForward = MoveVertical * Velocity * CameraTransform.forward; // W�BS �e���V��CameraTransform
        var vectorToCameraRight = MoveHorizontal * Velocity * CameraTransform.right; // A�BD���k�����P�z
        vectorToCameraForward.y = 0f; // Y �b�����A��ϥ�Move()�A������Y�b�ACamera�ݤѪšB�a�W�A����|���½��
        Vector3 finalVector = vectorToCameraForward + vectorToCameraRight; // �̫ᵲ�G�A��V�q�ۥ[

        Controller.Move(finalVector * Time.deltaTime); // ��ڨ̦V�q���G����

        #region ������V���G
        if (MoveVertical != 0 || MoveHorizontal != 0) // ���UW, A, S, D���ਤ��A����¦V���Y�e��
            transform.forward = Vector3.Lerp(transform.forward, vectorToCameraForward, 0.1f);
        #endregion
    }
}
