using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpriteItemTypeLibrary", menuName = "CIGA2020/SpriteItemTypeLibrary")]
public class SpriteItemTypeLibrary : ScriptableObject
{
    public List<ItemTypeSpritePair> ItemTypeSpritePairs;
    public Dictionary<ItemType, Sprite> ItemTypeSpriteLibrary;
    public Sprite DefaultSprite;

    public void Initialize()
    {
        ItemTypeSpriteLibrary = new Dictionary<ItemType, Sprite>();
        foreach (var pair in ItemTypeSpritePairs)
        {
            if (!ItemTypeSpriteLibrary.ContainsKey(pair.ItemType))
            {
                ItemTypeSpriteLibrary.Add(pair.ItemType, pair.Sprite);
            }
        }
    }

    public Sprite GetSpriteBasedOnType(ItemType type)
    {
        if (ItemTypeSpriteLibrary.ContainsKey(type))
        {
            return ItemTypeSpriteLibrary[type];
        }
        else
        {
            return DefaultSprite;
        }
    }
    
}

[Serializable]
public class ItemTypeSpritePair
{
    public ItemType ItemType;
    public Sprite Sprite;
}


