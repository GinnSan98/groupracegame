﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/*
public class GUITText : Lapcheckpoint
{

    public Text lapText;
    [SerializeField]
    private Lapcheckpoint lcp;
    [SerializeField]
    private TimeSlowdown tsd;
    [SerializeField]
    private healthTest ht;
    [SerializeField]
    private Image ragebar;
    [SerializeField]
    private Image healthbar;
    

    void Start ()
    {
       
        
    }
    void LateUpdate()
    {
        SetLapText();
        SetRageBar();
        SetHealth();
    }
	
	void SetRageBar()
    {
        ragebar.fillAmount = tsd.currentcharge / 30;
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

	void SetLapText ()
    {
        lapText.text = "Lap " + lcp.Lap + " / 4";
    }
}*/
