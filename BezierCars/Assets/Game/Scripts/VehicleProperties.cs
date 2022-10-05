using UnityEngine;

namespace Thoreas.Vehicles
{
    [System.Serializable]
    [CreateAssetMenu(fileName = "VehicleProperties", menuName = "Vehicle/Vehicle Properties")]
    public class VehicleProperties : ScriptableObject
    {
        public AnimationCurve drivingForceCurve;

        public float slowestSpeed;
        [Range(0f,200f)]
        public float sidewaysAccelerationLimit;
    }
}