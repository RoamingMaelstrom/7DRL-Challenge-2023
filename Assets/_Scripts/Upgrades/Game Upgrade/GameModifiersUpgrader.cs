using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SOEvents;
using System;

public class GameModifiersUpgrader : MonoBehaviour
{
    [SerializeField] UpgradeInfoIntSOEvent applyUpgradeEvent;
    [SerializeField] GameModifiersEvent gameModifiersUpdateEvent;
    [SerializeField] GameModifiers gameModifiers;

    private void Awake() {
        applyUpgradeEvent.AddListener(TryUpgradeGameModifiers);
    }



    private void TryUpgradeGameModifiers(UpgradeInfo upgradeInfoContainer, int itemID)
    {
        foreach (var upgrade in upgradeInfoContainer.upgrades)
        {
            if (upgrade.isPercent) TryPercentageUpgradeGameModifiers(upgrade);
            else TryAbsoluteUpgradeGameModifiers(upgrade);
        }
        gameModifiersUpdateEvent.Invoke(gameModifiers);
    }



    private void TryPercentageUpgradeGameModifiers(Upgrade upgrade)
    {
        foreach (var stat in gameModifiers.stats)
        {
            if (stat.MatchesType(upgrade.upgradeType)) stat.multiplicationValue += upgrade.magnitude / 100f;
        }

    }

    private void TryAbsoluteUpgradeGameModifiers(Upgrade upgrade)
    {
        foreach (var stat in gameModifiers.stats)
        {
            if (stat.MatchesType(upgrade.upgradeType)) stat.additionValue += upgrade.magnitude;
        }
    }
}
