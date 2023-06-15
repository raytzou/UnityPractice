using System;
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
    Transform CameraObj;

    [SerializeField]
    Transform CameraFollower;

    [SerializeField]
    [Range(250, 500)]
    float speed = 250f;

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
        HideMouse();
    }

    private void Update() // IDK, does Update mean real-time?
    {
        if (Input.GetKeyDown(KeyCode.Space) && IsEnableToJump) IsJump = true;
        if (Input.GetKeyDown(KeyCode.Escape)) ShowPauseMenu();
        if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift)) speed *= 2;
        if (Input.GetKeyUp(KeyCode.LeftShift) || Input.GetKeyUp(KeyCode.RightShift)) speed /= 2;
        if (Input.GetKey(KeyCode.Mouse0)) Shoot();

        PlayerEdgeProtection();
    }

    /// <summary>
    /// FixedUpdate() is executed at regular time intervals.
    /// </summary>
    private void FixedUpdate() // 物理運算於此運算似乎較精準?
    {
        if (IsJump)
        {
            rigidBody.AddForce(Vector3.up * jumpForce, ForceMode.Acceleration);
            IsJump = false; // 跳完直接切換
        }

        PlayerMove();
        
    }

    /// <summary>
    /// Same as Update(), it will be executed once every frame, the difference is that it will wait for all Update() of scripts to be executed before executing.
    /// </summary>
    private void LateUpdate() // IDK, but looks like when calculating angle, we often use LateUpdate()
    {
        MouseMove();
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

    private void PlayerMove()
    {
        float velY = rigidBody.velocity.y; // store original velocity.y
        Vector3 xValue = Input.GetAxis("Horizontal") * transform.right;
        Vector3 zValue = Input.GetAxis("Vertical") * transform.forward;
        Vector3 moveAmount = (xValue + zValue) * speed * Time.deltaTime;

        //transform.Translate(xValue * speed * Time.deltaTime, 0f, zValue * speed * Time.deltaTime);
        moveAmount.y = velY;
        rigidBody.velocity = moveAmount;
    }

    private void Shoot()
    {
        Ray ray = new(CameraObj.position, CameraObj.forward);
        int layerMask = 1 << LayerMask.NameToLayer("Ground");
        RaycastHit hit;
        bool isHit = Physics.Raycast(ray, out hit, 100f);

        if (isHit)
        {
            Debug.LogError(hit.collider.name);
        }
    }

    private void MouseMove()
    {
        int offset = 50;
        float cameraHeight = 0.7f;

        mouseYaw += mouseSpeed * offset * Time.deltaTime * Input.GetAxis("Mouse X");
        mousePitch -= mouseSpeed * offset * Time.deltaTime * Input.GetAxis("Mouse Y");

        // restrict angle
        if (mousePitch < -70) mousePitch = -70;
        if (mousePitch > 80) mousePitch = 80;

        CameraObj.position = Vector3.Lerp(CameraObj.position, CameraFollower.position + Vector3.up * cameraHeight, 0.65f); // 攝影機到玩家中間做內插，使鏡頭晃動幅度較低
        CameraObj.transform.eulerAngles = new Vector3(mousePitch, mouseYaw, 0f);
        transform.eulerAngles = new Vector3(0f, mouseYaw, 0f);
    }

    private void ShowPauseMenu()
    {
        ShowMouse();
        Time.timeScale = 0f; // stop the game
        GameObject.Find("UI").transform.Find("PauseMenu").gameObject.SetActive(true);
    }

    private void HideMouse()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void ShowMouse()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void PlayerEdgeProtection()
    {
        if(transform.position.y < -10)
        {
            transform.position = new Vector3(1f, 1f, 1f);
        }
    }
}
