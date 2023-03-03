using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttractItem : MonoBehaviour
{
    [SerializeField] public float attractionSpeed;

    private void OnTriggerStay2D(Collider2D other) {
        other.attachedRigidbody.velocity = attractionSpeed * (transform.position - other.transform.position).normalized;
    }
}
