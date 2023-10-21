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
            animator.SetTrigger("IsDestroyed");
            StartCoroutine(Break());
        }
        else if (other.gameObject.CompareTag("Wall"))
        {
            animator.SetTrigger("IsDestroyed");
            StartCoroutine(Break());
        }
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
    
    private IEnumerator Break()
    {
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        Destroy(gameObject);
    }
}
