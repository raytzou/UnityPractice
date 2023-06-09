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
    bool IsJump = false;


    void Update() // IDK real-time here?
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            IsJump = true;

        }

        /*Input.GetAxis("Vertical");
        Input.GetAxis("Horizontal");*/

        if(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            transform.position += speed * Time.deltaTime * Vector3.forward;
        }
        
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            transform.position += speed * Time.deltaTime * Vector3.left;
        }

        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            transform.position += speed * Time.deltaTime * Vector3.right;
        }

        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            transform.position += speed * Time.deltaTime * Vector3.back;
        }
    }

    void FixedUpdate() // 物理運算會於此執行
    {
        //rigidBody.velocity = Vector3.zero;

        if(IsJump)
        {
            
            rigidBody.AddForce(Vector3.up * 100, ForceMode.Acceleration);
            //transform.position += Vector3.up * 100 * Time.deltaTime;
            IsJump = false; // 跳完直接切換
        }
        
        
    }
}
