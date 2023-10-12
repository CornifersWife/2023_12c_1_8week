using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinCollection : MonoBehaviour
{
    private Rigidbody2D _rb;
    private Animator _animator;
    [SerializeField] int coinValue;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    void Start()
    {
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) 
        {
            Destroy(_rb.GetComponent<Collider2D>());
            _animator.SetBool("Collected", true);
            CoinManager.instance.AddValue(coinValue);
        }
    }
}
