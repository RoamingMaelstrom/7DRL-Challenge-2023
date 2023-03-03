using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SOEvents;
using System;

public class WeaponUpgrader : MonoBehaviour
{
    [SerializeField] UpgradeInfoIntSOEvent applyUpgradeEvent;
    // GameObject that is a direct parent of weapons that the player has.
    [SerializeField] GameObject playerWeaponsParent;
    [SerializeField] ObjectTypeIdRanges idRanges;

    // [SerializeField] UpgradeInfoIntSOEvent applyUpgradeEvent;

    private void Awake() {
        applyUpgradeEvent.AddListener(TryUpgradePlayerWeapons);
    }

    public void TryUpgradePlayerWeapons(UpgradeInfo upgradeInfo, int itemID)
    {
        if (!idRanges.CheckIdInGroupRange(itemID, "Weapons")) return;

        int numWeapons = playerWeaponsParent.transform.childCount;
        for (int i = 0; i < numWeapons; i++)
        {
            if (playerWeaponsParent.transform.GetChild(i).GetChild(0).TryGetComponent(out Weapon weapon))
            {
                if (weapon.weaponTypeID == itemID) UpgradeWeapon(weapon, upgradeInfo);
            }
        }
    }

    private void UpgradeWeapon(Weapon weapon, UpgradeInfo upgradeInfo)
    {
        foreach (var upgrade in upgradeInfo.upgrades)
        {
            float magnitude = upgrade.magnitude;
            if (upgrade.isPercent) 
            {
                magnitude /= 100f;
                if (upgrade.upgradeType == UpgradeType.DAMAGE_WEAPON) weapon.damageMod += magnitude;
                else if (upgrade.upgradeType == UpgradeType.COOLDOWN_WEAPON) weapon.cooldownMod += magnitude;
                else if (upgrade.upgradeType == UpgradeType.DOT_DAMAGE_WEAPON) weapon.dotDamageMod += magnitude;
                else if (upgrade.upgradeType == UpgradeType.DOT_INTERVAL_WEAPON) weapon.dotIntervalMod += magnitude;
            }
            else
            {
                if (upgrade.upgradeType == UpgradeType.DAMAGE_WEAPON) weapon.damageAdd += magnitude;
                else if (upgrade.upgradeType == UpgradeType.COOLDOWN_WEAPON) weapon.cooldownAdd += magnitude;
                else if (upgrade.upgradeType == UpgradeType.PIERCING_WEAPON) weapon.piercingAdd += magnitude;
                else if (upgrade.upgradeType == UpgradeType.DOT_DAMAGE_WEAPON) weapon.dotDamageAdd += magnitude;
                else if (upgrade.upgradeType == UpgradeType.DOT_INTERVAL_WEAPON) weapon.dotIntervalAdd += magnitude;
            }
        }
    }
}
