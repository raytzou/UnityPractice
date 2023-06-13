using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public void ResumeGame()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1f;
        GameObject.Find("UI").transform.Find("PauseMenu").gameObject.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
