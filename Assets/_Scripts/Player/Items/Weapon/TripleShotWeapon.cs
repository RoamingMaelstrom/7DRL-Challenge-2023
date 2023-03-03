using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TripleShotWeapon : Weapon
{

    public override void Fire()
    {
        StartCoroutine(FireThreeShots());
    }

    IEnumerator FireThreeShots()
    {
        for (int i = 0; i < 3; i++)
        {
            requestDamageDealerEvent.Invoke(this);
            requestTargetEvent.Invoke(this);
            currentDamageDealerObject.transform.position = transform.position;

            // Assumes that returned DamageDealer's rigidbody is set to desired velocity.
            Rigidbody2D projectileBody = currentDamageDealerObject.GetComponent<Rigidbody2D>();
            Vector2 newVel = projectileBody.velocity.magnitude * (currentTarget - transform.position).normalized;
            newVel += new Vector2(newVel.y * Random.Range(-1 + accuracyCoefficient, 1 - accuracyCoefficient) / 4f,
             newVel.x * Random.Range(-1 + accuracyCoefficient, 1 - accuracyCoefficient) / 4f);
            projectileBody.velocity = newVel;
            SetCurrentDamageDealerRotationToVelocity();
            
            yield return new WaitForSeconds(Mathf.Max((baseMaxCd * cooldownMod) / 10f, 0.02f));
        }
        yield return null;
    }
}
