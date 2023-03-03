using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SOEvents;

public class ItemPurchaseLogic : MonoBehaviour
{
    [SerializeField] IntSOEvent addItemToPlayerEvent;
    [SerializeField] UpgradeInfoIntSOEvent addUpgradeEvent;
    [SerializeField] FloatSOEvent updatePlayerMoneyEvent;
    [SerializeField] IntSOEvent purchaseSuccessfulEvent;
    [SerializeField] GetItemsToSell getItemsToSellScript;
    [SerializeField] PlayerMoney playerMoneyScript;
    

    // Called by buttons.
    public void TryPurchaseItem(int shopItemIndex)
    {
        ShopItem shopItem = getItemsToSellScript.shopItems[shopItemIndex];
        if (playerMoneyScript.money < shopItem.cost) 
        {
            Debug.Log("Cannot afford item.");
            return;
        }
        if (shopItem.isUpgrade) addUpgradeEvent.Invoke(shopItem.upgradeInfo, shopItem.itemID);
        else addItemToPlayerEvent.Invoke(shopItem.itemID);

        updatePlayerMoneyEvent.Invoke(-shopItem.cost);
        purchaseSuccessfulEvent.Invoke(shopItem.itemID);
    }
}
