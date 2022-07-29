

using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Player.Data
{
    [Serializable]
    public class SFX
    {
        [Serializable]
        public class SFXsDefinition
        {
            public AudioClip[] clips;
            [Range(0f, 1f)] public float volume = 0.5f;
        }

        public SFXsDefinition abilityAdquired;
        public SFXsDefinition punch;
        public SFXsDefinition dash;
        public SFXsDefinition heal;
        public SFXsDefinition death;
        public SFXsDefinition hurt;
        public SFXsDefinition jump;
        public SFXsDefinition ray;
    }
}