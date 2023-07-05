/* 
 * Resource Pool Training, the pool is designed for Target
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetReusePool
{
    private static TargetReusePool _singleton;
    public static TargetReusePool GetSingleton { get => _singleton ??= new(); set => _singleton = value; }

    /// <summary>
    /// an internal class for saving Target status
    /// </summary>
    internal class TargetStatus
    {
        public GameObject theTarget; // store Target in List for reusing, make a reusing pool
        public bool isUsed = false; // is target used?
    }

    private List<TargetStatus> TargetList; // store Target's status 

    /// <summary>
    /// Pool, actually it's just a fooking List lol.
    /// </summary>
    public void InitTargetPool()
    {
        TargetList = new List<TargetStatus>();
        Debug.Log("Object Pool is initialized, use \"AddObject()\" for storing GameObject.");
    }

    public void AddObject(Object obj)
    {
        TargetStatus newObj = new()
        {
            theTarget = obj as GameObject,
            isUsed = true
        };

        TargetList.Add(newObj);
    }


}
