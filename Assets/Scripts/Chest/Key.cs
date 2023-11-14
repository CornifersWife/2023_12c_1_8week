using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    [SerializeField] private string keyName;
 



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Destroy(GetComponent<Collider2D>());
            KeyManager.GetKeyManager().addKey(keyName);
            GetComponent<Animator>().SetTrigger("Collected");
        }
    }
}
