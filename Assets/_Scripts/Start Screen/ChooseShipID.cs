using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseShipID : MonoBehaviour
{
    [SerializeField] PlayerShipIdSelected playerShipIdSelected;

    public void SetPlayerShipID(int ID)
    {
        playerShipIdSelected.shipTypeID = ID;
    }
}
