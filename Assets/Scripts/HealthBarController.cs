using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HealthBarController : MonoBehaviour
{
    private void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene currentScene, LoadSceneMode sceneMode)
    {
        if (currentScene.buildIndex != 1) Destroy(gameObject);
    }

    private void Update()
    {
        GetComponent<Slider>().value = GameObject.Find("Player").GetComponent<TPSControlTraining>().PlayerHP;
    }
}
