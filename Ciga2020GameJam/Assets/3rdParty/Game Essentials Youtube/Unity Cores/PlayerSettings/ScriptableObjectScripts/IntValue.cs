using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

namespace UnityCore.Data
{
    [CreateAssetMenu(fileName = "newIntValue", menuName = "UnityDev/DataSystem/IntValue")]
    public class IntValue : ScriptableObject
    {
        [SerializeField]
        [Header("This is a Int Value.")]
        protected int m_value;
        
        public virtual int Value
        {
            get => m_value;
            set => m_value = value;
        }
    }
}

