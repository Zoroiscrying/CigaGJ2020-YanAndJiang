using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityCore.MenuSystem;
using UnityEngine;

namespace UnityCore.MenuSystem
{
    [CreateAssetMenu(fileName = "PageFadeAnimation", menuName = "UnityDev/MenuSystem/PageFadeAnimation")]
    public class PageFadeAnimation : PageAnimationType
    {
        public Ease easeType = Ease.InOutExpo;
        private bool _onFlag = false;
        
        public override void DoOffAnim(RectTransform rectTransform, CanvasGroup canvasGroup = null, float time = .2f)
        {
            if (!canvasGroup) return;
            if (_onFlag != false)
            {
                canvasGroup.alpha = 1f;   
            }
            // canvasGroup.DOComplete();
            canvasGroup.DOFade(0f, time).SetEase(easeType);
        }
        
        public override void DoOnAnim(RectTransform rectTransform, CanvasGroup canvasGroup = null, float time = .2f)
        {
            if (!canvasGroup) return;
            if (_onFlag != true)
            {
                canvasGroup.alpha = 0f;
            }
            canvasGroup.DOFade(1f, time).SetEase(easeType);
        }
    }
}

