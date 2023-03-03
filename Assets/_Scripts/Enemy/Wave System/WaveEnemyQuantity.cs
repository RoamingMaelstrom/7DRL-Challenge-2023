using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WaveEnemyQuantity
{
    public int enemyTypeId;
    public int initialEnemyQuantity;

    [HideInInspector]
    public int enemyQuantity;

    public void Init()
    {
        enemyQuantity = initialEnemyQuantity;
    }
}
