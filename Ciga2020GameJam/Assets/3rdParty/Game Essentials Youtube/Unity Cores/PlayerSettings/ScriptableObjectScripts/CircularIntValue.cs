using System;
using System.Collections;
using System.Collections.Generic;
using UnityCore.Data;
using UnityEngine;

namespace UnityCore.Data
{
    [CreateAssetMenu(fileName = "newCircularIntValue", menuName = "UnityDev/DataSystem/CircularIntValue")]
    public class CircularIntValue : IntValue
    {
        [Header("The Circular Int value will return to min value if the value reached the MaxValue+1")]
        public int minValue = 0;
        public int maxValue = 1;

        public override int Value
        {
            get=>m_value;
            set
            {
                m_value = value;
                CheckIntValue();
            }
        }

        private void CheckIntValue()
        {
            if (m_value >= maxValue+1)
            { 
                m_value = (m_value-1) % maxValue + minValue;   
            }
        }

        private void OnValidate()
        {
            minValue = Mathf.Clamp(minValue, int.MinValue, Value);
            CheckIntValue();
            maxValue = Mathf.Clamp(maxValue, Value, int.MaxValue);
        }
    }
}

