using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueInteract : Interactable
{

    
    public override void Interact(Transform interactor)
    {
        gameObject.GetComponent<DialogueTrigger>().triggerDialogue(transform, interactor, radius);
    }

}
