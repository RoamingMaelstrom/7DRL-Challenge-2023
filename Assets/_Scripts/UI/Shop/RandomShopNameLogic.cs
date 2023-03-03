using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SOEvents;
using TMPro;

public class RandomShopNameLogic : MonoBehaviour
{
    [SerializeField] Collider2DCollider2DSOEvent objectiveReachedEvent;
    [SerializeField] TextMeshProUGUI shopNameText;
    [SerializeField] List<string> shopNames = new List<string>();

    private void Awake() {
        objectiveReachedEvent.AddListener(SetShopName);
    }

    void SetShopName(Collider2D playerCollider, Collider2D objectiveCollider)
    {
        shopNameText.SetText(shopNames[Random.Range(0, shopNames.Count)]);
    }

}
