using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;
    public float distance = 3.0f;
    public float height = 3.0f;
    public float damping = 5.0f;
    public bool smoothRotation = true;
    public bool followBehind = true;
    public float rotationDamping = 10.0f;
    public Vector3 smooth;
    public Rigidbody rb;
    Vector3 position;

    void Start()
    {
        position = transform.position;
    }
    void LateUpdate()
    {
        Vector3 smooth = rb.velocity;
        Vector3 wantedPosition;
    
        wantedPosition = target.TransformPoint(0, height, -distance);

       transform.position = Vector3.SmoothDamp(transform.position, wantedPosition, ref smooth, damping);
        
        
        if (smoothRotation == true)
        {
            Quaternion wantedRotation = Quaternion.LookRotation(target.position - transform.position, target.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, wantedRotation, Time.deltaTime * rotationDamping);
        }
        else transform.LookAt(target, target.up);
    }
}
