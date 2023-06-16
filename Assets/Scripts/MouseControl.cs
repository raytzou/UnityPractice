using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseControl : MonoBehaviour
{
    private Camera camera;

    [SerializeField] LayerMask lm;

    [SerializeField] float moveSpeed = 1f;

    private void Start()
    {
        camera = Camera.main;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePosition = Input.mousePosition;

            Debug.LogError("mouse pos:" + mousePosition);

            Ray rayLine = camera.ScreenPointToRay(mousePosition);

            if (Physics.Raycast(rayLine, out RaycastHit rayHit1, 1000f))
            {
                Vector3 direction = rayHit1.point - transform.position;

                // Vector3.magnitude = �V�q���I���Z��? (square root of (x*x + y*y + z*z))
                if (direction.magnitude < moveSpeed * Time.deltaTime)
                {
                    Debug.LogError("TEST1");
                    transform.position = rayHit1.point;
                }
                else
                {
                    Debug.LogError("TEST2");
                    direction.Normalize();
                    MoveCharacter(direction, Time.deltaTime * moveSpeed);
                }
            }
        }
    }

    private void MoveCharacter(Vector3 direction, float moveAmount)
    {
        direction.y = 0f; // ��¥����A���Ҽ{Y�b
        transform.forward = direction;

        Vector3 vectorToPoint = transform.position + direction * moveAmount; // �쥻����l + (��V * ���ʶq)
        Vector3 vectorPointUp = vectorToPoint; // �w�Ƶ�����W�שY
        int layerMask = 1 << LayerMask.NameToLayer("Ground");

        vectorPointUp.y += 1f;

        if (Physics.Raycast(vectorPointUp, -Vector3.up, out RaycastHit rayHit2, 1f)) // �Q��raycast�P�_����O�_�ਫ��A�u�nhit terrain
        {
            transform.position = rayHit2.point;
            Debug.LogError("move to: " + rayHit2.point);
            Debug.LogError(rayHit2.collider.name);
        }
        else
            Debug.LogError("can't go to the point");
    }
}
