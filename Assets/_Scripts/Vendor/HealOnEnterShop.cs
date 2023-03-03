using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SOEvents;
using System;

public class HealOnEnterShop : MonoBehaviour
{
    [SerializeField] Collider2DCollider2DSOEvent objectiveReachedEvent;
    [SerializeField] Health playerHealth;

    private void Awake() {
        objectiveReachedEvent.AddListener(FullHealPlayer);
    }

    private void FullHealPlayer(Collider2D arg0, Collider2D arg1)
    {
        playerHealth.ManualSetCurrentHp(playerHealth.maxHp);
        playerHealth.ManualSetCurrentShieldHp(playerHealth.shieldHpMax);
    }
}
