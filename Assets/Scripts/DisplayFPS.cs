using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DisplayFPS : MonoBehaviour
{
    [SerializeField] Text UI;
    

    void Update()
    {
        var playerObject = GameObject.Find("Player");
        int currentFps = (int)(1f / Time.unscaledDeltaTime);
        float positionX = Mathf.Round(playerObject.transform.position.x);
        float positionY = Mathf.Round(playerObject.transform.position.y);
        float positionZ = Mathf.Round(playerObject.transform.position.z);
        

        UI.text = currentFps.ToString() + " FPS";
        UI.text += "   X: " + positionX;
        UI.text += " / Y: " + positionY;
        UI.text += " / Z: " + positionZ;
    }
}
