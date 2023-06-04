using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class SphereScript : MonoBehaviour
{
    [SerializeField]
    float speed = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var moveX = new Vector3(speed * Time.deltaTime, 0, 0);
        var moveZ = new Vector3(0, 0, speed * Time.deltaTime);
        var randomY = new Vector3(0, Random.Range(-10f, 10f) * speed * Time.deltaTime, 0);

        transform.position += (moveX + moveZ + randomY);
        
    }
}
