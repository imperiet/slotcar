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
    [SerializeField] Slider forceIndicator;
    [SerializeField] Rigidbody slotPinRigidbody, carRigidbody;
    [SerializeField] float throttleModifier;
    [SerializeField] HingeJoint joint;
    [SerializeField] float maxStickingForce;
    [ShowNonSerializedField] float throttleValue, hingeForce;
    //public AnimationCurve forceRelativeToSpeed;
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
    }

    void FixedUpdate()
    {
        if (throttleValue != 0f)
        {
            carRigidbody.AddForce(carRigidbody.transform.TransformDirection(Vector3.forward) * throttleValue * throttleModifier);
        }
        slotPinRigidbody.transform.position = spline.GetProjectionSample(slotPinRigidbody.position).location;
        slotPinRigidbody.transform.rotation = spline.GetProjectionSample(slotPinRigidbody.position).Rotation;

        //Debug.Log(carRigidbody.velocity.magnitude);
        // Vector3 adjustedCarVelocity = carRigidbody.velocity.normalized * forceRelativeToSpeed.Evaluate(carRigidbody.velocity.magnitude);
        // Vector3.Dot(joint.currentForce.normalized, carRigidbody.velocity.normalized);

        hingeForce = joint.currentForce.magnitude;

        if (hingeForce > maxStickingForce)
        {
            StartCoroutine(BreakOffTrack(joint.currentForce));
        }

        forceIndicator.maxValue = maxStickingForce;
        forceIndicator.value = hingeForce;
        forceIndicator.fillRect.GetComponent<Image>().color = Color.Lerp(Color.green, Color.red, hingeForce / maxStickingForce);
    }

    IEnumerator BreakOffTrack(Vector3 breakForce)
    {
        carRigidbody.velocity = Vector3.zero;
        carRigidbody.angularVelocity = Vector3.zero;

        carRigidbody.isKinematic = true;
        yield return new WaitForSeconds(.5f);
        carRigidbody.isKinematic = false;
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
}