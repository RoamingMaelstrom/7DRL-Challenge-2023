using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UpgradeInfo
{
    [SerializeField] public List<Upgrade> upgrades = new List<Upgrade>();
    public int cost;

    
    public UpgradeInfo(List<Upgrade> _upgrades, int _cost)
    {
        upgrades = _upgrades;
        cost = _cost;
    }
}
