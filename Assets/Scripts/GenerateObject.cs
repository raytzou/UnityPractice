using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateObject : MonoBehaviour
{
    [SerializeField]
    Object Target; // Object任何素材皆可放(material、prefab...etc)

    /// <summary>
    /// The maximum for resource pool
    /// </summary>
    [SerializeField]
    [Range(1f, 100f)]
    int SpawnMaximum = 5;

    [SerializeField]
    [Range(1f, 100f)]
    float SpawnRange = 1.0f;

    /// <summary>
    /// How many seconds for spawning a target.
    /// </summary>
    [SerializeField]
    [Range(10, 120)]
    int SpawnTimer = 60;

    /// <summary>
    /// How many objects you want to spawn at a time
    /// </summary>
    [SerializeField]
    [Range(1, 10)]
    int SpawnNumber = 3;

    private float _timeInterval = 0f;

    private void Start()
    {
        ObjectReusePool.GetSingleton.InitObjectPool(Target, SpawnMaximum);
    }

    private void Update()
    {
        if (!ObjectReusePool.GetSingleton.AllActived)
        {
            _timeInterval += Time.deltaTime;
            if(_timeInterval >= (SpawnTimer * 1.0))
            {
                ObjectReusePool.GetSingleton.SpawnObject(SpawnRange, SpawnNumber, transform.position);
                _timeInterval = 0.0f;
            }
        }
    }
}
