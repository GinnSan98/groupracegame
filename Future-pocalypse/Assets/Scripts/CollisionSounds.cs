using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionSounds : MonoBehaviour {
    AudioSource audioSource;
    public AudioClip soundCollision;
    // Use this for initialization
    void Start () {
        audioSource = GetComponent<AudioSource>();
    }
    void OnTriggerEnter(Collider other)
    {

        if(other.gameObject.tag == "Enemy")
        {
            Debug.Log("Enemy Hit!");
            audioSource.Play();
        }
        if (other.gameObject.tag == "Environment")
        {
            Debug.Log("Environment Hit!");
            audioSource.Play();
        }
    }

}
