using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] public Transform targetTransform;
    [SerializeField] Rigidbody2D body;
    [SerializeField] public float moveSpeed;

    public int updateRotationWait = 2;
    int updateRotationCounter = 0;

    private void FixedUpdate() {
        MoveTowardPlayer();
    }


    public void MoveTowardPlayer()
    {
        float sqrDistanceFromPlayer = (body.transform.position - targetTransform.position).sqrMagnitude;

        Vector2 targetRotationVector = (body.transform.position - targetTransform.position).normalized;
        body.velocity = - moveSpeed * targetRotationVector * Time.fixedDeltaTime;

        updateRotationCounter --;
        if (updateRotationCounter > 0) return;
        
        updateRotationCounter = updateRotationWait;
        // if Statement needed to prevent enemies rotating violently when very close to the player.
        if (sqrDistanceFromPlayer > 0.25f)
        {
            float angle = Mathf.Atan2(targetRotationVector.x, targetRotationVector.y) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, - angle + 180);
        }
    }

}
