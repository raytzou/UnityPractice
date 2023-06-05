using System;
using UnityEngine;

public class CalcScore : MonoBehaviour
{
    [NonSerialized] public int score = 0;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name.Contains("Star"))
        {
            score++;
        }
    }
}
