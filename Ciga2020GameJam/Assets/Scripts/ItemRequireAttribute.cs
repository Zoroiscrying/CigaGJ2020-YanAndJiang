using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemRequireAttribute", menuName = "CIGA2020/ItemRequireAttribute")]
public class ItemRequireAttribute : ScriptableObject
{
    public List<ItemType> ItemAttributes;
    public List<ItemType> RequireItems;
}
