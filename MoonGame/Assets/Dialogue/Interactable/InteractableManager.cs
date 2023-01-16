using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableManager : MonoBehaviour
{

    public Interactable focus;
    Camera cam;
    [SerializeField] private GameObject indicator;
    [SerializeField,Range(0, 10)] private float rayDistance;


    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitObject;
        bool hit = Physics.Raycast(ray, out hitObject, rayDistance);
        

        if (hit)
        {
            Interactable interactable = hitObject.collider.GetComponent<Interactable>();
            if (interactable != null)
            {
                indicator.SetActive(true);
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (interactable != null)
                {
                    interactable.Interact(cam.transform);
                }
            }
        }
        else
        {
            indicator.gameObject.SetActive(false);
        }


    }

}
