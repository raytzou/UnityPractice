using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PathFindingScene : MonoBehaviour
{
    private void Start()
    {
        Main.GetSingleton.CursorController(SceneManager.GetActiveScene().buildIndex);
    }
}