using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VehicleDebugGraphics : MonoBehaviour
{
    [SerializeField] Vehicle vehicle;
    [SerializeField] Slider drivingForceSlider,
        relativeAngleSlider,
        carRotationSpeedSlider,
        maxCarRotationSpeedRelativeTrackSlider;
    void Start(){
        if(!vehicle) vehicle = FindObjectOfType<Vehicle>();
    }
    void Update (){
        drivingForceSlider.value = vehicle.CurrentDrivingForce;
        carRotationSpeedSlider.value = vehicle.CarRotationSpeed;
        maxCarRotationSpeedRelativeTrackSlider.value = vehicle.MaxCarRotationSpeedRelativeTrack;
        relativeAngleSlider.value = vehicle.RelativeAngle/90f;
        relativeAngleSlider.targetGraphic.color = vehicle.RelativeAngle>30 ? Color.red : Color.white;
    }
}
