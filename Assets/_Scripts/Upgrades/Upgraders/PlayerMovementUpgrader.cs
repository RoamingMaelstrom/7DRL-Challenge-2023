using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SOEvents;

public class PlayerMovementUpgrader : Upgrader
{
    [SerializeField] BasePlayerController playerController;


    public override void ApplyUpgrade(Upgrade upgrade, int weaponTypeID)
    {
        if (upgrade.upgradeType == UpgradeType.MOVE_SPEED_CURRENT_GAME) 
        {
            if (upgrade.isPercent) playerController.playerThrust *= 1f + (upgrade.magnitude / 100f);
            else playerController.playerThrust += upgrade.magnitude;
        }
    }
}


