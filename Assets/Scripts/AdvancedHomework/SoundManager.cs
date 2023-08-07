using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource AudioSource;
    public AudioClip PortalSound;

    private bool _hasPlayed = false;

    private void Update()
    {
        if(Main.GetSingleton.IsPortalEnable && !_hasPlayed)
        {
            _hasPlayed = !_hasPlayed;
            AudioSource.PlayOneShot(PortalSound);
        }
    }
}
