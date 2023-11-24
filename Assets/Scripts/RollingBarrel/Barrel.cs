using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrel : MonoBehaviour
{
    private Animator animator;
    
    void Awake()
    {
        animator = GetComponent<Animator>();
    }
    void Start()
    {
        
    }

    
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerHealth playerHealth = other.gameObject.GetComponent<PlayerHealth>();
            playerHealth.takeDamage(playerHealth.maxHealth);
            StartCoroutine(Break());
        }
        
        else if (other.gameObject.CompareTag("Enemy"))
        {
            StartCoroutine(Break());
            Destroy(other.gameObject);
        }
        
        else if (other.gameObject.CompareTag("Wall") || other.gameObject.CompareTag("Barrel"))
        {
            StartCoroutine(Break());
        }
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
    
    private IEnumerator Break()
    {
        animator.SetTrigger("IsDestroyed");
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        Destroy(gameObject);
    }
}
