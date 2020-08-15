using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityAtoms.BaseAtoms;
using UnityEngine;

public class PausePanelIndicator : MonoBehaviour
{
    public TextMeshProUGUI ScoreText;
    public TextMeshProUGUI TimeText;
    public TextMeshProUGUI SatisfyText;

    public IntReference Score;
    public FloatReference Time;
    public IntReference SatisfyNum;

    void Update()
    {
        this.ScoreText.text = "Score: " + Score.Value;
        this.TimeText.text = "Time: " + Mathf.RoundToInt(Time.Value);
        this.SatisfyText.text = "Composed Item Num: " + SatisfyNum.Value;
    }
}
