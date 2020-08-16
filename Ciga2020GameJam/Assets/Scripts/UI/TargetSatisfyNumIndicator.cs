using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityAtoms.BaseAtoms;
using UnityEngine;

public class TargetSatisfyNumIndicator : MonoBehaviour
{
    public IntReference currentBrokenBlockNum;
    public IntReference currentSatisfiedNum;
    public IntReference targetSatisfiyNum;
    public GameObject Bg;
    [SerializeField] private TextMeshProUGUI _textMeshProUgui;

    public void UpdateUI()
    {
        if (currentBrokenBlockNum.Value <= 0)
        {
            _textMeshProUgui.enabled = false;
            Bg.SetActive(false);
            return;
        }

        Bg.SetActive(true);
        _textMeshProUgui.enabled = true;
        _textMeshProUgui.text =
            "Compose " + currentSatisfiedNum.Value + "/" + targetSatisfiyNum.Value + " Items to Repair!";
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateUI();
    }
}
