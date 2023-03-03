using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DefaultEnemyProfile", menuName = "EnemyProfiles/DefaultEnemyProfile", order = 0)]
public class EnemyProfile : ScriptableObject
{
    public string enemyName;
    public int enemyTypeID;
    public Sprite sprite;
    public float maxHp = 100f;
    public float hpRegenRate = 0f;
    public float maxShieldHp = 0f;
    public float shieldRegenRate = 1f;
    public float shieldOnHitRegenCooldown = 5f;
    public float damageValue = 0f;
    public float dotDamageValue = 5f;
    public float dotInterval = 0.1f;
    public float moveSpeed = 10f;
    public Vector2 scale = new Vector2(1, 1);
    public Vector2 colliderSize = new Vector2(1, 1);

    public List<DropInfo> dropInfo = new List<DropInfo>();
    public DespawnType despawnType = DespawnType.REGULAR;
}

[System.Serializable]
public enum DespawnType
{
    REGULAR,
    NEVER,
    FAR
}
