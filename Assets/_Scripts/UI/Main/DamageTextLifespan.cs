using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageTextLifespan : MonoBehaviour
{
    public float startLifespan;
    public float lifeRemaining;

    private void Start() 
    {
        lifeRemaining = startLifespan;
    }

    private void FixedUpdate() {
        lifeRemaining -= Time.fixedDeltaTime;
        if (lifeRemaining <=0) Destroy(gameObject);
    }
}
