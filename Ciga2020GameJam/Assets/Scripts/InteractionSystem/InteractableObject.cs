using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InteractableObject : MonoBehaviour, IInteractable
{
    public virtual bool CanInteract { get => true; }

    public virtual void OnWithInCheckRange()
    {
        
    }

    public virtual void OnWithOutCheckRange()
    {
        
    }
    
    public virtual void OnInteract()
    {
    }

    public virtual void OnCanInteract()
    {
    }

    public virtual void OnQuitInteract()
    {
    }
}
