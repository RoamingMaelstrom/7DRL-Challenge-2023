using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DefaultItemProfile", menuName = "ItemProfiles/DefaultItemProfile", order = 0)]
public class ItemProfile : ScriptableObject
{
    public int itemID;
    public string itemName;
    [TextArea] public string itemDescription;
    public int itemCost;

    public ItemType itemType;


    public Vector2 itemOffset;

    public Sprite sprite;
    public Material material;


    // Weapons use these variables.
    public WeaponType weaponType;
    public float baseMaxCd;
    public float accuracyCoefficient;

    public WeaponTargetingType targetingType;
    public int damageDealerTypeID;
    
    // All use these variables.
    public List<Upgrade> itemPerks;
    public List<UpgradeInfo> itemUpgrades;
}


public enum ItemType
{
    WEAPON,
    UTILITY
}


public enum WeaponType
{
    SINGLESHOT,
    TRIPLESHOT,
    LASERBEAM,
    HOMING_MISSILE,
    ON_PLAYER,
    SWEEPING_LASERBEAM
}