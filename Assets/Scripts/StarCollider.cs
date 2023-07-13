using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarCollider : MonoBehaviour
{
    [SerializeField] AudioSource SFX;
    [SerializeField] ParticleSystem[] VFXArray;


    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "Player")
        {
            transform.Find("SoftStar").gameObject.SetActive(false);
            foreach (var vfx in VFXArray)
            {
                vfx.Play();
            }
            SFX.Play();
            gameObject.GetComponent<SphereCollider>().isTrigger = false;

            var hp = other.gameObject.GetComponent<MyPlayerController>().PlayerHP;
           
            if (hp + .5f >= 1.0000f) other.gameObject.GetComponent<MyPlayerController>().PlayerHP = 1.0000f;
            else other.gameObject.GetComponent<MyPlayerController>().PlayerHP += .5f;
        }
    }
}
