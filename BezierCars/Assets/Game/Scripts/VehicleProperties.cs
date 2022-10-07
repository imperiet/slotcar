using UnityEngine;

namespace Thoreas.Vehicles
{
    [System.Serializable]
    [CreateAssetMenu(fileName = "VehicleProperties", menuName = "Vehicle/Vehicle Properties")]
    public class VehicleProperties : ScriptableObject
    {
        public AnimationCurve drivingForceCurve;

        [Range(0f,200f)]
        public float sidewaysAccelerationLimit;

        public float rigidbodySleepSpeed;

        public bool flipDirection;
    }
}