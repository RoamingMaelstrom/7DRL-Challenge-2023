using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SOEvents;

public class VendorMain : MonoBehaviour
{
    public GameObjectSOEvent vendorCreatedEvent;
    public Collider2DCollider2DSOEvent enterVendorEvent;
    [SerializeField] Collider2D ownCollider;
    public string objectiveDescription;
    public float playerReward;


    private void Start() 
    {
        if (!ownCollider) ownCollider = GetComponent<Collider2D>();
        vendorCreatedEvent.Invoke(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.transform.tag == "Player") enterVendorEvent.Invoke(other, ownCollider);
    }
}
