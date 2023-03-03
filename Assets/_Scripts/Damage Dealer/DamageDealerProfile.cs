using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DefaultDamageDealerProfile", menuName = "DamageDealerProfiles/DefaultDamageDealerProfile", order = 0)]
public class DamageDealerProfile : ScriptableObject
{
    public int damageDealerTypeID;
    public string damageDealerName;
    
    public Vector2 scale = new Vector2(0.2f, 0.2f);
    public Vector2 triggerScale = new Vector2(1.5f, 1.5f);
    public Sprite sprite;

    [SerializeField] public AudioClip creationSoundClip;
    [SerializeField] [Range(0.0f, 1.0f)] public float creationSoundVolume;
    public bool creationSoundIsDiscrete = true;

    public float lifeSpan;

    public float damageValue;
    public float dotDamageValue;
    public float dotInterval;
    // basePiercing == DamageDealer.life.
    public int basePiercing;

    public float baseMunitionSpeed;

    public DDBehaviourType behaviourType = DDBehaviourType.NONE;
    [Range(0f, 5f)] public float customValue;
    [Range(0f, 10f)] public float customValue2;

    public DDDeathType deathType = DDDeathType.NONE;
    public int onDeathDamageDealerID = -1;
    public float deathCustomValue = 1;
}

public enum DDBehaviourType 
{
    NONE,
    SLOWS,
    HOMING,
    GROWTH
}

public enum DDDeathType
{
    NONE,
    SPAWN_PROJECTILE
}
