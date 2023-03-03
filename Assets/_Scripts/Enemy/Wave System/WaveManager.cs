using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SOEvents;
using System;

public class WaveManager : MonoBehaviour
{
    // Coroutine that handles spawning from a wavedetails object.
    // Stores waves.

    [SerializeField] IntSOEvent basicSpawnEnemyEvent;
    public List<WaveDetails> waves = new List<WaveDetails>();
    public float currentTime = 0;


    float currentWaveSpawnInterval;
    int currentWaveNumEnemiesPerYield;

    private void Start() 
    {
        StartCoroutine(RunWaves());
    }

    IEnumerator RunWaves()
    {
        foreach (var wave in waves)
        {
            wave.ConfigureMetaData();
            ConfigureCurrentWaveSpawningCharacteristics(wave);

            if (currentWaveSpawnInterval < 0.02) 
            {

            }

            while (wave.totalNumEnemies > 0)
            {
                for (int i = 0; i < Mathf.Min(currentWaveNumEnemiesPerYield, wave.totalNumEnemies); i++)
                {
                    basicSpawnEnemyEvent.Invoke(wave.RandomlyPickEnemyID());   
                }
                yield return new WaitForSeconds(currentWaveSpawnInterval);
            }
            Debug.Log("Wave Completed");
        }
        Debug.Log("All waves have been executed");
        yield return null;
    }

    private void ConfigureCurrentWaveSpawningCharacteristics(WaveDetails wave)
    {
        currentWaveSpawnInterval = 1.0f / wave.spawnRate;
        currentWaveNumEnemiesPerYield = 1;
        while (currentWaveSpawnInterval < 0.02f)
        {
            currentWaveSpawnInterval *= 2;
            currentWaveNumEnemiesPerYield *= 2;
        }
    }
}
