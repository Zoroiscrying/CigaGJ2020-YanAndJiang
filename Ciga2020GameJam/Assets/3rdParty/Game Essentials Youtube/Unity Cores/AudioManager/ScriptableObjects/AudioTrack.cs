using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.Audio;

namespace UnityCore.AudioSystem
{
    /// <summary>
    /// A sound track will manage the audios that will be playing through an Audio Source
    /// A sound track will hold different audio objects and will only be playing one Audio Object at a time.
    /// </summary>
    [CreateAssetMenu(fileName = "AudioTrack", menuName = "UnityDev/AudioSystem/AudioTrack")]
    public class AudioTrack : ScriptableObject
    {
        private AudioSource m_AudioSource;
        public AudioSource AudioSource => m_AudioSource;
        public AudioObject[] AudioObjects;
        public AudioType PlayingType = AudioType.None;

        [Header("Audio Source Configurations")]
        public AudioSourceSetting audioSourceSetting;

        //Compared with the Editor configuration implementation of the audio track
        //this method will cause a little bit of initialization time
        //but the configurations can be saved in local files and easily reused for different purposes
        public void RegisterAudioSource(AudioSource audioSource)
        {
            this.m_AudioSource = audioSource;
            audioSourceSetting.ApplySettings(audioSource);
        }
    }
    
}

