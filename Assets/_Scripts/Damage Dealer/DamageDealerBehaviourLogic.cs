using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageDealerBehaviourLogic : MonoBehaviour
{
    
    public Rigidbody2D body;
    public DDBehaviourType behaviour;
    // movementValue will be used by different behaviour types for different things.
    [SerializeField] [Range(0f, 5f)] public float customValue;
    [SerializeField] [Range(0f, 10f)] public float customValue2;
    // Used by HOMING type. 
    public GameObject targetGameObject;

    private void Awake() {
        body = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate() {
        if (behaviour == DDBehaviourType.NONE) return;
        else if (behaviour == DDBehaviourType.SLOWS) body.velocity = Vector2.Lerp(body.velocity, Vector2.zero, customValue * Time.fixedDeltaTime);
        // customValue controls how quickly body changes direction  (Higher = changes directional velocity faster). 
        // customValue2 stores the body's speed.
        else if (behaviour == DDBehaviourType.HOMING) 
        {
            if (customValue2 <= 0) customValue2 = body.velocity.magnitude;
            if (targetGameObject == null) return;
            Vector2 targetVelDir = (targetGameObject.transform.position - transform.position).normalized;
            body.velocity = Vector2.Lerp(body.velocity.normalized, targetVelDir, customValue * Time.fixedDeltaTime) * customValue2;

            float targetRotation = - Mathf.Atan2(targetGameObject.transform.position.x - body.transform.position.x,
            targetGameObject.transform.position.y - body.transform.position.y) * Mathf.Rad2Deg;

            body.transform.rotation = Quaternion.Euler(0, 0, Mathf.LerpAngle(body.transform.rotation.eulerAngles.z, targetRotation, 0.1f));
        }
        // customValue controls growth rate, customValue2 final growth size.
        else if (behaviour == DDBehaviourType.GROWTH)
        {
            body.transform.localScale = Vector3.Lerp(body.transform.localScale, new Vector3(customValue2, customValue2, customValue2), customValue * Time.fixedDeltaTime);
        }
    }


}
