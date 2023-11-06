using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using TMPro;

public class SpeechBubble : MonoBehaviour
{
    [SerializeField] private float typingSpeed = 0.05f;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [TextArea]
    [SerializeField] private string[] dialogueSentences;

    [SerializeField] private GameObject continueButton;
    [SerializeField] private GameObject hostEntity;

    private int index;
    private bool finished;
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        Transform t = hostEntity.transform;
        Vector3 newPosition = new Vector3(t.position.x, t.position.y + 1.5f, t.position.z);
        transform.position = newPosition;
    }

    public void TriggerStartDialogue()
    {
        StartCoroutine(StartDialogue());
    }

    public IEnumerator StartDialogue()
    {
        animator.SetTrigger("OpenBubble");
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        StartCoroutine(TypeDialogue());
    }

    private IEnumerator CloseDialogue()
    {
        if (!finished)
            index = 0;
        dialogueText.text = string.Empty;
        continueButton.SetActive(false);
        animator.SetTrigger("CloseBubble");
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        if (finished)
            Destroy(gameObject);
    }

    private IEnumerator TypeDialogue()
    {
        foreach (char letter in dialogueSentences[index].ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
        continueButton.SetActive(true);
    }

    public void ContinueDialogue()
    {
        continueButton.SetActive(false);
        if (index < dialogueSentences.Length - 1)
        {
            index++;
            dialogueText.text = string.Empty;
            StartCoroutine(TypeDialogue());
        }
        else
        {
            finished = true;
            StartCoroutine(CloseDialogue());
        }
    }

    public void Activate(bool state)
    {
        if (!state)
        {
            StartCoroutine(CloseDialogue());
        }
        gameObject.SetActive(state);
    }
}
