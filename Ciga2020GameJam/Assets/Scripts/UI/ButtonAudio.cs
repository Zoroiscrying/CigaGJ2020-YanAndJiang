using System.Collections;
using System.Collections.Generic;
using UnityCore.AudioSystem;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class ButtonAudio : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        AudioController.Instance.RestartAudio(UnityCore.AudioSystem.AudioType.UISFX_ButtonClick);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        AudioController.Instance.RestartAudio(UnityCore.AudioSystem.AudioType.UISFX_ButtonHover);
    }
}
