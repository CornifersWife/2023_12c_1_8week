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

    public void takeDamage(int dmg)
    {
        health -= dmg;
        if(health <= 0)
        {
            animator.SetTrigger("DeadHit");
            health = maxHealth;
            this.gameObject.transform.position = lastCheckpointPos;
        }
        else
            animator.SetTrigger("Hit");
    }
    public void heal(int heal)
    {
        health += heal;
    }
}
