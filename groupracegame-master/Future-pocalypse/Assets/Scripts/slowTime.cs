using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class slowTime : MonoBehaviour
{
    public bool Activated = false;
    public bool timeGo = false;
    public int timeCharge = 0;

    void Start()
    {
        
    }

	void Update ()
    {
        if (timeCharge >= 0 && Activated == false && timeCharge <= 500)
        {
            timeCharge++;
        }
        
        else if (timeCharge >= 500 && timeGo == false)
        {
            timeGo = true;
        }
        else if (timeCharge == 0 && Activated == true)
        {
            timeGo = false;
            Activated = false;
        }

       


        if (Input.GetKey(KeyCode.Q) && timeGo == true)
        {
            Activated = true;
        }

        
        if (Activated == true)
        {
            Time.timeScale = 0.25f;
            Application.targetFrameRate = Mathf.RoundToInt(60f / Time.timeScale);
            timeCharge--;
        }
        else if (Activated == false)
        {
            Time.timeScale = 1f;
        }

        //if(timeGo == false)
        //{
        //    Time.timeScale = 1f;
        //}
       
	}
}
