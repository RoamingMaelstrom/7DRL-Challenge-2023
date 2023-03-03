using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GetItemsToSell : MonoBehaviour
{
    [SerializeField] List<ShopItemPaneLogic> shopItemPanes = new List<ShopItemPaneLogic>(); 


    public ItemProfileContainer weaponProfiles;
    public ItemProfileContainer utilityProfiles;

    public ItemsPurchased playerPurchaseLogContainer;

    [SerializeField] ItemNumberLimit itemNumberLimitScript;

    [HideInInspector]
    public ShopItem[] shopItems;


    public void PopulateShop(GameObject playerParent)
    {
        ItemProfile[] itemProfilesForShop = GetItems(playerParent);
        shopItems = GetShopItemData(playerParent, itemProfilesForShop);
        for (int i = 0; i < itemProfilesForShop.Length; i++)
        {
            if (itemProfilesForShop[i] is null) shopItemPanes[i].FillWithNoItemText();
            else shopItemPanes[i].FillItemText(shopItems[i]);
        }
    }

    List<int> GetPotentialItems(ItemProfileContainer itemProfileContainer, bool upgradesOnly = false)
    {
        List<int> outputItemIDs = new List<int>();
        foreach (ItemProfile item in itemProfileContainer.itemProfiles)
        {
            PurchaseLog itemPurchaseLog = playerPurchaseLogContainer.GetItemPurchaseLog(item.itemID);
            if (itemPurchaseLog is null && upgradesOnly) continue;
            if (playerPurchaseLogContainer.GetItemLevel(item.itemID) >= item.itemUpgrades.Count + 1) continue;
            outputItemIDs.Add(item.itemID);
        }
        return outputItemIDs;
    }

    ShopItem[] GetShopItemData(GameObject playerParent, ItemProfile[] profiles)
    {
        ShopItem[] shopItems = new ShopItem[4];
        for (int i = 0; i < profiles.Length; i++)
        {
            if (profiles[i] is null) continue;
            if (playerPurchaseLogContainer.GetItemLevel(profiles[i].itemID) < 1) shopItems[i] = GetInitialPurchaseShopItemData(profiles[i]);
            // else Get ShopItem based on profile upgrade data for specified level.   
            else shopItems[i] = GetUpgradeShopItemData(profiles[i]);
        }
        return shopItems;
    }

    ShopItem GetInitialPurchaseShopItemData(ItemProfile profile)
    {
        ShopItem outputShopItem = new ShopItem();
        outputShopItem.itemID = profile.itemID;
        outputShopItem.itemType = profile.itemType;
        outputShopItem.itemName = profile.itemName;
        outputShopItem.itemDescription = profile.itemDescription;
        outputShopItem.isUpgrade = false;
        outputShopItem.cost = profile.itemCost;
        return outputShopItem;
    }

    ShopItem GetUpgradeShopItemData(ItemProfile profile)
    {
        ShopItem outputShopItem = new ShopItem();
        outputShopItem.itemID = profile.itemID;
        outputShopItem.itemName = profile.itemName;
        outputShopItem.itemType = profile.itemType;
        outputShopItem.itemDescription = profile.itemDescription;
        outputShopItem.isUpgrade = true;
        outputShopItem.upgradeInfo = profile.itemUpgrades[playerPurchaseLogContainer.GetItemLevel(profile.itemID) - 1];
        outputShopItem.cost = outputShopItem.upgradeInfo.cost;

        return outputShopItem;
    }

    ItemProfile[] GetItems(GameObject playerParent)
    {

        bool WEAPON_UPGRADE_ONLY = false;
        bool UTILITY_UPGRADE_ONLY = false;

        if (itemNumberLimitScript.currentWeapons == itemNumberLimitScript.maxWeapons) WEAPON_UPGRADE_ONLY = true;
        if (itemNumberLimitScript.currentUtilities == itemNumberLimitScript.maxUtilities) UTILITY_UPGRADE_ONLY = true;

        List<int> validWeapons = GetPotentialItems(weaponProfiles, WEAPON_UPGRADE_ONLY);
        List<int> validUtilities = GetPotentialItems(utilityProfiles, UTILITY_UPGRADE_ONLY);

        int numWeapons = Random.Range(1, 4);
        int numUtilities = 4 - numWeapons;

        ItemProfile[] itemsForSale = new ItemProfile[4];

        for (int i = 0; i < numWeapons; i++)
        {
            itemsForSale[i] = GetRandomItem(weaponProfiles, validWeapons, playerParent.transform.GetComponentInChildren<PlayerMoney>());
            if (itemsForSale[i]) validWeapons.Remove(itemsForSale[i].itemID);

        }

        for (int i = numWeapons; i < 4; i++)
        {
            itemsForSale[i] = GetRandomItem(utilityProfiles, validUtilities, playerParent.transform.GetComponentInChildren<PlayerMoney>());
            if (itemsForSale[i]) validUtilities.Remove(itemsForSale[i].itemID);
        }

        return itemsForSale;
    }

    ItemProfile GetRandomItem(ItemProfileContainer profileContainer, List<int> itemIndexes, PlayerMoney playerMoney)
    {
        if (itemIndexes.Count == 0) return null;
        int randIndex = Random.Range(0, itemIndexes.Count);
        int randomID = itemIndexes[randIndex];

        float itemCost = GetItemCost(profileContainer.GetProfileWithID(randomID));
        // Done to reduce probability of player getting loads of overprices items in early game.
        if (itemCost > playerMoney.money) itemCost = GetItemCost(profileContainer.GetProfileWithID(randomID));

        return profileContainer.GetProfileWithID(randomID);
    }
    


    float GetItemCost(ItemProfile itemProfile)
    {
        PurchaseLog purchaseLog = playerPurchaseLogContainer.GetItemPurchaseLog(itemProfile.itemID);
        if (purchaseLog is null) return itemProfile.itemCost;
        if (purchaseLog.itemLevel >= itemProfile.itemUpgrades.Count) return -1;
        return itemProfile.itemUpgrades[purchaseLog.itemLevel - 1].cost;
    }
}
