using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SOEvents;

public class ObjectPoolMain : MonoBehaviour
{
    [Tooltip("Used to return valid GameObjects to their (Object)pool without accessing an ObjectPoolMain class instance directly.")]
    [SerializeField] List<GameObjectFloatSOEvent> returnToPoolFloatEvents = new List<GameObjectFloatSOEvent>();
    [Tooltip("Used to return valid GameObjects to their (Object)pool without accessing an ObjectPoolMain class instance directly.")]
    [SerializeField] List<GameObjectSOEvent> returnToPoolEvents = new List<GameObjectSOEvent>();
    [SerializeField] List<Pool> objectPools;

    // Guides the rate at which the pool will fill up. Note: Called every FixedUpdate, but the value is divided by 50 and then cast as an int.
    [SerializeField] int maxPoolFillRate = 50;

    // Determines whether pools that are not full will have new Objects added to them. 
    bool allowNewGameObjectInstantiation = true;


    private void Awake() 
    { 
        AssignPoolIDs();
        PopulatePoolsOnAwake();

        foreach (var returnToPoolEvent in returnToPoolFloatEvents) returnToPoolEvent.AddListener(ReturnObject);
        foreach (var returnToPoolEvent in returnToPoolEvents) returnToPoolEvent.AddListener(ReturnObject);    
    }

    // Starts process of filling up pools to instance Pool.suggestedPoolSize.
    private void Start() 
    {
        StartCoroutine(PopulatePools());
    }

    public void EnableNewGameObjectInstantiation() => allowNewGameObjectInstantiation = true;
    public void DisableNewGameObjectInstantiation() => allowNewGameObjectInstantiation = false;

    // Assigns each pool a unique ID, starting at 0. Based on the pools position in objectPools variable.
    private void AssignPoolIDs()
    {
        for (int i = 0; i < objectPools.Count; i++)
        {
            objectPools[i].poolID = i;
        }
    }


    public void TestGetObject()
    {
        GameObject newObject = GetObject(0);
        newObject.transform.position = new Vector3(Random.Range(-10f, 10f), Random.Range(-10f, 10f), 0);
    }


    // Part of class API. Gets an object from one of the pools under this class's management.
    // (Name assigned in Unity Inspector and stored in Pool class)
    public GameObject GetObject(string poolName)
    {
        foreach (var pool in objectPools)
        {
            if (pool.poolName == poolName) return GetObject(pool);
        }
        // Called if cannot find pool with provided name.
        throw new System.Exception(string.Format("CUSTOM ERROR: Invalid pool name provided (Could not find Pool with poolName = {0})", poolName));
    }

    // Part of class API. Gets an object from one of the pools under this class's management.
    // (ID is assigned by ObjectPoolMain.AssignPoolIDs).
    public GameObject GetObject(int poolID)
    {

    foreach (var pool in objectPools)
        {
            if (pool.poolID == poolID) return GetObject(pool);
        }

        // Called if cannot find pool with provided ID.
        throw new System.Exception(string.Format("CUSTOM ERROR: Invalid pool ID provided (Could not find Pool with poolID = {0})", poolID));
    }

    // Used internally by ObjectPoolMain to Get an object from a specified Pool.
    /* Getobject process works as follows:
        - If there IS NOT an object free, return the Object that has been active for the longest.
        - If there IS an object free, pop it off the Free Object stack and return it.
        - Object will always be SetActive on retrieval.
    */
    GameObject GetObject(Pool pool)
    {
        GameObject objectToGet;

        if (pool.objectsFree.Count == 0) 
        {
            objectToGet = pool.objectsInUse[0];
            pool.objectsInUse.RemoveAt(0);
        }
        else objectToGet = pool.objectsFree.Pop();

        
        objectToGet.SetActive(true);
        pool.objectsInUse.Add(objectToGet);

        return objectToGet;
    }



    // Part of class API. 
    // Returns the number of Objects in use for a pool with provied poolName. Returns -1 if cannot find pool.
    public int GetNumberOfObjectsInUse(string poolName)
    {
        foreach (var pool in objectPools)
        {
            if (pool.poolName == poolName) return pool.objectsInUse.Count;
        }
        return -1;
    }

    // Part of class API. 
    // Returns the number of Objects in use for a pool with provied poolID. Returns -1 if cannot find pool.
    public int GetNumberOfObjectsInUse(int poolID)
    {
        foreach (var pool in objectPools)
        {
            if (pool.poolID == poolID) return pool.objectsInUse.Count;
        }
        return -1;
    }


    public void ReturnObject(GameObject objectToReturn, float arg0)
    {
        ReturnObject(objectToReturn);
    }


    // Part of class API. Returns objects to one of the pools under this class's management.
    // arg1 included to ensure compatability with DespawnEvent.
    public void ReturnObject(GameObject objectToReturn)
    {
        int poolID = objectToReturn.GetComponent<ObjectIdentifier>().poolID;
        foreach (var pool in objectPools)
        {
            if (pool.poolID == poolID) 
            {
                ReturnObject(objectToReturn, pool);
                return;
            }
        }
        // throw exception if cannot find poolID in objectPools that matches poolID in Object ObjectIdentifier instance.
        throw new System.Exception(string.Format("CUSTOM ERROR: Invalid pool ID provided by parameter GameObject (Could not find Pool with poolID = {0})",
         poolID));
    }

    // Used internally by ObjectPoolMain to return an object to its pool.
    // Uses naive approach of looping through each object in the pool and checking if its InstanceID matches.
    // Objects are deactivated (SetActive(false) when returned to their pool.
    // If the object could not be found in the pool, no actions are performed on it.
    void ReturnObject(GameObject objectToReturn, Pool pool)
    {
        foreach (var item in pool.objectsInUse)
        {
            if (item.GetInstanceID() == objectToReturn.GetInstanceID()) 
            {
                pool.objectsFree.Push(objectToReturn);
                pool.objectsInUse.Remove(objectToReturn);
                objectToReturn.SetActive(false);
                return;
            }
        }
    }



    // Call on awake to ensure there are always some objects in the pool if some other objects requests one.
    // Defaults to providing either Max(10, Min(500 / number of pools, or 2% of the pools suggestedPoolSize variable)).
    void PopulatePoolsOnAwake(){
        for (int i = 0; i < objectPools.Count; i++)
        {
            int minFillAtStart = Mathf.Min(10, objectPools[i].maxPoolSize);
            int suggestedFillAtStart = Mathf.Min(50, (int)(objectPools[i].maxPoolSize / 5));
            int maxFillAtStart = Mathf.Max(100, objectPools[i].maxPoolSize);

            for (int j = 0; j < Mathf.Clamp(suggestedFillAtStart, minFillAtStart, maxFillAtStart); j++)
            {
                CreatePoolObject(objectPools[i]);
            }
        }
    }

    // Used to do the majority of ObjectPool Filling. 
    // Will constantly run until all pools are filled up.
    IEnumerator PopulatePools()
    {
        // Initial setup of variables used in method.
        List<float> poolFillPercentages = new List<float>();
        foreach (var pool in objectPools)
        {
            poolFillPercentages.Add(0);
        }

        float minFill;
        int leastFullPoolIndex = 0;
        float tempFill = 0;

        GameObject currentObject;

        // Calls every Max(physics update (default 0.02s) OR 1 / maxPoolFillRate)
        while (allowNewGameObjectInstantiation)
        {
            minFill = 1;
            // Find the pool index which is the lowest % full.
            for (int i = 0; i < objectPools.Count; i++)
            {
                tempFill = (float)(objectPools[i].objectsFree.Count + objectPools[i].objectsInUse.Count) / (float)objectPools[i].maxPoolSize;
                if (tempFill < minFill)
                {
                    leastFullPoolIndex = i;
                    minFill = tempFill;
                }
            }

            // Instantiate more objects if pool is not full.
            if (minFill < 1) 
            {
                for (int i = 0; i < Mathf.Max(1, (int) maxPoolFillRate / 50.0f); i++)
                {
                    currentObject = CreatePoolObject(objectPools[leastFullPoolIndex]);
                }
            }

            yield return new WaitForSeconds(Mathf.Max(1 / maxPoolFillRate, Time.fixedDeltaTime));
        }

        yield return null;
    }



    // Instantiates an instance of an object pools prefab, 
    // Adds an ObjectIdentifier Component to the new Object (with Pool information data), disables it, and then assigns it to the pool.objectsFree.
    GameObject CreatePoolObject(Pool pool)
    {
        GameObject newObject = Instantiate(pool.objectPrefab, transform);
        if (!newObject.TryGetComponent(out ObjectIdentifier identity)) identity = newObject.AddComponent<ObjectIdentifier>();

        identity.poolID = pool.poolID;
        identity.poolName = pool.poolName;
        
        newObject.gameObject.SetActive(false);
        pool.objectsFree.Push(newObject);
        return newObject;
    }

}
