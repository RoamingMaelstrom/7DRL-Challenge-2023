using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectTypeIdRanges : MonoBehaviour
{
    public List<ObjectIdRange> objectGroups = new List<ObjectIdRange>();

    public bool CheckIdInGroupRange(int itemID, string objectGroupName)
    {
        foreach (var group in objectGroups)
        {
            if (group.typeName != objectGroupName) continue;

            if (itemID >= group.minIndex && itemID < group.maxIndex) return true;
            return false;
        }
        return false;
    }
}

[System.Serializable]
public class ObjectIdRange
{
    public string typeName;
    public int minIndex;
    // Exclusive
    public int maxIndex;
}
