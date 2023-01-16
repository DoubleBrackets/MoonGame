using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    [SerializeField]protected float radius;

    public virtual void Interact(Transform interactor) 
    {
        //Overwrite
        Debug.Log("Interact with " + transform.name);
    }


    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
