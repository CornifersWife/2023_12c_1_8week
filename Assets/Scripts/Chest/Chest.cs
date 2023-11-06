using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    [SerializeField] private GameObject treasure;
    [SerializeField] private float summonDelay = 1f;
    [SerializeField] private string keyName;
    Animator animator;
    bool isOpened;
    void Awake()
    {
        animator = GetComponent<Animator>();
        isOpened = false;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isOpened) return;
        if (collision.collider == null || (collision.collider.name != "Player" && collision.collider.name != "Feet")) return;
        if (!KeyManager.GetKeyManager().tryUseKey(keyName)) return;
        animator.SetTrigger("opened");
        isOpened = true;
        StartCoroutine(OpenCoroutine());
    }

    private IEnumerator OpenCoroutine()
    {
        yield return new WaitForSeconds(summonDelay);
        Instantiate(treasure, new Vector2(transform.position.x, transform.position.y + 0.30f), transform.rotation);
    }
}
