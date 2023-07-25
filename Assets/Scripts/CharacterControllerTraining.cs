using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Modified from MyPlayerController.cs
 * **/

public class CharacterControllerTraining : MonoBehaviour
{
    [SerializeField]
    Transform CameraObj;

    [SerializeField]
    Transform CameraFollower;

    [SerializeField]
    [Range(250, 500)]
    float speed = 250f; // define speed = 250 (units/s)

    [SerializeField]
    float jumpForce = 200f;

    [SerializeField]
    [Range(1, 20)]
    float mouseSpeed = 3f;

    [SerializeField]
    LayerMask LayerMask;

    private Vector3 _gravity;
    private float _speedUnit;
    private float mouseYaw = 0f, mousePitch = 0f;
    private bool _isJump = false;

    private CharacterController characterChontroller;

    private void Start()
    {
        HideMouse();
        characterChontroller = GetComponent<CharacterController>();
        _speedUnit = Vector3.forward.magnitude * Time.deltaTime;
        _gravity = Physics.gravity * Time.deltaTime;
    }

    private void Update() // IDK, does Update mean real-time?
    {
        if (Input.GetKeyDown(KeyCode.Space)) _isJump = true;
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
        PlayerMove();
    }

    /// <summary>
    /// Same as Update(), it will be executed once every frame, the difference is that it will wait for all Update() of scripts to be executed before executing.
    /// </summary>
    private void LateUpdate() // IDK, but looks like when calculating angle, we often use LateUpdate()
    {
        MouseMove();
    }


    private void PlayerMove()
    {
        Vector3 xValue = Input.GetAxis("Horizontal") * transform.right;
        Vector3 zValue = Input.GetAxis("Vertical") * transform.forward;
        Vector3 moveAmount = Time.deltaTime * (xValue + zValue);

        //characterChontroller.SimpleMove(moveAmount);
        var jumpVel = 0f;


        /*
         * 處理結果很糟，不適合使用在第一人稱上，跳起來後彷彿直接在空中等降落
         * **/
        if (_isJump) // 處理jumpVel
        {
            Debug.LogError("Jumping");
            jumpVel = jumpForce * Time.deltaTime;
            _isJump = false;
        }
        else
        {
            Ray rayToGround = new(transform.position, Vector3.down);
            if (Physics.Raycast(rayToGround, out _, 1f, LayerMask))
                jumpVel = 0f;
            else
                jumpVel *= _gravity.y;
        }

        moveAmount.y += jumpVel;
        characterChontroller.Move(_speedUnit * speed * moveAmount + _gravity);
        //Debug.LogError(_gravity);
    }

    private void Shoot()
    {
        Ray ray = new(CameraObj.position, CameraObj.forward);
        //int layerMask = 1 << LayerMask.NameToLayer("Target"); // bit manipulation with OR for calculating layer mask which layer has been checked
        bool isHit = Physics.Raycast(ray, out RaycastHit hit, 1000f);

        if (isHit && hit.collider.name.Contains("Target"))
        {
            hit.collider.gameObject.SetActive(false);
        }
    }

    private void MouseMove()
    {
        int offset = 50;
        float cameraHeight = 1.0f;

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
