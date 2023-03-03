using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SOEvents;

public abstract class Weapon : MonoBehaviour
{
    [SerializeField] public WeaponSOEvent requestDamageDealerEvent;
    [SerializeField] public WeaponSOEvent requestTargetEvent;
    [SerializeField] public WeaponSOEvent requestCooldownEvent;
    public int weaponTypeID;
    public Vector2 offset;
    public float baseMaxCd;
    public float currentCd;
    public float accuracyCoefficient = 1f;

    public float damageMod;
    public float damageAdd;

    public float dotDamageMod;
    public float dotDamageAdd;

    public float dotIntervalMod;
    public float dotIntervalAdd;

    public float piercingAdd;

    public float cooldownMod;
    public float cooldownAdd;

    public int damageDealerTypeID;

    public WeaponTargetingType targetingType;
    public Vector3 currentTarget;
    public GameObject currentDamageDealerObject;


    private void FixedUpdate() {
        if (CheckIfCanFire()) 
        {
            Fire();
        }
    }
    public virtual bool CheckIfCanFire()
    {
        currentCd -= Time.fixedDeltaTime;
        if (currentCd > 0) return false;
        requestCooldownEvent.Invoke(this);
        return true;
    }
    public abstract void Fire();

    public void SetCurrentDamageDealerRotationToVelocity()
    {
        SetBodyRotationToVelocity(currentDamageDealerObject.GetComponent<Rigidbody2D>());
    }

    public void SetBodyRotationToVelocity(Rigidbody2D body)
    {
        float newRotation = - Mathf.Atan2(body.velocity.x, body.velocity.y) * Mathf.Rad2Deg;
        currentDamageDealerObject.transform.rotation = Quaternion.Euler(0, 0, newRotation);
    }
}

public enum WeaponTargetingType 
{
    NEAREST,
    FARTHEST,
    FORWARD,
    BACKWARD,
    RANDOM_ENEMY,
    RANDOM
}
