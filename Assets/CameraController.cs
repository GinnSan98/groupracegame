using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public Transform target;
    public float speed = 0.125f;
    public Vector3 offset;

    private void LateUpdate()
    {
        Vector3 targetPosition = target.position + offset;
        Vector3 lerpPosition = Vector3.Lerp(transform.position, targetPosition, speed);
        transform.position = lerpPosition;

        transform.LookAt(target);
    }
}
