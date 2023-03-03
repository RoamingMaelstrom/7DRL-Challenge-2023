using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameModifiers : MonoBehaviour
{
    public List<GameStatModifier> stats = new List<GameStatModifier>();

    public GameStatModifier GetGameStatModOfType(UpgradeType upgradeType)
    {
        foreach (var stat in stats)
        {
            if (stat.MatchesType(upgradeType)) return stat;
        }

        return null;
    }

    public float GetAdditionValueOfType(UpgradeType upgradeType)
    {
        GameStatModifier statMod = GetGameStatModOfType(upgradeType);
        if (statMod != null) return statMod.additionValue;
        return -123.123f;
    }

    public float GetMultiplicationValueOfType(UpgradeType upgradeType)
    {
        GameStatModifier statMod = GetGameStatModOfType(upgradeType);
        if (statMod != null) return statMod.multiplicationValue;
        return -123.123f;
    }
}


[System.Serializable]
public class GameStatModifier
{
    public UpgradeType upgradeType;
    public float additionValue = 0;
    public float multiplicationValue = 1;

    public bool MatchesType(UpgradeType otherUpgradeType) => otherUpgradeType == upgradeType ? true: false;

}