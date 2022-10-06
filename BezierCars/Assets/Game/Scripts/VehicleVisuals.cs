using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Thoreas.Vehicles
{
    public class VehicleVisuals : MonoBehaviour
    {
        private Vehicle vehicle;
        private bool vehicleVisibility;
        [SerializeField] private GameObject visualsParent;
        [SerializeField] private ParticleSystem tireSmokeParticleSystem;

        [SerializeField] AnimationCurve tireSmokeEmissionRelativeDriveForce;

        public bool VehicleVisibility
        {
            get => vehicleVisibility; set
            {
                visualsParent.SetActive(value);
                vehicleVisibility = value;
            }
        }
        private void OnEnable()
        {
            if (vehicle == null) vehicle = GetComponent<Vehicle>();
        }

        private void Update()
        {
            tireSmokeParticleSystem.emissionRate = tireSmokeEmissionRelativeDriveForce.Evaluate(vehicle.CarRigidbody.velocity.magnitude);
        }
    }
}