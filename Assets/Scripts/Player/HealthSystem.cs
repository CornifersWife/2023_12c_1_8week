using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 10;
    public int health;
    public static Vector2 lastCheckpointPos;
    
    private Animator animator;
    private PlayerMovement playerMovement;

    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
    }
    
    private void Start()
    {
        health = maxHealth;
        animator = GetComponent<Animator>();
    }

    public void takeDamage(int dmg)
    {
        health -= dmg;
        if(health <= 0)
        {
            playerMovement.enabled = false;
            animator.SetTrigger("DeadHit");
            Respawn();
        }
        else
            animator.SetTrigger("Hit");
    }
    public void heal(int heal)
    {
        health += heal;
    }
    public void Respawn()
    {
        animator.SetTrigger("Dead");
        gameObject.transform.position = lastCheckpointPos;
        health = maxHealth;
        playerMovement.enabled = true;
    }
}
