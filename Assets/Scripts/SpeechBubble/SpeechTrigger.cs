using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeechTrigger : MonoBehaviour
{
    [SerializeField] private ContainerActivate container;
    private bool trigerred;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (container != null)
        {
            if (other.CompareTag("Player") && !trigerred)
            {
                container.Activate();
                trigerred = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (container != null)
        {
            if (other.CompareTag("Player") && trigerred)
            {
                container.Deactivate();
                trigerred = false;
            }
        }
    }
    
}
