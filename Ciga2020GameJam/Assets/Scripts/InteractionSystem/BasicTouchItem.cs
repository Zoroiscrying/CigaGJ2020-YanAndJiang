using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityCore.AudioSystem;
using UnityEngine;
using AudioType = UnityCore.AudioSystem.AudioType;

public class BasicTouchItem : MonoBehaviour, ITouchable
{
    private float DisappearTime = 8f;
    private bool touched = false;
    public bool Touched => touched;
    private Renderer _renderer;
    private Coroutine runningCor;
    
    // Start is called before the first frame update
    void Start()
    {
        _renderer = GetComponent<Renderer>();
        BlinkToDisappear(DisappearTime, .6f, 4f);
    }

    public virtual void ItemFunction()
    {
        AudioController.Instance.RestartAudio(AudioType.PackageSFX_Generate);
    }

    public void OnTouch()
    {
        if (runningCor != null)
        {
            StopCoroutine(runningCor);
        }
        ItemFunction();
        DestroySelf();
    }

    public void BlinkToDisappear(float timeToDisappear, float blinkInterval, float blinkAfterSeconds)
    {
        runningCor = StartCoroutine(Blink(timeToDisappear, blinkInterval, blinkAfterSeconds));
    }

    private void DestroySelf()
    {
        Destroy(this.gameObject, 2.0f);
        this.gameObject.SetActive(false);
    }

    private IEnumerator Blink(float timeToDisappear, float blinkInterval, float blinkAfterSeconds)
    {
        float timeClock = 0.0f;
        int blinkedTime = 1;
        while (timeClock < timeToDisappear)
        {
            if (touched)
            {
                yield break;
            }

            float blinkTime = 0f;

            if (timeClock >= blinkAfterSeconds)
            {
                blinkTime = Mathf.Clamp(blinkInterval / blinkedTime, 0.02f, blinkInterval);
                _renderer.enabled = false;
                yield return new WaitForSeconds(blinkTime);
                _renderer.enabled = true;
                yield return new WaitForSeconds(blinkTime);
                blinkedTime++;
            }

            timeClock += Time.deltaTime + 2 * blinkTime;
            yield return null;
        }
        
        DestroySelf();
    }
    
}
