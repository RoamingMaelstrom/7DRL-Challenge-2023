using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SOEvents;
using TMPro;

public class NearestVendorDisplayLogic : MonoBehaviour
{
    [SerializeField] GameObjectSOEvent vendorCreatedEvent;
    [SerializeField] TextMeshProUGUI distanceText;

    [SerializeField] Rigidbody2D playerBody;
    GameObject nearestVendor;

    private void Awake() {
        vendorCreatedEvent.AddListener(LoadNewVendorData);
    }

    private void LoadNewVendorData(GameObject vendor)
    {
        nearestVendor = vendor;

        VendorMain vendorMainScript = vendor.GetComponent<VendorMain>();
        distanceText.SetText(GetDistanceText(playerBody, vendor));
    }

    private string GetDistanceText(Rigidbody2D playerBody, GameObject objective)
    {
        float distance = (playerBody.transform.position - objective.transform.position).magnitude;

        return string.Format("Distance: {0:0.0}mi", distance * 10);
    }

    private void FixedUpdate() 
    {
        if (nearestVendor) distanceText.SetText(GetDistanceText(playerBody, nearestVendor));
    }
}
