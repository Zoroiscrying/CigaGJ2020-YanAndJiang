using System.Collections;
using System.Collections.Generic;
using UnityAtoms.BaseAtoms;
using UnityEngine;

public class ItemGenerator : Singleton<ItemGenerator>
{
    public List<ItemInfoBase> ItemInfoBases = new List<ItemInfoBase>();
    public IntReference GameHardRate;

    public BasicItem GenerateRandomItem()
    {
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

        foreach (var randomRequireTypeList in itemInfoBase.RandomRequireTypes)
        {
            var requireType = randomRequireTypeList.GetRandomType();
            if (requireType)
            {
                requireTypes.Add(requireType);   
            }
        }
        
        for (int i = 1; i < GameHardRate.Value + 1 ; i++)
        {
            foreach (var randomRequireTypeList in itemInfoBase.RandomRequireTypes)
            {
                if (Random.Range(0f,1f) > 0.8f)
                {
                    var requireType = randomRequireTypeList.GetRandomType();
                    if (requireType)
                    {
                        requireTypes.Add(requireType);   
                    }
                }
            }  
        }
        
        
        basicItem.Initialize(itemTypes, requireTypes);

        return basicItem;
        // var visualObject = ItemInfoBases
    }
}
