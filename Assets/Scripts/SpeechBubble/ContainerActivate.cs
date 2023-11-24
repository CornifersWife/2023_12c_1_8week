using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerActivate : MonoBehaviour
{
    [SerializeField] private SpeechBubble speechBubble;
    public void Activate()
    {
        if (transform.parent.parent != null)
        {
            bool isWrong = ((Transform)transform.parent.parent.GetComponent<Transform>()).localScale.x < 0;
            if(isWrong)transform.localScale = new Vector3(-1, 1, 1);
        }
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
