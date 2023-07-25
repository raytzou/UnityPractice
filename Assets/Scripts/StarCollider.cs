using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarCollider : MonoBehaviour
{
    AudioSource SFX;
    [SerializeField] ParticleSystem[] VFXArray;


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Player")
        {
            SFX = other.gameObject.GetComponent<AudioSource>();
            SFX.Play();

            foreach (var vfx in VFXArray)
                vfx.Play();

            var hp = other.gameObject.GetComponent<MyPlayerController>().PlayerHP;
           
            if (hp + .5f >= 1.0000f) other.gameObject.GetComponent<MyPlayerController>().PlayerHP = 1.0000f;
            else other.gameObject.GetComponent<MyPlayerController>().PlayerHP += .5f;

            gameObject.SetActive(false);
        }
    }
}
