using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MoneyTextLogic : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI moneyText;
    [SerializeField] PlayerMoney playerMoney;

    private void Update() 
    {
        moneyText.SetText(string.Format("${0:0.00}",playerMoney.money));
    }
}
