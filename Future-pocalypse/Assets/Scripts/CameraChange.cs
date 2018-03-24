using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraChange : MonoBehaviour
{
    [SerializeField]
    private Camera mycam;
    [SerializeField]
    private Transform firstperson;
    [SerializeField]
    private Transform thirdperson;
    private bool camerastate = false;
	
    private void ToggleCamera()
    {
        if (camerastate == true)
        {
            mycam.transform.position = thirdperson.position;
            camerastate = false;
        }
        else
        {
            mycam.transform.position = firstperson.position;
            camerastate = true;
        }
    }

    public void Lerping(float velocity)
    {
        if (camerastate == true)
        {
            mycam.transform.position = Vector3.Lerp(thirdperson.position, thirdperson.position+thirdperson.forward*5,velocity);
            
        }
        else
        {
            mycam.transform.position = Vector3.Lerp(firstperson.position, firstperson.position+ firstperson.forward*5, velocity);
          
        }
    }

	// Update is called once per frame
	void Update ()
    {
		if (Input.GetButtonDown("Jump") == true)
        {
            ToggleCamera();
        }
	}
}
