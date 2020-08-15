using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityAtoms.BaseAtoms;
using UnityEngine;

public class HardRateIndicator : MonoBehaviour
{
    public IntReference HardRate;
    [SerializeField] private TextMeshProUGUI _textMeshProUgui;

    public void UpdateUI()
    {
        _textMeshProUgui.text =
            "Hard Rate: " + HardRate.Value;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateUI();
    }
}
