using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * knowing bug: 
 *  yaw cause rotation being weird
 *  player can go through object sometime
 * **/

public class MyPlayerController : MonoBehaviour
{
    [SerializeField]
    Rigidbody rigidBody;

    [SerializeField]
    [Range(0, 200)]
    float speed = 10f;

    [SerializeField]
    float jumpForce = 200f;

    [SerializeField]
    [Range(1, 20)]
    float mouseSpeed = 3f;

    private bool IsJump = false;
    private bool IsEnableToJump = false;
    private float mouseYaw = 0f, mousePitch = 0f;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update() // IDK, does Update mean real-time?
    {
        if (Input.GetKeyDown(KeyCode.Space) && IsEnableToJump)
        {
            IsJump = true;
        }

        PlayerMove();
        MouseMove();
        PlayerEdgeProtection();
    }

    private void OnCollisionStay(Collision collision)
    {
        
        if (collision.transform.tag == "Ground")
        {
            IsEnableToJump = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if(IsEnableToJump)
            IsEnableToJump = false;
    }

    private void FixedUpdate() // ���z�B��󦹹B����G�����?
    {
        if(IsJump)
        {
            rigidBody.AddForce(Vector3.up * jumpForce, ForceMode.Acceleration);
            IsJump = false; // ������������
        }
    }

    private void PlayerMove()
    {
        float xValue = Input.GetAxis("Horizontal");
        float zValue = Input.GetAxis("Vertical");

        transform.Translate(xValue * speed * Time.deltaTime, 0f, zValue * speed * Time.deltaTime);
    }

    private void MouseMove()
    {
        int offset = 50;

        mouseYaw += mouseSpeed * offset * Time.deltaTime * Input.GetAxis("Mouse X");
        mousePitch -= mouseSpeed * offset * Time.deltaTime * Input.GetAxis("Mouse Y");

        // restrict angle
        if (mousePitch < -70) mousePitch = -70;
        if (mousePitch > 80) mousePitch = 80;

        transform.Find("Camera").GetComponent<Camera>().transform.eulerAngles = new Vector3(mousePitch, mouseYaw, 0f);
        transform.eulerAngles = new Vector3(0f, mouseYaw, 0f);
    }

    private void PlayerEdgeProtection()
    {
        if(transform.position.y < -10)
        {
            transform.position = new Vector3(1f, 1f, 1f);
        }
    }
}
