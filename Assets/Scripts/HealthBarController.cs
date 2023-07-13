using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarController : MonoBehaviour
{
    private void Update()
    {
        GetComponent<Slider>().value = GameObject.Find("Player").GetComponent<MyPlayerController>().PlayerHP;
    }
}
