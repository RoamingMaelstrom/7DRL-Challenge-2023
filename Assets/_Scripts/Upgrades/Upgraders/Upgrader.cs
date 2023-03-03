using UnityEngine;
using SOEvents;

public abstract class Upgrader : MonoBehaviour
{
    [SerializeField] UpgradeInfoIntSOEvent applyUpgradeEvent;

    private void Awake() 
    {
        applyUpgradeEvent.AddListener(ApplyUpgrades);
    }

    void ApplyUpgrades(UpgradeInfo upgradeInfo, int weaponTypeID)
    {
        foreach (var upgrade in upgradeInfo.upgrades)
        {
            ApplyUpgrade(upgrade, weaponTypeID);
        }
    }

    public abstract void ApplyUpgrade(Upgrade upgrade, int weaponTypeID);

}
