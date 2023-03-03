using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using SOEvents;

public class BasePlayerController : MonoBehaviour
{
    [SerializeField] BoolSOEvent pauseEvent;
    [SerializeField] BoolSOEvent togglePauseScreenEvent;
    [SerializeField] public float playerThrust = 250.0f;
    [SerializeField] public float maxPlayerTorque = 4.0f;
    [SerializeField] public float maxAngularVelocity = 180.0f;
    private Vector2 movement;
    private Vector2 mousePos;
    private float isFiring;
    private bool isPaused;

    [SerializeField] Rigidbody2D playerBody;

    void OnMove(InputValue value) => movement = value.Get<Vector2>();

    void OnLook(InputValue value) => mousePos = value.Get<Vector2>();

    void OnFire(InputValue value) => isFiring = value.Get<float>();

    void OnPause(InputValue value) 
    {
        isPaused = (value.Get<float>() == 0) ? isPaused : !isPaused;
        pauseEvent.Invoke(isPaused);
        togglePauseScreenEvent.Invoke(isPaused);
    }


    public void FixedUpdate() 
    {
        if (!playerBody) return;
        HandleThrust();
        HandleRotation();
    }

    void HandleThrust()
    {
        Vector2 force = movement * playerThrust * Time.fixedDeltaTime;
        playerBody.AddRelativeForce(force);
    }

    public void HandleRotation()
    {
        float currentRotation = playerBody.transform.rotation.eulerAngles.z;
        float targetRotation = GetTargetRotation();
        float deltaRotation = targetRotation - currentRotation;

        SetPlayerTorque(deltaRotation, GetWorkingTorqueValue(deltaRotation, maxPlayerTorque, maxAngularVelocity), maxAngularVelocity);
        DampenPlayerWobble(deltaRotation, playerBody);
    }

    private float GetTargetRotation()
    {
        Vector2 mousePositionWorldSpace = Camera.main.ScreenToWorldPoint(mousePos);

        float targetRotation = - Mathf.Atan2(mousePositionWorldSpace.x - playerBody.transform.position.x,
         mousePositionWorldSpace.y - playerBody.transform.position.y) * Mathf.Rad2Deg;

        if (targetRotation < 0) targetRotation += 360;

        return targetRotation;
    }

    private void SetPlayerTorque(float deltaRotation, float torqueValue, float maxAngularVelocity)
    {
        if (deltaRotation < - 180 || (deltaRotation > 0 && deltaRotation < 180)) playerBody.AddTorque(torqueValue);
        else playerBody.AddTorque(- torqueValue);
        playerBody.angularVelocity = Mathf.Clamp(playerBody.angularVelocity, -maxAngularVelocity, maxAngularVelocity);
    }

    // Allows more precise rotation when deltaRotation is low.
    private float GetWorkingTorqueValue(float deltaRotation, float maxTorque, float maxAngularVelocity) 
    {
        float maxAngularVelocityFixedUpdate = maxAngularVelocity * Time.fixedDeltaTime;
        return Mathf.Clamp((Mathf.Abs(deltaRotation) - maxAngularVelocityFixedUpdate) 
         * maxTorque / maxAngularVelocityFixedUpdate, 0, maxTorque);
    }

    private void DampenPlayerWobble(float deltaRotation, Rigidbody2D playerBody)
    {
        for (int i = 8; i >= 1; i/=2)
        {
            if (i > Mathf.Abs(deltaRotation)) playerBody.angularVelocity = Mathf.Lerp(playerBody.angularVelocity, 0, (8 - i) / 8f);
        }
    }
}
