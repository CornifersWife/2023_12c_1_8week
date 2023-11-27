using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonBall : MonoBehaviour
{

    [SerializeField] private int despawnAfter = 10;
    [SerializeField] private bool canPlayerJumpOff;
    public float speed = 10f;
    public int damage = 10;
    private Animator animator;
    private Rigidbody2D rb;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        rb.velocity = transform.right * speed;
        StartCoroutine(destroyAfter());
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(collision.gameObject.name);
        if (collision.gameObject.name == "Cannon") return;
        if (IsBelowPlayerFeet(collision.collider)) return;
        if (collision.gameObject.name == "Player")
        {
            DealDamageToPlayer(collision.gameObject);
        }
        else Explode();

    }
    private bool IsBelowPlayerFeet(Collider2D playerCollider)
    {
        return  canPlayerJumpOff && playerCollider.bounds.min.y > transform.position.y;
    }


    void DealDamageToPlayer(GameObject player)
    {
        PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.takeDamage(damage);
        }

        Explode();
    }

    void Explode()
    {
        rb.rotation = 270;
        rb.velocity = Vector2.zero;
        animator.SetTrigger("Explosion");
    }

    IEnumerator destroyAfter()
    {
        yield return new WaitForSeconds(despawnAfter);
        Explode();
    }
}


