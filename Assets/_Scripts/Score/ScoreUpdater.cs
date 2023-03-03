using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SOEvents;
using System;

public class ScoreUpdater : MonoBehaviour
{
    [SerializeField] FloatSOEvent updatePlayerMoneyEvent;
    [SerializeField] ScoreObject scoreObject;
    [SerializeField] GameModifiers gameModifiers;
    [SerializeField] float scorePerSecond = 5f;

    private void Awake() {
        scoreObject.score = 0;
        updatePlayerMoneyEvent.AddListener(AddMoneyValueToScore);
    }

    private void FixedUpdate() {
        scoreObject.score += Time.fixedDeltaTime * scorePerSecond;
    }

    private void AddMoneyValueToScore(float value)
    {
        if (value > 0) scoreObject.score += (value + gameModifiers.GetAdditionValueOfType(UpgradeType.XP_GAIN_CURRENT_GAME)) 
         * gameModifiers.GetMultiplicationValueOfType(UpgradeType.XP_GAIN_CURRENT_GAME);
    }
}
