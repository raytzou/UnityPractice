using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public void ResumeGame()
    {
        if(SceneManager.GetActiveScene().buildIndex != 2)
            Main.GetSingleton.HideCursor();
        else
            Main.GetSingleton.ShowCursor();

        Time.timeScale = 1f;
        GameObject.Find("UI").transform.Find("PauseMenu").gameObject.SetActive(false);
        Main.GetSingleton.IsGamePause = false;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
