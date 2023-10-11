using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    public int maxHealth = 10;
    public int health;

    private Animator animator;

    private void Start()
    {
        health = maxHealth;
        animator = GetComponent<Animator>();
    }

    public void takeDamage(int dmg)
    {
        health -= dmg;
        if (health <= 0)
        {
            animator.SetTrigger("DeadHit");
            Destroy(this.gameObject);
        }
        else
            animator.SetTrigger("Hit");
    }
    public void heal(int heal)
    {
        health += heal;
    }
}
