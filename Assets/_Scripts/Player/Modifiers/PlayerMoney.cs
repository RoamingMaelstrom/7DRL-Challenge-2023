using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SOEvents;

public class PlayerMoney : MonoBehaviour
{
    [SerializeField] FloatSOEvent updatePlayerMoney;
    public float money;

    private void Awake() {
        updatePlayerMoney.AddListener(UpdateMoneyValue);
    }

    public void UpdateMoneyValue(float value)
    {
        money += value;
    }
}
