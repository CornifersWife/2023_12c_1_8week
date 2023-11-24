using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TotemProjectile : MonoBehaviour
{
    [SerializeField] private float _speed;
    private Rigidbody2D _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        _rb.velocity = (transform.right * _speed);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) 
        {
            collision.gameObject.GetComponent<PlayerHealth>().takeDamage(100);         
        }
        GetComponent<Animator>().SetTrigger("IsHit");
        _rb.velocity = _rb.velocity * 0.2f;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("TempProjectileDestroyPoint")) 
        {
            Destroy(gameObject);
        }
    }

}
