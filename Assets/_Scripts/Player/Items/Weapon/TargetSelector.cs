using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SOEvents;

public class TargetSelector : MonoBehaviour
{
    [SerializeField] WeaponSOEvent requestTargetEvent;

    LayerMask enemyLayerMask;

    private void Awake() {
        requestTargetEvent.AddListener(ProvideTarget);
        enemyLayerMask = LayerMask.GetMask("Enemy");
    }

    // When called by a Weapon, sets its .currentTarget Value to a particular position (Vector3).
    // Todo: Could probably store some common targets. e.g. Closest 5 enemies, furthest enemy, etc.
    public void ProvideTarget(Weapon weapon)
    {
        // weapon.currentTarget
        if (weapon.targetingType == WeaponTargetingType.FORWARD)
        {
            float weaponRotation = weapon.transform.rotation.eulerAngles.z + 90;
            
            weapon.currentTarget = weapon.transform.position + new Vector3(Mathf.Cos(weaponRotation * Mathf.Deg2Rad), Mathf.Sin(weaponRotation * Mathf.Deg2Rad), 0);
            return;
        }

        else if (weapon.targetingType == WeaponTargetingType.NEAREST)
        {
            float searchRadius = 15;
            Collider2D[] enemiesInRadius;
            while (searchRadius < 100)
            {
                // Gradually increases the search radius until it finds enemies, then finds the nearest out of them. Based around weapon position.
                enemiesInRadius = Physics2D.OverlapCircleAll(weapon.transform.position, searchRadius, enemyLayerMask);
                if (enemiesInRadius.Length > 0) 
                {
                    weapon.currentTarget = GetNearestEnemyPosition(enemiesInRadius, weapon.transform.position);
                    return;
                }
                searchRadius += 15;
            }

            // If a target cannot be found, default to shooting towards 0, 0, 0
            weapon.currentTarget = new Vector3(-123, 123, -10000);
            return;
        }

        else if (weapon.targetingType == WeaponTargetingType.BACKWARD)
        {
            float weaponRotation = weapon.transform.rotation.eulerAngles.z - 90;
            
            weapon.currentTarget = weapon.transform.position + new Vector3(Mathf.Cos(weaponRotation * Mathf.Deg2Rad), Mathf.Sin(weaponRotation * Mathf.Deg2Rad), 0);
            return;
        }

        else if (weapon.targetingType == WeaponTargetingType.RANDOM_ENEMY)
        {
            float searchRadius = 30f;
            Collider2D[] enemiesInRadius = Physics2D.OverlapCircleAll(weapon.transform.position, searchRadius, enemyLayerMask);
            if (enemiesInRadius.Length > 0) 
            {
                weapon.currentTarget = enemiesInRadius[Random.Range(0, enemiesInRadius.Length)].transform.position;
                return;
            }

            weapon.currentTarget = new Vector3(-123, 123, -10000);
        }

        else if (weapon.targetingType == WeaponTargetingType.RANDOM)
        {
            Vector2 randomPoint = Random.insideUnitCircle.normalized * 40;
            weapon.currentTarget = weapon.transform.position + new Vector3(randomPoint.x, randomPoint.y, 0);
            return;
        }

        else if (weapon.targetingType == WeaponTargetingType.FARTHEST)
        {
            float searchRadius = 35f;
            Collider2D[] enemiesInRadius = Physics2D.OverlapCircleAll(weapon.transform.position, searchRadius, enemyLayerMask);

            if (enemiesInRadius.Length == 0) 
            {
                weapon.currentTarget = new Vector3(-123, 123, -10000);
                return;
            }

            float sqrFurthestDistance = 0;
            int furthestEnemyIndex = 0;
            float sqrTempDistance;
            for (int i = 0; i < enemiesInRadius.Length; i++)
            {
                sqrTempDistance = (weapon.transform.position - enemiesInRadius[i].transform.position).sqrMagnitude;
                if (sqrTempDistance > sqrFurthestDistance) 
                {
                    sqrFurthestDistance = sqrTempDistance;
                    furthestEnemyIndex = i;
                }
            }

            weapon.currentTarget = enemiesInRadius[furthestEnemyIndex].transform.position;
            return;
        }

    }

    private Vector3 GetNearestEnemyPosition(Collider2D[] enemiesInRadius, Vector3 pointToCompare)
    {
        Collider2D nearest = null;
        float sqrSmallestDistance = 100000000;
        float sqrCurrentDistance;
        foreach (var enemy in enemiesInRadius)
        {
            sqrCurrentDistance = (enemy.transform.position - pointToCompare).sqrMagnitude;
            if (sqrCurrentDistance < sqrSmallestDistance)
            {
                nearest = enemy;
                sqrSmallestDistance = sqrCurrentDistance;
            }
        }

        return nearest.transform.position;
    }
}
