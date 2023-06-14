using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateObject : MonoBehaviour
{
    [SerializeField]
    Object Target; // Object任何素材皆可放(material、prefab...etc)

    [SerializeField]
    [Range(1f, 10f)]
    int TargetNum = 5;

    [SerializeField]
    [Range(1f, 10f)]
    float SpawnRange = 1.0f;

    private List<GameObject> _targetList;

    private void Start()
    {
        _targetList = new();
       
        for (int i = 0; i < TargetNum; i++)
        {
            Vector3 randomSpawnPoint = new(Random.Range(-1f, 1f), Random.Range(0, 1f), Random.Range(-1f, 1f));
            GameObject target = Instantiate(Target) as GameObject;

            if (randomSpawnPoint.magnitude < 0.001f)
                randomSpawnPoint.x = 1f;

            randomSpawnPoint.Normalize();
            target.transform.position = randomSpawnPoint * SpawnRange + transform.position; // transform.position => 以Object所在位置生成
            _targetList.Add(target);
        }
    }
}
