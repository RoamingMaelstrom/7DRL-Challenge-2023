using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemProfileContainer : MonoBehaviour
{
    public List<ItemProfile> itemProfiles = new List<ItemProfile>();

    public ItemProfile GetProfileWithID(int itemID)
    {
        foreach (var profile in itemProfiles)
        {
            if (profile.itemID == itemID) return profile;
        }
        return null;
    }
}
