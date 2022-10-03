using UnityEngine;

namespace Thoreas.Vehicles
{
    [System.Serializable]
    [CreateAssetMenu(fileName = "VehicleProperties", menuName = "Vehicle/Vehicle Properties")]
    public class VehicleProperties : ScriptableObject
    {
        public AnimationCurve breakingForceCurve;

        public float drivingForceModifier, slowestSpeed, breakingAngle, breakingSpeed;
    }
}