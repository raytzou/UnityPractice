using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Class Note:
 * �|���Ʀh�b�ϥΩ�V�q�ݭn��Rotation��transform
 * **/

public class TPSControlTraining : MonoBehaviour
{
    [SerializeField]
    [Range(1f, 20f)]
    float MouseSensitive = 3f;

    public TPSCameraTarget CameraTarget;
    public CharacterController Controller;
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
        CameraTarget.UpdateCameraTargetTransform(); // ����h������P��Update�y������(Target��Camera)�ֳt�ݰʡA�]����Update Target�AUpdate Camera
        MouseMove();
        CharacterMove();
    }

    private void MouseMove()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y"); // -1 to 1

        HorizontalDirection = Quaternion.Euler(0, mouseX * MouseSensitive, 0) * HorizontalDirection; // ��������
        //var newCameraPosition = CameraTarget.transform.position - HorizontalDirection * Distance; // �������൲�G

        VerticalDegree += mouseY * MouseSensitive;
        //Debug.LogError(VerticalDegree); //mouse up = positive degree
        if (VerticalDegree > 80f) VerticalDegree = 80f;
        else if(VerticalDegree < -80f) VerticalDegree = -80f;
        Vector3 horizontalAxis = Vector3.Cross(Vector3.up, HorizontalDirection); // ������V�P����y�ФW�褬�ۥ~�n�A�D�o�����b�A�ΨӻP�����׼ƭp��V�q�b���|����
        var finalVector = Quaternion.AngleAxis(VerticalDegree, horizontalAxis) * HorizontalDirection; // �̫ᵲ�G���V�q

        var newCameraPosition = CameraTarget.transform.position - finalVector * DistanceToTarget; // �u����v���w��


        CameraTransform.position = newCameraPosition;
        CameraTransform.LookAt(CameraTarget.transform.position);
    }

    private void CharacterMove()
    {
        float MoveVertical = Input.GetAxis("Vertical");
        float MoveHorizontal = Input.GetAxis("Horizontal");

        transform.Rotate(0, MoveHorizontal, 0); // �������A���Rotate Horizontal Axis�Y�i
        Controller.SimpleMove(MoveVertical * Velocity * transform.forward);
    }
}
