using TMPro;
using UnityEngine;


public class ScoreUI : MonoBehaviour
{
    [SerializeField] TMP_Text scoreUI;
    private int _score = 0;
    int starCounts;

    private void Start()
    {
        starCounts = GameObject.FindGameObjectsWithTag("Star").Length;
    }

    private void Update()
    {
        _score = GameObject.Find("Player").GetComponent<CalcScore>().PlayerScore;
        scoreUI.text = ("Score: " + _score);

        if(_score == starCounts)
        {
            scoreUI.text = "All Stars have been found!";
        }
    }
}
