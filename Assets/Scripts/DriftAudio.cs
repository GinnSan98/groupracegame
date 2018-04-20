using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DriftAudio : MonoBehaviour {

    [SerializeField]
    private AudioSource audioSource;

    public GameObject player;
    public CarDriving playerScript;
	// Use this for initialization
	void Start () {
        playerScript = player.GetComponent<CarDriving>();
        audioSource = GetComponent<AudioSource>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if(playerScript.isDrifting)
        {
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }
        else
        {
            audioSource.Stop();
        }
        //audioSource.pitch = playerScript.rb.velocity.magnitude / playerScript.topspeed*2 + 1;
    }
}
