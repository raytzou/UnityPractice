using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class ScoreUI : MonoBehaviour
{
    [SerializeField] TextMeshPro scoreUI;
    private int score;

    private void Start()
    {
        score = 0;
    }
    private void Update()
    {
        //scoreUI.text
    }
}
