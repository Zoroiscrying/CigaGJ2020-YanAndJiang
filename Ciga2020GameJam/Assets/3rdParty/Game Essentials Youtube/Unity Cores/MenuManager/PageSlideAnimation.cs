using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace UnityCore.MenuSystem
{
    [CreateAssetMenu(fileName = "PageSlideAnimation", menuName = "UnityDev/MenuSystem/PageSlideAnimation")]
    public class PageSlideAnimation : PageAnimationType
    {
        public Vector2 anchoredPositionFrom = Vector2.zero;
        public Vector2 anchoredPositionTo = Vector2.zero;
        public Ease easeType = Ease.InOutExpo;
        private bool _onFlag = false;

        public override void DoOffAnim(RectTransform rectTransform, CanvasGroup canvasGroup = null, float time = .2f)
        {
            if (!rectTransform) return;
            if (_onFlag != false)
            {
                rectTransform.anchoredPosition = anchoredPositionTo;   
            }
            rectTransform.DOAnchorPos(anchoredPositionFrom, time).SetEase(easeType);
        }
        
        public override void DoOnAnim(RectTransform rectTransform, CanvasGroup canvasGroup = null, float time = .2f)
        {
            if (!rectTransform) return;
            if (_onFlag != true)
            {
                rectTransform.anchoredPosition = anchoredPositionFrom;   
            }
            rectTransform.DOAnchorPos(anchoredPositionTo, time).SetEase(easeType);
        }
    }
}


