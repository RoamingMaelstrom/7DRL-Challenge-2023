using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SOEvents;
using System;

public class SetupPlayer : MonoBehaviour
{
    [SerializeField] IntSOEvent addItemToPlayerEvent;
    [SerializeField] UpgradeInfoIntSOEvent applyUpgradeEvent;
    [SerializeField] PlayerShipIdSelected shipIdSelected;
    [SerializeField] List<PlayerShipProfile> playerShipProfiles = new List<PlayerShipProfile>();
    [SerializeField] GameObject playerParentObject;
    [SerializeField] GameObject playerMainBodyObject;
    [SerializeField] GameObject playerShieldObject;
    [SerializeField] GameObject gameModifiersObject;


    void Start() 
    {
        StartCoroutine(RunSetup());
    }

    public IEnumerator RunSetup()
    {
        yield return new WaitForFixedUpdate();

        PlayerShipProfile profile = playerShipProfiles.Find(x => x.shipID == shipIdSelected.shipTypeID);

        BasePlayerController playerController = playerParentObject.GetComponent<BasePlayerController>();
        playerController.playerThrust = profile.shipThrust;
        playerController.maxPlayerTorque = profile.shipTorque;
        playerController.maxAngularVelocity = profile.shipMaxRotationSpeed;

        SpriteRenderer playerSpriteRenderer = playerMainBodyObject.GetComponent<SpriteRenderer>();
        playerSpriteRenderer.sprite = profile.shipSprite;
        if (profile.shipID == 9004) playerSpriteRenderer.flipY = true;
        playerSpriteRenderer.transform.localScale = new Vector3(1.25f, 1.25f, 1.25f) * profile.spriteScale;
        playerShieldObject.transform.localScale = new Vector3(1.75f, 1.75f, 1.75f) / profile.spriteScale;

        Health healthScript = playerMainBodyObject.GetComponent<Health>();
        healthScript.maxHp = profile.shipBaseMaxHealth;
        healthScript.ManualSetCurrentHp(profile.shipBaseMaxHealth);
        healthScript.hpRegenRate = profile.shipBaseHealthRegen;

        healthScript.shieldHpMax = profile.shipBaseMaxShieldHealth;
        healthScript.ManualSetCurrentShieldHp(profile.shipBaseMaxHealth);
        healthScript.shieldRegenRate = profile.shipBaseShieldRegen;
        healthScript.shieldOnDamagedRegenCooldown = profile.shipBaseShieldRegenCooldown;

        gameModifiersObject.GetComponentInChildren<PlayerMoney>().money = profile.playerStartingMoney;

        ItemNumberLimit itemNumberLimitScript = gameModifiersObject.GetComponentInChildren<ItemNumberLimit>();
        itemNumberLimitScript.maxWeapons = profile.shipMaxNumWeapons;
        itemNumberLimitScript.maxUtilities = profile.shipMaxNumUtilities; 

        foreach (var startingItemId in profile.shipStartingItemIds)
        {
            addItemToPlayerEvent.Invoke(startingItemId);
        }

        applyUpgradeEvent.Invoke(profile.shipStartStatUpgrades, -1);

        yield return null;

    }
}
