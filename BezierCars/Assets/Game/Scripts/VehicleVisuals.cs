using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Thoreas.Vehicles
{
    public class VehicleVisuals : MonoBehaviour
    {
        private bool vehicleVisibility;
        [SerializeField] private GameObject visualsParent;

        public bool VehicleVisibility
        {
            get => vehicleVisibility; set
            {
                visualsParent.SetActive(value);
                vehicleVisibility = value;
            }
        }
    }
}