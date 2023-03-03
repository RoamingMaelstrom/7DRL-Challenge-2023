using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPickupRangeUpgrader : Upgrader
{
    [SerializeField] CircleCollider2D pickupCollider;
    
    public override void ApplyUpgrade(Upgrade upgrade, int weaponTypeID)
    {
        if (upgrade.upgradeType == UpgradeType.DROP_ATTRACTION_DISTANCE) 
        {
            if (upgrade.isPercent) pickupCollider.radius *= 1f + (upgrade.magnitude / 100f);
            else pickupCollider.radius += upgrade.magnitude;
        }
    }
}
