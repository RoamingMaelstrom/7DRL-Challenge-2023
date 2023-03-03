using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldDisplayLogic : MonoBehaviour
{
    [SerializeField] public Health objectHealthScript;
    [SerializeField] public SpriteRenderer shieldRenderer;

    private void Awake() {
        if (!shieldRenderer) shieldRenderer = GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate() {
        if (objectHealthScript.shieldHpMax == 0)
        {
            shieldRenderer.color = new Color(1f, 1f, 1f, 0f);
            return;
        }
        float shieldPercentage = objectHealthScript.GetCurrentShieldHp() / objectHealthScript.shieldHpMax;
        shieldRenderer.color = new Color(0.9f, 0.95f, 1f, Mathf.Clamp(shieldPercentage * 0.6f, 0f, 0.6f));
    }



}
