using System;
using System.Collections;
using System.Collections.Generic;
using UnityCore.AudioSystem;
using UnityEngine;
using UnityEngine.iOS;

namespace UnityCore.MenuSystem
{
    public class PageController : Singleton<PageController>
    {

        public bool debug;
        public PageTypeAtom entryPage;
        public Page[] pages;
        private Hashtable m_pageHashTable;

        #region Unity Functions
        
        private void Awake()
        {
            if (PageController.Instance == this)
            {
                DontDestroyOnLoad(this.gameObject);
                m_pageHashTable = new Hashtable();
                RegisterAllPages();
        
                if (entryPage != null)
                {
                    TurnPageOn(entryPage);
                }
            }
            else
            {
                Destroy(this.gameObject);
            }   
        }

        #endregion
        
        #region Public Functions

        public void TurnPageOn(PageTypeAtom pageType, bool turnOtherPagesOff = false)
        {
            if (pageType == null)
            {
                return;
            }

            if (!PageExists(pageType))
            {
                LogWarning("The page type [" + pageType + "] is not registered!");
            }

            // AudioController.Instance.PlayAudio(UnityCore.AudioSystem.AudioType.UISFX_PageOn);   
            Page onPage = GetPage(pageType);
            onPage.gameObject.SetActive(true);
            onPage.Animate(true);

            if (turnOtherPagesOff)
            {
                foreach (var page in m_pageHashTable)
                {
                    var pageObject = page as Page;
                    if (pageObject && pageObject != onPage)
                    {
                        TurnPageOff(pageObject.pageType);
                    }
                }
            }
        }

        /// <summary>
        /// Turn a page off and turn a page on, afterwards or at the same time, controlled by the waitForExit parameter
        /// </summary>
        /// <param name="off">The page that will be turned off</param>
        /// <param name="on">The page that will be turned on</param>
        /// <param name="waitForExit">If the page that is going to be turned on
        /// wait for the other page to be turned off</param>
        public void TurnPageOff(PageTypeAtom off, PageTypeAtom on = null, bool waitForExit = false)
        {
            if (off == null)
            {
                return;
            }
            
            if (!PageExists(off))
            {
                LogWarning("The page type [" + off + "] is not registered!");
            }

            // AudioController.Instance.PlayAudio(UnityCore.AudioSystem.AudioType.UISFX_PageOff);
            Page offPage = GetPage(off);
            if (offPage.gameObject.activeSelf)
            {
                offPage.Animate(false);
            }

            if (waitForExit)
            {
                Page onPage = GetPage(on);
                StopCoroutine(WaitForPageExit(onPage, offPage));
                StartCoroutine(WaitForPageExit(onPage, offPage));
            }
            else
            {
                TurnPageOn(on);
            }
        }

        public bool IsPageOn(PageTypeAtom pageType)
        {
            Page page = GetPage(pageType);
            if (page)
            {
                return page.IsOn;
            }
            return false;
        }

        #endregion

        #region Private Functions

        //wait for the off page to be turning off before the on page turns on
        private IEnumerator WaitForPageExit(Page on, Page off)
        {
            //wait for the page's state to become FLAG_NONE
            //so that it means that the off page finishes animating itself, and the on page can be turned on.
            while (off.targetState != Page.FLAG_NONE)
            {
                yield return null;
            }
            TurnPageOn(on.pageType);
        }

        private void RegisterAllPages()
        {
            foreach (var page in pages)
            {
                RegisterPage(page);
            }
        }
        
        private void RegisterPage(Page page)
        {
            if (PageExists(page.pageType))
            {
                LogWarning("Trying to register a page that is already Registered! Type: " + page.pageType +
                           ". GameObject name: " + page.gameObject.name);
                return;
            }
            
            m_pageHashTable.Add(page.pageType, page);
            Log("Registered new page:[" + page.pageType + "]");
        }

        private Page GetPage(PageTypeAtom pageType)
        {
            if (!PageExists(pageType))
            {
                LogWarning("You are trying to get a page that has not been registered. Type:[" + pageType +"]");
                return null;
            }
            
            return (Page)m_pageHashTable[pageType];
        }

        private bool PageExists(PageTypeAtom pageType)
        {
            return m_pageHashTable.ContainsKey(pageType);
        }

        private void Log(string msg)
        {
            if (debug)
            {
                Debug.Log("[PageController]:" + msg);
            }
        }

        private void LogWarning(string msg)
        {
            if (debug)
            {
                Debug.LogWarning("[PageController]:" + msg);
            }
        }

        #endregion
    }   
}
