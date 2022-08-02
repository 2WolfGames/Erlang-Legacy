using BehaviorDesigner.Runtime;
using UnityEngine;

namespace Core.IA.Bahavior.SharedVariable
{
    [System.Serializable]
    public class SharedAudioClip : SharedVariable<AudioClip>
    {
        public static implicit operator SharedAudioClip(AudioClip value) { return new SharedAudioClip { Value = value }; }
    }
}
