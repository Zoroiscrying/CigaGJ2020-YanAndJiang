using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityCore.MenuSystem;
using UnityEngine;

public class UIController : Singleton<UIController>
{
    [Header("Page Type")] public PageTypeAtom PausePage;
    public PageTypeAtom LoadingPage;
    public PageTypeAtom GamePage;
    public PageTypeAtom MenuPage;
    public PageTypeAtom DevPage;

    public int MaxScoreForGradient;
    public Gradient ScoreTextColorGradient;
    public Transform ScoreTextParent;
    public GameObject ScoreTextPrefab;

    public Transform ItemUIHolderParent;
    public GameObject ItemUIHolderPrefab;
    public SpriteItemTypeLibrary SpriteItemTypeLibrary;
    public RectTransform MainCanvas;

    [Header("Pause and End Panel")] 
    public GameObject pausePanel;
    public GameObject ResumeBtn;
    
    
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

    public void GenerateScoreTextFromPosition(int score, Vector3 pos)
    {
        var obj = Instantiate(ScoreTextPrefab, Vector3.zero, Quaternion.identity);
        obj.transform.SetParent(ScoreTextParent);
        var text = obj.GetComponent<TextMeshProUGUI>();
        var percentage = Mathf.Clamp01((float)score/MaxScoreForGradient);
        text.color = ScoreTextColorGradient.Evaluate(percentage);
        text.text = "+" + score;
        var rectTransform = text.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = UIMethods.GetScreenPosViaWorldPos(MainCanvas, pos);
        rectTransform.DOLocalMoveX(rectTransform.localPosition.x + Random.Range(-10f, 10f), 1.0f);
        rectTransform.DOLocalMoveY(rectTransform.localPosition.y + Random.Range(10f, 15f), 1.0f);
        rectTransform.DOScale(Vector3.zero, 1.0f).SetEase(Ease.InExpo);
        Timer.Register(1.01f, (() => Destroy(rectTransform.gameObject)));
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
            // PageController.Instance.TurnPageOn(MenuPage);
            Timer.Register(0.49f , (() => PageController.Instance.TurnPageOn(MenuPage)));
        }
        else
        {
            // PageController.Instance.TurnPageOff(MenuPage);
            Timer.Register(0.49f , (() => PageController.Instance.TurnPageOff(MenuPage)));   
        }
    }

    public void TurnDeveloperPage(bool on)
    {
        if (on)
        {
            PageController.Instance.TurnPageOn(DevPage);   
        }
        else
        {
            PageController.Instance.TurnPageOff(DevPage);   
        }
    }

    public void Quit()
    {
        Application.Quit();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
