using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "PlayerShipProfile", menuName = "PlayerShipProfile", order = 0)]
public class PlayerShipProfile : ScriptableObject
{
    public int shipID;
    public string shipName;
    [SerializeField] [TextArea] public string shipDescription;
    public Sprite shipSprite;
    public float spriteScale = 1;

    public float shipThrust = 5000;
    public float shipTorque = 100;
    public float shipMaxRotationSpeed = 270;

    public float shipBaseMaxHealth = 100;
    public float shipBaseHealthRegen;
    public float shipBaseMaxShieldHealth;
    public float shipBaseShieldRegen;
    public float shipBaseShieldRegenCooldown;

    public float playerStartingMoney;

    public int shipMaxNumWeapons = 6;
    public int shipMaxNumUtilities = 6;

    public List<int> shipStartingItemIds = new List<int>();

    public UpgradeInfo shipStartStatUpgrades = new UpgradeInfo(null, 0);
}
