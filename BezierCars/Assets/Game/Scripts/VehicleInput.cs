using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName ="VehicleInputAsset",menuName ="Vehicle/Input/Vehicle Input Asset")]
public class VehicleInput : ScriptableObject
{
    public Action<float> OnThrottle;
    public Action OnThrottleDown, OnThrottleUp;

    public InputAction throttle;
    [ShowNonSerializedField] float throttleValue;

    public void Setup()
    {
        throttle.Enable();

        throttle.canceled += (InputAction.CallbackContext obj) => { if (OnThrottleUp != null) OnThrottleUp(); };
        throttle.performed += (InputAction.CallbackContext obj) => { if (OnThrottleDown != null) OnThrottleDown(); };

    }
    public void Stop()
    {
        throttle.canceled -= (InputAction.CallbackContext obj) => { if (OnThrottleUp != null) OnThrottleUp(); };
        throttle.performed -= (InputAction.CallbackContext obj) => { if (OnThrottleDown != null) OnThrottleDown(); };
        throttle.Disable();
    }

    public float QueryThrottle()
    {
        throttleValue = throttle.ReadValue<float>();

        if (throttleValue != 0f)
        {
            OnThrottle?.Invoke(throttleValue);
        }

        return throttleValue;
    }
}
