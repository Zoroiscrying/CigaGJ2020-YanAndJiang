using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemTypeList", menuName = "CIGA2020/ItemTypeList")]
public class ItemTypeList : ScriptableObject
{
    public List<ItemType> ItemTypes;

    public ItemType GetRandomType()
    {
        if (ItemTypes.Count>0)
        {
            int randomIndex = Random.Range(0, ItemTypes.Count);
            return ItemTypes[randomIndex];
        }

        return null;
    }
}
