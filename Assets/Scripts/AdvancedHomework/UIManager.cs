using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;
using static Unity.Burst.Intrinsics.X86.Avx;

public class UIManager : MonoBehaviour
{
    [SerializeField] Text FPS; // Legacy UI Text
    [SerializeField] TMP_Text Hint;
    [SerializeField] TMP_Text Score;
    [SerializeField] GameObject HealthBar;

    private int _starCounter = 0;
    private int _score = 0;

    private bool[] _enables = new bool[3];

    private CanvasGroup _canvasGroup;

    private void Start()
    {
        DontDestroyOnLoad(this);
        _canvasGroup = GetComponent<CanvasGroup>();
        _canvasGroup.alpha = 0f;
    }

    private void Update()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        
        if (_enables[0]) // score
            UpdateScore();

        if (_enables[1]) // health bar
            UpdateHealthBar();

        if (_enables[2]) // fps
            DisplayFPS(SceneManager.GetActiveScene().buildIndex);
    }

    private void OnSceneLoaded(Scene currentScene, LoadSceneMode sceneMode)
    {
        _canvasGroup.alpha = 1f;
        
        switch (currentScene.buildIndex)
        {
            case 1: // Lobby (enemy tracing + UI showcase)
                _starCounter = GameObject.FindGameObjectsWithTag("Star").Length;
                if (!Score.enabled) Score.enabled = true;
                if (!HealthBar.active) HealthBar.SetActive(true);
                if (!FPS.enabled) FPS.enabled = true;
                _enables[0] = true;
                _enables[1] = true;
                _enables[2] = true;
                break;
            case 2: // Path Finding (A* and object seek)
                if (Score.enabled) Score.enabled = false;
                if (HealthBar.active) HealthBar.SetActive(false);
                _enables[0] = false;
                _enables[1] = false;
                Destroy(Hint);
                break;
            case 3: // Object Pool (just List does object recycle)
                Hint.text = "Objects will be generated at center of map";
                StartCoroutine(FadeOutHint());
                if (Score.enabled) Score.enabled = false;
                if (HealthBar.active) HealthBar.SetActive(false);
                if (!FPS.enabled) FPS.enabled = true;
                _enables[0] = false;
                _enables[1] = false;
                _enables[2] = true;
                break;
            default:
                break;
        }
    }

    private void DisplayFPS(int sceneIndex)
    {
        GameObject playerObject;

        if(sceneIndex == 2) // path finding scene
            playerObject = GameObject.Find("Character");
        else
            playerObject = GameObject.Find("Player");

        int currentFps = (int)(1f / Time.unscaledDeltaTime);
        float positionX = Mathf.Round(playerObject.transform.position.x);
        float positionY = Mathf.Round(playerObject.transform.position.y);
        float positionZ = Mathf.Round(playerObject.transform.position.z);


        FPS.text = currentFps.ToString() + " FPS";
        FPS.text += "   X: " + positionX;
        FPS.text += " / Y: " + positionY;
        FPS.text += " / Z: " + positionZ;
    }

    private void UpdateScore()
    {
        if (_score == _starCounter)
        {
            Score.text = "All Stars have been found!";
            return;
        }

        _score = GameObject.Find("Player").GetComponent<CalcScore>().PlayerScore;
        Score.text = ("Score: " + _score);
    }

    private void UpdateHealthBar()
    {
        HealthBar.GetComponent<Slider>().value = GameObject.Find("Player").GetComponent<TPSControlTraining>().PlayerHP;
    }

    private IEnumerator FadeOutHint()
    {
        float duration = 3f; // 3 seconds
        float currentTime = 0f;

        while (currentTime < duration)
        {
            /// public static float Lerp(float a, float b, float t);
            /// Linearly interpolates between a and b by t.
            float textAlpha = Mathf.Lerp(1f, 0f, currentTime / duration);
            Hint.color = new Color(Hint.color.r, Hint.color.g, Hint.color.b, textAlpha);
            currentTime += Time.deltaTime;
            yield return null;
        }

        yield break;
    }
}
