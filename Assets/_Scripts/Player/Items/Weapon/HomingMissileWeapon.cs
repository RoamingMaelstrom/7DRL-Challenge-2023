using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingMissileWeapon : Weapon
{
    DamageDealerBehaviourLogic[] activeMissiles = new DamageDealerBehaviourLogic[16];
    GameObject[] targets = new GameObject[16];

    LayerMask enemyLayerMask;

    private void Awake() 
    {
        StartCoroutine(UpdateMissileTargeting());
        enemyLayerMask = LayerMask.GetMask("Enemy");
    }

    public override void Fire()
    {
        requestDamageDealerEvent.Invoke(this);
        requestTargetEvent.Invoke(this);

        currentDamageDealerObject.transform.position = transform.position;

        DamageDealerBehaviourLogic behaviourLogic = currentDamageDealerObject.GetComponent<DamageDealerBehaviourLogic>();

        Collider2D nearestEnemyCollider = Physics2D.OverlapCircle(currentTarget, 5, enemyLayerMask);
        behaviourLogic.targetGameObject = (nearestEnemyCollider != null) ? nearestEnemyCollider.gameObject: null;
        
        if (nearestEnemyCollider)
        {
            Rigidbody2D dDBody = currentDamageDealerObject.GetComponent<Rigidbody2D>();
             dDBody.velocity = ((nearestEnemyCollider.transform.position - transform.position).normalized
             + new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0)).normalized * dDBody.velocity.magnitude;

            SetBodyRotationToVelocity(dDBody);
        }





        for (int i = 0; i < activeMissiles.Length; i++)
        {
            if (activeMissiles[i] is null) 
            {
                activeMissiles[i] = behaviourLogic;
                targets[i] = behaviourLogic.targetGameObject;
            }

        }

    }

    IEnumerator UpdateMissileTargeting()
    {
        for (int i = 0; i < activeMissiles.Length; i++)
        {
            if (activeMissiles[i] == null) continue;
            if (!activeMissiles[i].gameObject.activeInHierarchy)
            {
                activeMissiles[i] = null;
                targets[i] = null;
                continue;
            }

            if (targets[i] == null) continue;
            if (!targets[i].activeInHierarchy)
            {
                activeMissiles[i].behaviour = DDBehaviourType.NONE;
                targets[i] = null;
                continue;
            }
        }

        yield return new WaitForFixedUpdate();
    }


}
