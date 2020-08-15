using System.Collections;
using System.Collections.Generic;
using UnityCore.MenuSystem;
using UnityEngine;

public class UIController : Singleton<UIController>
{
    [Header("Page Type")] public PageTypeAtom PausePage;
    public PageTypeAtom LoadingPage;
    public PageTypeAtom GamePage;
    public PageTypeAtom MenuPage;
    
    public Transform ItemUIHolderParent;
    public GameObject ItemUIHolderPrefab;
    public SpriteItemTypeLibrary SpriteItemTypeLibrary;
    public RectTransform MainCanvas;

    [Header("Pause and End Panel")] 
    public GameObject pausePanel;
    
    
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

    public void TurnPausePage(bool on)
    {
        if (on)
        {
            PageController.Instance.TurnPageOn(PausePage);   
        }
        else
        {
            PageController.Instance.TurnPageOff(PausePage);   
        }
    }
    
    public void TurnGamePage(bool on)
    {
        if (on)
        {
            PageController.Instance.TurnPageOn(GamePage);   
        }
        else
        {
            PageController.Instance.TurnPageOff(GamePage);   
        }
    }
    
    public void TurnLoadingPage(bool on)
    {
        if (on)
        {
            PageController.Instance.TurnPageOn(LoadingPage);   
        }
        else
        {
            PageController.Instance.TurnPageOff(LoadingPage);   
        }
    }
    
    public void TurnMenuPage(bool on)
    {
        if (on)
        {
            PageController.Instance.TurnPageOn(MenuPage);   
        }
        else
        {
            PageController.Instance.TurnPageOff(MenuPage);   
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
