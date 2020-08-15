using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityAtoms.BaseAtoms;
using UnityEngine;

public class MassIndicator : MonoBehaviour
{
    public IntReference CurrentMass;
    public IntReference MaxMass;
    [SerializeField] private TextMeshProUGUI _textMeshPro;

    public void UpdateUI()
    {
        _textMeshPro.text = "Room Mass " + CurrentMass.Value + "/" + MaxMass.Value;
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
