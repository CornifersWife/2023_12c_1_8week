using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleBarrel : DestructibleBox
{
    [SerializeField] private GameObject containObject;
    [SerializeField] private int objectCount = 1;

    public override void SwitchToShattered()
    {
        Instantiate(shatteredVersion, transform.position, transform.rotation);
        for (int i = 0; i < objectCount; i++)
        {
            float offset = i * 0.1f;
            Vector3 pos = transform.position + transform.right * offset;
            Instantiate(containObject, pos, transform.rotation);
        }
        Destroy(gameObject);
    }
}
