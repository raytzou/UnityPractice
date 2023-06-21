using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main
{
    /// <summary>
    /// Private instance
    /// </summary>
    private static Main Instance { get; set; }

    /// <summary>
    /// My Singleton
    /// </summary>
    public static Main Singleton
    {
        /// Unity suggests we should not use null coalescing operator e.g., "is null", "is not null", "??", "??=".... etc,
        /// Unity also suggests we should not new our own object in MonoBehavior(inherited) lol...
        get => Instance ??= new();
        //get { return Instance; } // return null all the time.
        set => Instance = value;
    }

    /*private void Awake()
    {
        /// If we new object by myself, then "this" in Awake() is probably useless?
        /// Decide to not inherit MonoBehavior, thus the Awake() is not really needed
        Instance = this;
    }*/

    public void Init()
    {
        Debug.LogError("My singleton");
    }

    /// <summary>
    /// Example for loading resource
    /// </summary>
    public void LoadResourcesTest()
    {
        var obj = Resources.Load("Target");
        Debug.Log("Resource load: " + obj.name + " type: " + obj.GetType());
    }

    /// <summary>
    /// Find any resource under Resource folder, return its GameObject if it has been found, otherwise return new GameObject.
    /// </summary>
    /// <param name="fileName">File name or path of resource.</param>
    /// <returns></returns>
    public GameObject FindResource(string fileName)
    {
        if(string.IsNullOrEmpty(fileName))
        {
            Debug.LogError("file name is empty or null.");
            return new GameObject();
        }

        var result = Resources.Load(fileName) as GameObject;
        
        if (result == null)
        {
            Debug.LogError("Cannot find: " + fileName);
            return new GameObject();
        }

        return result;
    }
}
