using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityAtoms.BaseAtoms;
using UnityEngine;

public class ItemGenerator : Singleton<ItemGenerator>
{
    // public List<ItemInfoBase> ItemInfoBases = new List<ItemInfoBase>();
    public Material outlineMaterial;
    public List<ItemInfoBase> Decorators = new List<ItemInfoBase>();
    public List<ItemInfoBase> Holders = new  List<ItemInfoBase>();
    public IntReference GameHardRate;

    private bool ShouldGenerateHolder()
    {
        var itemsInScene = FindObjectsOfType<BasicItem>().ToList();
        int holderCount = 0;
        int decoratorCount = 0;
        foreach (var item in itemsInScene)
        {
            if (item.IsDecorator())
            {
                decoratorCount++;
            }
            else
            {
                holderCount++;
            }
        }

        if (decoratorCount > holderCount * 1.5f)
        {
            return true;
        }
        else return false;
    }

    public BasicItem GenerateRandomItem()
    {
        List<ItemInfoBase> ItemInfoBases;
        if (ShouldGenerateHolder())
        {
            ItemInfoBases = Holders.ToList();
        }
        else
        {
            ItemInfoBases = Decorators.ToList();
        }
        
        int randomIndex = Random.Range(0, ItemInfoBases.Count);
        var itemInfoBase = ItemInfoBases[randomIndex];
        
        var obj = Instantiate(itemInfoBase.ItemPrefab, Vector3.zero, Quaternion.identity);
        obj.SetActive(false);
        
        var basicItem = obj.GetComponent<BasicItem>();
        //get random types
        List<ItemType> itemTypes = new List<ItemType>();
        List<ItemType> requireTypes = new List<ItemType>();
        foreach (var itemType in itemInfoBase.MustTypes)
        {
            itemTypes.Add(itemType);
        }

        foreach (var randomTypeList in itemInfoBase.RandomTypes)
        {
            var itemType = randomTypeList.GetRandomType();
            if (itemType)
            {
                itemTypes.Add(randomTypeList.GetRandomType());   
            }
        }

        int requiredTypeGenerated = 0;

        foreach (var randomRequireTypeList in itemInfoBase.RandomRequireTypes)
        {
            var requireType = randomRequireTypeList.GetRandomType();
            if (requireType)
            {
                requiredTypeGenerated++;
                requireTypes.Add(requireType);   
            }
        }
        
        for (int i = 1; i < GameHardRate.Value + 1 ; i++)
        {
            foreach (var randomRequireTypeList in itemInfoBase.RandomRequireTypes)
            {
                if (Random.Range(0f,1f) > 0.75f && requiredTypeGenerated < basicItem.PositionPointCount)
                {
                    var requireType = randomRequireTypeList.GetRandomType();
                    if (requireType)
                    {
                        requiredTypeGenerated++;
                        requireTypes.Add(requireType);   
                    }
                }
            }  
        }
        basicItem.Initialize(itemTypes, requireTypes,outlineMaterial);

        return basicItem;
        // var visualObject = ItemInfoBases
    }
}
