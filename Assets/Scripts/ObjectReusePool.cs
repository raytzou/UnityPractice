/* 
 * Resource Pool Training, the pool is designed for Object
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectReusePool
{

    private static ObjectReusePool _singleton;
    public static ObjectReusePool GetSingleton { get => _singleton ??= new(); set => _singleton = value; }

    /// <summary>
    /// a class for saving Object status
    /// </summary>
    public class ObjectStatus
    {
        public GameObject theObject; // store Object in List for reusing, make a reusing pool
        public bool isUsing = false; // is object using?
    }

    private List<ObjectStatus> _objectList; // store Object's status

    /// <summary>
    /// Pool, actually it's just a fooking List lol.
    /// </summary>
    public void InitObjectPool(Object obj, int spawnTimes)
    {
        _objectList = new List<ObjectStatus>();
        
        for(int i = 0; i < spawnTimes; i++)
        {
            _objectList.Add(new ObjectStatus
            {
                theObject = obj as GameObject,
                isUsing = false
            });

            _objectList[i].theObject.SetActive(false);
        }

        if (_objectList.Count == spawnTimes)
        {
            foreach(var status in _objectList)
            {
                Debug.Log("Object Name: " + status.theObject.name + ", status: " + status.isUsing);
            }
            Debug.Log("Initialize Object List successfully.");
        }
        else
            Debug.LogError("Failed to initialize Object List.");
    }

    /// <summary>
    /// Spawn object
    /// </summary>
    /// <param name="spawnRange">float: how far the object can be</param>
    /// <param name="spawnPoint">Vector3: the position of spawn point</param>
    public void SpawnObject(float spawnRange, Vector3 spawnPoint)
    {
        //Debug.LogWarning($"spawn point: {spawnPoint}");
        /*for (int i = 0; i < TargetNum; i++)
        {
            Vector3 randomSpawnPoint = new(Random.Range(-1f, 1f), Random.Range(0, 1f), Random.Range(-1f, 1f));
            GameObject target = Instantiate(Target) as GameObject;

            if (randomSpawnPoint.magnitude < 0.001f)
                randomSpawnPoint.x = 1f;

            randomSpawnPoint.Normalize();
            target.transform.position = randomSpawnPoint * SpawnRange + transform.position; // transform.position => 以Object所在位置生成
            //TargetReusePool.GetSingleton..Add(target);
        }*/
        GameObject obj = new();
        var unusedObjectIndex = SearchUnusedObjectIndex();

        if (unusedObjectIndex != -1 && !_objectList[unusedObjectIndex].isUsing)
        {
            
            _objectList[unusedObjectIndex].isUsing = true;
            _objectList[unusedObjectIndex].theObject.SetActive(true);
        }
        else
            Debug.Log("All objects are active.");

        Vector3 randomSpawnPoint = new(Random.Range(-1f, 1f), Random.Range(0, 1f), Random.Range(-1f, 1f));

        if (randomSpawnPoint.magnitude < 0.001f)
            randomSpawnPoint.x = 1f;

        randomSpawnPoint.Normalize();
        obj.transform.position = randomSpawnPoint * spawnRange + spawnPoint; // transform.position => 以Object所在位置生成
    }

    public void DisableObject(GameObject gameObject)
    {
        GameObject.Destroy(gameObject);
    }

    private int SearchUnusedObjectIndex()
    {
        for(int i = 0; i < _objectList.Count; i++)
            if (!_objectList[i].isUsing) return i;

        return -1;
    }
}
