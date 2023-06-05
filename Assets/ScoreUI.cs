using TMPro;
using UnityEngine;


public class ScoreUI : MonoBehaviour
{
    [SerializeField] TMP_Text scoreUI;
    private int score;

    private void Start()
    {
        score = 0;
    }
    private void Update()
    {
        score = GameObject.Find("unitychan").GetComponent<CalcScore>().score;
        scoreUI.text = ("Score: " + score);
    }
}
