using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SweepingLaserBeamWeapon : Weapon
{
    LineRenderer lineRenderer;

    Coroutine currentSweepFireLaser;

    private void Start() {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.widthMultiplier = 0.3f;
    }


    public override void Fire()
    {
        if (currentSweepFireLaser != null) StopCoroutine(currentSweepFireLaser);
        currentSweepFireLaser = StartCoroutine(SweepFireLaser());
    }

    IEnumerator SweepFireLaser()
    {
        // Needed to draw line.
        lineRenderer.positionCount = 2;

        requestDamageDealerEvent.Invoke(this);
        requestTargetEvent.Invoke(this);
        requestCooldownEvent.Invoke(this);

        Debug.Log(currentTarget);

        // How long the coroutine runs.
        float durationRemaining = 0.25f + (currentCd * 0.5f);
        currentDamageDealerObject.GetComponent<LifeSpanLimiter>().spanRemaining = durationRemaining;


        Vector3 currentTargetVariance = Random.insideUnitCircle * 20;

   
        Vector3 endTarget = currentTarget + new Vector3(currentTargetVariance.x, currentTargetVariance.y); 
        // How much currentTarget changes by each iteration.
        Vector3 stepSize = (currentTarget - endTarget) / (durationRemaining / Time.fixedDeltaTime) * 2;


        while(durationRemaining > 0)
        {
            currentTarget = transform.position + ((currentTarget - transform.position).normalized * 30f);
            currentTarget += stepSize;

            RaycastHit2D firstHit;
            firstHit = Physics2D.Raycast(transform.position, currentTarget - transform.position, 30f, LayerMask.GetMask("Enemy"));

            lineRenderer.SetPosition(0, transform.position);


            if (!firstHit.transform) 
            {
                lineRenderer.SetPosition(1, currentTarget);
                currentDamageDealerObject.transform.position = transform.position;
            }
            else
            {
                lineRenderer.SetPosition(1, transform.position + (firstHit.distance * (currentTarget - transform.position) / 30f));
                currentDamageDealerObject.transform.position = transform.position + (firstHit.distance * (currentTarget - transform.position) / 30f);
            }



            durationRemaining -= Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }

        // Makes the line renderer not visible once firing stops.
        lineRenderer.positionCount = 0;
        yield return null;
    }
}
