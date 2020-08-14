using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

namespace UnityCore.AudioSystem
{
    public class AudioController : Singleton<AudioController>
    {
        public bool debug = false;
        //relationships between audio types(key) and audio tracks(value) (Audio Tracks)
        private Hashtable m_AudioTable;
        //relationships between audio types(key) and jobs(value) (Coroutine, IEnumerator)
        private Hashtable m_JobTable;
        public AudioTrack[] Tracks;

        private class AudioJob
        {
            public AudioJobAction Action;
            public AudioType AudioType;
            public bool Fade;
            public float FadeTime;
            public float DelayTime;

            public AudioJob(AudioJobAction action, AudioType type, bool fade, float fadeTime, float delayTime)
            {
                this.Action = action;
                this.AudioType = type;
                this.Fade = fade;
                this.FadeTime = fadeTime;
                this.DelayTime = delayTime;
            }
        }
        
        private enum AudioJobAction
        {
            START,
            STOP,
            RESTART
        }
        
        #region Public Functions

        public void PlayAudio(AudioType type, bool fade = false, float fadeTime = 0.2f, float delay = 0.0f)
        {
            AddJob(new AudioJob(AudioJobAction.START, type, fade, fadeTime, delay));
        }

        public void StopAudio(AudioType type, bool fade = false, float fadeTime = 0.2f, float delay = 0.0f)
        {
            AddJob(new AudioJob(AudioJobAction.STOP, type, fade, fadeTime, delay));
        }

        public void RestartAudio(AudioType type, bool fade = false, float fadeTime = 0.2f, float delay = 0.0f)
        {
            AddJob(new AudioJob(AudioJobAction.RESTART, type, fade, fadeTime, delay));
        }

        #endregion

        #region Unity Functions

        private void Awake()
        {
            if (AudioController.Instance == this)
            {
                //initialized the controller
                Configure();
                // m_AudioTable = new Hashtable();
                // m_JobTable = new Hashtable();
            }
            else
            {
                Destroy(this.gameObject);
            }   
        }

        private void OnDisable()
        {
            //close all ienumerators
            Dispose();
        }

        #endregion

        #region Private Functions

        private void AddJob(AudioJob audioJob)
        {
            //remove conflicting jobs
            if (audioJob.Action != AudioJobAction.START)
            {
                RemoveConflictingJobs(audioJob.AudioType);   
            }
            else
            {
                //it is a start Job, with an audio type
                //we need to know if the type is currently playing in any of the track in the scene
                foreach (DictionaryEntry entry in m_AudioTable)
                {
                    AudioTrack track = (AudioTrack) entry.Value;
                    if (track.PlayingType == audioJob.AudioType)
                    {
                        return;
                    }
                }
            }

            //start job
            IEnumerator jobRunner = RunAudioJob(audioJob);
            m_JobTable.Add(audioJob.AudioType, jobRunner);
            StartCoroutine(jobRunner);
            Log("Starting job on Job type[" + audioJob.AudioType + "] with operation: " + audioJob.Action);
        }

        private void RemoveConflictingJobs(AudioType type)
        {
            if (m_JobTable.ContainsKey(type))
            {
                RemoveJob(type);
            }

            //check if the type shares the same track with the running jobs
            //and kills the conflicting job if it is.
            AudioType conflictingAudioType = AudioType.None;
            foreach (DictionaryEntry entry in m_JobTable)
            {
                AudioType audioType = (AudioType)entry.Key;
                AudioTrack audioTrackInUse = (AudioTrack) m_AudioTable[audioType];
                AudioTrack audioTrackNeeded = (AudioTrack) m_AudioTable[type];
                if (audioTrackNeeded == audioTrackInUse)
                {
                    //there is a conflict, we store the type of it and remove the job accordingly
                    conflictingAudioType = audioType;
                }
            }

            if (conflictingAudioType != AudioType.None)
            {
                RemoveJob(conflictingAudioType);
            }
            
        }

        private void RemoveJob(AudioType type)
        {
            if (!m_JobTable.ContainsKey(type))
            { 
                LogWarning("Trying to stop a job that is not running! Audio type: " + type);
                return;
            }

            IEnumerator runningJob = (IEnumerator) m_JobTable[type];
            StopCoroutine(runningJob);
            m_JobTable.Remove(type);
        }

        private IEnumerator RunAudioJob(AudioJob audioJob)
        {
            // yield return this;
            AudioTrack track = (AudioTrack) m_AudioTable[audioJob.AudioType];
            track.AudioSource.clip = GetAudioClipFromAudioTrack(audioJob.AudioType, track);
            //do delay
            yield return new WaitForSeconds(audioJob.DelayTime);
            
            switch (audioJob.Action)
            {
                case AudioJobAction.START:
                    track.AudioSource.Play();
                    track.PlayingType = audioJob.AudioType;
                    break;
                case AudioJobAction.STOP:
                    if (!audioJob.Fade)
                    {
                        track.AudioSource.Stop();
                        track.PlayingType = AudioType.None;
                    }
                    break;
                case AudioJobAction.RESTART:
                    track.AudioSource.Stop();
                    track.AudioSource.Play();
                    track.PlayingType = audioJob.AudioType;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (audioJob.Fade)
            {
                float initialValue = audioJob.Action == AudioJobAction.STOP ? 1.0f : 0.0f;
                float endValue = 1.0f - initialValue;
                float timer = 0.0f;

                while (timer < audioJob.FadeTime)
                {
                    track.AudioSource.volume = Mathf.Lerp(initialValue, endValue, timer / audioJob.FadeTime);
                    timer += Time.deltaTime;
                    yield return null;
                }

                if (audioJob.Action == AudioJobAction.STOP)
                {
                    track.AudioSource.Stop();
                    track.PlayingType = AudioType.None;
                }
            }
            
            m_JobTable.Remove(audioJob.AudioType);
            Log("Job Count: " + m_JobTable.Count);
            yield return null;
        }

        private AudioClip GetAudioClipFromAudioTrack(AudioType type, AudioTrack track)
        {
            //find the AudioObject that matches the requiring type in specific track
            //get the AudioClip attached to it and return
            foreach (var audioObject in track.AudioObjects)
            {
                if (audioObject.Type == type)
                {
                    return audioObject.Clip;
                }
            }
            return null;
        }

        //Initialize the Audio Controller
        private void Configure()
        {
            m_AudioTable = new Hashtable();
            m_JobTable = new Hashtable();
            GenerateAudioSourcesBasedOnTrack();
            GenerateAudioTable();
        }

        private void Dispose()
        {
            foreach (DictionaryEntry entry in m_JobTable)
            {
                IEnumerator job = (IEnumerator) entry.Value;
                StopCoroutine(job);
            }
        }

        private void GenerateAudioTable()
        {
            foreach (var audioTrack in Tracks)
            {
                foreach (var audioObject in audioTrack.AudioObjects)
                {
                    // do not duplicate keys
                    if (m_AudioTable.ContainsKey(audioObject.Type))
                    {
                        LogWarning("Trying to register a page that is already Registered! Type: " + audioObject.Type);
                    }
                    else
                    {
                        m_AudioTable.Add(audioObject.Type, audioTrack);
                        Log("Registering audio[" + audioObject.Type + "].");
                    }
                    
                }
            }
        }

        private void GenerateAudioSourcesBasedOnTrack()
        {
            foreach (var audioTrack in Tracks)
            {
                var obj = new GameObject();
                obj.transform.SetParent(this.transform);
                var audioSource = obj.AddComponent<AudioSource>();
                audioTrack.RegisterAudioSource(audioSource);
            }
            Log("Finished generating AudioSources for audio tracks.");
        }

        private void Log(string msg)
        {
            if (debug)
            {
                Debug.Log("[AudioController]:" + msg);
            }
        }

        private void LogWarning(string msg)
        {
            if (debug)
            {
                Debug.LogWarning("[AudioController]:" + msg);
            }
        }

        #endregion
    }
}

