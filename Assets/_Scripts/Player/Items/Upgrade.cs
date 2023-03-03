using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Upgrade
{
    public UpgradeType upgradeType;
    public float magnitude;
    public bool isPercent;

    
    public Upgrade(UpgradeType _upgradeType, float _magnitude, bool _isPercent)
    {
        upgradeType = _upgradeType;
        magnitude = _magnitude;
        isPercent = _isPercent;
    }
}


public enum UpgradeType
{
    DAMAGE_WEAPON,
    DOT_DAMAGE_WEAPON,
    DOT_INTERVAL_WEAPON,
    PIERCING_WEAPON,
    COOLDOWN_WEAPON,

    ATTACK_SPEED_CURRENT_GAME,
    DAMAGE_CURRENT_GAME,
    DAMAGE_DOT_CURRENT_GAME,
    DAMAGE_TAKEN_CURRENT_GAME,

    DELIVERY_DISTANCE_CURRENT_GAME,
    DELIVERY_PAYOUT_CURRENT_GAME,
    DOT_INTERVAL_CURRENT_GAME,

    DROP_PROBABILITY_CURRENT_GAME,
    DROP_ATTRACTION_DISTANCE,

    HEALTH_REGEN_CURRENT_GAME,
    MAX_HP_CURRENT_GAME,
    SHIELD_MAX_HP,
    SHIELD_REGEN_RATE,
    SHIELD_REGEN_COOLDOWN,

    MOVE_SPEED_CURRENT_GAME,
    XP_GAIN_CURRENT_GAME

}
