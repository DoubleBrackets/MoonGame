using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;

    public void triggerDialogue(Transform interactionPoint, Transform interactor, float radius)
    {
        DialogueManager dm = FindObjectOfType<DialogueManager>();
        if (dm.getHasEnded())
        {
            dm.startDialogue(dialogue, interactionPoint, interactor, radius);
        }
    }
}
