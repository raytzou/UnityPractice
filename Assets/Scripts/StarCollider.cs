using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarCollider : MonoBehaviour
{
    [SerializeField] AudioSource SFX;
    [SerializeField] ParticleSystem[] VFXArray;


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Player")
        {
            foreach (var vfx in VFXArray)
            {
                vfx.Play();
            }
            SFX.Play();
            gameObject.GetComponent<BoxCollider>().isTrigger = true;
            transform.Find("SoftStar").gameObject.SetActive(false);
        }
    }
}
