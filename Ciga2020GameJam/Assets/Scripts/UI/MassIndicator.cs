using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityAtoms.BaseAtoms;
using UnityEngine;

public class MassIndicator : MonoBehaviour
{
    public IntReference CurrentMass;
    public IntReference MaxMass;
    [SerializeField] private TextMeshProUGUI _textMeshPro;
    private bool TurnedOn = false;
    private RectTransform _rectTransform;

    public void UpdateUI()
    {
        if (!TurnedOn)
        {
            if (ShouldAppear())
            {
                this._rectTransform.DOLocalMoveY(150, 1.0f);
                TurnedOn = true;
            }
        }
        else
        {
            if (!ShouldAppear())
            {
                this._rectTransform.DOLocalMoveY(-150, 1.0f);
                TurnedOn = false;
            }
        }
        _textMeshPro.text = "Room Mass " + CurrentMass.Value + "/" + MaxMass.Value;
    }

    private bool ShouldAppear()
    {
        if (CurrentMass.Value >= MaxMass.Value - 10)
        {
            return true;
        }
        return false;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        _rectTransform = this.GetComponent<RectTransform>();
        _rectTransform.anchoredPosition = new Vector2(0, -540);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateUI();
    }
}
