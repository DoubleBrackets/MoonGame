using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    [SerializeField]private Text nameText;
    [SerializeField] private Text dialogueText;

    private bool hasEnded = true;

    [SerializeField] private Animator animator;

    private Queue<string> sentences;

    private Transform interactionPoint;
    private Transform interactor;
    private float radius;

    // Start is called before the first frame update
    void Start()
    {
        sentences = new Queue<string>();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F))
        {
            displayNextSentence();
        }
        if(!hasEnded)
        {
            float distance = Vector3.Distance(interactionPoint.position, interactor.position);
            if (distance > radius)
            {
                endDialogue();
            }
        }
        
    }

    public bool getHasEnded()
    {
        return hasEnded;
    }

    public void startDialogue(Dialogue dialogue, Transform interactPoint, Transform i, float r)
    {
        interactionPoint = interactPoint;
        interactor = i;
        radius = r;
        hasEnded = false;
        animator.SetBool("IsOpen", true);


        nameText.text = dialogue.name;

        sentences.Clear();

        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }
        displayNextSentence();
    }

    public void displayNextSentence()
    {
        if(sentences.Count == 0)
        {
            endDialogue();
            return;
        }
        string sentence = sentences.Dequeue();
        dialogueText.text = sentence;

        StopAllCoroutines();
        StartCoroutine(typeSentence(sentence));
    }

    IEnumerator typeSentence(string sentence)
    {
        dialogueText.text = "";
        foreach(char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return null;
        }
    }

    void endDialogue()
    {
        hasEnded = true;
        animator.SetBool("IsOpen", false);
    }
}
