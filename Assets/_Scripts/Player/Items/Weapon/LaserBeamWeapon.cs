using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBeamWeapon : Weapon
{
    ContactFilter2D enemyFilter;
    LineRenderer lineRenderer;


    private void Awake() {
        enemyFilter.layerMask = LayerMask.GetMask("Enemy");
    }

    private void Start() {
        requestDamageDealerEvent.Invoke(this);
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
        lineRenderer.widthMultiplier = 0.2f;
        StartCoroutine(RefreshDamageDealer());
    }

    // Works by casting a ray in the direction of currentTarget, drawing the Linerenderer between weapon position and currentTarget, and 
    // moving the DamageDealer to the currentTarget.
    public override void Fire()
    {
        List<RaycastHit2D> hits = new List<RaycastHit2D>();
        requestTargetEvent.Invoke(this);

        Physics2D.Raycast(transform.position, currentTarget, enemyFilter, hits);
        lineRenderer.SetPosition(0, transform.position);

        if ((transform.position - currentTarget).sqrMagnitude > 625)
        {
            lineRenderer.SetPosition(1, transform.position);  
            currentDamageDealerObject.transform.position = transform.position;
            return;
        }
        lineRenderer.SetPosition(1, currentTarget);

        currentDamageDealerObject.transform.position = currentTarget;

    }

    // Needed in case player decides to upgrade this weapon.
    IEnumerator RefreshDamageDealer()
    {
        while (true)
        {
            requestDamageDealerEvent.Invoke(this);
            yield return new WaitForSeconds(2f);
        }
    }
}
