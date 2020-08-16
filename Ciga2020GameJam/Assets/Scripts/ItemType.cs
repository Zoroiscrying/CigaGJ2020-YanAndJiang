using System.Collections;
using System.Collections.Generic;
using UnityAtoms.BaseAtoms;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemType", menuName = "CIGA2020/ItemType")]
public class ItemType : StringVariable
{
    public bool HasColor = false;
    [ColorUsage(false, true)]
    public Color Color;
}
