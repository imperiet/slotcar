using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VehicleDebugGraphics : MonoBehaviour
{
    [SerializeField] Vehicle vehicle;
    [SerializeField] Slider slider;
    void Start(){
        if(!vehicle) vehicle = FindObjectOfType<Vehicle>();
    }
    void Update (){
        slider.value = vehicle.RelativeAngle/90f;
        slider.targetGraphic.color = vehicle.RelativeAngle>30 ? Color.red : Color.white;
    }
}
