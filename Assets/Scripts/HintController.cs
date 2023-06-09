using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HintController : MonoBehaviour
{
    [SerializeField]
    TMP_Text tmp;
    void Awake()
    {
        tmp = GetComponent<TMP_Text>();
        StartCoroutine(FadeOutCoroutine());
    }

    private IEnumerator FadeOutCoroutine()
    {
        float duration = 2f; // 3 seconds
        float currentTime = 0f;

        while(currentTime < duration)
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
