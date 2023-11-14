using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerActivate : MonoBehaviour
{
    [SerializeField] private SpeechBubble speechBubble;
    public void Activate()
    {
        gameObject.SetActive(true);
        speechBubble.gameObject.SetActive(true);
        speechBubble.TriggerStartDialogue();
    }
    
    public void Deactivate()
    {
        speechBubble.Activate(false);
        gameObject.SetActive(false);
    }
}
