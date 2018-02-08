using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Text : Lapcheckpoint
{

    public UnityEngine.UI.Text lap;
    public int lapText;
    internal string text;

    void Start ()
    {
        SetLapText();
        lapText = Lap;
        
    }
    void Update()
    {
        SetLapText();
    }
	
	
	void SetLapText ()
    {
        lapText = Lap;
        lap.text = "Lap: " + Lap;
    }
}
