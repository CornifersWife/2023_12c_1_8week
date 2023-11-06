using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeechTrigger : MonoBehaviour
{
    [SerializeField] private SpeechBubble speechBubble;
    private bool trigerred;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (speechBubble != null)
        {
            if (other.CompareTag("Player") && !trigerred)
            {
                speechBubble.Activate(true);
                speechBubble.TriggerStartDialogue();
                trigerred = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (speechBubble != null)
        {
            if (other.CompareTag("Player") && trigerred)
            {
                speechBubble.Activate(false);
                trigerred = false;
            }
        }
    }
    
}
