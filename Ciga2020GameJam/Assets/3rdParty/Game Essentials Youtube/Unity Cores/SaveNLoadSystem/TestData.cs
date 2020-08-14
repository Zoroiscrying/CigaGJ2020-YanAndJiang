using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityCore.Data
{
    public class TestData : MonoBehaviour
    {
        public DataController DataController;

#if UNITY_EDITOR

        private void Update()
        {
            if (Input.GetKeyUp(KeyCode.R))
            {
                TestAddScore(1);
            }
            if (Input.GetKeyUp(KeyCode.T))
            {
                TestAddScore(-1);
            }
            if (Input.GetKeyUp(KeyCode.Space))
            {
                ResetScore();
            }
        }

#endif

        private void TestAddScore(int delta)
        {
            
        }

        private void ResetScore()
        {
            
        }
    }
}

