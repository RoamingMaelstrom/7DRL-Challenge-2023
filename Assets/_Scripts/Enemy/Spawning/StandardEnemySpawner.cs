using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SOEvents;

public class StandardEnemySpawner : MonoBehaviour
{
    [SerializeField] IntSOEvent basicSpawnEnemyEvent;
    [SerializeField] ObjectPoolMain objectPool;
    [SerializeField] EnemyConfigurer enemyConfigurer;
    [SerializeField] Rigidbody2D playerBody;
    public float minSpawnDistanceFromPlayer = 25f;
    public float maxSpawnDistanceFromPlayer = 50f;

    LayerMask layerMask;

    private void Awake() {
        layerMask = LayerMask.GetMask("Enemy");
        basicSpawnEnemyEvent.AddListener(SpawnEnemy);
    }

    public void SpawnEnemy(int enemyTypeID)
    {
        GameObject newEnemy = objectPool.GetObject(0);
        newEnemy = enemyConfigurer.ConfigureEnemy(newEnemy, enemyTypeID);
        SetupEnemyPosition(newEnemy);
    }

    void SetupEnemyPosition(GameObject newEnemy)
    {
        Vector3 enemySpawnPos = GetFreeSpawnPosition(0.5f);
        Transform enemyBodyTransform = newEnemy.transform.GetChild(0);
        enemyBodyTransform.GetComponent<EnemyMovement>().targetTransform = playerBody.transform;
        enemyBodyTransform.position = enemySpawnPos;
    }

    // Tries to find a position where no other enemy is currently situated.
    private Vector3 GetFreeSpawnPosition(float radiusToCheck)
    {
        float minDistance = minSpawnDistanceFromPlayer;
        float maxDistance = maxSpawnDistanceFromPlayer;
        Vector3 spawnPos = Vector3.zero;
        bool running = true;
        while (running)
        {
            // If the spawnPos will be almost directly behind the players travelling direction, recalculate spawnPos. 
            // Has the effect of decreasing the probability that an enemy spawns behind the player.
            spawnPos = (Random.insideUnitCircle.normalized + (playerBody.velocity.normalized * Random.Range(0.0f, 1.0f))).normalized;

            spawnPos *= Random.Range(minDistance, maxDistance);
            spawnPos += playerBody.transform.position;
            if (CheckSpawnCircleClear(spawnPos, radiusToCheck)) return spawnPos;
            minDistance += 0.25f;
            maxDistance ++;
        }
        return spawnPos;
    }

    private bool CheckSpawnCircleClear(Vector3 centrePoint, float radiusToCheck)
    {
        if (Physics2D.OverlapCircle(centrePoint, radiusToCheck, layerMask)) return false;
        return true;
    }
}
