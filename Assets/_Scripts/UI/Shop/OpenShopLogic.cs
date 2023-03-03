using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SOEvents;
using System;

public class OpenShopLogic : MonoBehaviour
{
    [SerializeField] Collider2DCollider2DSOEvent objectiveReachedEvent;
    [SerializeField] Collider2DSOEvent createNewObjectiveEvent;
    [SerializeField] BoolSOEvent pauseEvent;
    [SerializeField] GameObject shopContent;
    [SerializeField] GetItemsToSell itemsToSellScript;
    [SerializeField] CurrentItemsLogic currentWeaponsLogic;
    [SerializeField] CurrentItemsLogic currentUtilitiesLogic;

    [SerializeField] HintsLogic hintsLogic;

    Collider2D playerCollider;


    private void Awake() {
        objectiveReachedEvent.AddListener(LoadAndOpenShopWindow);
    }

    public void LoadAndOpenShopWindow(Collider2D _playerCollider, Collider2D objectiveCollider)
    {
        // Randomly select what the shop will sell (Based on players current items and net worth).
        // Retrieve the players current items.
        // Load the items content selected to be sold to the shop. Make visible relevant panels.
        // Load players items. Make visible relevant tabs.
        playerCollider = _playerCollider;
        shopContent.SetActive(true);
        itemsToSellScript.PopulateShop(playerCollider.transform.parent.gameObject);
        
        currentWeaponsLogic.SetupText();
        currentUtilitiesLogic.SetupText();

        hintsLogic.GetHint();
        
        pauseEvent.Invoke(true);

    }

    public void CloseShopWindow()
    {
        shopContent.SetActive(false);
        pauseEvent.Invoke(false);
        createNewObjectiveEvent.Invoke(playerCollider);
    }
}
