using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;    //For Text Mesh Pro stuff

public class PlayerGUI : MonoBehaviour {


    public TextMeshProUGUI lapText;
    [SerializeField] private Lapcheckpoint lcp;
    [SerializeField] private CarDriving cd;
    [SerializeField] private TimeSlowdown tsd;
    [SerializeField] private healthTest ht;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private GameObject SPDOMneedle;

    [SerializeField]
    private Image
        ragebar,
        healthbar;

    [SerializeField]
    private float
        maxSPDOMAngle = 360,
        minSPDOMAngle = 180,
        maxSPDOM = 100,
        topSpeed = 170;


    void LateUpdate()
    {
        SetLapText();
        SetRageBar();
        SetHealth();
        SetSpeedometer();
    }

    void SetSpeedometer()
    {
        float vel = rb.velocity.magnitude;
        float ang = Mathf.Lerp(minSPDOMAngle, maxSPDOMAngle, Mathf.InverseLerp(0.0f, maxSPDOM, vel/10));
        SPDOMneedle.transform.eulerAngles = new Vector3(0, 0, ang);
    }

    void SetRageBar()
    {
        ragebar.fillAmount = tsd.currentcharge / 30f;
        if (tsd.canturnon == true && tsd.tickdown == false)
        {
            ragebar.color = Color.green;

        }
        else if (tsd.tickdown == true && tsd.canturnon == true)
        {
            ragebar.color = Color.red;
        }
        else
        {
            ragebar.color = Color.blue;
        }
    }

    void SetHealth()
    {
        healthbar.fillAmount = ht.Returnhealth() / ht.ReturnMaxhealth();
    }

    void SetLapText()
    {
        lapText.text = "Lap " + lcp.Lap + " / 4";
    }
}
