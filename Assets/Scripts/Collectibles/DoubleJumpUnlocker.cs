using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleJumpUnlocker : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag("Player")) return;
        if (!enabled) return;
        enabled = false;
        collision.gameObject.GetComponent<PlayerMovement>().canDoubleJump = true;
        gameObject.GetComponent<Animator>().SetTrigger("Collected");
    }
}
