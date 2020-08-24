using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityAtoms.BaseAtoms;
using UnityEngine;

public class HardRateIndicator : MonoBehaviour
{
    public IntReference HardRate;
    [SerializeField] private TextMeshProUGUI _textMeshProUgui;
    public Color EasyCol;
    public Color MidCol;
    public Color HardCol;
    public Color OtherCol;
    public RectTransform BgObj;
    private RectTransform _rectTransform;

    private void Start()
    {
        _rectTransform = this.GetComponent<RectTransform>();
        HintHardRate();
    }

    public void HintHardRate()
    {
        switch (HardRate.Value)
        {
            case 0:
                _textMeshProUgui.text = "Current Mode: Easy";
                _textMeshProUgui.color = EasyCol;
                break;
            case 1:
                _textMeshProUgui.text = "Current Mode: Medium";
                _textMeshProUgui.color = MidCol;
                break;
            case 2:
                _textMeshProUgui.text = "Current Mode: Hard";
                _textMeshProUgui.color = HardCol;
                break;
            default:
                _textMeshProUgui.text = "Current Mode: Unknown!";
                _textMeshProUgui.color = OtherCol;
                break;        
        }
        
        BgObj.DOLocalMoveX(260, 1.0f).SetEase(Ease.OutBack);
        this._rectTransform.DOLocalMoveX(260, 1.0f).SetEase(Ease.OutBack);

        Timer.Register(5.0f, () =>
        {
            BgObj.DOLocalMoveX(-230, 1.0f).SetEase(Ease.OutBack);
            this._rectTransform.DOLocalMoveX(-230, 1.0f).SetEase(Ease.OutBack);
        });
        
    }
    
}
