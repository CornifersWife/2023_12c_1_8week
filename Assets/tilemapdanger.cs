using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tilemapdanger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerHealth playerHealth = other.gameObject.GetComponent<PlayerHealth>();
            playerHealth.takeDamage(playerHealth.maxHealth);
        }
    }
}
