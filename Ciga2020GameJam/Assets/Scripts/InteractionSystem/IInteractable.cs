using System.Collections;
using System.Collections.Generic;
using UnityAtoms.BaseAtoms.Editor;
using UnityEngine;

public interface IInteractable
{
    bool CanInteract { get; }
    void OnInteract();

    void OnCanInteract();

    void OnQuitInteract();
}
