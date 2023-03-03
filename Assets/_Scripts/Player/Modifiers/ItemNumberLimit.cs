using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SOEvents;
using System;

public class ItemNumberLimit : MonoBehaviour
{
    [SerializeField] IntSOEvent addItemToPlayerEvent;
    
    [SerializeField] StringSOEvent maxItemsOfTypePurchasedEvent;
    [SerializeField] ObjectTypeIdRanges objectTypeIdRanges;
    public int maxWeapons = 6;
    public int maxUtilities = 6;

    public int currentWeapons = 0;
    public int currentUtilities = 0;

    List<int> registeredItems = new List<int>();

    private void Awake() {
        addItemToPlayerEvent.AddListener(RunUpdate);
    }

    private void RunUpdate(int newItemID)
    {
        foreach (var itemID in registeredItems)
        {
            if (itemID == newItemID) return;
        }

        if (objectTypeIdRanges.CheckIdInGroupRange(newItemID, "Weapons"))
        {
            currentWeapons ++;
        }
        else if (objectTypeIdRanges.CheckIdInGroupRange(newItemID, "Utilities"))
        {
            currentUtilities ++;
        }
        else throw new System.Exception(string.Format("Did not recognise {0}. Is it not a Weapon or Utility?", newItemID));

        registeredItems.Add(newItemID);
        string invokeValue = "";
        if (currentWeapons >= maxWeapons) invokeValue += "Weapon";
        else if (currentUtilities >= maxUtilities) invokeValue += "Utility";
        if (invokeValue.Length > 0) maxItemsOfTypePurchasedEvent.Invoke(invokeValue);
    }
}
