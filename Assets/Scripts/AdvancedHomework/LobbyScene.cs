using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbyScene : MonoBehaviour
{
    Transform zombieCanvas;
    Transform capsuleCanvas;

    private void Start()
    {
        Main.GetSingleton.CursorController(SceneManager.GetActiveScene().buildIndex);

        zombieCanvas = GameObject.Find("Zombie1").transform.Find("Canvas").transform;
        capsuleCanvas = GameObject.Find("Enemy").transform.Find("Canvas").transform;
    }

    private void Update()
    {
        zombieCanvas.LookAt(Camera.main.transform);
        zombieCanvas.Rotate(0, 180, 0);
        capsuleCanvas.LookAt(Camera.main.transform);
        capsuleCanvas.Rotate(0, 180, 0);

        if (Input.GetKeyDown(KeyCode.Escape))
            Main.GetSingleton.ShowPauseMenu();
    }
}
