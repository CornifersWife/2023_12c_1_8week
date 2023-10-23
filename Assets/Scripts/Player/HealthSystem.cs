using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 10;
    public int health;
    
    private Animator animator;

    private void Start()
    {
        health = maxHealth;
        animator = GetComponent<Animator>();
    }
    //wywo�ywane z innych skrypt�w gdy player ma dosta� jaki� dmg
    public void takeDamage(int dmg)
    {
        health -= dmg;
        //je�eli umiera (hp<0)
        if(health <= 0)
        {
            //animacja �miertelnego ciosu
            animator.SetTrigger("DeadHit");
            Destroy(this.gameObject);
            //@todo player death
        }
        else
            animator.SetTrigger("Hit");
    }

    //jakby�my kiedy� heal chcieli zrobi� 
    public void heal(int heal)
    {
        health += heal;
    }
}
