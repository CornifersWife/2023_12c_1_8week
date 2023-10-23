using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadCheck : MonoBehaviour
{
    [SerializeField] HealthSystem healthSystem;
    [SerializeField] int damageOnHeadCollision;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Feet>())
        {
            healthSystem.takeDamage(damageOnHeadCollision);
        } 
    }
}
