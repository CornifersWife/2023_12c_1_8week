using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] private float typingSpeed = 0.05f;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [TextArea]
    [SerializeField] private string[] dialogueSentences;

    [SerializeField] private GameObject continueButton;

    private int index;
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        StartCoroutine(StartDialogue());
    }

    public IEnumerator StartDialogue()
    {
        animator.SetTrigger("OpenBubble");
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        StartCoroutine(TypeDialogue());
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
            dialogueText.text = string.Empty;
            animator.SetTrigger("CloseBubble");
        }
    }
}
