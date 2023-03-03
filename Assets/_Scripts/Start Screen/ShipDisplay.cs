using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ShipDisplay : MonoBehaviour
{
    [SerializeField] public PlayerShipProfile shipProfile;
    [SerializeField] public Image outline;
    [SerializeField] public Image shipImage;
    [SerializeField] public TextMeshProUGUI shipNameText;
    [SerializeField] public TextMeshProUGUI shipDescriptionText;

    [SerializeField] Color normalColour;
    [SerializeField] Color highlightColor;

    void Start() 
    {
        shipImage.sprite = shipProfile.shipSprite;
        shipNameText.SetText(shipProfile.shipName);
        shipDescriptionText.SetText(shipProfile.shipDescription);
    }

    public void SetOutlineSelected() => outline.color = highlightColor;
    public void SetOutlineNormal() => outline.color = normalColour;
}
