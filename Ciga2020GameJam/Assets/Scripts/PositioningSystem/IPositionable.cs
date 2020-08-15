using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPositionable 
{
    Vector3 AnchoredPosition3D { get;}
    Vector3 LocalPosition3D { get; }

    bool HasNewPosition();

    Vector3 CalculatePosition(IPositionable installingObject);
}
