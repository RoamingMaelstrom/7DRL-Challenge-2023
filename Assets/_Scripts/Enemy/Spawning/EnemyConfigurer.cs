using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SOEvents;

public class EnemyConfigurer : MonoBehaviour
{
    [SerializeField] GameObjectFloatSOEvent despawnEvent;
    [SerializeField] GameObjectFloatSOEvent takeDamageEvent;
    [SerializeField] Rigidbody2D playerBody;
    [SerializeField] public List<EnemyProfile> enemyProfiles;
    [SerializeField] GameObject shieldDisplayPrefab;

    public GameObject ConfigureEnemy(GameObject enemyObject, int enemyTypeID)
    {
        EnemyProfile enemyProfile = GetEnemyProfile(enemyTypeID);
        return ConfigureEnemy(enemyObject, enemyProfile);
    }

    public GameObject ConfigureEnemy(GameObject enemyObject, string enemyName)
    {        
        EnemyProfile enemyProfile = GetEnemyProfile(enemyName);
        return ConfigureEnemy(enemyObject, enemyProfile);
    }

    public GameObject ConfigureEnemy(GameObject enemyObject, EnemyProfile enemyProfile)
    {
        enemyObject.GetComponent<EnemyDespawnType>().despawnType = enemyProfile.despawnType;

        GameObject enemyBody = enemyObject.transform.GetChild(0).gameObject;

        Health enemyHealth = enemyBody.GetComponent<Health>();
        enemyHealth.onDeathEvent = despawnEvent;
        enemyHealth.onTakeDamageEvent = takeDamageEvent;
        enemyHealth.maxHp = enemyProfile.maxHp;
        enemyHealth.ManualSetCurrentHp(enemyProfile.maxHp);
        enemyHealth.hpRegenRate = enemyProfile.hpRegenRate;
        enemyHealth.shieldHpMax = enemyProfile.maxShieldHp;
        enemyHealth.ManualSetCurrentShieldHp(enemyProfile.maxShieldHp);
        enemyHealth.shieldRegenRate = enemyProfile.shieldRegenRate;
        enemyHealth.shieldOnDamagedRegenCooldown = enemyProfile.shieldOnHitRegenCooldown;

        HandleShieldScript(enemyBody, enemyProfile);

        DamageDealer enemyDamageDealer = enemyBody.GetComponent<DamageDealer>();
        enemyDamageDealer.damageValue = enemyProfile.damageValue;
        enemyDamageDealer.dotDamageValue = enemyProfile.dotDamageValue;
        enemyDamageDealer.dotInterval = enemyProfile.dotInterval;
        enemyDamageDealer.life = 10000000;
        enemyDamageDealer.ManuallyRestartDotDamageCoroutine();

        EnemyMovement enemyMovement = enemyBody.GetComponent<EnemyMovement>();
        enemyMovement.moveSpeed = enemyProfile.moveSpeed;
        enemyMovement.targetTransform = playerBody.transform;

        CapsuleCollider2D enemyCollider = enemyBody.GetComponent<CapsuleCollider2D>();
        enemyCollider.size = enemyProfile.colliderSize;

        enemyBody.transform.localScale = enemyProfile.scale;
        SpriteRenderer enemySpriteRenderer = enemyBody.GetComponent<SpriteRenderer>();
        enemySpriteRenderer.sprite = enemyProfile.sprite;

        DropItems dropItems = enemyObject.GetComponent<DropItems>();
        dropItems.drops.Clear();
        foreach (var dropInfo in enemyProfile.dropInfo)
        {
            dropItems.drops.Add(new DropInfo(dropInfo.droppableTypeID, dropInfo.quantity, dropInfo.dropProbability));
        }

        ObjectIdentifier enemyObjectIdentifier = enemyObject.GetComponent<ObjectIdentifier>();
        enemyObjectIdentifier.objectTypeID = enemyProfile.enemyTypeID;

        return enemyObject;
    }

    private void HandleShieldScript(GameObject enemyBody, EnemyProfile enemyProfile)
    {
        ShieldDisplayLogic shieldLogic = enemyBody.transform.GetComponentInChildren<ShieldDisplayLogic>();
        if (enemyProfile.maxShieldHp <= 0) 
        {
            if (shieldLogic) Destroy(shieldLogic.gameObject);
        }
        else
        {
            if (!shieldLogic) 
            {
                GameObject shieldLogicObject = Instantiate(shieldDisplayPrefab, enemyBody.transform);
                shieldLogic = shieldLogicObject.GetComponent<ShieldDisplayLogic>();
                shieldLogicObject.transform.localScale = enemyProfile.scale;
            }
            shieldLogic.objectHealthScript = enemyBody.GetComponent<Health>();
        }
    }

    private EnemyProfile GetEnemyProfile(string enemyName)
    {
        foreach (var profile in enemyProfiles)
        {
            if (profile.enemyName == enemyName) return profile;
        }

        Debug.Log(string.Format("Could not specified profile with name \"{0}\"", enemyName));
        return null;
    }

    private EnemyProfile GetEnemyProfile(int enemyTypeID)
    {
        foreach (var profile in enemyProfiles)
        {
            if (profile.enemyTypeID == enemyTypeID) return profile;
        }

        Debug.Log(string.Format("Could not specified profile with ID \"{0}\"", enemyTypeID));
        return null;
    }
}
