using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Pool
{
    // The unique ID of the pool
    public int poolID;
    public string poolName;
    public GameObject objectPrefab;
    public int maxPoolSize;

    public int currentPoolSize 
    {
        get {return objectsFree.Count + objectsInUse.Count;} 
        private set {}
    }
    public Stack<GameObject> objectsFree = new Stack<GameObject>();
    public List<GameObject> objectsInUse = new List<GameObject>();
}



