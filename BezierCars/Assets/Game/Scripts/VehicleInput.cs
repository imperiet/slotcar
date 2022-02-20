using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.InputSystem;

public class VehicleInput : MonoBehaviour
{
    public Action<float> OnThrottle;
    public Action OnThrottleDown, OnThrottleUp;

    public InputAction throttle;
    [ShowNonSerializedField] float throttleValue;

    void OnEnable()
    {
        throttle.Enable();

        throttle.canceled += (InputAction.CallbackContext obj) => { if (OnThrottleUp != null) OnThrottleUp(); };
        throttle.performed += (InputAction.CallbackContext obj) => { if (OnThrottleDown != null) OnThrottleDown(); };

    }
    void OnDisabled()
    {
        throttle.canceled -= (InputAction.CallbackContext obj) => { if (OnThrottleUp != null) OnThrottleUp(); };
        throttle.performed -= (InputAction.CallbackContext obj) => { if (OnThrottleDown != null) OnThrottleDown(); };
        throttle.Disable();
    }

    void Update()
    {
        throttleValue = throttle.ReadValue<float>();

        if (throttleValue != 0f)
        {
            OnThrottle(throttleValue);
        }
    }
}
