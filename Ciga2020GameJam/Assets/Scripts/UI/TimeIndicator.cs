using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityAtoms.BaseAtoms;
using UnityEngine;

public class TimeIndicator : MonoBehaviour
{
    public FloatReference currentTime;
    [SerializeField] private TextMeshProUGUI _textMeshProUgui;
    public RectTransform BgObj;
    private int multiplier = 0;
    private RectTransform _rectTransform;

    public void UpdateUI()
    {
        if (currentTime >= 60 * multiplier)
        {
            // Debug.Log("sLIDING OUT!");
            multiplier++;
            BgObj.DOLocalMoveX(150, 1.0f).SetEase(Ease.OutBack);
            this._rectTransform.DOLocalMoveX(150, 1.0f).SetEase(Ease.OutBack);

            Timer.Register(5.0f, () =>
            {
                BgObj.DOLocalMoveX(-120, 1.0f).SetEase(Ease.OutBack);
                this._rectTransform.DOLocalMoveX(-120, 1.0f).SetEase(Ease.OutBack);
            });
        }
        
        _textMeshProUgui.text =
            "Time: " + Mathf.RoundToInt(currentTime.Value);
    }
    
    // Start is called before the first frame update
    void Start()
    {
        _rectTransform = this.GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateUI();
    }
}
