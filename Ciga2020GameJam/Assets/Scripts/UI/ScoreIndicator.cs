using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityAtoms.BaseAtoms;
using UnityEngine;

public class ScoreIndicator : MonoBehaviour
{
    public IntReference currentScore;
    [SerializeField] private TextMeshProUGUI _textMeshProUgui;

    public void UpdateUI()
    {
        _textMeshProUgui.text =
            "Score: " + currentScore.Value;
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
