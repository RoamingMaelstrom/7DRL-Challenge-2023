using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnPlayerWeapon : Weapon
{

    Coroutine currentKeepDamageDealerOnPlayer;

    // Only allowed to have one damage dealer on the player at a time.
    public override void Fire()
    {
        // Stops the previous Coroutine keeping the previous damage dealer on the player.
        if (currentDamageDealerObject) currentDamageDealerObject.GetComponent<LifeSpanLimiter>().spanRemaining = 0;
        if (currentKeepDamageDealerOnPlayer != null) StopCoroutine(currentKeepDamageDealerOnPlayer);

        requestDamageDealerEvent.Invoke(this);
        currentDamageDealerObject.transform.position = transform.position;

        currentKeepDamageDealerOnPlayer = StartCoroutine(KeepDamageDealerOnPlayer());
    }

    IEnumerator KeepDamageDealerOnPlayer()
    {
        while (currentDamageDealerObject.activeInHierarchy)
        {

            if (currentDamageDealerObject) currentDamageDealerObject.transform.position = transform.position;
            yield return new WaitForFixedUpdate();
        }
        yield return null;
    }
}
