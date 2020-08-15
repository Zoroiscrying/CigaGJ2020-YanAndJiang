using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleMatInteractableObject : InteractableObject
{
    private Material _material;
    void Start()
    {
        _material = this.GetComponent<Renderer>().material;
        _material.SetColor("_BaseColor", Color.black);
    }

    public override void OnInteract()
    {
        
    }

    public override void OnCanInteract()
    {
        _material.SetColor("_BaseColor", Color.white);
    }

    public override void OnQuitInteract()
    {
        _material.SetColor("_BaseColor", Color.black);
    }
}
