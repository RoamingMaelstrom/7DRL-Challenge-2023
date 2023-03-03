using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SOEvents;

public class PickupLogic : MonoBehaviour
{
    [SerializeField] DroppedItemMainSOEvent pickupItemEvent;
    [SerializeField] GameObjectFloatSOEvent despawnEvent;
    [SerializeField] FloatSOEvent updatePlayerMoneyEvent;
    [SerializeField] DroppedItemMainSOEvent generateItemEffectEvent;

    private void OnTriggerEnter2D(Collider2D other) {
        pickupItemEvent.Invoke(other.GetComponent<DroppedItemMain>());
    }

    private void Awake() {
        pickupItemEvent.AddListener(ProcessDroppedItem);
    }

    public void ProcessDroppedItem(DroppedItemMain droppedItem)
    {
        if (droppedItem.droppableType == DroppableType.MONEY) updatePlayerMoneyEvent.Invoke(droppedItem.value1);
        despawnEvent.Invoke(droppedItem.gameObject, 0);
        // Todo: Need to add event call to play sound
    }

}
