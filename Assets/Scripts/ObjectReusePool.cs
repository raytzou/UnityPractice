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

    private bool _allActived = false;
    public bool AllActived { get => _allActived; }

    private List<ObjectStatus> _objectList; // store Object's status
    private string _objectName;

    /// <summary>
    /// Pool, actually it's just a fooking List lol.
    /// </summary>
    public void InitObjectPool(Object obj, int spawnTimes)
    {
        _objectList = new List<ObjectStatus>();
        _objectName = obj.name;
        
        for(int i = 0; i < spawnTimes; i++)
        {
            GameObject target = GameObject.Instantiate(obj) as GameObject;
            target.name = target.name.Replace("(Clone)", "") + i;
            _objectList.Add(new ObjectStatus
            {
                theObject = target,
                isUsing = false,
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
        var unusedObjectIndex = SearchUnusedObjectIndex();

        if (unusedObjectIndex != -1 && !_objectList[unusedObjectIndex].isUsing)
        {
            _objectList[unusedObjectIndex].isUsing = true;
            _objectList[unusedObjectIndex].theObject.SetActive(true);
            _allActived = false;
        }
        else
        {
            Debug.Log("All objects are active.");
            _allActived = true;
            return;
        }

        _objectList[unusedObjectIndex].theObject.transform.position = CalcRandomSpawnPoint(spawnRange, spawnPoint);
    }

    public void DisableObject(string targetName)
    {
        string token = targetName.Replace(_objectName, "");
        int targetIndex = int.Parse(token);

        //Debug.LogError("targetIndex: " + targetIndex);
        _objectList[targetIndex].isUsing = false;
        _objectList[targetIndex].theObject.SetActive(false);
        _allActived = false;
    }

    private int SearchUnusedObjectIndex()
    {
        for(int i = 0; i < _objectList.Count; i++)
            if (!_objectList[i].isUsing) return i;

        return -1;
    }

    private Vector3 CalcRandomSpawnPoint(float spawnRange, Vector3 spawnPoint)
    {
        Vector3 randomSpawnPoint = new(Random.Range(-1f, 1f), Random.Range(0, 1f), Random.Range(-1f, 1f));

        if (randomSpawnPoint.magnitude < 0.001f)
            randomSpawnPoint.x = 1f;

        randomSpawnPoint.Normalize();

        return randomSpawnPoint * spawnRange + spawnPoint;
    }
}
