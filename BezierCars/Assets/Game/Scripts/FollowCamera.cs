using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    [SerializeField] Transform followObject;
    [SerializeField] float followSpeed;
    Vector3 offset;
    void Start()
    {
        offset = transform.position - followObject.position;
    }
    void Update()
    {
        Vector3 offsetPos = followObject.position + offset;
        transform.position = Vector3.Lerp(transform.position, offsetPos, followSpeed * Time.deltaTime);
        transform.LookAt(followObject, Vector3.up);
    }
}
