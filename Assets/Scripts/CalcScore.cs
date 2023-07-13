using System;
using UnityEngine;

public class CalcScore : MonoBehaviour
{
    public int PlayerScore { get; set; } = 0;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.Contains("Star"))
        {
            PlayerScore++;
        }
    }
}
