using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WaveDetails", menuName = "Waves/WaveDetails", order = 0)]
public class WaveDetails : ScriptableObject
{
    public float duration;
    public List<WaveEnemyQuantity> enemiesToSpawn = new List<WaveEnemyQuantity>();

    // NOTE: DOES NOT CURRENTLY SUPPORT CHANGING enemiesToSpawn AT RUNTIME.
    [HideInInspector]
    public int totalNumEnemies;
    [HideInInspector]
    public float spawnRate;

    // NOTE: THIS MUST BE RUN FOR spawnRate AND totalNumEnemies TO HAVE THE CORRECT VALUES.
    public void ConfigureMetaData()
    {   
        totalNumEnemies = 0;
        foreach (var waveEQ in enemiesToSpawn)
        {
            waveEQ.Init();
            totalNumEnemies += waveEQ.enemyQuantity;
        }
        spawnRate = totalNumEnemies / duration;
    }

    public int RandomlyPickEnemyID()
    {
        int randNum = Random.Range(0, totalNumEnemies);
        int runningTotal = 0;
        foreach (var waveEQ in enemiesToSpawn)
        {
            if (randNum < runningTotal + waveEQ.enemyQuantity) 
            {
                waveEQ.enemyQuantity --;
                totalNumEnemies --;
                return waveEQ.enemyTypeId;
            }
            runningTotal += waveEQ.enemyQuantity;
        }
        throw new System.Exception("This should not have happened");
    }
}
