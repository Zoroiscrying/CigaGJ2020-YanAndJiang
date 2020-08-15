using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityAtoms.BaseAtoms;
using UnityEngine;

public class TimeIndicator : MonoBehaviour
{
    public FloatReference currentTime;
    [SerializeField] private TextMeshProUGUI _textMeshProUgui;

    public void UpdateUI()
    {
        _textMeshProUgui.text =
            "Time: " + Mathf.RoundToInt(currentTime.Value);
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
