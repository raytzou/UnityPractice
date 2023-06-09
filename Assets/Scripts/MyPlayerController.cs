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
    //[Range(250, 500)]
    float speed = 250f;

    [SerializeField]
    [Range(1, 20)]
    float mouseSpeed = 3f;

    private bool IsJump = false;
    private bool IsEnableToJump = false;
    private float mouseYaw = 0f, mousePitch = 0f;

    //[NonSerialized]
    public float PlayerHP { get; set; } = 0.3f;

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
        if (Input.GetKeyDown(KeyCode.Mouse0)) Shoot();

        PlayerEdgeProtection();
    }

    /// <summary>
    /// FixedUpdate() is executed at regular time intervals.
    /// </summary>
    private void FixedUpdate() // 物理運算於此運算似乎較精準?
    {
        if (IsJump)
        {
            rigidBody.AddForce(Vector3.up * speed, ForceMode.Acceleration);
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

    private void PlayerMove()
    {
        //Debug.Log(transform.position);
        Ray ray = new(transform.position, -transform.up);

        if(Physics.Raycast(ray, out RaycastHit hitInfo, 1.6f))
        {
            IsEnableToJump = true;
        }
        else
        {
            IsEnableToJump = false;
        }

        float velY = rigidBody.velocity.y; // store original velocity.y
        Vector3 xValue = Input.GetAxis("Horizontal") * transform.right;
        Vector3 zValue = Input.GetAxis("Vertical") * transform.forward;
        Vector3 moveAmount = speed * Time.deltaTime * (xValue + zValue);

        moveAmount.y = velY;
        rigidBody.velocity = moveAmount;
    }

    private void Shoot()
    {
        Ray ray = new(CameraObj.position, CameraObj.forward);
        //int layerMask = 1 << LayerMask.NameToLayer("Ground"); // bit manipulation with OR for calculating layer mask which layer has been checked

        if (Physics.Raycast(ray, out RaycastHit hit, 100f))
        {
            Debug.Log("hit: " + hit.collider.name);
            if (hit.collider.name.Contains("Target"))
            {
                //Destroy(hit.collider.gameObject);
                hit.collider.gameObject.SendMessage("CalcDamage");
                //Enemy.EnemySingleton().CalcDamage();
            }
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
