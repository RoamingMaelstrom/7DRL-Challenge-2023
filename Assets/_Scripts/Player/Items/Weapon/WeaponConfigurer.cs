using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SOEvents;

public class WeaponConfigurer : MonoBehaviour
{
    [SerializeField] public WeaponSOEvent requestDamageDealerEvent;
    [SerializeField] public WeaponSOEvent requestTargetEvent;
    [SerializeField] public WeaponSOEvent requestCooldownEvent;
    public ItemProfileContainer weaponProfiles;
    public GameObject ConfigureWeapon(GameObject weaponObject, int weaponID)
    {
        GameObject weaponBody = weaponObject.transform.GetChild(0).gameObject;
        ItemProfile profile = GetWeaponProfile(weaponID);

        AddWeaponScriptToWeapon(weaponBody, profile);

        // This should be temporary only. Actual weapon should not be visible.
        // Also need to set parent weaponObject's position equal to player/enemy it belongs to position on spawn.
        SpriteRenderer weaponSpriteRenderer = weaponBody.GetComponent<SpriteRenderer>();
        weaponSpriteRenderer.sprite = profile.sprite;

        Weapon weaponScript = weaponBody.GetComponent<Weapon>();

        weaponScript.requestDamageDealerEvent = requestDamageDealerEvent;
        weaponScript.requestTargetEvent = requestTargetEvent;
        weaponScript.requestCooldownEvent = requestCooldownEvent;      

        weaponScript.accuracyCoefficient = profile.accuracyCoefficient;
        weaponScript.baseMaxCd = profile.baseMaxCd;
        weaponScript.offset = profile.itemOffset;
        
        weaponScript.targetingType = profile.targetingType;
        weaponScript.weaponTypeID = profile.itemID;
        weaponScript.damageDealerTypeID = profile.damageDealerTypeID;

        weaponScript.damageMod = 1;
        weaponScript.damageAdd = 0;

        weaponScript.dotDamageMod = 1;
        weaponScript.dotDamageAdd = 0;

        weaponScript.dotIntervalMod = 1;
        weaponScript.dotIntervalAdd = 0;

        weaponScript.piercingAdd = 1;
        
        weaponScript.cooldownMod = 1;
        weaponScript.cooldownAdd = 0;

        weaponObject.transform.localPosition = Vector3.zero;
        weaponObject.transform.localRotation = Quaternion.Euler(0, 0, 0);
        weaponBody.transform.localPosition = new Vector3(weaponScript.offset.x, weaponScript.offset.y, 0);

        return weaponObject;
    }

    private ItemProfile GetWeaponProfile(int weaponTypeID)
    {
        foreach (var profile in weaponProfiles.itemProfiles)
        {
            if (profile.itemID == weaponTypeID) return profile;
        }

        throw new System.Exception(string.Format("Could not find profile with ID {0}", weaponTypeID));
    }

    private void AddWeaponScriptToWeapon(GameObject weaponBody, ItemProfile profile)
    {
        if (weaponBody.TryGetComponent(out Weapon oldWeapon)) Destroy(oldWeapon);
        if (weaponBody.TryGetComponent(out LineRenderer oldLineRenderer)) Destroy(oldLineRenderer);

        if (profile.weaponType == WeaponType.SINGLESHOT)
        {
            weaponBody.AddComponent(typeof(SingleShotWeapon));
            return;
        }

        if (profile.weaponType == WeaponType.TRIPLESHOT) 
        {
            weaponBody.AddComponent(typeof(TripleShotWeapon));
            return;
        }

        if (profile.weaponType == WeaponType.LASERBEAM)
        {
            weaponBody.AddComponent(typeof(LaserBeamWeapon));
            weaponBody.AddComponent(typeof(LineRenderer));
            weaponBody.GetComponent<LineRenderer>().material = profile.material;
            weaponBody.GetComponent<LineRenderer>().startWidth = 0.5f;
            return;
        }

        if (profile.weaponType == WeaponType.HOMING_MISSILE)
        {
            weaponBody.AddComponent(typeof(HomingMissileWeapon));
            return;
        }

        if (profile.weaponType == WeaponType.ON_PLAYER)
        {
            weaponBody.AddComponent(typeof(OnPlayerWeapon));
            return;
        }

        if (profile.weaponType == WeaponType.SWEEPING_LASERBEAM)
        {
            weaponBody.AddComponent(typeof(SweepingLaserBeamWeapon));
            weaponBody.AddComponent(typeof(LineRenderer));
            weaponBody.GetComponent<LineRenderer>().material = profile.material;
            return;
        }
    }
}
