using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace UnityCore.MenuSystem
{
    public class TestMenu : MonoBehaviour
    {
        public PageController pageController;

        [SerializeField] private PageTypeAtom Menu;
        [SerializeField] private PageTypeAtom LoadingPage;
        
#if UNITY_EDITOR
        private void Update()
        {
            if (Input.GetKeyUp(KeyCode.F))
            {
                //turn the page on
                pageController.TurnPageOn(LoadingPage);
            }
            if (Input.GetKeyUp(KeyCode.G))
            {
                //turn the page off
                pageController.TurnPageOff(LoadingPage);
            }

            if (Input.GetKeyUp(KeyCode.H))
            {
                //turn the page off and then turn the page on
                pageController.TurnPageOff(LoadingPage, Menu);
            }

            if (Input.GetKeyUp(KeyCode.J))
            {
                //turn the page off while turning the page on
                pageController.TurnPageOff(LoadingPage, Menu, true);
            }
        }
#endif
    }
}

