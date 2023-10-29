using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleBarrel : DestructibleBox
{
    [SerializeField] private GameObject containObject;

    public override void SwitchToShattered()
    {
        Instantiate(shatteredVersion, transform.position, transform.rotation);
        Instantiate(containObject, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
