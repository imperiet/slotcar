using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class Vehicle : MonoBehaviour
{
    [Foldout("Object References")] [SerializeField] VehicleInput input;

    [Foldout("Object References")] [SerializeField] private Rigidbody carRigidbody, slotPinRigidbody;
    [Foldout("Object References")] [SerializeField] private SplineMesh.Spline spline;
    [SerializeField] float drivingForceModifier;
    [SerializeField] float slowestSpeed;
    [ShowNonSerializedField] float currentDrivingForce;

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
        currentDrivingForce = throttle;
    }
    private void StopThrottle()
    {
        currentDrivingForce = 0;
    }
    void FixedUpdate()
    {
        if (currentDrivingForce != 0)
        {
            carRigidbody.AddForce(carRigidbody.transform.TransformDirection(Vector3.forward) * currentDrivingForce * drivingForceModifier);
        }
        else if (carRigidbody.velocity.magnitude < slowestSpeed)
        {
            Debug.Log(carRigidbody.name +" is so slow that it fell asleep");
            carRigidbody.Sleep();
        }

        slotPinRigidbody.transform.position = spline.GetProjectionSample(slotPinRigidbody.position).location;
        slotPinRigidbody.transform.rotation = spline.GetProjectionSample(slotPinRigidbody.position).Rotation;

        //No logic for breaking of track yet. 
    }
}
