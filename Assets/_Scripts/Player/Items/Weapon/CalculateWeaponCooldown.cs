using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SOEvents;
using System;

public class CalculateWeaponCooldown : MonoBehaviour
{
    [SerializeField] WeaponSOEvent requestCooldownEvent;
    [SerializeField] GameModifiers gameModifiers;


    private void Awake() {
        requestCooldownEvent.AddListener(SetWeaponCurrentCd);
    }

    void SetWeaponCurrentCd(Weapon weapon)
    {
        weapon.currentCd = (weapon.baseMaxCd + weapon.cooldownAdd + gameModifiers.GetAdditionValueOfType(UpgradeType.ATTACK_SPEED_CURRENT_GAME))
         * weapon.cooldownMod * gameModifiers.GetMultiplicationValueOfType(UpgradeType.ATTACK_SPEED_CURRENT_GAME);
    }
}
