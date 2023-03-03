using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SOEvents;
using System;

public class PlayerItemAdder : MonoBehaviour
{
    [SerializeField] IntSOEvent addItemToPlayerEvent;
    [SerializeField] GameObject playerParent;
    [SerializeField] ObjectTypeIdRanges idRanges;
    [SerializeField] GameObjectIntSOEvent createAndAttachWeaponEvent;
    [SerializeField] GameObjectIntSOEvent createAndAttachUtilityEvent;

    private void Awake() {
        addItemToPlayerEvent.AddListener(AddItemToPlayer);
    }

    private void AddItemToPlayer(int itemTypeID)
    {
        if (idRanges.CheckIdInGroupRange(itemTypeID, "Weapons")) createAndAttachWeaponEvent.Invoke(playerParent, itemTypeID);
        else if (idRanges.CheckIdInGroupRange(itemTypeID, "Utilities")) createAndAttachUtilityEvent.Invoke(playerParent, itemTypeID);
        else throw new System.Exception(string.Format("ItemTypeID provided is not a Weapon or a Utility Item. (ItemTypeID = {0})", itemTypeID));
    }
}
