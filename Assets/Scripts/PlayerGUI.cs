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
        minSPDOMAngle = 0,
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
        float ang = Mathf.Lerp(minSPDOMAngle, maxSPDOMAngle, Mathf.InverseLerp(0.0f, maxSPDOM, vel));
        SPDOMneedle.transform.eulerAngles = new Vector3(0, 0, ang);
    }

    void SetRageBar()
    {
        ragebar.fillAmount = tsd.currentcharge / 30f;
      
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
