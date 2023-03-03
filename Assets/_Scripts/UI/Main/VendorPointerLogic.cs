using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VendorPointerLogic : MonoBehaviour
{
    [SerializeField] GameObject pointerPrefab;
    [SerializeField] GameObject nearestVendor;
    [SerializeField] GameObject playerBody;

    GameObject pointer;

    public float pointerMaxDistanceFromPlayer = 5;

    private void Start() 
    {
        pointer = Instantiate(pointerPrefab, transform);   
    }

    private void GetNearestObjective()
    {
        
    }

    private void Update() 
    {
        GetNearestObjective();
        PointTowardsNearestVendor();
    }

    void PointTowardsNearestVendor()
    {
        if (!nearestVendor) 
        {
            pointer.SetActive(false);
            return;
        }
        
        pointer.SetActive(true);
        Vector3 dirVector = (nearestVendor.transform.position - playerBody.transform.position);
        dirVector.z = playerBody.transform.position.z;
        dirVector = dirVector.normalized;

        // By default, places pointer specified distance (pointerMaxDistanceFromPlayer) away from player in the direction of the objective.
        // Exception is made if the pointer is about to go over the actual objective (happens when the player gets near to it). In this case,
        // the pointer is drawn between the player and objective (bias towards being closer to the objective).
        pointer.transform.position = playerBody.transform.position + (dirVector * Mathf.Clamp(pointerMaxDistanceFromPlayer, 0.5f,
        (nearestVendor.transform.position - playerBody.transform.position).magnitude - 1));

        float newRotation = - Mathf.Atan2(dirVector.x, dirVector.y) * Mathf.Rad2Deg;
        pointer.transform.rotation = Quaternion.Euler(0, 0, newRotation);
    }
}
