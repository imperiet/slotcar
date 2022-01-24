using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SplineMesh;
using NaughtyAttributes;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System;

public class FollowPath : MonoBehaviour
{
    [SerializeField] Spline spline;
    [SerializeField] Transform testPosObject, testCursor;
    [SerializeField] float testDistance;
    [SerializeField] Slider throttleSlider;
    [SerializeField] Rigidbody slotPinRigidbody, carRigidbody;
    [SerializeField] float throttleModifier;
    [ShowNonSerializedField] float throttleValue;

    public InputAction throttle;

    void OnEnable()
    {
        throttle.Enable();
        throttle.performed += throttlePressed;
    }
    void OnDisable()
    {
        throttle.performed -= throttlePressed;
    }

    private void throttlePressed(InputAction.CallbackContext obj)
    {
        Debug.Log("Throttle action pressed");
    }

    void Update()
    {
        throttleValue = throttle.ReadValue<float>();
        
        Debug.Log(throttle.ReadValue<float>());
    }

    void FixedUpdate()
    {
        if (throttleValue != 0f)
        {
            carRigidbody.AddForce(carRigidbody.transform.TransformDirection(Vector3.forward) * throttleValue * throttleModifier);
        }
        slotPinRigidbody.transform.position = spline.GetProjectionSample(slotPinRigidbody.position).location;
        slotPinRigidbody.transform.rotation = spline.GetProjectionSample(slotPinRigidbody.position).Rotation;
    }

    [Button]
    public void TestPlace()
    {
        /*
        Debug.Log(spline.Length);
        var splineSample = spline.GetSampleAtDistance(testDistance);
        testPosObject.transform.position = splineSample.location;
        */
        var curveSample = spline.GetProjectionSample(testCursor.position);
        testPosObject.transform.position = curveSample.location;
    }

    static float GetLoopingSplineTime(float span, float t)
    {
        float adjustedValue = t;

        if (t > span)
        {
            while (adjustedValue > span)
            {
                adjustedValue -= span;
            }

        }
        if (t < 0)
        {
            while (adjustedValue < 0)
            {
                adjustedValue += span;
            }
        }
        return adjustedValue;
    }
}