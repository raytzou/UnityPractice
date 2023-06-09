using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyPlayerController : MonoBehaviour
{
    [SerializeField]
    Rigidbody rigidBody;

    [SerializeField]
    [Range(0, 200)]
    float speed = 10f;

    [SerializeField]
    float jumpForce = 200f;

    Camera camera;

    private bool IsJump = false;

    private void Start()
    {
        camera = GetComponent<Camera>();
    }

    private void Update() // IDK, does Update mean real-time here?
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            IsJump = true;
        }

        float xValue = Input.GetAxis("Horizontal");
        float zValue = Input.GetAxis("Vertical");
        transform.Translate(xValue * speed * Time.deltaTime, 0f, zValue * speed * Time.deltaTime);
    }

    private void FixedUpdate() // 物理運算會於此執行
    {
        //rigidBody.velocity = Vector3.zero;

        if(IsJump)
        {
            rigidBody.AddForce(Vector3.up * jumpForce, ForceMode.Acceleration);
            IsJump = false; // 跳完直接切換
        }
    }
}
