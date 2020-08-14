using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace UnityCore.AudioSystem
{
    public class TestAudioController : MonoBehaviour
    {
        public AudioController AudioController;
        private float timer = 0.0f;
#if UNITY_EDITOR
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.T))
            {
                AudioController.PlayAudio(AudioType.Music_01);
            }
            if (Input.GetKeyDown(KeyCode.G))
            {
                AudioController.PlayAudio(AudioType.Music_02);
            }
            if (Input.GetKeyDown(KeyCode.B))
            {
                if (Random.Range(0f,1f) > 0.5f)
                {
                    AudioController.RestartAudio(AudioType.SFX_01);
                }
                else
                {
                    AudioController.RestartAudio(AudioType.SFX_02);   
                }
            }

            timer += Time.deltaTime;
            if (timer>0.2f)
            {
                timer = 0.0f;
                AudioController.RestartAudio(AudioType.SFX_01);
            }
        }
#endif
    }
}

