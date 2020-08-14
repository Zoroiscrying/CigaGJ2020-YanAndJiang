using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace UnityCore.AudioSystem
{
    // /// <summary>
    // /// An audio object holds an AudioType and an associated AudioClip.
    // /// </summary>
    [CreateAssetMenu(fileName = "AudioObject", menuName = "UnityDev/AudioSystem/AudioObject")]
    public class AudioObject : ScriptableObject
    {
        public AudioType Type;
        public AudioClip Clip;
    }
}

