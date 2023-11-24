using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelSpawner : MonoBehaviour
{
    [SerializeField] private GameObject barrel;
    [SerializeField] private float cooldown;

    private bool exited = true;
    
    private IEnumerator Spawn()
    {
        while (!exited)
        {
            Instantiate(barrel, transform.position, transform.rotation);
            yield return new WaitForSeconds(cooldown);
        }
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            exited = false;
            StartCoroutine(Spawn());
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            exited = true;
        }
    }
}
