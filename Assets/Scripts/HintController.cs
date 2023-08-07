using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HintController : MonoBehaviour
{
    [SerializeField]
    TMP_Text tmp;

    private int _currentScene = 0;

    void Start()
    {
        _currentScene = SceneManager.GetActiveScene().buildIndex;

        if (_currentScene == 3)
        {
            tmp = GetComponent<TMP_Text>();
            tmp.text = "Objects will be generated at center of map";
            StartCoroutine(FadeOutCoroutine());
        }
    }

    

    private void Update()
    {
        if (_currentScene < 2 && Main.GetSingleton.IsPortalEnable)
        {
            tmp.text = "Left Portal: Path Finding scene";
            tmp.text += "\nRight Portal: FPS + Object Pool showcase";
        }
    }

    private IEnumerator FadeOutCoroutine()
    {
        float duration = 3f; // 3 seconds
        float currentTime = 0f;

        while (currentTime < duration)
        {
            /// public static float Lerp(float a, float b, float t);
            /// Linearly interpolates between a and b by t.
            float textAlpha = Mathf.Lerp(1f, 0f, currentTime / duration);

            tmp.color = new Color(tmp.color.r, tmp.color.g, tmp.color.b, textAlpha);
            currentTime += Time.deltaTime;

            yield return null;
        }

        yield break;
    }
}
