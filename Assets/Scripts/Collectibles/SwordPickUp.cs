using UnityEngine;

public class SwordPickUp : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag("Player")) return;
        if (!enabled) return;
        enabled = false;
        collision.gameObject.GetComponent<PlayerMovement>().hasWeapon = true;
        gameObject.GetComponent<Animator>().SetTrigger("Collected");
    }
}
