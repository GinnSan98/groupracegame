 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoSpin : MonoBehaviour {

    [SerializeField]
    private GameObject carMesh;
	
	// Update is called once per frame
	void LateUpdate () {

        float currentPosition = carMesh.transform.localPosition.y;
        carMesh.transform.localRotation = Quaternion.identity;
        currentPosition = Mathf.Clamp(currentPosition, -0.07f, 1f);
        carMesh.transform.localPosition = new Vector3(0,currentPosition,0);
	}

}
