﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectionEntity : MonoBehaviour
{
    
    private List<InteractableObject> interactables = new List<InteractableObject>();
    private InteractableObject currentInteractable;
    [SerializeField] private ItemHolder _itemHolder;

    // Update is called once per frame
    void Update()
    {
        if (currentInteractable && Input.GetKeyDown(KeyCode.E))
        {
            currentInteractable.OnInteract();
        }
        
        // Debug.Log("Remove current Interactable");
        if (currentInteractable && !currentInteractable.CanInteract)
        {
            // Debug.Log("Remove current Interactable");
            if (interactables.Contains(currentInteractable))
            {
                Debug.Log("Remove current Interactable");
                currentInteractable.OnQuitInteract();
                interactables.Remove(currentInteractable);
            }
            currentInteractable = null;
            CheckInteractable();
        }
    }

    private void CheckInteractable ()
    {
        for (int i = interactables.Count-1; i >= 0; i--)
        {
            if (!interactables[i] || !interactables[i].CanInteract)
            {
                interactables.RemoveAt(i);
            }
        }

        if (interactables.Count == 0)
        {
            if (currentInteractable != null)
            {
                currentInteractable.OnQuitInteract();
                currentInteractable = null;   
            }
        }
        else if (interactables.Count == 1)
        {
            // Debug.Log(interactables[0].CanInteract);
            if (currentInteractable == null)
            {
                currentInteractable = interactables[0];
                currentInteractable.OnCanInteract();
            }
            else if(currentInteractable != interactables[0])
            {
                currentInteractable.OnQuitInteract();
                currentInteractable = interactables[0];
                currentInteractable.OnCanInteract();
            }
        }
        else
        {
            float interactableDistance = float.MaxValue;
            if (currentInteractable)
            {
                interactableDistance =
                    Vector3.Distance(this.transform.position, currentInteractable.transform.position);   
            }
            float minDis = interactableDistance;
            InteractableObject interactableObject = null;
            foreach (var interactable in interactables)
            {
                var dis = Vector3.Distance(this.transform.position, interactable.transform.position);
                if (dis < minDis)
                {
                    minDis = dis;
                    interactableObject = interactable;
                }
            }

            //有更小距离的物体
            if (minDis < interactableDistance && interactableObject != null && currentInteractable)
            {
                currentInteractable.OnQuitInteract();
                currentInteractable = interactableObject;
                currentInteractable.OnCanInteract();
            }else if (!currentInteractable)
            {
                currentInteractable = interactableObject;
                currentInteractable.OnCanInteract();
            }

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        var interactable = other.GetComponent<InteractableObject>();
        if (interactable != null && interactable.CanInteract)
        {
            if (!interactables.Contains(interactable))
            {
                interactables.Add(interactable);
            }
            CheckInteractable();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        var interactable = other.GetComponent<InteractableObject>();
        if (interactable != null && interactable.CanInteract)
        {
            if (interactables.Contains(interactable))
            {
                interactables.Remove(interactable);
            }
            CheckInteractable();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        var interactable = other.GetComponent<InteractableObject>();
        if (interactable != null && interactable.CanInteract && !interactables.Contains(interactable))
        {
            interactables.Add(interactable);
        }
        
        CheckInteractable();
        // var interactable = other.GetComponent<InteractableObject>();
        // if (interactable != null)
        // {
        //     interactable.OnWithInCheckRange();
        // }
    }
}