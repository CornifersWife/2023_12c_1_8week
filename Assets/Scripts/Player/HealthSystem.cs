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
    //wywo³ywane z innych skryptów gdy player ma dostaæ jakiœ dmg
    public void takeDamage(int dmg)
    {
        health -= dmg;
        //je¿eli umiera (hp<0)
        if(health <= 0)
        {
            //animacja œmiertelnego ciosu
            animator.SetTrigger("DeadHit");
            Destroy(this.gameObject);
            //@todo player death
        }
        else
            animator.SetTrigger("Hit");
    }

    //jakbyœmy kiedyœ heal chcieli zrobiæ 
    public void heal(int heal)
    {
        health += heal;
    }
}
