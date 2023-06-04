using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarCollider : MonoBehaviour
{
    [SerializeField] AudioSource SFX;
    [SerializeField] ParticleSystem[] VFXArray;
    public int score;

    void Start()
    {
        VFXArray = GetComponents<ParticleSystem>();
        SFX = GetComponent<AudioSource>();
        score = 0;
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "unitychan")
        {
            foreach (var vfx in VFXArray)
            {
                vfx.Play();
            }
            SFX.Play();
            transform.Find("SoftStar").gameObject.SetActive(false);
            gameObject.GetComponent<BoxCollider>().isTrigger = true;
            score++;
            Debug.LogError(score);
        }
    }
}
