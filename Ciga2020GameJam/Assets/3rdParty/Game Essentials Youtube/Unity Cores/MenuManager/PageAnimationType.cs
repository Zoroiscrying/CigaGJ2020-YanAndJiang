using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace UnityCore.MenuSystem
{
    public class PageAnimationType : ScriptableObject
    {
        public virtual void DoOffAnim(RectTransform rectTransform, CanvasGroup canvasGroup = null, float time = .2f)
        {
            
        }
        
        public virtual void DoOnAnim(RectTransform rectTransform, CanvasGroup canvasGroup = null, float time = .2f)
        {
            
        }
    }
}

