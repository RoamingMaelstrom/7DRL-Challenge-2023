using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SOEvents;

public class ItemsPurchased : MonoBehaviour
{
    [SerializeField] IntSOEvent addItemToPlayerEvent;
    [SerializeField] UpgradeInfoIntSOEvent applyUpgradeEvent;
    public List<PurchaseLog> purchaseLogs = new List<PurchaseLog>();


    private void Awake() {
        addItemToPlayerEvent.AddListener(UpdateLogs);
        applyUpgradeEvent.AddListener(UpdateLogs);

    }
    public void UpdateLogs(UpgradeInfo upgradeInfoObject, int itemId)
    {
        UpdateLogs(itemId);
    }

    public void UpdateLogs(int itemId)
    {
        if (itemId < 0) return;
        foreach (var log in purchaseLogs)
        {
            if (log.itemId == itemId) 
            {
                log.itemLevel ++;
                return;
            }
        }
        purchaseLogs.Add(new PurchaseLog(itemId, 1));
    }  

    public bool Contains(int itemID)
    {
        foreach (var purchaseLog in purchaseLogs)
        {
            if (purchaseLog.itemId == itemID) return true;
        }
        return false;
    }

    public PurchaseLog GetItemPurchaseLog(int itemID)
    {
        foreach (var purchaseLog in purchaseLogs)
        {
            if (purchaseLog.itemId == itemID) return purchaseLog;
        }
        return null;
    }

    public int GetItemLevel(int itemID)
    {
        PurchaseLog purchaseLog = GetItemPurchaseLog(itemID);
        if (purchaseLog is null) return -1;
        return purchaseLog.itemLevel;
    }
}



[System.Serializable]
public class PurchaseLog
{
    public int itemId;
    public int itemLevel;

    public PurchaseLog(int _itemId, int _itemLevel)
    {
        itemId = _itemId;
        itemLevel = _itemLevel;
    }
}