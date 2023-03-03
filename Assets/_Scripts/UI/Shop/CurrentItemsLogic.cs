using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using SOEvents;
using System;

public class CurrentItemsLogic : MonoBehaviour
{
    [SerializeField] IntSOEvent purchaseSuccessfulEvent;
    [SerializeField] List<GameObject> itemContainers = new List<GameObject>();
    [SerializeField] TextMeshProUGUI totalItemsText;
    [SerializeField] ItemNumberLimit itemNumberLimit;
    [SerializeField] ItemsPurchased itemsPurchased;
    [SerializeField] ItemProfileContainer profiles;

    public ItemType itemType;
    int currentTextIndex = 0;

    private void Awake() {
        purchaseSuccessfulEvent.AddListener(SetupText);
    }

    // Ensures compatible with purchaseSuccessfulEvent
    public void SetupText(int value)
    {
        SetupText();
    }

    public void SetupText()
    {
        currentTextIndex = 0;
        SetContainersTextBlank();
        PopulateItemsText();
        PopulateNumberOfItemsText();
    }

    private void SetContainersTextBlank()
    {
        foreach (var container in itemContainers)
        {
        container.transform.GetChild(0).GetComponent<TextMeshProUGUI>().SetText("");
        container.transform.GetChild(1).GetComponent<TextMeshProUGUI>().SetText("");
        }
    }

    private void PopulateNumberOfItemsText()
    {
        if (itemType == ItemType.WEAPON) totalItemsText.SetText(string.Format("({0}/{1})", itemNumberLimit.currentWeapons,
         itemNumberLimit.maxWeapons));
        else if (itemType == ItemType.UTILITY) totalItemsText.SetText(string.Format("({0}/{1})", itemNumberLimit.currentUtilities,
         itemNumberLimit.maxUtilities));
    }

    private void PopulateItemsText()
    {
        foreach (var item in itemsPurchased.purchaseLogs)
        {
            ItemProfile profile = profiles.GetProfileWithID(item.itemId);
            if (profile) PopulateItemRowText(item, profile);
        }
    }

    private void PopulateItemRowText(PurchaseLog item, ItemProfile profile)
    {
        itemContainers[currentTextIndex].transform.GetChild(0).GetComponent<TextMeshProUGUI>().SetText(profile.itemName);
        itemContainers[currentTextIndex].transform.GetChild(1).GetComponent<TextMeshProUGUI>().SetText(string.Format("({0}/{1})",
         item.itemLevel, profile.itemUpgrades.Count + 1));
        currentTextIndex ++;
    }
}
