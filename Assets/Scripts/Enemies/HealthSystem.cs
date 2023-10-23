using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    public int maxHealth = 10;
    public int health;
    public bool isDamaged = false;

    private Animator animator;
    public bool isDead;
    private void Start()
    {
        health = maxHealth;
        animator = GetComponent<Animator>();
        isDead = false;
    }

    public void takeDamage(int dmg)
    {
        isDamaged = true; 
        health -= dmg;
        if (health <= 0)
        {
            animator.SetTrigger("DeadHit");
            isDead = true;
        }
        else
            animator.SetTrigger("Hit");
    }
    public void heal(int heal)
    {
        health += heal;
    }
}