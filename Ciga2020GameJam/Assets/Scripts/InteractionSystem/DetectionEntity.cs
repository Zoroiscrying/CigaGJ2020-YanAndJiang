using System;
using System.Collections;
using System.Collections.Generic;
using UnityAtoms.BaseAtoms;
using UnityEngine;

public class DetectionEntity : MonoBehaviour
{
    
    private List<InteractableObject> interactables = new List<InteractableObject>();
    private InteractableObject currentInteractable;
    [SerializeField] private ItemHolder _itemHolder;
    [SerializeField] private float detectionRange = 2.0f;
    [SerializeField] private GameObject CheckDistancePosition;

    private float holdKeyDetectTime = .3f;
    private float holdKeyTimer = 0f;
    private bool skipKeyUp = false;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.E))
        {
            holdKeyTimer += Time.deltaTime;
            if (holdKeyTimer > holdKeyDetectTime)
            {
                EventKit.Broadcast("Drop Item");
                skipKeyUp = true;
                holdKeyTimer = 0.0f;
            }
        }

        if (currentInteractable && Input.GetKeyUp(KeyCode.E) && !skipKeyUp)
        {
            holdKeyTimer = 0.0f;
            currentInteractable.OnInteract();
        }
        
        if (Input.GetKeyUp(KeyCode.E))
        {
            holdKeyTimer = 0.0f;
            skipKeyUp = false;
        }
        
        // Debug.Log("Remove current Interactable");
        if (currentInteractable && !currentInteractable.CanInteract)
        {
            // Debug.Log("Remove current Interactable");
            if (interactables.Contains(currentInteractable))
            {
                // Debug.Log("Remove current Interactable");
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
                    Vector3.Distance(CheckDistancePosition.transform.position, currentInteractable.transform.position);   
            }
            float minDis = interactableDistance;
            InteractableObject interactableObject = null;
            foreach (var interactable in interactables)
            {
                var dis = Vector3.Distance(CheckDistancePosition.transform.position, interactable.transform.position);
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
            interactable.OnWithInCheckRange();
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
            interactable.OnWithOutCheckRange();
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
            // interactable.OnWithInCheckRange();
        }
        CheckInteractable();

        var touchable = other.GetComponent<BasicTouchItem>();
        if (touchable && Vector3.Distance(this.transform.position, touchable.transform.position) <= this.detectionRange)
        {
            touchable.OnTouch();
        }
        // var interactable = other.GetComponent<InteractableObject>();
        // if (interactable != null)
        // {
        //     interactable.OnWithInCheckRange();
        // }
    }
}
