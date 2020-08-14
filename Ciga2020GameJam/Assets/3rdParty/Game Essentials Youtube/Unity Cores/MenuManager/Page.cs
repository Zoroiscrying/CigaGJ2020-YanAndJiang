using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace UnityCore.MenuSystem
{
    public enum PageMotionType
    {
        None,
        Animator,
        ScriptableTween
    }
    
    public class Page : MonoBehaviour
    {
        public static readonly string FLAG_ON = "On";
        public static readonly string FLAG_OFF = "Off";
        public static readonly string FLAG_NONE = "None";

        public PageMotionType pageMotionType;
        public PageAnimationOrganizer scriptableTween;
        public PageTypeAtom pageType;
        // public PageType type;
        // public bool useAnimation = false;
        public string targetState { get; private set; }

        public bool IsOn => m_isOn;

        private bool m_isOn = false;
        private Animator m_animator;
        private RectTransform _rectTransform;
        private CanvasGroup _canvasGroup;

        #region Unity Function

        private void OnEnable()
        {
            if (pageMotionType == PageMotionType.Animator)
            {
                CheckAnimatorIntegrity();   
            }

            _canvasGroup = GetComponent<CanvasGroup>();

            if (_canvasGroup == null)
            {
                Log("No Canvas Group for page: " + this.pageType +
                    ", this can lead to no effects when page get animated.");
            }

            _rectTransform = GetComponent<RectTransform>();
            
            if (_rectTransform == null)
            {
                Log("No Rect Transform for page: " + this.pageType +
                    ", this can lead to no effects when page get animated.");
            }
        }
        
        #endregion

        #region Public Functions

        public void Animate(bool on)
        {
            if (pageMotionType == PageMotionType.Animator)
            {
                //the animator is going to run automatically
                //and the coroutine below is going to check relevant parameter
                //to decide whether the page finishes animating
                m_animator.SetBool("on", on);
                StopCoroutine(AwaitAnimation(on));
                StartCoroutine(AwaitAnimation(on));
            }
            else if (pageMotionType == PageMotionType.ScriptableTween)
            {
                StopCoroutine(AwaitPageTween(on));
                StartCoroutine(AwaitPageTween(on));
            }
            else
            {
                if (!on)
                {
                    gameObject.SetActive(false);
                    m_isOn = false;
                }
                else
                {
                    m_isOn = true;
                }
            }
        }

        #endregion

        #region Private Functions

        private IEnumerator AwaitPageTween(bool on)
        {
            targetState = on ? FLAG_ON : FLAG_OFF;
            
            if (on)
            {
                scriptableTween.DoOnAnim(this._rectTransform, this._canvasGroup);   
            }
            else
            {
                scriptableTween.DoOffAnim(this._rectTransform, this._canvasGroup);
            }

            yield return new WaitForSeconds(scriptableTween.animTime);
            
            Log("PageType[" + this.pageType + "] finished transitioning to " + (on?"On":"Off") + "state");
            targetState = FLAG_NONE;
            _rectTransform.DOComplete();
            _canvasGroup.DOComplete();
            
            if (!on)
            {
                gameObject.SetActive(false);
                m_isOn = false;
            }
            else
            {
                m_isOn = true;
            }
        }

        private IEnumerator AwaitAnimation(bool on)
        {
            //set the animation state of the page based on the "on" parameter
            targetState = on ? FLAG_ON : FLAG_OFF;
            
            //wait for the animator to reach the target state
            while (!m_animator.GetCurrentAnimatorStateInfo(0).IsName(targetState))
            {
                yield return null;
            }
            
            //wait for the animator to finish animating
            while (m_animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1)
            {
                yield return null;
            }

            targetState = FLAG_NONE;
            Log("PageType[" + this.pageType + "] finished transitioning to " + (on?"On":"Off") + "state");
            
            if (!on)
            {
                gameObject.SetActive(false);
                m_isOn = false;
            }
            else
            {
                m_isOn = true;
            }
            
        }

        private void CheckAnimatorIntegrity()
        {
            if (pageMotionType == PageMotionType.Animator)
            {
                m_animator = this.GetComponent<Animator>();
                if (!m_animator)
                {
                    LogWarning("The animator for page" + this.gameObject.name + " is not added! \n " +
                               "A raw animator is going to be added and perform no function, please fix the issue.");
                    m_animator = this.gameObject.AddComponent<Animator>();
                }
            }
        }

        private void Log(string msg)
        {
            if (PageController.Instance.debug)
            {
                Debug.Log("[Page]:" + msg);
            }
        }

        private void LogWarning(string msg)
        {
            if (PageController.Instance.debug)
            {
                Debug.LogWarning("[Page]:" + msg);
            }
        }

        #endregion
    }   
}
