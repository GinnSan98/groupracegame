using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightsManager : MonoBehaviour
{
    enum HeadLightEnum { off, on, bright };
    enum BrakeLightEnum { off, on};

    [SerializeField] HeadLightEnum headlightSetting = HeadLightEnum.off;
    [SerializeField] BrakeLightEnum brakeLightSetting = BrakeLightEnum.off;

    [SerializeField]
    GameObject[]
        headlights,
        brakelights;

    // Update is called once per frame
    void Update()
    {
        // Toggle headlights
        if (Input.GetButtonDown("LightToggle"))
        {
            HeadlightToggle();
        }

        // Turn brake light on
        if (Input.GetAxis("Vertical") < 0)
        {
            foreach (GameObject bl in brakelights)
            {
                bl.SetActive(true);
            }
        }
        else
        {
            foreach (GameObject bl in brakelights)
            {
                bl.SetActive(false);
            }
        }
    }

    public void HeadlightToggle()
    {
        // Move headlight onto the next setting
        switch(headlightSetting)
        {
            case HeadLightEnum.off:
                headlightSetting = HeadLightEnum.on;
                break;
            case HeadLightEnum.on:
                headlightSetting = HeadLightEnum.bright;
                break;
            case HeadLightEnum.bright:
                headlightSetting = HeadLightEnum.off;
                break;

            default:
                headlightSetting = HeadLightEnum.off;
                break;
        }

        // Make chanages to headlight
        foreach (GameObject hl in headlights)
        {
            Light hll = hl.GetComponent<Light>(); // hll is the Headlight's (game object) light (light)

            switch(headlightSetting)
            {
                case HeadLightEnum.off:
                    hl.SetActive(false);
                    break;
                case HeadLightEnum.on:
                    hl.SetActive(true);
                    hll.intensity = 20;
                    hll.range = 100;
                    break;
                case HeadLightEnum.bright:
                    hl.SetActive(true);
                    hll.intensity = 80;
                    hll.range = 200;
                    break;
            }
        }
    }
}
