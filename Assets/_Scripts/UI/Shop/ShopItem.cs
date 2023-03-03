using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ShopItem
{
    public int itemID;
    public ItemType itemType;
    public string itemName;
    public string itemDescription;
    public int cost;
    public bool isUpgrade;
    public UpgradeInfo upgradeInfo;
}
