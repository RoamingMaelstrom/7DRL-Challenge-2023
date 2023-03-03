using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleShotWeapon : Weapon
{
    public override void Fire()
    {
        requestTargetEvent.Invoke(this);
        requestDamageDealerEvent.Invoke(this);

        currentDamageDealerObject.transform.position = transform.position;
        // Copied from TripleShotWeapon Coroutine.
        Rigidbody2D projectileBody = currentDamageDealerObject.GetComponent<Rigidbody2D>();
        Vector2 newVel = projectileBody.velocity.magnitude * (currentTarget - transform.position).normalized;
        newVel += new Vector2(newVel.y * Random.Range(-1 + accuracyCoefficient, 1 - accuracyCoefficient) / 4f,
            newVel.x * Random.Range(-1 + accuracyCoefficient, 1 - accuracyCoefficient) / 4f);
        projectileBody.velocity = newVel;
        
        SetCurrentDamageDealerRotationToVelocity();
    }
}
