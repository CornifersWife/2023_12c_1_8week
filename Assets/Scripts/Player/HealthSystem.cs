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

    private void Start()
    {
        health = maxHealth;
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (health <= 0 && Input.GetKeyDown(KeyCode.R))
        {
            respawn();
            animator.Play("Idle");
        }
    }

    public void takeDamage(int dmg)
    {
        health -= dmg;
        if(health <= 0)
        {
            animator.SetTrigger("DeadHit");
            gameObject.GetComponent<PlayerMovement>().enabled = false;
        }
        else
            animator.SetTrigger("Hit");
    }
    public void heal(int heal)
    {
        health += heal;
    }

    public void respawn()
    {
        gameObject.transform.position = lastCheckpointPos;
        health = maxHealth;
        gameObject.GetComponent<PlayerMovement>().enabled = true;
    }
}
