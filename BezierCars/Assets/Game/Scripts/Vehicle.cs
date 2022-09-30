using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class Vehicle : MonoBehaviour
{
    [Foldout("Object References")][SerializeField] VehicleInput input;

    [Foldout("Object References")][SerializeField] private Rigidbody carRigidbody, slotPinRigidbody;
    [Foldout("Object References")][SerializeField] private SplineMesh.Spline spline;
    [SerializeField] AnimationCurve maxCarRotationBasedOnSpeed;
    [SerializeField] float drivingForceModifier, slowestSpeed, breakingAngle, breakingSpeed;
    [ShowNonSerializedField] float currentDrivingForce, relativeAngle, lastRelativeAngle, carRotationSpeed, maxCarRotationSpeedRelativeTrack;
    [ShowNonSerializedField] bool disableBreakoff,outsideOfMaxAngle;


    public float CurrentDrivingForce { get => currentDrivingForce; set => currentDrivingForce = value; }
    public float RelativeAngle { get => relativeAngle; set => relativeAngle = value; }
    public float LastRelativeAngle { get => lastRelativeAngle; set => lastRelativeAngle = value; }
    public float CarRotationSpeed { get => carRotationSpeed; set => carRotationSpeed = value; }
    public float MaxCarRotationSpeedRelativeTrack { get => maxCarRotationSpeedRelativeTrack; set => maxCarRotationSpeedRelativeTrack = value; }

    private void OnEnable()
    {
        if (input) SetupVehicleInput(input);
    }
    private void OnDisable()
    {
        if (input) ReleaseVehicleInput(input);
    }

    public void SetupVehicleInput(VehicleInput vehicleInput)
    {
        Debug.Log("Successfully set up input module " + vehicleInput.name + " with vehicle " + this.name, this);
        vehicleInput.OnThrottle += Drive;
        vehicleInput.OnThrottleUp += StopThrottle;
        vehicleInput.OnThrottleDown += StartThrottle;
    }

    public void ReleaseVehicleInput(VehicleInput vehicleInput)
    {
        vehicleInput.OnThrottle -= Drive;
        vehicleInput.OnThrottleUp -= StopThrottle;
        vehicleInput.OnThrottleDown -= StartThrottle;
    }

    private void StartThrottle()
    {

    }


    public void Drive(float throttle)
    {
        CurrentDrivingForce = throttle;
    }
    private void StopThrottle()
    {
        CurrentDrivingForce = 0;
    }
    void FixedUpdate()
    {
        if (CurrentDrivingForce != 0)
        {
            carRigidbody.AddForce(carRigidbody.transform.TransformDirection(Vector3.forward) * CurrentDrivingForce * drivingForceModifier);
        }
        else if (carRigidbody.velocity.magnitude < slowestSpeed)
        {
            Debug.Log(carRigidbody.name + " is so slow that it fell asleep");
            carRigidbody.Sleep();
            slotPinRigidbody.Sleep();
        }

        slotPinRigidbody.transform.position = spline.GetProjectionSample(slotPinRigidbody.position).location;
        slotPinRigidbody.transform.rotation = spline.GetProjectionSample(slotPinRigidbody.position).Rotation;




        //Calculate angle relative track
        Vector3 trackForwardVector = spline.GetProjectionSample(slotPinRigidbody.position).Rotation * Vector3.back;
        RelativeAngle = Vector3.Angle(carRigidbody.transform.forward, trackForwardVector);
        RelativeAngle = RelativeAngle >= 90f ? 180f - RelativeAngle : RelativeAngle;

        //Debug
        Debug.DrawRay(slotPinRigidbody.transform.position, trackForwardVector);
        Debug.DrawRay(slotPinRigidbody.transform.position, carRigidbody.transform.forward);

        //Calculate "flick" force
        CarRotationSpeed = Mathf.Abs(LastRelativeAngle - RelativeAngle);
        maxCarRotationSpeedRelativeTrack = maxCarRotationBasedOnSpeed.Evaluate(carRigidbody.velocity.magnitude);
        LastRelativeAngle = RelativeAngle;

        outsideOfMaxAngle = false;
        //If Both max angle and a max speed is exceeded, Break car off track.
        if (RelativeAngle > breakingAngle)
        {
            if (CarRotationSpeed > maxCarRotationSpeedRelativeTrack)
            {
                outsideOfMaxAngle = true;

                Debug.Log("Break");
                if (!disableBreakoff)
                {
                    StartCoroutine(Break());
                }
            }
        }



    }

    private IEnumerator Break()
    {
        carRigidbody.isKinematic = true;
        carRigidbody.velocity = Vector3.zero;
        yield return new WaitForSeconds(1f);
        carRigidbody.isKinematic = false;
        disableBreakoff = true;
        yield return new WaitForSeconds(.5f);
        disableBreakoff = false;
    }
}
