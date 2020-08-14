using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityCore.MenuSystem
{
    [CreateAssetMenu]
    public class PageAnimationOrganizer : ScriptableObject
    {
        public float animTime = .5f;
        public List<PageAnimationType> AnimationTypes = new List<PageAnimationType>();

        public void DoOffAnim(RectTransform rectTransform, CanvasGroup canvasGroup = null)
        {
            foreach (var pageAnimationType in AnimationTypes)
            {
                pageAnimationType.DoOffAnim(rectTransform, canvasGroup, animTime);
            }
        }

        public void DoOnAnim(RectTransform rectTransform, CanvasGroup canvasGroup = null)
        {
            foreach (var pageAnimationType in AnimationTypes)
            {
                pageAnimationType.DoOnAnim(rectTransform, canvasGroup, animTime);
            }
        }
    }
}

