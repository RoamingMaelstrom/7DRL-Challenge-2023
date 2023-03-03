using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ShopButtonColourLogic : MonoBehaviour
{
    [SerializeField] Color canPurchaseButtonColour;
    [SerializeField] Color cannotPurchaseButtonColour;

    [SerializeField] PlayerMoney playerMoneyScript;

    [SerializeField] ShopItemPaneLogic shopItemPaneLogic;
    [SerializeField] Button button;
    [SerializeField] TextMeshProUGUI buttonText;

    private void Update() 
    {
        if (shopItemPaneLogic.itemSelling == null) SetButtonStyleCannotPurchase();
        else if (shopItemPaneLogic.itemSelling.cost < playerMoneyScript.money) SetButtonStyleCanPurchase();
        else SetButtonStyleCannotPurchase();
    }

    void SetButtonStyleCanPurchase()
    {
        button.interactable = true;
        buttonText.color = canPurchaseButtonColour;

    }

    void SetButtonStyleCannotPurchase()
    {
        button.interactable = false;
        buttonText.color = cannotPurchaseButtonColour;
    }
}
