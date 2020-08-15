using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemTypeMaterialGuide", menuName = "CIGA2020/ItemTypeMaterialGuide")]
public class ItemTypeMaterialGuide : ScriptableObject
{
    public ItemType TargetType;
    public List<MaterialInfoPair> MaterialInfoPairs;
}

[Serializable]
public class MaterialInfoPair
{
    [ColorUsage(false, true)]
    public Color Color;
    public String MaterialProperty;
}
