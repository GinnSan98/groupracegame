using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(Lapcheckpoint))]
public class Lapcheckpointcheck : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        Lapcheckpoint myScript = (Lapcheckpoint)target;
        if (GUILayout.Button("Check my progress Senpai <3"))
        {
            myScript.Showmyprogress();
        }
    }
}