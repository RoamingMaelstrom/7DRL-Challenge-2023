using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IItem
{
    public int itemTypeID {get; set;}
    public string itemName {get; set;}
    public int itemCost {get; set;}
    public List<Upgrade> itemPerks {get; set;}

    public List<UpgradeInfo> itemUpgrades {get; set;}
}
