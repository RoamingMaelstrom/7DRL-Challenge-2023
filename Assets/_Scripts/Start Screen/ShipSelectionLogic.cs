using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipSelectionLogic : MonoBehaviour
{
    public List<ShipDisplay> shipDisplays = new List<ShipDisplay>();
    public PlayerShipIdSelected playerShipIdSelectedObject;

    private void Start() 
    {
        if (shipDisplays.Count == 0) return;
        SelectPlayerShip(0);
    }

    void UpdateShipDisplaysOutlines()
    {
        foreach (var ship in shipDisplays)
        {
            if (ship.shipProfile.shipID != playerShipIdSelectedObject.shipTypeID) ship.SetOutlineNormal();
            else ship.SetOutlineSelected();
        }
    }

    public void SelectPlayerShip(int shipDisplayIndex)
    {
        playerShipIdSelectedObject.shipTypeID = shipDisplays[shipDisplayIndex].shipProfile.shipID;
        UpdateShipDisplaysOutlines();
    }
}
