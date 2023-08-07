using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Main
{
    private static Main _singleton = null;
    public static Main GetSingleton { get => _singleton ??= new Main(); set => _singleton = value; }
    
    private bool _portalEnable = false;
    public bool IsPortalEnable { get { return _portalEnable; } }

    public bool IsGamePause { get; set; } = false;

    public void CursorController(int sceneIndex)
    {
        Cursor.visible = sceneIndex switch
        {
            1 or 3 => false,
            _ => true,
        };
        Cursor.lockState = sceneIndex switch
        {
            1 or 3 => CursorLockMode.Locked,
            _ => CursorLockMode.None,
        };
    }

    public void HideCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void ShowCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ShowPauseMenu()
    {
        IsGamePause = true;
        ShowCursor();
        Time.timeScale = 0f; // zawarudo VIP version
        GameObject.Find("UI").transform.Find("PauseMenu").gameObject.SetActive(true);
    }

    public void EnablePortal(GameObject buttonObj)
    {
        if (_portalEnable) return;

        _portalEnable = true;
        var button = buttonObj.transform.Find("Button");
        var newButtonPos = button.transform.position;

        newButtonPos.y -= 3f;
        button.transform.position = Vector3.Lerp(button.transform.position, newButtonPos, 0.01f);

        GameObject[] portals = GameObject.FindGameObjectsWithTag("Level Portal");

        foreach(var obj in portals)
        {
            ParticleSystem portal = obj.GetComponent<ParticleSystem>();
            portal.Play();
        }
    }
}
