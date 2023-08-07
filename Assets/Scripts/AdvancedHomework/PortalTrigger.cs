using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (!Main.GetSingleton.IsPortalEnable) return;

        if (other.CompareTag("Player"))
        {
            var trigger = transform.parent.name;
            switch (trigger)
            {
                case "Portal (1)":
                    SceneManager.LoadScene(2);
                    break;
                case "Portal":
                    SceneManager.LoadScene(3);
                    break;
            }
        }
    }
}
