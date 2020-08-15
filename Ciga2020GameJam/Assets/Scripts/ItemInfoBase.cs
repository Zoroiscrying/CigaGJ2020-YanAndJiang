using System.Collections;
using System.Collections.Generic;
using UnityAtoms.BaseAtoms;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemInfoBase", menuName = "CIGA2020/ItemInfoBase")]
public class ItemInfoBase : ScriptableObject
{
    [Tooltip("物品重量")]
    public int MassBase;
    [Tooltip("物品模型，带有Position等属性")]
    public GameObject ItemPrefab;
    [Tooltip("物品携带的属性，随机从中抽取")] public List<ItemTypeList> RandomTypes;
    [Tooltip("物品绑定的属性")] public List<ItemType> MustTypes;
    [Tooltip("物品需要的属性，随机从中抽取")] public List<ItemTypeList> RandomRequireTypes;
}
