using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * 使用空物件包膠囊，空物件為玩家
 * 所有Raycast()都必須使用含有LayerMask參數的版本，否則ray會打到膠囊或是玩家空物件本身
 * 原本單純使用transform控制鏡頭位置，改為使用follow target的方式
 * **/

public class MouseControl : MonoBehaviour
{
    private new Camera camera;

    [SerializeField] LayerMask LayerMask;

    [SerializeField] float moveSpeed = 1f;

/*    private float _cameraHeight;
    private float _cameraDeep; // camera 到 player 的 Z 軸深度*/

    private void Start()
    {
        camera = Camera.main;
        /*_cameraHeight = camera.transform.position.y;
        _cameraDeep = camera.transform.position.z - transform.position.z;*/
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Vector3 mousePosition = Input.mousePosition;

            Debug.LogError("mouse pos:" + mousePosition);

            Ray mouseRayLine = camera.ScreenPointToRay(mousePosition); // 滑鼠的位置當射線點，螢幕中心射向滑鼠點

            if (Physics.Raycast(mouseRayLine, out RaycastHit rayHit1, 1000f, LayerMask)) // 滑鼠點下是否在指定的LayerMask上
            {
                Vector3 direction = rayHit1.point - transform.position;

                // Vector3.magnitude = 向量到點的距離? (square root of (x*x + y*y + z*z))
                if (direction.magnitude < moveSpeed * Time.deltaTime)
                {
                    Debug.LogError("Mouse still hits under player object.");
                    transform.position = rayHit1.point;
                }
                else
                {
                    Debug.LogError("Proceed to MoveCharacter() function.");
                    direction.Normalize();
                    MoveCharacter(direction, Time.deltaTime * moveSpeed);
                }
            }
        }

        /*var newCameraPosition = new Vector3(transform.position.x, _cameraHeight, transform.position.z + _cameraDeep);
        camera.transform.position = Vector3.Lerp(camera.transform.position, newCameraPosition, 1f);*/
    }

    private void MoveCharacter(Vector3 direction, float moveAmount)
    {
        direction.y = 0f; // 單純平移，不考慮Y軸
        transform.forward = direction;

        Vector3 vectorToPoint = transform.position + direction * moveAmount; // 原本的位子 + (方向 * 移動量)
        Vector3 vectorPointUp = vectorToPoint; // 預備給角色上斜坡

        vectorPointUp.y += 1f;

        if (Physics.Raycast(vectorPointUp, -Vector3.up, out RaycastHit rayHit2, 1.2f, LayerMask)) // 利用raycast判斷角色是否能走到，須注意LayerMask
        {
            transform.position = rayHit2.point;
            Debug.LogError("move to: " + rayHit2.point);
            //Debug.LogError(rayHit2.collider.name);
        }
        else
            Debug.LogError("can't go to the point");
    }
}
