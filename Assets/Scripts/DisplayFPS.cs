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
        var playerObject = GameObject.Find("unitychan");
        int currentFps = (int)(1f / Time.unscaledDeltaTime);
        float positionX = playerObject.transform.position.x;
        float positionY = playerObject.transform.position.y;
        float positionZ = playerObject.transform.position.z;
        

        UI.text = currentFps.ToString() + " FPS";
        UI.text += "   / " + positionX.ToString();
        UI.text += " / " + positionY.ToString();
        UI.text += " / " + positionZ.ToString();
    }
}
