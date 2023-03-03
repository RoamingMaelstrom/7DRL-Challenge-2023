using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using SOEvents;
using System;

public class ShopItemPaneLogic : MonoBehaviour
{
    [SerializeField] IntSOEvent purhcaseSuccessfulEvent;
    [SerializeField] StringSOEvent maxItemsOfTypePurchasedEvent;
    [SerializeField] TextMeshProUGUI itemNameText;
    [SerializeField] TextMeshProUGUI itemTypeText;
    [SerializeField] TextMeshProUGUI itemBodyText;
    [SerializeField] TextMeshProUGUI itemCostText;
    [SerializeField] Button buyButton;
    public ShopItem itemSelling {get; private set;}

    [SerializeField] Color weaponOutlineColour;
    [SerializeField] Color utilityOutlineColour;
    [SerializeField] Color noneOutlineColour;

    [SerializeField] Image outlineImage;

    private void Awake() 
    {
        if (purhcaseSuccessfulEvent) purhcaseSuccessfulEvent.AddListener(CheckIfItemPurchased);
        if (maxItemsOfTypePurchasedEvent) maxItemsOfTypePurchasedEvent.AddListener(CheckIfCanStillPurchase);
    }

    private void CheckIfCanStillPurchase(string itemTypeName)
    {
        if (itemSelling == null) return;
        if (itemSelling.isUpgrade) return;
        if (itemTypeName.Contains("Weapon") && itemSelling.itemType == ItemType.WEAPON) FillWithNoItemText();
        else if (itemTypeName.Contains("Utility") && itemSelling.itemType == ItemType.UTILITY) FillWithNoItemText();
    }

    private void CheckIfItemPurchased(int itemID)
    {
        if (itemSelling == null)
        {
            FillPurchasedText();
            return;
        }
        if (itemSelling.itemID == itemID) FillPurchasedText();

    }

    public void FillPurchasedText()
    {
        itemSelling = null;
        buyButton.interactable = false;
        itemNameText.SetText("");
        itemTypeText.SetText("");
        itemBodyText.SetText("Out of Stock");
        itemCostText.SetText("...");
        outlineImage.color = noneOutlineColour;
    }

    public void FillWithNoItemText()
    {
        itemSelling = null;
        buyButton.interactable = false;
        itemNameText.SetText("");
        itemTypeText.SetText("");
        itemBodyText.SetText("No Item Available");
        itemCostText.SetText("..."); 
        outlineImage.color = noneOutlineColour;
    }

    public void FillItemText(ShopItem shopItem)
    {
        itemSelling = shopItem;
        buyButton.interactable = true;
        if (shopItem.isUpgrade) FillItemTextForUpgrade(shopItem);
        else FillItemTextFirstTimePurchase(shopItem);
        if (shopItem.itemType == ItemType.WEAPON) outlineImage.color = weaponOutlineColour;
        else if (shopItem.itemType == ItemType.UTILITY) outlineImage.color = utilityOutlineColour;  
        else outlineImage.color = noneOutlineColour;
    }

    void FillItemTextForUpgrade(ShopItem shopItem)
    {
        itemNameText.SetText(string.Format("{0} Upgrade", shopItem.itemName));
        itemTypeText.SetText(GetItemTypeText(shopItem));
        string body = "";
        string singleUpgradeBody;
        foreach (var upgrade in shopItem.upgradeInfo.upgrades)
        {
            singleUpgradeBody = GetUpgradeTypeText(upgrade.upgradeType);
            if (upgrade.magnitude >= 0) singleUpgradeBody += " + ";
            else singleUpgradeBody += " - ";
            singleUpgradeBody += Mathf.Abs(upgrade.magnitude).ToString("n2");
            if (upgrade.isPercent) singleUpgradeBody += "%";
            body += singleUpgradeBody + "\n";
        }
        
        itemBodyText.SetText(body);
        itemCostText.SetText(string.Format("${0}", shopItem.cost));
    }

    private string GetItemTypeText(ShopItem shopItem)
    {
        if (shopItem.itemType == ItemType.WEAPON) return "Weapon";
        else if (shopItem.itemType == ItemType.UTILITY) return "Utility";
        else return "Unknown Type";
    }

    string GetUpgradeTypeText(UpgradeType upgradeType)
    {
        if (upgradeType == UpgradeType.ATTACK_SPEED_CURRENT_GAME) return "Attack Speed";
        if (upgradeType == UpgradeType.DAMAGE_CURRENT_GAME) return "Damage";
        if (upgradeType == UpgradeType.DAMAGE_DOT_CURRENT_GAME) return "Damage Over Time (DOT)";
        if (upgradeType == UpgradeType.DOT_INTERVAL_CURRENT_GAME) return "Damage Over Time (DOT) Interval";
        if (upgradeType == UpgradeType.DAMAGE_TAKEN_CURRENT_GAME) return "Damage Taken";
        if (upgradeType == UpgradeType.DELIVERY_DISTANCE_CURRENT_GAME) return "Delivery Distance";
        if (upgradeType == UpgradeType.DELIVERY_PAYOUT_CURRENT_GAME) return "Delivery Payout";
        if (upgradeType == UpgradeType.DROP_PROBABILITY_CURRENT_GAME) return "Drop Probability";
        if (upgradeType == UpgradeType.DROP_ATTRACTION_DISTANCE) return "Drop Attraction Radius";
        if (upgradeType == UpgradeType.HEALTH_REGEN_CURRENT_GAME) return "Ship Health Regen";
        if (upgradeType == UpgradeType.MAX_HP_CURRENT_GAME) return "Ship Health";
        if (upgradeType == UpgradeType.MOVE_SPEED_CURRENT_GAME) return "Ship Speed";
        if (upgradeType == UpgradeType.XP_GAIN_CURRENT_GAME) return "Experience Gain";
        if (upgradeType == UpgradeType.SHIELD_MAX_HP) return "Shield Health";      
        if (upgradeType == UpgradeType.SHIELD_REGEN_RATE) return "Shield Regen";   
        if (upgradeType == UpgradeType.SHIELD_REGEN_COOLDOWN) return "Shield Regen Cooldown";   
        if (upgradeType == UpgradeType.DAMAGE_WEAPON) return "(This Weapon) Damage";
        if (upgradeType == UpgradeType.COOLDOWN_WEAPON) return "(This Weapon) Cooldown";
        if (upgradeType == UpgradeType.DOT_DAMAGE_WEAPON) return "(This Weapon) Damage Over Time (DOT)";
        if (upgradeType == UpgradeType.DOT_INTERVAL_WEAPON) return "(This Weapon) Damage Over Time (DOT) Interval";
        if (upgradeType == UpgradeType.PIERCING_WEAPON) return "(This Weapon) Weapon Piercing";
        return "Not Implemented yet";
    }

    private void FillItemTextFirstTimePurchase(ShopItem shopItem)
    {
        itemNameText.SetText(shopItem.itemName);
        itemTypeText.SetText(GetItemTypeText(shopItem));
        itemBodyText.SetText(shopItem.itemDescription);
        itemCostText.SetText(string.Format("${0}", shopItem.cost));
    }
}
