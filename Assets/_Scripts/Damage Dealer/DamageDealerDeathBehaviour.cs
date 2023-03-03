using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SOEvents;

public class DamageDealerDeathBehaviour : MonoBehaviour
{
    [SerializeField] GameObjectFloatSOEvent despawnEvent;
    [SerializeField] DamageDealerConfigurer damageDealerConfigurer;

    private void Awake() {
        despawnEvent.AddListener(RunIfDamageDealer);
    }

    private void RunIfDamageDealer(GameObject oldDamageDealer, float arg1)
    {
        if (oldDamageDealer.TryGetComponent(out DamageDealer oldDD))
        {
            if (oldDD.deathType == DDDeathType.NONE) return;
            if (oldDD.deathType == DDDeathType.SPAWN_PROJECTILE) 
            {
                for (int i = 0; i < (int)oldDD.deathCustomValue; i++)
                {
                    GameObject newDD = damageDealerConfigurer.CreateDamageDealer(oldDD.onDeathDamageDealerID,
                     oldDD.createdBy.GetComponent<Weapon>());
                    newDD.transform.position = oldDD.transform.position;
                    newDD.GetComponent<Rigidbody2D>().velocity = Random.insideUnitCircle.normalized *
                     newDD.GetComponent<Rigidbody2D>().velocity.magnitude;  
                }

            }
        }
    }
}
