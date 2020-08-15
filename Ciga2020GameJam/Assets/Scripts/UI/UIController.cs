using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : Singleton<UIController>
{
    public Transform ItemUIHolderParent;
    public GameObject ItemUIHolderPrefab;
    public SpriteItemTypeLibrary SpriteItemTypeLibrary;
    public RectTransform MainCanvas;
    
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        SpriteItemTypeLibrary.Initialize();
    }

    public ItemUIHolder GenerateUIHolder(BasicItem item)
    {
        var obj = Instantiate(ItemUIHolderPrefab, Vector3.zero, Quaternion.identity);
        obj.transform.SetParent(ItemUIHolderParent);
        var itemUIHolder = obj.GetComponent<ItemUIHolder>();
        return itemUIHolder;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
