using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class healthTest : MonoBehaviour
{
    [SerializeField]
    private Text healthText;
    private int playerHealth;
    // Use this for initialization

    void Start()
    {
        playerHealth = 100;
        SetHealthText();
    }

    // Update is called once per frame

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("smallDamage"))
        {
            playerHealth -= 1;
            SetHealthText();
        }

        if (other.gameObject.CompareTag("mediumDamage"))
        {
            playerHealth -=25;
            SetHealthText();
        }

        if (other.gameObject.CompareTag("largeDamage"))
        {
            playerHealth -= 50;
            SetHealthText();
        }
    }

    void SetHealthText()
    {
        healthText.text = "Health: " + playerHealth.ToString();
        if (playerHealth <= 0)
        {
            healthText.text = "You're dead boi";
        }
    }
}
