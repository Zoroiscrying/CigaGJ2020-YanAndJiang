using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UniRx.Triggers;
using UnityAtoms.BaseAtoms;
using UnityEngine;

public class ScoreIndicator : MonoBehaviour
{
    public IntReference currentScore;
    [SerializeField] private TextMeshProUGUI _textMeshProUgui;
    public IntReference currentHardRate;
    private float BeatTimer = 0.0f;
    [SerializeField] private int roundScoreAmount = 3600;

    public void UpdateUI()
    {
        _textMeshProUgui.text =
            "SCORE: " + currentScore.Value;

        _textMeshProUgui.color =
            Color.HSVToRGB((float) (currentScore.Value % roundScoreAmount) / roundScoreAmount, .6f, .9f);
    }
    
    public void BeatText()
    {
        this._textMeshProUgui.rectTransform.DORewind();
        this._textMeshProUgui.rectTransform.DOPunchScale(Vector3.one * 0.9f, .25f);
    }

    private void Start()
    {
        UpdateUI();
    }
}
