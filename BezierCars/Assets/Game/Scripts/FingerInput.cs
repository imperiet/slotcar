using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "VehicleFingerInputAsset", menuName = "Vehicle/Input/Vehicle Finger Input Asset")]

public class FingerInput : VehicleInput
{
    LeapFingerThrottle fingerThrottle;

    public override void Setup()
    {
        fingerThrottle = FindObjectOfType<LeapFingerThrottle>();
    }

    public override void Stop()
    {

    }

    public override float QueryThrottle()
    {
        float throttleValue = fingerThrottle.QueryThrottle();
        OnThrottle?.Invoke(throttleValue);
        return throttleValue;
    }
}
