using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    [SerializeField] int targetHP = 100;

    Color originalEnemyColor;

    private float _timeInterval = 0f;

    private void Start()
    {
        originalEnemyColor = GetComponent<Renderer>().material.color;
    }

    private void Update()
    {
        if (GetComponent<Renderer>().material.color != originalEnemyColor)
        {
            _timeInterval += Time.deltaTime;

            if(_timeInterval > 0.1f)
                GetComponent<Renderer>().material.color = originalEnemyColor;
        }
    }

    public void CalcDamage()
    {
        GetComponent<Renderer>().material.color = new Color(1f, 0f, 0f);

        targetHP -= 50;
        Debug.LogError("HP: " + targetHP);
        if(targetHP <= 0)
        {
            Destroy(gameObject);
        }
    }
}
