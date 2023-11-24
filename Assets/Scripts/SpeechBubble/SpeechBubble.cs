using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using TMPro;
using UnityEngine.UIElements;

public class SpeechBubble : MonoBehaviour
{
    [SerializeField] private float typingSpeed = 0.05f;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [TextArea]
    [SerializeField] private string[] dialogueSentences;

    [SerializeField] private GameObject continueButton;
    [SerializeField] private GameObject hostEntity;

    [SerializeField] private bool moveBubble = true;

    private int index;
    private bool finished;
    private Animator animator;
    private RectTransform rt;
    private GameObject canvas;
    private GameObject container;

    private void Awake()
    {
        canvas = gameObject.transform.parent.gameObject.transform.parent.gameObject;
        container = gameObject.transform.parent.gameObject;
        rt = canvas.GetComponent<RectTransform>();
        animator = container.GetComponent<Animator>();
    }
    
    private void Start()
    {
        if (!moveBubble) return;
        //zmiana pozycji na pozycję nad host entity
        Transform t = hostEntity.transform;
        Vector2 newPosition = new Vector2(t.position.x, t.position.y + 2f);
        rt.anchoredPosition = newPosition;
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
            Destroy(gameObject.transform.parent.gameObject);
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
