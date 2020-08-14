using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityCore.Data
{
    [CreateAssetMenu(fileName = "newIntValue", menuName = "UnityDev/DataSystem/FloatValue")]
   public class FloatValue : ScriptableObject
   {
       [Header("This is a Float Value.")]
       public float value;
   } 
}

