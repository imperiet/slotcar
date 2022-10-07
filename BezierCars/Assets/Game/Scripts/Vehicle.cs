using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

namespace Thoreas.Vehicles
{
    public class Vehicle : MonoBehaviour
    {
        [Expandable] [SerializeField] VehicleProperties properties;
        [Expandable] [SerializeField] VehicleInput input;
        [Foldout("Object References")] [SerializeField] private Rigidbody carRigidbody, slotPinRigidbody;
        [Foldout("Object References")] [SerializeField] private SplineMesh.Spline spline;
        [Foldout("Object References")]
        [ShowAssetPreview] [SerializeField] private GameObject breakOffCarPrefab;

        private VehicleVisuals visuals;
        private HingeJoint joint;
        private Coroutine queryThrottle;
        private bool offTrack;

        [HorizontalLine(color: EColor.Black)]
        [Space]

        [ProgressBar("Sideways Acceleration", 300, EColor.Red)]
        [SerializeField] float xAcceleration = 0f;

        [ProgressBar("Driving Force", 1, EColor.Yellow)]
        [SerializeField] float currentDrivingForce;
        [ProgressBar("Relative Angle", 180, EColor.Blue)]
        [SerializeField] float relativeAngle;

        public float RelativeAngle { get => relativeAngle; }
        public Rigidbody CarRigidbody { get => carRigidbody; set => carRigidbody = value; }
        public float XAcceleration { get => xAcceleration; set => xAcceleration = value; }

        public Action<float> OnDrive;

        protected virtual void OnEnable()
        {
            if (input) SetupVehicleInput(input);

            joint = GetComponent<HingeJoint>();
            visuals = GetComponent<VehicleVisuals>();
        }
        protected virtual void OnDisable()
        {
            if (input) ReleaseVehicleInput(input);
        }

        public void SetupVehicleInput(VehicleInput vehicleInput)
        {
            Debug.Log("Successfully set up input module " + vehicleInput.name + " with vehicle " + this.name, this);

            vehicleInput.Setup();

            vehicleInput.OnThrottle += Drive;
            vehicleInput.OnThrottleUp += StopThrottle;
            vehicleInput.OnThrottleDown += StartThrottle;

            queryThrottle = StartCoroutine(QueryThrottle());
        }

        public void ReleaseVehicleInput(VehicleInput vehicleInput)
        {
            vehicleInput.OnThrottle -= Drive;
            vehicleInput.OnThrottleUp -= StopThrottle;
            vehicleInput.OnThrottleDown -= StartThrottle;

            StopCoroutine(queryThrottle);
            queryThrottle = null;
        }

        protected virtual void StartThrottle()
        {

        }

        public virtual void Drive(float throttle)
        {
            currentDrivingForce = throttle;

            OnDrive?.Invoke(throttle);
        }

        protected virtual void StopThrottle()
        {
            currentDrivingForce = 0;
        }

        private IEnumerator QueryThrottle()
        {
            while (true)
            {
                yield return null;
                input.QueryThrottle();
            }
        }

        void FixedUpdate()
        {
            //If Drivingforce is not zero, drive the car, else if speed is very low, sleep the rigidbody
            if (currentDrivingForce != 0)
            {
                carRigidbody.AddForce(carRigidbody.transform.TransformDirection(Vector3.forward) * properties.drivingForceCurve.Evaluate(currentDrivingForce));
            }
            else if (carRigidbody.velocity.magnitude < properties.rigidbodySleepSpeed)
            {
                carRigidbody.Sleep();
                slotPinRigidbody.Sleep();
            }

            //Move Vehicle to position relative track
            slotPinRigidbody.transform.position = spline.GetProjectionSample(slotPinRigidbody.position).location;
            slotPinRigidbody.transform.rotation = spline.GetProjectionSample(slotPinRigidbody.position).Rotation;
            if (properties.flipDirection)
            {
                slotPinRigidbody.transform.Rotate(Vector3.up * 180);
            }

            //Calculate sideways acceleration
            xAcceleration = GetXAcceleration(carRigidbody.transform.InverseTransformDirection(carRigidbody.velocity).x);

            //Calculate angle relative track
            Vector3 trackForwardVector = spline.GetProjectionSample(slotPinRigidbody.position).Rotation * Vector3.back;
            /*
            relativeAngle = Vector3.Angle(carRigidbody.transform.forward, trackForwardVector);
            relativeAngle = relativeAngle >= 90f ? 180f - relativeAngle : relativeAngle;
            */

            relativeAngle = Mathf.Abs(joint.angle);

            if (CalculateBreakoff())
            {
                StartCoroutine(Break());
            }

            //Debug
            Debug.DrawRay(slotPinRigidbody.transform.position, trackForwardVector);
            Debug.DrawRay(slotPinRigidbody.transform.position, carRigidbody.transform.forward);

        }

        /// <summary>
        /// Returns true if car should fly off track
        /// </summary>
        /// <returns></returns>
        private bool CalculateBreakoff()
        {

            //If Both max angle and max sideways acceleration is exceeded, return true.
            return (relativeAngle > Mathf.Abs(joint.limits.max) && xAcceleration > properties.sidewaysAccelerationLimit);

        }

        float lastXVelocity = 0;
        private float GetXAcceleration(float newXVelocity)
        {
            float acceleration = newXVelocity - lastXVelocity / Time.fixedDeltaTime;
            lastXVelocity = newXVelocity;
            return MathF.Abs(acceleration);
        }

        private IEnumerator Break()
        {
            if (!offTrack)
            {
                offTrack = true;

                GameObject clone = CreateCloneDummy(carRigidbody.velocity, carRigidbody.angularVelocity);

                visuals.VehicleVisibility = false;

                carRigidbody.isKinematic = true;
                carRigidbody.velocity = Vector3.zero;

                yield return new WaitForSeconds(1f);
                carRigidbody.isKinematic = false;

                visuals.VehicleVisibility = true;
                offTrack = false;
                Destroy(clone);
            }
        }

        private GameObject CreateCloneDummy(Vector3 velocity, Vector3 angularVelocity)
        {
            GameObject clone = Instantiate(breakOffCarPrefab, carRigidbody.transform.position, carRigidbody.transform.rotation);

            Rigidbody cloneRigidbody = clone.GetComponent<Rigidbody>();

            cloneRigidbody.velocity = velocity;
            cloneRigidbody.angularVelocity = angularVelocity;
            cloneRigidbody.AddForce(Vector3.up * 10f);

            return clone;
        }
    }
}